
// =============================================
function Common(backImagePath, resources) {
    var that = this;

    this.Background = null;
    this.BackgroundImagePath = null;

    // ---------------------------------------------
    this.Init = function() {
       
        //Body
        $("body").addClass("common");

        //Header
        $("header").empty().append(
            '<h2 class="title">FOTOSTEAM<p>' + i18n.get('claim') + '</p></h2>'
        );

        $.when(
            promiseBackground(),
            promiseResources()
        ).then(function() {
            that.SetPage();
        });

        //------------------------------------------
        function promiseBackground() {
            var d = $.Deferred();
            if (backImagePath) {
                that.BackgroundImagePath = backImagePath;
                that.Background = null;
                d.resolve();
            } else {
                $.getJSON("/Content/Background/background_4.json", function(bdata) {
                    var b = randomNumber(1, bdata.length);
                    that.BackgroundImagePath = "/Content/Background/back" + pad(b, 2);

                    var back = Enumerable.From(bdata).Where(function (x) { return x.Id === b; }).FirstOrDefault();
                    that.Background = new CommonBackground(back);

                    d.resolve();
                });
            }
            return d.promise();
        }

        //------------------------------------------
        function promiseResources() {
            var d = $.Deferred();
            resourceLoader(resources, function () {
                d.resolve();
            });
            return d.promise();
        }
    };

    // ---------------------------------------------
    this.SetPage = function() {

        //Background-Image
        $("head").append(
            '<style type="text/css">' +
                '@media only screen { body.common { background-image: url("' + this.BackgroundImagePath + '/640.jpg"); }}' +
                '@media only screen and (min-width: 40.063em) { body.common { background-image: url("' + this.BackgroundImagePath + '/1024.jpg"); }}' +
                '@media only screen and (min-width: 64.063em) { body.common { background-image: url("' + this.BackgroundImagePath + '/1440.jpg"); }}' +
                '@media only screen and (min-width: 90.063em) { body.common { background-image: url("' + this.BackgroundImagePath + '/1920.jpg"); }}' +
            '</style>'
        );

        //Footer
        var dataF = { Background: that.Background };
        $("footer").append(i18n.translate(fsTemplates.tFooter(dataF)));

        //Sidebar
        var data = {
            AuthMember: (global_auth_member) ? global_auth_member : null,
            Registered: (global_auth_member && global_auth_member.IsRegistered),
            ShowMy: true
        };
        $("nav.sidebar").append(i18n.translate(fsTemplates.tSidebarCommon(data)));

        initSidebar();

        //Resize-Binding
        //$(window).on('resize', function () {
        //    
        //});

        //Scroll-Binding
        var lastScrollTop = 0;
        $(window).on('scroll', function() {

            //Logomenu
            if (global_format == "S") {
                if ($(this).scrollTop() > lastScrollTop) {
                    $("#logomenu").css("opacity", "0.5");
                } else {
                    $("#logomenu").css("opacity", "1");
                }
                lastScrollTop = $(this).scrollTop();
            }

        });

        //Key-Bindings
        global_keyBindings.showCollection();
    };

    // ---------------------------------------------
    that.Init();
}
// =============================================
function Start(fSuccess) {

    var resources = [];

    new Common(null, resources);

    var data = {
            AuthMember: global_auth_member
        },
        sData = i18n.translate(fsTemplates["tStart-" + global_lang.toUpperCase()](data)),
        jData = $(sData);

    jData.find(".feature")
        .click(function() {
            showStartScreenshot($(this));
            return false;
        })
        .mouseenter(function () {
            showStartScreenshot($(this));
        });

    var jTI = jData.find("#tweet-invite-code");
    if (jTI.length !== 0) {
        var sTI = jTI.html(),
            aTI = '<a href="#">#fotosteam</a>';
        sTI = sTI.replace("#fotosteam", aTI);
        jTI.html(sTI);
        //--
        jTI.find("a").css("color", "#005599").click(function () {
            tweetForInviteCode();
            return false;
        });
    }    
    
    $("#content").empty().append(jData);

    showFotosNew();
    showFotosTop();
    showMembersRandom(5);

    $("#start-member h1").append('<a href="#" onclick="showMembersRandom(10); return false;" class="icon icon-arrows-cw"></a>');

    setTimeout(function () {
        $("#start-intro").height($("#start-intro-text").outerHeight(true));
    }, 250);

    $(window).on('resize', function () {
        $("#start-intro").height($("#start-intro-text").outerHeight(true));
    });

    fittingContent();

    if (fSuccess !== undefined) { fSuccess(); }

}
// ---------------------------------------------
function getFotoGridDimension() {
    switch (global_format) {
        case "XL": // 16,6% > 6 
            return 6; 
        case "L": // 20% > 5
            return 5; 
        case "M": // 25% > 4
            return 4; 
        default: // 33,3% > 3
            return 3;
    }
}
// ---------------------------------------------
function showFotosNew() {

    setTimeout(function () {
        var wrap = $("#start-fotos-new-wrapper"),
            page = parseInt(wrap.data("page")) + 1,
            itemsRow = getFotoGridDimension(),
            itemsGrid;

        itemsGrid = (itemsRow * itemsRow);

        if (page === 1) { itemsGrid -= 1; }

        getSteamData("fotos-new", { skip: ((page - 1) * itemsGrid), take: itemsGrid }, function (resNF) {
            var newFotos = new MemberFotos(resNF.Data);
            $.each(newFotos, function (index, foto) {

                foto.SteamPath = window.location.origin + "/" + foto.Alias;

                var data = {
                    Foto: foto
                },
                jFoto = $(i18n.translate(fsTemplates.tCubicle(data)));

                jFoto.find(".details").css("background-color", "rgba(" + foto.Color + ", 0.4)");
                jFoto.find(".title").css("background-color", "rgba(" + foto.Color + ", 1)");
               
                $("#fotos-new-more").before(jFoto);
            });

            doFullsizable(wrap.find("a.btn-show"), {
                detailButton: true,
                avatarButton: true
            });

            wrap.data("page", page);
            trimFotosToGrid($("#start-fotos-new-container"), $("#start-fotos-new-wrapper"), function () { getFotoGridDimension(); });
        });

        $(window).on('resize', function () {
            trimFotosToGrid($("#start-fotos-new-container"), $("#start-fotos-new-wrapper"), function () { getFotoGridDimension(); });
        });

    }, 10);

}
// ---------------------------------------------
function showFotosTop() {

    setTimeout(function() {
        var wrap = $("#start-fotos-top-wrapper"),
            page = parseInt(wrap.data("page")) + 1,
            itemsRow = getFotoGridDimension(),
            itemsGrid;

        itemsGrid = (itemsRow * itemsRow);

        if (page === 1) { itemsGrid -= 1; }

        getSteamData("fotos-top", { skip: ((page - 1) * itemsGrid), take: itemsGrid }, function (resTF) {
            var topFotos = new MemberFotos(resTF.Data);

            $.each(topFotos, function (index, foto) {

                foto.SteamPath = window.location.origin + "/" + foto.Alias;

                var data = {
                    Foto: foto
                },
                jFoto = $(i18n.translate(fsTemplates.tCubicle(data)));

                jFoto.find(".details").css("background-color", "rgba(" + foto.Color + ", 0.4)");
                jFoto.find(".title").css("background-color", "rgba(" + foto.Color + ", 1)");

                $("#fotos-top-more").before(jFoto);
                trimFotosToGrid($("#start-fotos-top-container"), $("#start-fotos-top-wrapper"), function () { getFotoGridDimension(); });                

            });

            doFullsizable(wrap.find("a.btn-show"), {
                detailButton: true,
                avatarButton: true
            });

            wrap.data("page", page);
        });

        $(window).on('resize', function () {
            trimFotosToGrid($("#start-fotos-top-container"), $("#start-fotos-top-wrapper"), function () { getFotoGridDimension(); });
        });

    }, 10);
}
// ---------------------------------------------
function showMembersRandom(itemsRow) {

    var take = itemsRow * 2;

    getSteamData("members-random", take, function (resM) {
        var members = new Members(resM.Data),
            foto = {};

        $("#start-member-wrapper").empty();

        $.each(members, function (index, member) {

            foto.Name = member.Steam;
            foto.Caption = member.PlainName;
            foto.Url = member.SteamPath;
            foto.CubicleUrl = member.Avatar200Url;
            foto.DummyUrl = "/Content/Images/dummy200.png";

            var data = {
                Foto: foto
            },
            jMember = $(i18n.translate(fsTemplates.tCubicle(data)));

            jMember.find(".details").css("background-color", "rgba(" + member.AvatarColor + ", 0.4)");
            jMember.find(".title").css("background-color", "rgba(" + member.AvatarColor + ", 1)");

            $("#start-member-wrapper").append(jMember);
        });

        $(window).on('resize', function () {
            trimFotosToGrid($("#start-member-container"), $("#start-member-wrapper"), function() { return itemsRow; });
        });

    });
}
// ---------------------------------------------
function showStartScreenshot(jE) {

    document.getElementById('slide_' + jE.data("part")).checked = true;

    $(".feature-current").removeClass("feature-current");
    jE.addClass("feature-current");
}
// ---------------------------------------------
function showHowTo(e) {
    $("#start-howto").slideDown();
    $.scrollTo("#start-howto", 500);
    $(e).slideUp(function () { $(this).remove(); });
}
// =============================================
function LegalInfo() {
    setTitle(i18n.get("legalinfo"));

    new Common(null);

    var data = { langDE: (global_lang.toUpperCase() !== 'DE') },
        sData = fsTemplates.tLegalInfo(data),
        jData = $(sData);

    $("#content").empty().append(jData);

    fittingContent();
}
// =============================================
function FSError(number, route) {
    setTitle(i18n.get("error"));

   // Sollten unerwartete 404-Fehler auftauchen, dann BrowserLink in VS überprüfen!

    new Common("/Content/Background/error");

    var data = {
        ErrorNo: number,
        Route: route,
        Member: (global_member) ? global_member : null,
        Subject: i18n.get("error-contact-default").replace("%1", number), //Hack, weil i18n-Value nicht funktioniert
        SenderName: (global_auth_member) ? global_auth_member.PlainName : "",
        SenderMail: (global_auth_member) ? global_auth_member.Email : ""
    },
    sData = i18n.translate(fsTemplates.tError(data)),
    jData = $(sData);

    $("#content").empty().append(jData);

    //INPUTS
    $(".input input[type='text']").Inputs();
    $(".input input[type='email']").Inputs();
    $(".input textarea").Inputs();

    //IMG-Select
    $("#err-who-failed img").each(function() {
        $(this).click(function() {
            $("#err-who-failed img").removeClass("selected");
            $(this).addClass("selected");
            $("#WhoFailed").val($(this).data("dev"));
        });
    });

    $("#err-contact-form").validate({
        rules: {
            ErrSubject: { required: true },
            ErrSenderName: { required: true },
            ErrSenderMail: {
                required: true, 
                email: true
            }
        },
        messages: {
            ErrSubject: { required: i18n.get("contact-subject-required") },
            ErrSenderName: { required: i18n.get("your-name-required") },
            ErrSenderMail: {
                required: i18n.get("your-mail-address-required"),
                email: i18n.get("your-mail-address-invalid")
            }
        },
        errorPlacement: function($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });
    //--
    $("#err-contact-form").find("#ErrSubject, #ErrSenderName, #ErrSenderMail").change(function () {
        var $field = $(this);
        if (!$field.valid()) { 
            //nothing to do 
        }
    });

    $("#ErrContactSend").click(function() {
        if ($("#err-contact-form").valid()) {
            var body = $("#ErrMessage").val() + "\n\n" +
                       "Url: " + route.FullPath + "\n" +
                       "Who failed? " + $("#WhoFailed").val();

            var model = {
                Title: $("#ErrSubject").val(),
                Body: body,
                SenderName: $("#ErrSenderName").val(),
                SenderEMail: $("#ErrSenderMail").val()
            };

            executeSteamAction("send-error", null, model,
                function () {
                    $("#err-contact-form")
                        .after("<p>" + i18n.get("error-contact-success") + "</p>")
                        .slideUp(function() {
                            $(this).remove();
                        }
                    );
                },
                function(result) {
                    toastErrorText(result, "error-contact-error");
                }
            );
        }
        return false;
    });

    fittingContent();

    //Resize-Binding
    $(window).on('resize', function () {
        fittingContent();
    });

}
// =============================================
function sendContact(type) {
    
    $("#contact-form").validate({
        rules: {
            Subject: { required: true },
            Message: { required: true },
            SenderName: { required: true },
            SenderMail: {
                required: true,
                email: true
            }
        },
        messages: {
            Subject: { required: i18n.get("contact-subject-required") },
            Message: { required: i18n.get("contact-message-required") },
            SenderName: { required: i18n.get("your-name-required") },
            SenderMail: {
                required: i18n.get("your-mail-address-required"),
                email: i18n.get("your-mail-address-invalid")
            }
        },
        errorPlacement: function ($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });

    if ($("#contact-form").valid()) {
        var model = {
            Title: $("#Subject").val(),
            Body: $("#Message").val(),
            SenderName: $("#SenderName").val(),
            SenderEMail: $("#SenderMail").val()
        };

        executeSteamAction("send-" + type, null, model,
            function () {
                $("#contact-form")
                    .after('<p class="subtitle">' + i18n.get("contact-success") + '</p>')
                    .slideUp(function () {
                        $(this).remove();
                    }
                );
            },
            function (result) {
                toastErrorText(result, "contact-error");
            }
        );
        return false;
    }

}