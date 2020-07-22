var StackTypes = {
    JOURNAL: 0,
    CATEGORY: 1,
    TOPICS: 2,
    LOCATIONS: 3,
    EVENTS: 4
}; Object.freeze(StackTypes);

// =============================================
function Stack(stackType, stackValue) {
    var that = this;

    var _currentFigureOpen = "",
        _currentDetailsOpen = -1;

    this.Fotos = null;
    this.Title = "";
    this.Key = "";
    this.Subtitle = "";

    this.PaginationCount = 4;
    this.PaginationIndex = 0;
    //this.AppendRunning = false;

    this.CurrentFigureIndex = -1;

    this.Resources = [
        {
            "Key": "gmap",
            "LocalUrl": "/Scripts/gmap3.min.js",
            "NeedOnline": true
        },
        {
            "Key": "fullsizable",
            "LocalUrl": "/Scripts/jquery.fullsizable" + global_min + ".js"
        },
        {
            "Key": "carousel",
            "LocalUrl": "/Scripts/owl.carousel.min.js"
        },
        {
            "Key": "jquery_visible",
            "LocalUrl": "/Scripts/jquery.visible" + global_min + ".js"
        }
    ];

    // ---------------------------------------------
    this.Init = function() {

        switch (stackType) {
            case StackTypes.JOURNAL:
                that.Key = "journal";
                break;
            case StackTypes.CATEGORY:
                that.Key = "categories";
                break;
            case StackTypes.TOPICS:
                that.Key = "topics";
                break;
            case StackTypes.LOCATIONS:
                that.Key = "locations";
                break;
            case StackTypes.EVENTS:
                that.Key = "events";
                break;
            default:
        }        

        resourceLoader(that.Resources, function () {
            global_member.SetPageStandards(that.Key);
            that.SetPage();
        });

    };

    // ---------------------------------------------
    this.SetPage = function() {

        $("#content").empty();

        //Fotos
        //Je nach StackType filtern und Seitenelemente festlegen
        switch (stackType) {
            case StackTypes.JOURNAL:
                that.Title = i18n.get("journal");
                that.Subtitle = null;
                that.Fotos = Enumerable
                    .From(global_member.Fotos)
                    .Where(function (x) { return x.IsForStoryOnly === false && x.IsNew === false; })
                    .OrderByDescending(function (x) { return x.PublishDate; })
                    .Select()
                    .ToArray();
                break;

            case StackTypes.CATEGORY:
                that.Title = stackValue.CategoryName;
                that.Subtitle = "category";
                that.Fotos = Enumerable
                    .From(global_member.Fotos)
                    .Where(function (x) { return x.HasCategory(stackValue.CategoryType) && x.IsForStoryOnly === false && x.IsNew === false; })
                    .OrderByDescending(function (x) { return x.PublishDate; })
                    .Select()
                    .ToArray();
                break;

            case StackTypes.TOPICS:
                that.Title = stackValue.TopicName;
                that.Subtitle = "topic";
                that.Fotos = Enumerable
                    .From(global_member.Fotos)
                    .Where(function (x) { return x.HasTopic(stackValue.TopicKey) && x.IsForStoryOnly === false && x.IsNew === false; })
                    .OrderByDescending(function (x) { return x.PublishDate; })
                    .Select()
                    .ToArray();
                break;

            case StackTypes.LOCATIONS:
                that.Title = stackValue.LocationCaption;
                that.Subtitle = "location";
                that.Fotos = Enumerable
                    .From(global_member.Fotos)
                    .Where(function (x) { return x.HasLocation(stackValue.LocationKey) && x.IsForStoryOnly === false && x.IsNew === false; })
                    .OrderByDescending(function (x) { return x.PublishDate; })
                    .Select()
                    .ToArray();

                var locationInfo = $('<div class="location-info"></div>');
                if (stackValue.LocationLatitude && stackValue.LocationLongitude) {
                    loadMap(
                        locationInfo,
                        stackValue.LocationLatitude,
                        stackValue.LocationLongitude,
                        false);
                } else {
                    loadWorldMap(locationInfo);
                }
                $("#content").empty().append(locationInfo);
                break;

            case StackTypes.EVENTS:
                that.Title = stackValue.EventCaption;
                that.Subtitle = "event";
                that.Fotos = Enumerable
                    .From(global_member.Fotos)
                    .Where(function(x) { return x.HasEvent(stackValue.EventKey); })
                    .OrderByDescending(function (x) { return x.PublishDate; })
                    .Select()
                    .ToArray();

                var eventInfo =
                    $('<div class="event-info">' +
                          '<div class="event-info-description">' +
                              '<div class="content-box"></div>' +
                              '</div>' +
                          '<div class="event-info-map"></div>' +
                      '</div>');
                eventInfo.find(".event-info-description .content-box").append('<p>' + stackValue.EventDescription + '</p>');

                if (stackValue.EventLocation) {
                    loadMap(
                        eventInfo.find(".event-info-map"),
                        stackValue.EventLocation.Latitude,
                        stackValue.EventLocation.Longitude,
                        false);
                    $("#content").append(eventInfo);
                }
                break;

            default:
        }

        //Fotos einschränken, wenn nicht eigene
        if (!global_auth_member || global_auth_member.Steam !== global_member.Steam) {
            that.Fotos = Enumerable.From(that.Fotos)
                .Where(function (x) { return x.IsForStoryOnly === false; })
                .Select()
                .ToArray();
        }

        appendToTitle(that.Title);

        $("body").css("background-color", "rgb(" + global_member.HeaderColor + ")");

        //die ersten Bilder ohne Ladebalken (loadNextFigures) holen
        var aFotos = getFigures();
        if (aFotos.length !== 0) {
            appendFigures(aFotos);
        } else {
            //TODO: Anzeige, wenn es keine Bilder gibt
        }

        //Header
        $("header").find("h2.title > span").append(that.Title);
        if (that.Subtitle) {
            $("header").find("h2.title > small").append(i18n.get(that.Subtitle));
        }
        $("header").find("h2.title > .title-commands")
            .append(
                $('<a href="#" class="tooltip-left" title="' + i18n.get("full-screen-slideshow") + '"><i class="icon icon-slideshow"></i></a>').click(function () {
                    $("#content a.btn-show").first().trigger("click");
                    $(document).trigger('fullsizable:slideshow');
                    return false;
                })
            );

        fittingContent();
        setTooltips($("header"), "left");
        setTooltips($("#content"), "left");

        //Resize-Binding
        $(window).on('resize', function () {
            setFotoHeight($("#content"));
        });

        //Scroll-Binding
        $(window).on('scroll', function () {
            if ($("footer").visible(true)) {
                loadNextFigures();
            }
        });

        //Key-Bindings
        global_keyBindings.add("right", i18n.get("key-next-photo"), function() {
            if (that.CurrentFigureIndex < (that.Fotos.length - 1)) {
                that.CurrentFigureIndex += 1;
            } else {
                that.CurrentFigureIndex = 0;
            }
            if ((that.CurrentFigureIndex + 2) == ((that.PaginationIndex) * that.PaginationCount)) {
                loadNextFigures();
            }
            gotoFigure(that.Fotos[that.CurrentFigureIndex].fId);
        });
        // --------------------
        global_keyBindings.add("left", i18n.get("key-prev-photo"), function() {
            if (that.CurrentFigureIndex > 0) {
                that.CurrentFigureIndex -= 1;
                gotoFigure(that.Fotos[that.CurrentFigureIndex].fId);
            }
        });
        // --------------------
        global_keyBindings.add("f", i18n.get("key-full-screen"), function() {
            if (that.CurrentFigureIndex > -1) {
                $("#figure-" + that.Fotos[that.CurrentFigureIndex].fId + " .figure-bar .btn-show").click();
            }
        });
        // --------------------
        global_keyBindings.add("r", i18n.get("key-rate"), function () {
            if (that.CurrentFigureIndex > -1) {
                $("#figure-" + that.Fotos[that.CurrentFigureIndex].fId + " .figure-bar .btn-rating").click();
            }
        });
        // --------------------
        global_keyBindings.add("s", i18n.get("key-share"), function () {
            if (that.CurrentFigureIndex > -1) {
                $("#figure-" + that.Fotos[that.CurrentFigureIndex].fId + " .figure-bar .btn-share").click();
            }
        });
        // --------------------
        global_keyBindings.add("i", i18n.get("key-open-info"), function() {
            if (that.CurrentFigureIndex > -1) {
                openFigureDetails(that.Fotos[that.CurrentFigureIndex].fId, 0);
            }
        });
        // --------------------
        global_keyBindings.add("e", i18n.get("key-open-exif"), function() {
            if (that.CurrentFigureIndex > -1) {
                openFigureDetails(that.Fotos[that.CurrentFigureIndex].fId, 1);
            }
        });
        // --------------------
        global_keyBindings.add("c", i18n.get("key-open-comments"), function() {
            if (that.CurrentFigureIndex > -1) {
                openFigureDetails(that.Fotos[that.CurrentFigureIndex].fId, 2);
            }
        });
        // --------------------
        global_keyBindings.add("x", i18n.get("key-close-details"), closeFigureDetails);
        // --------------------
        global_keyBindings.showCollection();
        // --------------------
        window.global_pageResets = function() {
            closeFigureDetails();
        };
    };
    // ---------------------------------------------
    function appendFigures(aFotos, fComplete) {

        $.each(aFotos, function(index, foto) {

            var data = {
                Foto: foto,
                Member: (global_member) ? global_member : null,
                AuthMember: (global_auth_member) ? global_auth_member : null
            },
            jFoto = $(i18n.translate(fsTemplates.tFigure(data)));

            jFoto.find(".foto")
                .mouseenter(function() {
                    for (var i = 0; i < that.Fotos.length; i++) {
                        if (that.Fotos[i].fId === $(this).prop("id")) {
                            that.CurrentFigureIndex = i;
                        }
                    }
                });

            setFotoHeight(jFoto);

            // Figure-Bar-Buttons bestücken
            jFoto.find(".btn-info").click(function() {
                toggleFigureDetails(foto.fId, 0);
                return false;
            });
            jFoto.find(".btn-exif").click(function() {
                toggleFigureDetails(foto.fId, 1);
                return false;
            });
            jFoto.find(".btn-comments").click(function() {
                toggleFigureDetails(foto.fId, 2);
                return false;
            });

            $("#content").append(jFoto);

            jFoto.css("background-color", "rgb(" + foto.Color + ")");

            if (fComplete !== undefined) { fComplete(); }
        });

        $("a.btn-popout").popout( function() {
            closeFigureDetails();
        });

        doFullsizable($("a.btn-show"), {
            detailButton: true,
            slideshowButton: true,
            slideshowDelay: 6000,
            infinityLoad: function () {
                return loadNextFigures();
            },
        });

        that.PaginationIndex += 1;
    }
    // ---------------------------------------------
    function getFigures() {

        var iSkip = that.PaginationIndex * that.PaginationCount;

        var query = Enumerable.From(that.Fotos)
            .Skip(iSkip)
            .Take(that.PaginationCount)
            .Select()
            .ToArray();

        return query;
    }
    // ---------------------------------------------
    function loadNextFigures() {

        var aFotos = getFigures();

        if (aFotos.length !== 0) {
            appendFigures(aFotos);
            return true;
        }
        return false;
    }
    // ---------------------------------------------
    function setFotoHeight(jContainer) {

        var pMin = 0.85, lMin = 0.25;
        var pMax = 1.35, lMax = 0.85;

        var pFactor, lFactor;

        var wMax = 1440,
            w = window.innerWidth;

        pFactor = ((w * pMax) / wMax) + pMin;
        lFactor = ((w * lMax) / wMax) + lMin;

        jContainer.find(".foto.portrait").height((Math.ceil(window.innerHeight * pFactor)) + "px");
        jContainer.find(".foto.landscape").height((Math.ceil(window.innerHeight * lFactor)) + "px");
    }
    // ---------------------------------------------
    function gotoFigure(fId) {
        closeFigureDetails();
        var iTop = $("#figure-" + fId).find(".foto").offset().top - ($(".tile").height() * 2);
        $.scrollTo(iTop, 500);
    }
    // ---------------------------------------------
    function openFigureDetails(fId, owlIndex) {

        var jF = $("#figure-" + fId),
            jD = jF.find(".figure-details");

        var fDoOpen = function() {
            //Open...
            if ($("body").hasClass("sidebar-open") === false) {
                closeFigureDetails(function() {

                    gotoFigure(fId);
                    jF.addClass("open");
                    _currentFigureOpen = fId;

                    var owl = jF.find(".owl-carousel");
                    owl.trigger('owl.goTo', owlIndex);
                    _currentDetailsOpen = owlIndex;

                });
            }
        };

        if (jF.hasClass("details-loaded")) {
            fDoOpen();
        } else {

            var foto = Enumerable
                .From(global_member.Fotos)
                .Where(function (x) { return x.fId == fId; })
                .Select()
                .ToArray()[0];

            var data = {
                Foto: foto,
                Member: (global_member) ? global_member : null,
                AuthMember: (global_auth_member) ? global_auth_member : null
            },
            jDetails = $(i18n.translate(fsTemplates.tFigureDetails(data)));

            jD.append(jDetails);

            //Init (kurz warten, das APPEND fertig wird)
            window.setTimeout(function() {

                //License
                if (foto.LicenseArray.length > 0) {
                    var jL = jD.find(".info-license span");
                    $.each(foto.LicenseArray, function(index, value) {
                        jL.append('<i class="icon icon-' + value + '"></i>');
                    });
                }

                //Map
                if (foto.Exif.Latitude) {
                    loadMap(
                        jD.find(".exif-map").show(),
                        foto.Exif.Latitude,
                        foto.Exif.Longitude,
                        true);
                }

                //Slide Panels
                jD.owlCarousel({
                    items: 3, // items above 1280px
                    itemsDesktop: [1280, 2], // items between 1280px and 800px
                    itemsDesktopSmall: [800, 1],
                    itemsTablet: false,
                    itemsMobile: false,
                    mouseDrag: false,
                    touchDrag: false,
                    pagination: false
                });

                //Comments
                $.when(
                    loadComments(foto)
                ).then(function () {

                    //Scrollbars
                    jD.find(".panel-scroll").mCustomScrollbar({
                        theme: "dark-thick",
                        scrollInertia: 200
                    });
                });

                //Open Details
                jF.addClass("details-loaded");
                fDoOpen();

            }, 200);

        }
    }
    // ---------------------------------------------
    function closeFigureDetails(f) {
        if (_currentFigureOpen.length !== 0) {
            $("#" + _currentFigureOpen).parents("figure").removeClass("open");
            _currentFigureOpen = "";
            _currentDetailsOpen = -1;
        }
        if (typeof f == 'function') {
            f();
        }
    }
    // ---------------------------------------------
    function toggleFigureDetails(fId, owlIndex) {
        var jF = $("#figure-" + fId);
        if (jF.hasClass("open") && _currentDetailsOpen == owlIndex) {
            closeFigureDetails();
        } else {
            openFigureDetails(fId, owlIndex);
        }
    }
    // ---------------------------------------------
    that.Init();
}