//(function () {
//})();
//TODO: JS-Scope
//http://stackoverflow.com/questions/6888570/declaring-variables-without-var-keyword

var NotificationTypes = [
    "general",
    "comment",
    "rating",
    "buddy-request",
    "buddy-confirmation",
    "buddy-new-photo",
    "dropbox-synchronization",
    "photo-synch"
];

var LanguageCodes = ["de", "en"];

var global_config = new fsConfig(),
    global_version = "fs" + global_config.version(),
    global_apipath = "/api",
    global_lang,
    global_member,
    global_auth_member,
    global_route,
    global_pageResets,
    global_keyBindings = new KeyBindings(),
    global_format,
    global_online = false,
    global_env = (window.location.hostname.indexOf("fotosteam.com") > -1) ? "P" : "D",
    global_min = (global_env === "P") ? ".min" : "",
    i18n,
    markdown = new MarkdownDeep.Markdown();
    markdown.ExtraMode = true;
    markdown.SafeMode = false;

try {
    global_online = (google !== undefined); //navigator.onLine funktioniert im FF nicht, daher schauen ob Maps geladen wurde
} catch (e) { }

global_lang = $.cookie("fotosteam-language");
if (!global_lang) {
    global_lang = (navigator.languages ? navigator.languages[0] : (navigator.language || navigator.userLanguage)).split("-")[0];
}
initApp();
// =============================================
function initApp() {
    $(document).ready(function () {

        var path = window.location.href.replace(window.location.origin, "");

        $.ajaxSetup({ cache: false });

        //Angemeldeten Benutzer ermitteln
        $.when(
            getSteamData("memberinfo", null, function (resA) {
                if (resA.Data !== null) {
                    window.global_auth_member = new Member(resA.Data);
                    if (window.global_auth_member.Options && $.cookie("fotosteam-language") === undefined) {
                        setLanguage(LanguageCodes[window.global_auth_member.Options.Language]);
                    }
                    connectToHub();
                }
            })
        ).then(function () {

            i18n = new i18n_Translator(global_lang, function() {
                //Attention-Message laden... (type: warning|info)
                if ($.cookie("fotosteam-message") !== getDayNumber()) {
                    $.getJSON("/Content/fotosteam-attention-message.json", function (json) {
                        if (json && json.type) {
                            var sMsg = json[global_lang],
                                jMsg = $('<div id="attention-bar"></div>'),
                                jMsgClose = $('<i class="icon icon-cancel-thin"></i>');

                            if (json.icon.length !== 0) { jMsg.addClass("icon icon-" + json.icon); }
                            if (json.type.length !== 0) { jMsg.addClass(json.type); }

                            jMsgClose.click(function () {
                                $.cookie("fotosteam-message", getDayNumber(), { path: '/' });
                                jMsg.animate({ height: "hide" }, 300, function () {
                                    $("#logomenu").animate({ top: "0" }, 300);
                                    $("body .sidebar").animate({ top: "0" }, 300);
                                    $("#container").animate({ top: "0" }, 300);
                                });
                            });
                            jMsg.append('<span>' + sMsg + '</span>').append(jMsgClose);
                            $("body").prepend(jMsg);
                        }
                    });
                }

                //Logomenu
                var data = {
                    AuthMember: (global_auth_member) ? global_auth_member : null
                };
                $("#logomenu").append(i18n.translate(fsTemplates.tLogoMenu(data)));                

                global_format = window.getComputedStyle($("#logomenu #tile-12.tile")[0], ":before").getPropertyValue("content").replace(/'|"/g, "");

                fittingContent();
                setTooltips($("#logomenu"), "menu");

                global_route = new Route(path);

                global_keyBindings.add("m", i18n.get("key-open-menu"), openSidebar);
                global_keyBindings.add("esc", i18n.get("key-close-menu"), closeSidebar);

                setTimeout(function() {
                    setAppInfo();
                }, 1000);

            });
        });

        swipedetect($("#sidebarTouchPanel")[0], 20, 40, 600,
            function (swipedir) {
                if (swipedir === 'right') {
                    openSidebar();
                    $("#sidebarTouchPanel").hide();
                }
            }
        );

        $(window).on('resize', function () {
            setAppInfo();

            fittingContent();

            //Sidebar
            fittingSidebar();
            fittingSidebarBay();
        });

        $(window).on('scroll', function () {

            if ($(this).scrollTop() > $("#content").offset().top === true) {
                $("#logomenu .tile-moveup i").fadeIn();
            } else {
                $("#logomenu .tile-moveup i").fadeOut();
            }

        });

    });
}
// =============================================
function connectToHub() {
     
    var chat = $.connection.notificationHub; // Declare a proxy to reference the hub    
    chat.client.showNotification = function (pushNotification) { // Create a function that the hub can call to broadcast messages

        // > NotificationHub.PushNotification

        var notification = new Notification(pushNotification);

        if (notification.Counts === true) {
            fillUnreadNotifications();
        }
        toastMessageText(notification.TypeCaption);
    };

    // Start the connection.
    $.connection.hub.start();

    //$.connection.hub.disconnected(function () {
    //    setTimeout(function () {
    //        $.connection.hub.start();
    //    }, 5000); // Restart connection after 5 seconds.
    //});
}
// =============================================
function KeyBindings() {

    this.Collection = [];

    this.clear = function() {
        this.Collection = [];
        $(".keycollection").empty();
    };

    this.add = function (key, desc, func) {

        Mousetrap.bind(key, func, 'keyup');

        var newItem = { "key": key, "desc": desc };
        this.Collection.push(newItem);
    };

    this.showCollection = function () {
        $.each(this.Collection, function (index, value) {
            $(".keycollection").append("<span class='keycap key-" + value.key + "'>" + value.desc + "</span>");
        });
    };
}
// =============================================
function setTitle(pageValue) {
    document.title = "FOTOSTEAM | " + pageValue;
}
function appendToTitle(value) {
    document.title = document.title + " - " + value;
}
// ---------------------------------------------
function setLanguage(lang) {
    global_lang = lang;
}
// ---------------------------------------------
function changeLanguage(lang) {
    $.cookie("fotosteam-language", lang, { path: '/' });
    setLanguage(lang);
    window.location.reload();
}
// ---------------------------------------------
function setAppInfo() {
    global_format = window.getComputedStyle($("#logomenu #tile-12.tile")[0], ":before").getPropertyValue("content").replace(/'|"/g, "");

    setTimeout(function() {        
        var formatCaption;
        switch (global_format) {
            case "XL": formatCaption = "X-LARGE"; break;
            case "L": formatCaption = "LARGE"; break;
            case "M": formatCaption = "MEDIUM"; break;
            default: formatCaption = "SMALL";
        }
        var appInfoLink = $('<a href="#"></a>');
        appInfoLink
            .html("Changelog, v" + global_config.version() + ", " + formatCaption)
            .click(function () {
                getChangelogContent(function(content) {
                    popoutContent("changelog", content);
                    $("#content-popout .popout-content-inner").mCustomScrollbar({
                        theme: "light",
                        scrollInertia: 200,
                        autoHideScrollbar: true
                    });
                    return false;
                });
                return false;
            }
        );
        $("footer #moreinfo-app").empty().append(appInfoLink);
    }, 100);
}
// ---------------------------------------------
function getChangelogContent(fSuccess) {
    $.getJSON("/changelog.json", function (json) {
        var wrapper = $('<div class="changelog"></div>');

        $.each(json, function () {
            wrapper.append("<h4>v" + this.Version + "</h4>");
            var list = $('<ul class="outer"></ul>');

            if (this.Fixed.length !== 0) {
                var fixes = $("<li>Fixed</li>"),
                    fixesList = $('<ul class="inner"></ul>');
                $.each(this.Fixed, function (index, value) {
                    var aValue = value.split("|"),
                        valueText = aValue[0],
                        valueLink = (aValue[1]) ? '<br /><a href="' + aValue[1] + '" target="_blank">' + aValue[1] + '</a>' : '';
                    fixesList.append("<li>" + valueText + valueLink + "</li>");
                });
                fixes.append(fixesList);
                list.append(fixes);
            }

            if (this.New !== 0) {
                var news = $("<li>New</li>"),
                    newsList = $('<ul class="inner"></ul>');
                $.each(this.New, function (index, value) {
                    var aValue = value.split("|"),
                        valueText = aValue[0],
                        valueLink = (aValue[1]) ? '<br /><a href="' + aValue[1] + '" target="_blank">' + aValue[1] + '</a>' : '';
                    newsList.append("<li>" + valueText + valueLink + "</li>");
                });
                news.append(newsList);
                list.append(news);
            }

            wrapper.append(list);
        });

        fSuccess(wrapper.get(0).outerHTML);
    });
}
// ---------------------------------------------
function initSidebar() {

    $(".input input[type='text']").Inputs();
    $(".input input[type='email']").Inputs();
    $(".input textarea").Inputs();

    $(".cmd-bay").each(function () {
        var bay = $(this).data("bay");
        $(this).click(function () {
            showSidebarBay(bay);
            return false;
        });
    });
    $(".lang-" + global_lang).addClass("lang-current");

    setTooltips($("#sidebar"), "bottom");

    fillUnreadNotifications();

    fittingSidebar();
}
// ---------------------------------------------
function fillUnreadNotifications() {

    //TODO: Switch to ALL-NOTIFICATIONS

    if (global_auth_member) {
        getSteamData("unread-notifications", { skip: 0, take: 100 }, function (result) {
            var sNotif, jNotif;
            $("#sidebar-notifications").empty();

            if (result.Data.length === 0) {
                var data = { };
                sNotif = i18n.translate(fsTemplates.tNotifications(data));
                jNotif = $(sNotif);
                $("#sidebar-notifications").append(jNotif);
                $("#dismiss-all-notifications").hide();
            } else {
                //COUNTER
                var jE = $('<a class="tooltip-menu" title="' + i18n.get("notifications") + '" href="#">' + result.Data.length + '</a>');
                jE.click(function() {
                    openSidebarBay("notifications");
                    return false;
                });
                $("#tile-32").empty().append(jE);
                setTooltips($("#logomenu"), "menu");

                // UNREAD NOTIFICATION
                var notif = new NotificationGroups(result.Data);
                $.each(notif, function(index, value) {                    
                    if (value.Notifications.length === 1) {
                        sNotif = i18n.translate(fsTemplates.tNotificationUnread(value.Notifications[0]));
                        jNotif = $(sNotif);
                        jNotif.find("a.dismiss").click(function() {
                            var jG = $(this).parent().find("a.go"),
                                id = jG.data("id");
                            dismissUnreadNotification(id, function() {
                                jG.parents("li").slideUp(function() { jG.remove(); });
                            });
                            return false;
                        });
                    } else {
                        jNotif = $(i18n.translate(fsTemplates.tNotificationUnreadGroup(value)));
                        jNotif.find("div.toggle").click(function() {
                            $(this).find('.items').slideToggle();
                            return false;
                        });
                        jNotif.find("a.dismiss").click(function() {
                            var jGs = $(this).parent().find("a.go").each(function() {
                                var jG = $(this),
                                    id = jG.data("id");
                                dismissUnreadNotification(id, function() {
                                    jG.parents("li").slideUp(function() { jG.remove(); });
                                });
                            });
                            return false;
                        });
                    }
                    jNotif.find("a.go").click(function() {
                        var id = $(this).data("id"),
                            url = $(this).data("url");
                        dismissUnreadNotification(id, function() { window.location.href = url; });
                        return false;
                    });
                    $("#sidebar-notifications").append(jNotif);
                });

                $("#dismiss-all-notifications").click(function() {
                    $("#sidebar-notifications li").each(function() {
                        var jG = $(this).find("a.go"),
                            id = jG.data("id");
                        dismissUnreadNotification(id, function () {
                            jG.parents("li").slideUp(function () { jG.remove(); });
                        });
                    });
                });
            }
        });
    }
}
// ---------------------------------------------
function dismissUnreadNotification(id, fSuccess) {
    updateModel("Notification", id, "IsRead", true, null, function () {

        //Counter und Liste zurücksetzen
        var jCounter = $("#tile-32").find("a"),
            count = parseInt(jCounter.html());

        jCounter.html(count - 1);

        if (count === 1) { //der letzte
            jCounter.remove();
            var data = {},
                jNotif = $(i18n.translate(fsTemplates.tNotifications(data)));
            $("#sidebar-notifications").empty().append(jNotif);
            $("#dismiss-all-notifications").hide();
        }

        if (fSuccess !== undefined) { fSuccess(); }
    });    
}
// ---------------------------------------------
function fittingContent() {
    $("#content").css("min-height", Math.ceil($(window).height() - ($("header").height() + $("footer").height() + 10)) + "px");
}
// ---------------------------------------------
function fittingSidebar() {
    var h = $(window).height();
    $(".sidebar-wrapper").height(h).mCustomScrollbar({
        theme: "dark",
        scrollInertia: 200,
        autoHideScrollbar: true
    });
}
// ---------------------------------------------
function openSidebar(fSuccess) {
    if (global_pageResets !== undefined) {
        global_pageResets();
    }
    $("body").addClass("sidebar-open");
    if (fSuccess !== undefined) { fSuccess(); }

    window.setTimeout(function () {
        $("header").click(function () { closeSidebar(); });
        $("#content").click(function () { closeSidebar(); });
    }, 300);
}
function closeSidebar() {
    closeSidebarBays();
    $("body").removeClass("sidebar-open");
    $("header").unbind("click");
    $("#content").unbind("click");
    $("#sidebarTouchPanel").show();
}
function toggleSidebar() {
    if ($("body").hasClass("sidebar-open")) {
        closeSidebar();
    } else {
        openSidebar();
    }
}
// ---------------------------------------------
function fittingSidebarBay() {
    var h = $(window).height(),
        ul = $(".sidebar-wrapper ul.sidebar-commands"),
        bay = $(".sidebar-bay.bay-open");

    if (bay.length > 0) {
        if (h > bay.height()) {
            bay.height(h - ul.position().top);
        }
        ul.height(bay.height() - 50);
    }
}
// ---------------------------------------------
function showSidebarBay(key) {
    closeSidebarBays();
    window.setTimeout(function () {
        var bay = $("#sidebar-bay-" + key);

        if (bay.length) {
            bay.addClass("bay-open");
        }

        fittingSidebarBay();

    }, 300);
}
// ---------------------------------------------
function closeSidebarBays() {
    $(".sidebar-bay").removeClass("bay-open");
    $(".sidebar-wrapper ul.sidebar-commands").css('height', 'auto');
}
// ---------------------------------------------
function openSidebarBay(key, fSuccess) {
    openSidebar(showSidebarBay(key));
    if (fSuccess !== undefined) { fSuccess(); }
}
// ---------------------------------------------
function setupMDDToolbar(container) {
    container.find("a#mdd_undo").prop("title", i18n.get("undo"));
    container.find("a#mdd_redo").prop("title", i18n.get("redo"));
    container.find("a#mdd_bold").prop("title", i18n.get("bold"));
    container.find("a#mdd_italic").prop("title", i18n.get("italic"));
    container.find("a#mdd_ullist").prop("title", i18n.get("bullets"));
    container.find("a#mdd_ollist").prop("title", i18n.get("numbering"));
}
// ---------------------------------------------
function setTooltips(container, type) {

    var options = {
        animation: 'fade',
        delay: 200,
        touchDevices: false,
        trigger: 'hover',
        position: type
    };

    //Specials
    switch (type) {
        case "menu":
            options.arrow = false;
            options.theme = 'tooltipster-menu';
            break;
        default:
    }

    container.find(".tooltip-" + type).tooltipster(options);
}
// ---------------------------------------------
function doFullsizable(jE, options) {
    var opts = $.extend({
        //detach_id: 'container',
        titleGoPrev: i18n.get("go-prev"),
        titleGoNext: i18n.get("go-next"),
        //titleClose: i18n.get("close"),
        titleFullscreen: i18n.get("full-screen"),
        titleDetail: i18n.get("foto"),
        titleSlideshowStart: i18n.get("slideshow-play"),
        titleSlideshowPause: i18n.get("slideshow-pause")
    }, options || {});
    jE.fullsizable(opts);
}
// ---------------------------------------------
function scrollUp() {
    if (global_pageResets !== undefined) {
        global_pageResets();
    }
    $.scrollTo(0, 500);
}
// ---------------------------------------------
function sharingHover(e, text) {
    var jE = $(e),
        jP = jE.parent().find(".sharing-platform");
    jP.text(text);
}
// ---------------------------------------------
function tweetForInviteCode() {
    var shareUrl =
        "https://" + "twitter.com/share?" +
        "&text=" + encodeURIComponent(i18n.get("invitecode-tweet") + " :") +
        "&url=http%3A%2F%2Ffotosteam.com";
    sharingPopup(shareUrl, 620, 260);
}
// ---------------------------------------------
function sharingClick(name, platform) {
    var shareUrl = "",
        w = 0,
        h = 0,
        jF = $("#figure-" + name),
        url = encodeURIComponent(jF.data("share-image-url")),
        title = jF.data("title");

    switch (platform) {
        case "gplus":
            shareUrl = "https://" + "plus.google.com/share?url=" + url;
            w = 510; h = 650;
            break;
        case "facebook":
            shareUrl = "http://" + "www.facebook.com/sharer.php?u=" + url;
            w = 600; h = 535;
            break;
        case "twitter":
            shareUrl = "https://" + "twitter.com/share?url=" + url + "&text=" + title + "&via=Fotosteam&hashtags=#fotosteam";
            w = 620; h = 260;
            break;
        case "pinterest":
            shareUrl = "https://" + "pinterest.com/pin/create/bookmarklet/?media=" + url + "&url=" + url + "&is_video=false&description=" + title;
            w = 770; h = 320;
            break;
        case "tumblr":
            shareUrl = "http://" + "www.tumblr.com/share/link?url=" + url + "&name=" + title + "&description=Fotosteam Image";
            w = 450; h = 430;
            break;
        default:
    }
    if (shareUrl.length !== 0) {
        sharingPopup(shareUrl, w, h);
    }
}
// ---------------------------------------------
function sharingPopup(shareUrl, w, h) {
    var px = Math.floor(((screen.availWidth || 1024) - w) / 2),
        py = Math.floor(((screen.availHeight || 700) - h) / 2);

    var popup = window.open(shareUrl, "social",
            "width=" + w + "," +
            "height=" + h + "," +
            "left=" + px + "," +
            "top=" + py + "," +
            "location=0,menubar=0,toolbar=0,status=0,scrollbars=0,resizable=1");

    if (popup) {
        popup.focus();
    }
}
// ---------------------------------------------
function getBackgroundImage(id) {
    var img = document.getElementById(id),
    style = img.currentStyle || window.getComputedStyle(img, false),
    bi = style.backgroundImage.slice(4, -1);
}
// ---------------------------------------------
function loadComments(foto) {
    var d = $.Deferred();

    getSteamData("comments", { id: foto.Id, skip: 0, take: 100 }, function (resC) {
        var ownedByAuthMember = (global_auth_member) ? (global_auth_member.Id === foto.MemberId) : false;
        var dataC = {
            Comments: new Comments(resC.Data, ownedByAuthMember)
        },
        jComLst = $(i18n.translate(fsTemplates.tCommentList(dataC)));
        processComment(jComLst, ownedByAuthMember);

        var dataN = {
            Foto: foto,
            AuthMember: (global_auth_member) ? global_auth_member : null,
        },
        jComNew = $(i18n.translate(fsTemplates.tCommentNew(dataN)));
        jComNew.find("a").click(function () {
            openSidebarBay('login');
            return false;
        });

        $(".foto-comments").append(jComLst);

        if (global_member.Options.AllowComments && foto.AllowCommenting) {
            $(".foto-comments").append(jComNew);
        }

        d.resolve();
    });
    return d.promise();
}
// ---------------------------------------------
function processComment(jE, ownedByAuthMember) {
    if (ownedByAuthMember) {
        jE.find(".comment-delete").click(function () {
            var id = $(this).data("id");

            executeSteamAction("delete-comment", { Id: id }, null,
                function (result) {
                    if (result.Data === true) {
                        $("#comment-" + id).slideUp(function () {
                            $(this).remove();
                        });
                    }
                },
                function (result) { toastErrorText(result, "comment-delete-error"); }
            );
        });
    }
}
// ---------------------------------------------
function cancelAddComment(e) {
    var elm = $(e),
        nwrap = elm.parents(".new-comment"),
        text = nwrap.find(".new-comment-text");

    text.html("");
    nwrap.removeClass("add");
}
// ---------------------------------------------
function sendComment(e, id) {
    var elm = $(e),
        nwrap = elm.parents(".new-comment"),
        cwrap = nwrap.siblings(".comment-wrapper"),
        jText = nwrap.find(".new-comment-text"),
        sText = convertContentEditableText(jText);

    var model = {
        "UserName": global_auth_member.PlainName,
        "UserAlias": global_auth_member.Steam,
        "UserAvatarLink": (global_auth_member.IsRegistered === true) ? global_auth_member.Avatar100Url : null,
        "Text": sText,
        "PhotoId": id,
        "Date": moment().format("DD.MM.YYYY HH:mm:ss")
        //TODO: "ParentCommentId": 0
        //TODO: "TotalCount": 0 ... was ist das?
    };

    executeSteamAction("new-comment", null, model,
        function (result) {
            var ownedByAuthMember = (global_auth_member) ? (global_auth_member.Id === global_member.Id) : false;

            var sComment = i18n.translate(Handlebars.partials.Comment(
                        new Comment(result.Data, ownedByAuthMember))),
                jComment = $(sComment);

            processComment(jComment, ownedByAuthMember);

            var xwrap = cwrap.find(".mCSB_container");
            if (xwrap.length !== 0) { //Wrapper mit mCustomScrollbar
                xwrap.append(jComment);
                cwrap.mCustomScrollbar("scrollTo", "bottom");
            } else {
                cwrap.append(jComment);
            }
            cancelAddComment(e);
        },
        function (result) { toastErrorText(result, "your-comment-error"); }
    );
}
// ---------------------------------------------
function rateFoto(e, name, id, rating) {
    var jE = $(e),
        jR = $("#rating-popout-" + name),
        jF = $("#figure-" + name);

    var model = {
        "UserName": global_auth_member.PlainName,
        "UserAlias": global_auth_member.Steam,
        "UserAvatarLink": (global_auth_member.IsRegistered === true) ? global_auth_member.Avatar100Url : null,
        "Value": rating,
        "PhotoId": id
    };

    executeSteamAction("new-rating", null, model,
        function (result) {

            jE.addClass("rated");
            jF.find(".btn-rating span").text(result.Data.RatingSum);

            jR.find("a").prop("onclick", null).unbind().bind('click', function () {
                return false;
            });
        },
        function (result) {
            toastErrorText(result, "rating-error");
        }
    );
}
// ---------------------------------------------
function gmapOptions(lat, lng, scroll, zoom) {
    var options = {
        mapTypeId: "terrain", //google.maps.MapTypeId.TERRAIN,
        center: [lat, lng],
        zoom: zoom,
        zoomControl: false,
        mapTypeControl: false,
        navigationControl: false,
        streetViewControl: false,
        overviewMapControl: false,
        rotateControl: false,
        scaleControl: false,
        scrollwheel: scroll
    };
    return options;
}
// ---------------------------------------------
function loadMap(jE, lat, lng, scroll, zoom) {
    if (zoom === undefined) { zoom = 10; }
    if (global_online) {
        jE.gmap3({
            map: { options: gmapOptions(lat, lng, scroll, zoom) },
            marker: { latLng: [lat, lng] }
        });
    }
}
// ---------------------------------------------
function loadWorldMap(jE) {
    if (global_online) {
        jE.gmap3({
            map: { options: gmapOptions(0, 0, false, 2) }
        }).css("opacity", "0.5");
    }
}
// ---------------------------------------------
actionStatusCode = {
    Success: 0,
    Failure: 1,
    Timeout: 2,
    UnknownAction: 3,
    InternalException: 4,
    NoStorageAccessDefined: 5,
    NotAuthorized: 6,
    NoData: 7,
    CannotDeserializer: 8,
    NotValidEntity: 9,
    UserHasAlreadyRated: 11
};
// ---------------------------------------------
function getErrorLocale(code) {
    switch (code) {
        case actionStatusCode.Failure: return "action-statuscode-failure";
        case actionStatusCode.Timeout: return "action-statuscode-timeout";
        case actionStatusCode.UnknownAction: return "action-statuscode-unknown-action";
        case actionStatusCode.InternalException: return "action-statuscode-internal-exception";
        case actionStatusCode.NoStorageAccessDefined: return "action-statuscode-no-storage";
        case actionStatusCode.NotAuthorized: return "action-statuscode-not-authorized";
        case actionStatusCode.NoData: return "action-statuscode-no-data";
        case actionStatusCode.CannotDeserializer: return "action-statuscode-cannot-deserializer";
        case actionStatusCode.NotValidEntity: return "action-statuscode-not-valid-entity";
        case actionStatusCode.UserHasAlreadyRated: return "action-statuscode-user-has-already-rated";
        default: return "";
    }
}
// ---------------------------------------------
function updateModel(modelType, modelId, fieldName, fieldValue, fieldKey, fUpdated) {
    var model = {
        "Id": modelId,
        "Type": modelType,
        "PropertyName": fieldName,
        "Value": fieldValue
    };
    executeSteamAction("update-field", null, model,
        function () {
            if (fieldKey) {
                toastMessageText(i18n.get("field-update-message").replace("%FIELD%", i18n.get(fieldKey)));
            }
            if (fUpdated !== undefined) { fUpdated(); }
        },
        function (result) {
            toastErrorTextAndTitle(result, i18n.get("field-update-error").replace("%FIELD%", "'" + i18n.get(fieldKey) + "'"));
        }
    );
}
// ---------------------------------------------
function executeSteamAction(sAction, jParam, jData, fSuccess, fError) {
    var d = $.Deferred();

    var path = null,
        type = null,
        errorKey = null;

    switch (sAction) {
        case "delete-account": type = "DELETE"; path = "/authorize/remove"; errorKey = "delete-account-error"; break;
        case "register-external": type = "POST"; path = "/authorize/RegisterExternal"; errorKey = "authentication-error"; break;
        case "update-field": type = "POST"; path = "/data/update"; errorKey = "field-update-error"; break;
        case "multi-update": type = "PUT"; path = "/data/multiupdate"; errorKey = "multiupdate-error"; break;
        case "create-homelocation": type = "POST"; path = "/data/location"; errorKey = "create-homelocation-error"; break;
        case "save-homelocation": type = "PUT"; path = "/data/location"; errorKey = "save-homelocation-error"; break;
        case "new-topic": type = "POST"; path = "/data/topic"; errorKey = "topic-new-error"; break;
        case "delete-topic": type = "DELETE"; path = "/data/topic/" + jParam.Id; errorKey = "topic-delete-error"; break;
        case "save-location": type = "PUT"; path = "/data/location"; errorKey = "location-save-error"; break;
        case "new-location": type = "POST"; path = "/data/location"; errorKey = "location-new-error"; break;
        case "merge-location": type = "POST"; path = "/data/mergelocations"; errorKey = "location-merge-error"; break;
        case "delete-location": type = "DELETE"; path = "/data/location/" + jParam.Id; errorKey = "location-delete-error"; break;
        case "new-event": type = "POST"; path = "/data/event"; errorKey = "event-new-error"; break;
        case "delete-event": type = "DELETE"; path = "/data/event/" + jParam.Id; errorKey = "event-delete-error"; break;
        case "new-comment": type = "POST"; path = "/communication/addComment"; errorKey = ""; break;
        case "new-rating": type = "POST"; path = "/communication/addRating"; errorKey = ""; break;
        case "save-social-media": type = "PUT"; path = "/data/socialmedia"; errorKey = "social-media-error"; break;
        case "new-social-media": type = "POST"; path = "/data/socialmedia"; errorKey = "social-media-error"; break;
        case "delete-social-media": type = "DELETE"; path = "/data/socialmedia/" + jParam.Id; errorKey = "social-media-delete-error"; break;
        case "color-reset": type = "PUT"; path = "/data/ColorReset"; errorKey = "foto-reset-color-error"; break;
        case "delete-photo": type = "DELETE"; path = "/data/photo/" + jParam.Id; errorKey = "foto-delete-error"; break;
        case "send-error": type = "POST"; path = "/communication/trello"; errorKey = "error-contact-error"; break;
        case "send-mail": type = "POST"; path = "/communication/contact"; errorKey = "contact-error"; break;
        case "send-trello": type = "POST"; path = "/communication/trello"; errorKey = "contact-error"; break;
        case "edit-phototopic": type = "POST"; path = "/data/phototopic"; errorKey = "phototopic-error"; break;
        case "header-color-reset": type = "PUT"; path = "/data/colorresetheader"; errorKey = "header-reset-color-error"; break;
        case "request-buddy": type = "POST"; path = "/data/addbuddy"; errorKey = ""; break;
        case "confirm-buddy": type = "POST"; path = "/data/confirmbuddy"; errorKey = ""; break;
        case "delete-comment": type = "DELETE"; path = "/data/comment/" + jParam.Id; errorKey = "comment-delete-error"; break;        

        //case "": type = ""; path = ""; errorKey = ""; break;
    }

    try {
        $.ajax({
            type: type,
            url: global_apipath + path,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: (jData) ? JSON.stringify(jData) : "{}",
            success: function (result) {

                try {
                    if (result.Status.Code === actionStatusCode.Success) {
                        fSuccess(result);
                    } else {
                        var errorLocale = getErrorLocale(result.Status.Code);
                        if (errorLocale) {
                            throw errorLocale;
                        } else {
                            throw errorKey;
                        }
                    }
                } catch (e) { fError(i18n.get(e)); }
            },
            error: function (result) {
                fError(JSON.parse(result.responseText).Message);
            }
        });
    } catch (err) {
        fError(i18n.get(errorKey));
    }

    return d.promise();
}
// ---------------------------------------------
function getSteamData(sAction, jParam, fSuccess) {
    var d = $.Deferred();

    var path = null;

    switch (sAction) {
        case "memberinfo": path = "/account/memberinfo"; break;
        case "member": path = "/account/member/" + jParam; break;
        case "memberbyid": path = "/account/memberbyid/" + jParam; break;
        case "member-invitecodes": path = "/account/invitecodes"; break;
        case "members": path = "/account/members"; break;
        case "members-random": path = "/account/membersrandom/" + jParam; break;
        case "buddies": path = "/account/buddies/" + jParam; break;
        case "categories": path = "/data/" + jParam + "/categories"; break;
        case "topics": path = "/data/" + jParam + "/topics"; break;
        case "locations": path = "/data/" + jParam + "/locations"; break;
        case "locationGroupsByCity": path = "/data/" + jParam + "/locationgroups/city"; break;
        case "events": path = "/data/" + jParam + "/events"; break;
        case "stories": path = "/data/" + jParam + "/stories"; break;
        case "fotos": path = "/data/" + jParam + "/photos"; break;
        case "comments": path = "/data/commentsForPhoto/" + jParam.id + "/" + jParam.skip + "/" + jParam.take; break;
        case "fotos-new": path = "/data/all/newphotos/" + jParam.skip + "/" + jParam.take; break;
        case "fotos-top": path = "/data/all/topratedphotos/" + jParam.skip + "/" + jParam.take; break;
        case "fotos-mytop": path = "/data/all/mytopratedphotos/" + jParam.skip + "/" + jParam.take; break;
        case "fotos-member-random": path = "/data/" + jParam.alias + "/randomphotos/" + jParam.take; break;
        case "fotos-member-toprated": path = "/data/" + jParam.alias + "/topratedphotos/0/" + jParam.take; break;
        case "unread-notifications": path = "/data/all/unreadnotifications/" + jParam.skip + "/" + jParam.take; break;
        case "all-notifications": path = "/data/all/allnotifications/" + jParam.skip + "/" + jParam.take; break;
        case "ratings": path = "/data/ratingsforphoto/" + jParam.id + "/" + jParam.skip + "/" + jParam.take; break;
        case "fotos-cc0": path = "/data/all/cc0/" + jParam.skip + "/" + jParam.take; break;

        //case "": path = ""; break;
        default:
    }

    try {
        $.getJSON(global_apipath + path, function (json) {
            fSuccess(json);
            d.resolve();
        });
    } catch (e) {
        toastErrorText(i18n.get("loading-data-error").replace("%1", action), "error");
    }

    return d.promise();
}
