
// =============================================
function Member(json) {
    var that = this;

    for (var property in json) {
        switch (property) {
            case "Alias":
                if (json[property] !== "NOT SET") {
                    this.IsRegistered = true;
                    this.Steam = json[property];
                    this.SteamPath = window.location.origin + "/" + json[property];
                } else {
                    this.IsRegistered = false;
                    this.Steam = "";
                    this.SteamPath = "";
                }
                break;
            case "Avatar100Url":
                that.Avatar100Url = (!json[property]) ? "/Content/Images/avatar-100.png" : json[property];
                break;
            case "Avatar200Url":
                that.Avatar200Url = (!json[property]) ? "/Content/Images/avatar-200.png" : json[property];
                break;
            case "AvatarColor":
                that.AvatarColor = (!json[property]) ? "34,34,34" : json[property];
                break;
            case "Header640Url":
                that.Header640Url = (!json[property]) ? "/Content/Background/error/640.jpg" : json[property];
                break;
            case "Header1024Url":
                that.Header1024Url = (!json[property]) ? "/Content/Background/error/1024.jpg" : json[property];
                break;
            case "Header1440Url":
                that.Header1440Url = (!json[property]) ? "/Content/Background/error/1440.jpg" : json[property];
                break;
            case "Header1920Url":
                that.Header1920Url = (!json[property]) ? "/Content/Background/error/1920.jpg" : json[property];
                break;
            case "HeaderColor":
                that.HeaderColor = (!json[property]) ? "34,34,34" : json[property];
                var arrHeaderColor = this.HeaderColor.split(",");
                this.HeaderColorR = arrHeaderColor[0];
                this.HeaderColorG = arrHeaderColor[1];
                this.HeaderColorB = arrHeaderColor[2];
                this.HeaderColorHex = "#" + rgbToHex(this.HeaderColorR, this.HeaderColorG, this.HeaderColorB);
                break;
            case "HomeLocation":
                if (json[property]) {
                    that.HomeLocation = new Location(json[property], that);
                } else {
                    that.HomeLocation = new Location({ Id: 0, Latitude: 0, Longitude: 0 });
                }
                break;
            case "SocialMedias":
                this.SocialMedias = new SocialMedias(json[property]);                
                break;
            case "Buddies":
                this.BuddyList = json[property]; //nicht direkt holen, um Schleife zu verhindern > getBuddies
                break;
            case "StorageAccessType":
                that.StorageAccessType = parseInt(json[property]);
                switch (that.StorageAccessType) {
                case 1:
                    that.StorageProviderKey = "dropbox";
                    that.StorageProviderCaption = "Dropbox";
                    break;
                case 2:
                    that.StorageProviderKey = "googledrive";
                    that.StorageProviderCaption = "Google Drive";
                    break;
                case 3:
                    that.StorageProviderKey = "onedrive";
                    that.StorageProviderCaption = "Microsoft OneDrive";
                    break;
                default:
                    that.StorageProviderKey = "";
                    that.StorageProviderCaption = "";
                }
                break;
            default:
                this[property] = json[property];
        }
    }
    // =============================================
    this.LoadDetailsAndRoute = function (fRoute) {

        $.when(
            getSteamData("categories", this.Steam, function (resC) {
                var lst = new Categories(resC.Data, that);
                that.Categories = Enumerable.From(lst)
                    .OrderByDescending(function (x) { return x.CategoryPhotoCount; }).ToArray();
                that.CategoriesUsed = Enumerable.From(lst)
                    .Where(function (x) { return x.CategoryPhotoCount > 0; })
                    .OrderByDescending(function (x) { return x.CategoryPhotoCount; }).ToArray();
            }),
            getSteamData("topics", this.Steam, function (resT) {
                var lst = new Topics(resT.Data, that);
                that.Topics = Enumerable.From(lst)
                    .Where(function (x) { return x.TopicName.toUpperCase() != "NOTSET"; })
                    .OrderByDescending(function (x) { return x.TopicPhotoCount; })
                    .ThenBy(function (x) { return x.TopicName; }).ToArray();
                that.TopicsUsed = Enumerable.From(lst)
                    .Where(function (x) { return x.TopicPhotoCount > 0; })
                    .OrderByDescending(function (x) { return x.TopicPhotoCount; })
                    .ThenBy(function (x) { return x.TopicName; }).ToArray();
            }),
            //getSteamData("locations", this.Steam, function (resL) {
            //    var lst = new Locations(resL.Data, that);
            //    that.Locations = Enumerable.From(lst)
            //        .Where(function (x) { return x.LocationName.toUpperCase() != "NOTSET"; })
            //        .OrderByDescending(function (x) { return x.LocationPhotoCount; })
            //        .ThenBy(function (x) { return x.LocationName; }).ToArray();
            //    that.LocationsUsed = Enumerable.From(lst)
            //        .Where(function (x) { return x.LocationPhotoCount > 0; })
            //        .OrderByDescending(function (x) { return x.LocationPhotoCount; })
            //        .ThenBy(function (x) { return x.LocationName; }).ToArray();
            //}),
            getSteamData("locationGroupsByCity", this.Steam, function(resL) {
                var lst = new LocationGroups(resL.Data, that);
                if (lst.length>0) {
                    var list = [];
                    for (var i=0;i<lst.length;i++) {
                        var locations = new Locations(lst[i].LocationLocations, that);
                        if (i === 0)
                            list = locations;
                        else
                           list= list.concat(locations);
                    }
                    that.LocationGroups = lst;
                    that.Locations = Enumerable.From(list)
                            .Where(function (x) { return x.LocationName.toUpperCase() != "NOTSET"; })
                            .OrderByDescending(function (x) { return x.LocationPhotoCount; })
                            .ThenBy(function (x) { return x.LocationName; }).ToArray();
                    that.LocationsUsed = Enumerable.From(list)
                            .Where(function (x) { return x.LocationPhotoCount > 0; })
                            .OrderByDescending(function (x) { return x.LocationPhotoCount; })
                            .ThenBy(function (x) { return x.LocationName; }).ToArray();
                }
            }),
            getSteamData("events", this.Steam, function (resE) {
                var lst = new Events(resE.Data, that);
                that.Events = Enumerable.From(lst)
                    .Where(function (x) { return x.EventName.toUpperCase() != "NOTSET"; })
                    .OrderByDescending(function (x) { return x.EventPhotoCount; })
                    .ThenBy(function (x) { return x.EventName; }).ToArray();
                that.EventsUsed = Enumerable.From(lst)
                    .Where(function (x) { return x.EventPhotoCount > 0; })
                    .OrderByDescending(function (x) { return x.EventPhotoCount; })
                    .ThenBy(function (x) { return x.EventName; }).ToArray();
            }),
            getSteamData("stories", this.Steam, function (resI) {
                var lst = new Stories(resI.Data, that);
                that.Stories = Enumerable.From(lst)
                    .OrderByDescending(function (x) { return x.StoryPhotoCount; })
                    .ThenBy(function (x) { return x.StoryName; }).ToArray();
            }),
            getSteamData("buddies", this.Steam, function (resB) {
                that.Buddies = new Buddies(resB.Data, that);
            })
        ).then(function () {

            getSteamData("fotos", that.Steam, function (resF) {
                that.Fotos = new Fotos(resF.Data, that);
                if (typeof fRoute === 'function') {
                    fRoute(that);
                }
            });

        });
    };
    // =============================================
    this.deleteAccount = function() {
        try {
            if (global_auth_member != that) {
                throw "delete-account-error";
            }

            popoutConfirm("delete-account",
                function() {
                    executeSteamAction("delete-account", null, null,
                        function() {
                            popoutAffirm("delete-account", function() {
                                window.location.href = "/"; //TODO: JS-Route
                            });
                        },
                        function (result) { toastErrorText(result, "delete-account-error"); }
                    );
                }
            );
        } catch (err) {
            toastErrorText(err, "delete-account-error");
        }
    };
    // ---------------------------------------------
    this.updateField = function(fieldName, fieldValue, fieldKey, fUpdated) {
        updateModel("Member", that.Id, fieldName, fieldValue, fieldKey, fUpdated);
    };
    // ---------------------------------------------
    this.storeHomeLocation = function (location, bOverwrite, fSuccess) {
        var _shl = that;

        var poco = location.getPoco();

        if (that.HomeLocation.LocationId === 0 || bOverwrite === false) {
            //CREATE-NEW
            executeSteamAction("create-homelocation", null, poco,
                function (result) {                    
                    that.updateField("HomeLocation_Id", result.Data.Id, "your-location", function() {                        
                        _shl.setHomeLocation(result.Data);
                        fSuccess();
                    });                    
                },
                function (result) { toastErrorText(result, "create-homelocation-error"); }
            );
        } else {
            //SAVE
            poco.Id = that.HomeLocation.LocationId;
            executeSteamAction("save-homelocation", null, poco,
                function (result) {
                    _shl.setHomeLocation(result.Data);
                    toastMessageText(i18n.get("field-update-message").replace("%FIELD%", i18n.get("your-location")));
                    fSuccess();
                },
                function (result) { toastErrorText(result, "save-homelocation-error"); }
            );
        }
        this.setHomeLocation = function (data) {
            var resultLocation = new Location(data, that);
            that.HomeLocation = resultLocation;
            that.replaceResultLocation(resultLocation);
        };
    };
    // ---------------------------------------------
    this.replaceResultLocation = function (resultLocation) {
        $.each(that.Locations, function (index, value) {
            if (value.LocationId === resultLocation.LocationId) {
                that.Locations[index] = resultLocation;
                return false;
            }
        });        
    };
    // ---------------------------------------------
    this.removeHomeLocation = function (fSuccess) {
        var id = that.HomeLocation.LocationId;
        that.HomeLocation = null;
        that.updateField("HomeLocation_Id", 0, "your-location", function () {
            removeLocation(id);
            fSuccess();
        });
    };
    // ---------------------------------------------
    this.removeLocation = function(id) {
        $.each(that.Locations, function(index, value) {
            if (value.LocationId === id) {
                that.Locations.splice(index, 1);
                return false;
            }
        });
    };
    // ---------------------------------------------
    this.requestBuddyStatus = function(idMember) {

        var model = { BuddyMemberId: idMember };

        executeSteamAction("request-buddy", null, model,
            function (result) {
                window.location.reload();
                //TODO: Result verarbeiten
                //that.showBuddyStatus();
                //toastMessage("buddies-request-message");
            },
            function (result) { toastErrorText(result, "buddies-request-error"); }
        );
    };
    // ---------------------------------------------
    this.confirmBuddyStatus = function (idMember) {

        var model = { BuddyMemberId: idMember };

        executeSteamAction("confirm-buddy", null, model,
            function (result) {
                window.location.reload();
                //TODO: Result verarbeiten
                //that.showBuddyStatus();
                //toastMessage("buddies-confirm-message");
            },
            function (result) { toastErrorText(result, "buddies-confirm-error"); }
        );
    };
    // ---------------------------------------------
    this.showBuddyStatus = function() {

        var authMember = (global_auth_member) ? global_auth_member : null,
            isAuthMember = (authMember && authMember.Steam === that.Steam),
            buddyFromAuthMember = (authMember && isAuthMember === false) ? Enumerable.From(authMember.BuddyList).Where(function (x) { return x.MemberId === authMember.Id && x.BuddyMemberId === that.Id; }).ToArray()[0] : null,
            buddyToMember = (authMember && isAuthMember === false) ? Enumerable.From(that.BuddyList).Where(function (x) { return x.BuddyMemberId === authMember.Id && x.MemberId === that.Id; }).ToArray()[0] : null,
            buddy = (typeof buddyFromAuthMember != 'undefined') ? buddyFromAuthMember : (typeof buddyToMember != 'undefined') ? buddyToMember : null,
            action = "";

        if (buddy !== null) {
            if ((typeof buddyFromAuthMember != 'undefined')) {
                if (buddyFromAuthMember.IsMutual) {
                    action = "MUTUAL";
                } else {
                    action = "WAIT";
                }
            } else if (buddyToMember !== null) {
                if (buddyToMember.IsMutual) {
                    action = "MUTUAL";
                } else {
                    action = "CONFIRM";
                }
            }
        } else { action = "REQUEST"; }


        var data = {
            Member: that,
            AuthMember: authMember,
            IsAuthMember: isAuthMember,
            ShowRequest: (action === "REQUEST"),
            ShowWait: (action === "WAIT"),
            ShowConfirm: (action === "CONFIRM"),
            ShowMutual: (action === "MUTUAL")
        },
        sBuddies = i18n.translate(fsTemplates.tAboutBuddies(data)),
        jBuddies = $(sBuddies);

        jBuddies.find("#buddy-request").click(function () {
            that.requestBuddyStatus(that.Id);
            return false;
        });
        jBuddies.find("#buddy-confirm").click(function () {
            that.confirmBuddyStatus(that.Id);
            return false;
        });

        $("#about-meta-buddy").empty().append(jBuddies);
    };
    // ---------------------------------------------
    this.getHeaderStyle = function (foto) {

        var url640,
            url1024,
            url1440,
            url1920,
            bgGradient = "";

        if (foto) {
            url640 = foto.Url640;
            url1024 = foto.Url1024;
            url1440 = foto.Url1440;
            url1920 = foto.Url1920;
            bgGradient = "linear-gradient(rgba(0, 0, 0, 0.1), rgba(0, 0, 0, 0.2)), ";
        } else {
            url640 = that.Header640Url;
            url1024 = that.Header1024Url;
            url1440 = that.Header1440Url;
            url1920 = that.Header1920Url;
        }

        var style =
            '<style id="header-style" type="text/css" id="member-header-style">' +
                '.bg-wrap:before, figure:before { content: "' + i18n.get("loading") + '"; }' +
                '@media only screen { header div.bg { background-image: ' + bgGradient + 'url("' + $.trim(url640) + '"); }}\n' +
                '@media only screen and (min-width: 40.063em) { header div.bg { background-image: ' + bgGradient + 'url("' + $.trim(url1024) + '"); }}\n' +
                '@media only screen and (min-width: 64.063em) { header div.bg { background-image: ' + bgGradient + 'url("' + $.trim(url1440) + '"); }}\n' +
                '@media only screen and (min-width: 90.063em) { header div.bg { background-image: ' + bgGradient + 'url("' + $.trim(url1920) + '"); }}' +
            '</style>';

        return style;
    };
    // ---------------------------------------------
    this.SetPageStandards = function(currentKey, foto) {
        setTitle(that.PlainName);

        $("head").remove("#header-style").append(this.getHeaderStyle(foto));

        $("body").addClass("steam");

        //Header
        $("header").empty().append(
            '<div class="bg-wrap"><div class="bg"></div></div>' +
            '<h2 class="title">' +
            '<small></small>' +
            '<div class="title-commands"></div>' +
            '<span></span>' +
            '</h2>' +
            '<a class="avatar" href="/' + this.Steam + '/about' + '" style="background-image: url(' + that.Avatar100Url + ')"></a>' +
            '<h1 class="member">' + this.PlainName.replace(/ /, "<br/>") + '</h1>'
        ).addClass(currentKey);

        //Footer
        $("footer").empty().append(i18n.translate(fsTemplates.tFooter({})));

        //Sidebar
        var data = {
            Member: that,
            AuthMember: (global_auth_member) ? global_auth_member : null,
            Registered: (global_auth_member && global_auth_member.IsRegistered),
            ShowMy: !(global_auth_member && (global_auth_member.Steam === that.Steam))
        },
        jSidebar = $(i18n.translate(fsTemplates.tSidebarMember(data)));

        $("nav.sidebar").append(jSidebar);

        initSidebar();

        if (currentKey) {
            $(".sidebar-commands").find(".cmd-" + currentKey).addClass("current");
        }

        //Reset Header
        $("header").removeClass("headerbar");
        $("#content").css("padding-top", "0");

        //Scroll-Binding
        var headerHeight = $("header").height(),
            lastScrollTop = 0;

        function setScroll() {

            //Parallax-Header
            var speed = 100,
                //STOP AT HEADER-EDGE: headerOffset = (headerHeight > window.pageYOffset) ? (window.pageYOffset / headerHeight) : 0,
                headerOffset = (window.pageYOffset / headerHeight),
                backgroundPos = "50% calc(50% - " + (Math.ceil(headerOffset * speed)) + "px)";
            $("header div.bg")[0].style.backgroundPosition = backgroundPos;

            //Header-Bar
            if (window.pageYOffset >= headerHeight - $(".tile").outerHeight()) {
                $("header").addClass("headerbar");
                $("#content").css("padding-top", headerHeight + "px");
            } else {
                $("header").removeClass("headerbar");
                $("#content").css("padding-top", "0");
            }

            //Logomenu
            if ($(this).scrollTop() > headerHeight) {
                if ($(this).scrollTop() > lastScrollTop) { // down
                    $("#logomenu").css("opacity", "0.5");
                } else { // up
                    $("#logomenu").css("opacity", "1");
                }
                lastScrollTop = $(this).scrollTop();
            }
        }

        $(window).on('scroll', function() {
            setScroll();
        });
        setScroll();
    };

    // =============================================
    this.Resources = [];

    // =============================================
    this.ShowPage_About = function() {
        appendToTitle("About");

        that.Resources.push({
            "Key": "gmap",
            "LocalUrl": "/Scripts/gmap3.min.js",
            "NeedOnline": true
        });

        resourceLoader(that.Resources, function () {
            that.SetPageStandards("about");
            that.SetPage_About();
        });

    };
    // ---------------------------------------------
    this.SetPage_About = function () {

        var authMember = (global_auth_member) ? global_auth_member : null,
            isAuthMember = (authMember && authMember.Steam === that.Steam);

        //nur man selbst, darf die Non-Mutual-Buddies sehen
        var buddiesList;
        if (isAuthMember) {
            buddiesList = that.Buddies;
        } else {
            buddiesList = Enumerable.From(that.Buddies).Where(function (x) { return x.IsMutual === true; }).ToArray();
        }

        var data = {
                Member: that,
                BuddiesList: buddiesList
            },
            sAbout = i18n.translate(fsTemplates.tAbout(data)),
            jAbout = $(sAbout);

        //Header
        $("header").find("h2.title > span").append(that.PlainName);

        //Map
        if (that.HomeLocation.LocationId !== 0) {
            loadMap(
                jAbout.find("#about-map"),
                that.HomeLocation.LocationLatitude,
                that.HomeLocation.LocationLongitude,
                false);
        } else {
            loadWorldMap(jAbout.find("#about-map"));
        }

        //Collection-Buttons initialisieren
        var jHeadline = jAbout.find("#about-meta-collection-current");
        jAbout.find("#about-meta-collections > a").hover(
            function() {
                var col = jAbout.find("#about-meta-collections > a.current").data("collection");
                if (col != $(this).data("collection")) {
                    jHeadline.text("... " + i18n.get($(this).data("collection")));
                }
            },
            function() {
                jHeadline.text(i18n.get(jAbout.find("#about-meta-collections > a.current").data("collection")));
            }
        ).click(function() {
            var col = $(this).data("collection");
            jAbout.find("#about-meta-collections > a.current").removeClass("current");
            $(this).addClass("current");
            showCollection(col, false);
            return false;

        });
        //Erste Collection setzen
        showCollection(jAbout.find("#about-meta-collections > a.current").data("collection"), true);

        //---
        function showCollection(col, bInit) {
            if (global_format != "S") {

                jHeadline.text(i18n.get(col));

                var jItems = jAbout.find("#about-meta-collection-items"),
                    jItemsContainer;

                if (bInit) {
                    jItemsContainer = jItems;
                } else {
                    jItemsContainer = jItems.find(".mCSB_container");
                }
                jItems.prop("class", col);
                jItems.find("a.collection-item").remove();
                jItems.find("p").remove();

                switch (col) {
                case "categories":
                    $.each(that.CategoriesUsed, function(index, value) {
                        jItemsContainer.append('<a href="' + value.CategoryUrl + '" class="collection-item collection-category">' + value.CategoryName + '<i>' + value.CategoryPhotoCount + '</i></a>');
                    });
                    break;
                case "topics":
                    $.each(that.TopicsUsed, function (index, value) {
                        jItemsContainer.append('<a href="' + value.TopicUrl + '" class="collection-item collection-topic">' + value.TopicName + '<i>' + value.TopicPhotoCount + '</i></a>');
                    });
                    break;
                case "locations":
                    $.each(that.LocationsUsed, function (index, value) {
                        jItemsContainer.append('<a href="' + value.LocationUrl + '" class="collection-item collection-location">' + value.LocationCaption + '<i>' + value.LocationPhotoCount + '</i></a>');
                    });
                    break;
                case "events":
                    $.each(that.EventsUsed, function(index, value) {
                        jItemsContainer.append('<a href="' + value.EventUrl + '" class="collection-item collection-event">' + value.EventCaption + '<i>' + value.EventPhotoCount + '</i></a>');
                    });
                    break;
                case "stories":
                    $.each(that.Stories, function(index, value) {
                        jItemsContainer.append('<a href="' + value.StoryUrl + '" class="collection-item collection-story">' + value.StoryName + '<i>' + value.StoryPhotoCount + '</i></a>');
                    });
                    break;
                default:
                }
                if (jItems.find(".mCSB_container").is(':empty')) {
                    jItems.find(".mCSB_container").append(i18n.translate('<p data-i18n="no-items" style="font-size: 16px; text-align: right;">keine Einträge</p>'));
                } else {
                    jItems.mCustomScrollbar("update");
                }
            }
        }

        $("#content").empty().append(jAbout);

        jAbout.find("#about-meta-collection-items").mCustomScrollbar({
            theme: "dark-thick",
            scrollInertia: 200
        });

        $("body").css("background-color", "rgb(" + that.HeaderColor + ")");

        //Buddies
        that.showBuddyStatus();

        //FOTOS
        function getFotoGridDimension() {
            switch (global_format) {
                case "XL": return 6;
                case "L": return 5;
                case "M": return 4;
                default: return 3;
            }
        }
        //--
        var itemsRow = getFotoGridDimension();
        var itemsGrid = (itemsRow * itemsRow);
        //--
        function getFotosMember(action) {
            getSteamData(action, { alias: that.Steam, take: itemsGrid }, function (resF) {
                var fotos = new Fotos(resF.Data, that);

                var wrap = $("#about-meta-fotos .collection-wrapper");
                wrap.empty();

                $.each(fotos, function (index, foto) {
                    foto.Avatar100Url = that.Avatar100Url;
                    foto.Alias = that.Steam;
                    foto.PlainName = that.PlainName;

                    var dataT = {
                        Foto: foto
                    },
                    jFoto = $(i18n.translate(fsTemplates.tCubicle(dataT)));

                    jFoto.find(".details").css("background-color", "rgba(" + foto.Color + ", 0.4)");
                    jFoto.find(".title").css("background-color", "rgba(" + foto.Color + ", 1)");

                    wrap.append(jFoto);

                    trimFotosToGrid(
                        $("#about-meta-fotos .collection-container"),
                        $("#about-meta-fotos .collection-wrapper"),
                        function () { getFotoGridDimension(); }
                    );

                    $("#about-meta-fotos h3#" + action).removeClass("link");
                    $("#about-meta-fotos h3:not('#" + action + "')").addClass("link");
                });

                doFullsizable(wrap.find("a.btn-show"), {
                    detailButton: true
                });
            });           
        }
        //--
        getFotosMember("fotos-member-random");
        //--
        $("#about-meta-fotos h3").each(function() {
            var action = $(this).attr("id");
            $(this).click(function() {
                getFotosMember(action);
            });            
        });

        fittingContent();

        //Resize-Binding
        //$(window).on('resize', function () {
        //    
        //});

        //Scroll-Binding
        //$(window).on('scroll', function() {
        //    
        //});

        //Key-Bindings
        global_keyBindings.showCollection();
    };
    // =============================================
    this.ShowPage_Setup = function() {

        resourceLoader(that.Resources, function () {
            that.SetPageStandards("setup");
            setTitle("Setup");

            that.SetPage_Setup();
        });

    };
    // ---------------------------------------------
    this.SetPage_Setup = function() {

        var sSetup = i18n.translate(fsTemplates["tSetup-" + global_lang.toUpperCase()](that)),
            jSetup = $(sSetup);

        $("#content").empty().append(jSetup);

        $("body").css("background-color", "rgb(" + that.HeaderColor + ")");

        fittingContent();
    };
    // ---------------------------------------------
    this.SelectProvider = function(e, storageProviderKey) {
        var jE = $(e);
        jE.parent().find(".selected").removeClass("selected").find("i").remove();
        jE.addClass("selected").append("<i class='icon icon-ok-circled'></i>");

        var provider = "";
        switch (storageProviderKey) {
            case storageProvider.Dropbox:
                provider = "Dropbox";
                break;
            case storageProvider.GoogleDrive:
                provider = "Google Drive";
                break;
            case storageProvider.OneDrive:
                provider = "Microsoft OneDrive";
                break;
        default:
        }
        $("#setup-intro-info").slideUp(function() {
            $(".setup-intro-info-provider").hide();
            $("#setup-intro-info-provider-" + storageProviderKey).show();
        }).slideDown();
        $.scrollTo("#setup-intro-info", 500);
        $("#setup-intro-command")
            .text(i18n.get("authorization-command").replace("%PROVIDER%", provider))
            .prop('disabled', false)
            .unbind().click(function () {
                $(this).prop('disabled', true);
                authorize(storageProviderKey);
            }
        );
    };
    // =============================================
    this.ShowPage_Dashboard = function(routeHash) {

        that.Resources.push({
            "Key": "gmap",
            "LocalUrl": "/Scripts/gmap3.min.js",
            "NeedOnline": true
        }, {
            "Key": "dropzone",
            "LocalUrl": "/Scripts/dropzone/dropzone" + global_min + ".js"
        }, {
            "Key": "tabulous",
            "LocalUrl": (global_env === "P") ? "/Scripts/Build/" + global_version + "/jquery.tabulous.min.js"
                                             : "/Scripts/Resource/jquery.tabulous.js"
        }, {
            "Key": "fotosteam-dashboard",
            "LocalUrl": (global_env === "P") ? "/Scripts/Build/" + global_version + "/fotosteam-dashboard.min.js"
                                             : "/Scripts_Custom/Resource/fotosteam-dashboard.js"
        }, {
            "Key": "fotosteam-multiedit",
            "LocalUrl": (global_env === "P") ? "/Scripts/Build/" + global_version + "/fotosteam-multiedit.min.js"
                                             : "/Scripts_Custom/Resource/fotosteam-multiedit.js"
        }, {
            "Key": "dropzone-css",
            "LocalUrl": "/Scripts/dropzone/dropzone" + global_min + ".css"
        }, {
            "Key": "tabulous-css",
            "LocalUrl": (global_env === "P") ? "/Content/Build/" + global_version + "/tabulous.min.css"
                                             : "/Content/CSS/Resource/tabulous.css"
        }, {
            "Key": "chosen",
            "LocalUrl": "/Scripts/chosen/chosen.jquery" + global_min + ".js"
        }, {
            "Key": "chosen-css",
            "LocalUrl": "/Scripts/chosen/chosen.css"
        });

        resourceLoader(that.Resources, function () {
            that.SetPageStandards("dashboard");
            appendToTitle(i18n.get("dashboard"));

            $.when(
                getSteamData("member-invitecodes", that.Steam, function (resI) {
                    that.InviteCodes = resI.Data;
                })
            ).then(function() {
                that.SetPage_Dashboard(routeHash);
                
            });
        });

    };
    // ---------------------------------------------
    this.SetPage_Dashboard = function (routeHash) {
        initDashboard(that, routeHash);
    };
    // ---------------------------------------------
}
//---
function Members(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Member(x);
        });
    }
    return list;
}
