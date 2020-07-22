var OverviewTypes = {
    GRID: 0,
    LIST: 1
}; Object.freeze(StackTypes);

function Overview(overviewType) {
    var that = this;

    this.Title = i18n.get("overview");
    //this.Fotos = Enumerable
    //    .From(global_member.Fotos)
    //    .OrderByDescending(function (x) { return x.PublishDate; })
    //    .Select()
    //    .ToArray();

    this.Resources = [
        {
            "Key": "jquery_visible",
            "LocalUrl": "/Scripts/jquery.visible" + global_min + ".js"
        }        
    ];

    // ---------------------------------------------
    this.Init = function () {

        if (global_auth_member && global_auth_member.Steam == global_member.Steam) {
            that.Resources.push({
                "Key": "chosen",
                "LocalUrl": "/Scripts/chosen/chosen.jquery" + global_min + ".js"
            }, {
                "Key": "chosen-css",
                "LocalUrl": "/Scripts/chosen/chosen.css"
            }, {
                "Key": "fotosteam-multiedit",
                "LocalUrl": (global_env === "P") ? "/Scripts/Build/" + global_version + "/fotosteam-multiedit.min.js"
                                                 : "/Scripts_Custom/Resource/fotosteam-multiedit.js"
            });
        }

        resourceLoader(that.Resources, function () {
            global_member.SetPageStandards("");

            appendToTitle(that.Title);

            //Header
            $("header").find("h2.title > span").append(that.Title);

            switch (overviewType) {
                case OverviewTypes.GRID:
                    that.SetGrid();
                    break;
                case OverviewTypes.LIST:
                    that.SetList();
                    break;
                default:
            }

            $("body").css("background-color", "rgb(" + global_member.HeaderColor + ")");
        });
    };
    // ---------------------------------------------
    this.AppendRunning = false;
    this.CurrentGridRowIndex = 0;
    this.CurrentGridAmount = 0;
    this.CurrentGridScale = 0;
    this.autoGridScale = true;

    if ($.cookie("fotosteam-gridscale")) {
        this.CurrentGridScale = parseInt($.cookie("fotosteam-gridscale"));
        this.autoGridScale = false;
    }

    this.showAutoGridScale = function (fComplete) {

        if (that.autoGridScale === true) {
            var scale = 0;
            switch (global_format) {
                case "XL": scale = 6; break;
                case "L": scale = 5; break;
                case "M": scale = 4; break;
                default: scale = 3;
            }
            that.CurrentGridScale = scale;
            $("#overview-scale-auto").addClass("current");
            $("#overview-scale .scale-block").removeClass("current").removeClass("auto");
            $("#overview-scale .scale-block.scale-" + scale).addClass("current").addClass("auto");
        } else {
            $("#overview-scale-auto").removeClass("current");
            $("#overview-scale .scale-block").removeClass("current").removeClass("auto");
            $("#overview-scale .scale-block.scale-" + that.CurrentGridScale).addClass("current");
        }
        if (fComplete !== undefined) { fComplete(); }
    };
    // ---------------------------------------------
    this.setGridScale = function (dir) {

        var oldScale = that.CurrentGridScale;

        if (dir === 0) {
            if (that.autoGridScale === true) { return false; }
            $(".collection-wrapper .cubicle").removeClass("scale-" + oldScale);
            $("#overview-scale-up").css("opacity", "");
            $("#overview-scale-down").css("opacity", "");
            that.autoGridScale = true;
            that.showAutoGridScale();
            $.removeCookie("fotosteam-gridscale");
        } else {
            that.autoGridScale = false;
            if (dir === -1) {
                if (that.CurrentGridScale > 3) {
                    that.CurrentGridScale -= 1;
                    $("#overview-scale-down").css("opacity", "");
                    if (that.CurrentGridScale === 2) {
                        $("#overview-scale-up").css("opacity", "0.4");
                    }
                } else { return false; }
            }
            if (dir === 1) {
                if (that.CurrentGridScale < 10) {
                    that.CurrentGridScale += 1;
                    $("#overview-scale-up").css("opacity", "");
                    if (that.CurrentGridScale === 10) {
                        $("#overview-scale-down").css("opacity", "0.4");
                    }
                    if ($("footer").visible(true)) {
                        that.LoadNextGridFotos();
                    }
                } else { return false; }
            }
            $("#overview-scale-auto").removeClass("current");
            $("#overview-scale .scale-block").removeClass("current").removeClass("auto");
            $("#overview-scale .scale-block.scale-" + that.CurrentGridScale).addClass("current");

            $(".collection-wrapper .cubicle")
                .removeClass("scale-" + oldScale)
                .addClass("scale-" + that.CurrentGridScale);

            $.cookie("fotosteam-gridscale", that.CurrentGridScale);
        }
        that.FillGridFotoGaps();
        that.TrimFotosToGrid();
        return false;
    };
    // ---------------------------------------------
    this.TrimFotosToGrid = function() {
        var w = $("#content .collection-container").width(),
            x = w / that.CurrentGridScale,
            y = Math.ceil(x),
            z = y * that.CurrentGridScale;

        $("#content .collection-wrapper").width(z);
    };
    // ---------------------------------------------
    this.SetGrid = function() {

        var data = {},
            sGrid = i18n.translate(fsTemplates.tOverviewGrid(data)),
            jGrid = $(sGrid);        

        $("#content").empty().append(jGrid);

        //Commands
        that.showAutoGridScale();
        $("#overview-scale-auto").click(function () { return that.setGridScale(0); });
        $("#overview-scale-down").click(function () { return that.setGridScale(+1); });
        $("#overview-scale-up").click(function () { return that.setGridScale(-1); });
        $("#overview-go-list").prop("href", global_member.SteamPath + "/list");

        ////Fotos einschränken, wenn nicht eigene
        //if (!global_auth_member || global_auth_member.Steam !== global_member.Steam) {
        //    that.Fotos = Enumerable.From(global_member.Fotos)
        //        .Where(function (x) { return x.IsForStoryOnly === false; })
        //        .OrderByDescending(function (x) { return x.PublishDate; })
        //        .Select()
        //        .ToArray();
        //}

        //die ersten Bilder holen
        var aFotos = that.GetGridFotosPerRow(3);
        if (aFotos.length !== 0) {
            that.AppendGridFotos(aFotos, function() {
                that.TrimFotosToGrid();
            });
        } else {
            //TODO: Anzeige, wenn es keine Bilder gibt
        }

        setTooltips($("header"), "left");
        setTooltips($(".page-commands"), "bottom");
        fittingContent();

        //Resize-Binding
        $(window).on('resize', function () {
            that.showAutoGridScale(function() {
                that.FillGridFotoGaps();
                that.TrimFotosToGrid();
            });
        });

        //Scroll-Binding
        $(window).on('scroll', function () {
            if ($("footer").visible(true)) {
                that.LoadNextGridFotos();
            }
        });

        //Key-Bindings
        global_keyBindings.add("plus", i18n.get("key-scale-up"), function() {
            that.setGridScale(-1);
        });
        // --------------------
        global_keyBindings.add("-", i18n.get("key-scale-down"), function () {
            that.setGridScale(+1);
        });
        // --------------------
        global_keyBindings.showCollection();

        if ($("footer").visible(true)) {
            that.LoadNextGridFotos();
        }

    };
    // ---------------------------------------------
    this.AppendGridFotos = function(aFotos, fComplete) {

        $.each(aFotos, function (index, foto) {

            foto.Avatar100Url = global_member.Avatar100Url;
            foto.Alias = global_member.Steam;
            foto.PlainName = global_member.PlainName;

            var data = {
                Foto: foto,
                SelectorTrigger: !(!global_auth_member || global_auth_member.Steam !== global_member.Steam)
            },
            jFoto = $(i18n.translate(fsTemplates.tCubicle(data)));

            if ($("#content .collection-container").hasClass("selector")) {
                jFoto.find(".cubicle-link").unbind().click(function (e) {
                    e.preventDefault();
                    triggerSelector($(this), that);
                    return false;
                });
            } else {
                doFullsizable(jFoto.find(".cubicle-link"), {
                    detailButton: true,
                    infinityLoad: function () {
                        return that.LoadNextGridFotos();
                    },
                });
            }

            jFoto.find(".selectortrigger a").click(function(e) {
                e.stopPropagation();
                triggerSelector($(this), that);
                return false;
            });

            jFoto.find(".details").css("background-color", "rgba(" + foto.Color + ", 0.4)");
            //jFoto.find(".selectortrigger").css("background-color", "rgba(" + foto.Color + ", 0.6)");
            jFoto.find(".title").css("background-color", "rgba(" + foto.Color + ", 1)");

            if (that.autoGridScale === false) {
                jFoto.addClass("scale-" + that.CurrentGridScale);
            }

            $("#content .collection-wrapper").append(jFoto);

            if (fComplete !== undefined) { fComplete(); }

        });

        that.CurrentGridAmount = $("#content .collection-wrapper .cubicle").length;
        that.CurrentGridRowIndex = that.CurrentGridAmount / that.CurrentGridScale;
    };
    // ---------------------------------------------
    this.GetGridFotosPerRow = function (iRows) {

        var iSkip = that.CurrentGridAmount,
            iTake = (iRows * that.CurrentGridScale) - that.CurrentGridAmount;

        var query;
        if (!global_auth_member || global_auth_member.Steam !== global_member.Steam) {
            query = Enumerable.From(global_member.Fotos)
                .Where(function(x) { return x.IsForStoryOnly === false && x.IsNew === false; })
                .OrderByDescending(function(x) { return x.PublishDate; })
                .Skip(iSkip)
                .Take(iTake)
                .Select()
                .ToArray();
        } else {
            query = Enumerable.From(global_member.Fotos)
                .OrderByDescending(function (x) { return x.PublishDate; })
                .Skip(iSkip)
                .Take(iTake)
                .Select()
                .ToArray();
        }

        return query;
    };
    // ---------------------------------------------
    //this.GetGridFotos = function (iCount) {

    //    var iSkip = that.CurrentGridAmount,
    //        iTake = (iCount) - that.CurrentGridAmount;

    //    var query = Enumerable
    //        .From(global_member.Fotos)
    //        .OrderByDescending(function (x) { return x.PublishDate; })
    //        .Skip(iSkip)
    //        .Take(iTake)
    //        .Select()
    //        .ToArray();

    //    return query;
    //};
    // ---------------------------------------------
    this.FillGridFotoGaps = function () {

        that.CurrentGridAmount = $("#content .collection-wrapper .cubicle").length;

        var rows = Math.ceil(that.CurrentGridAmount / that.CurrentGridScale),
            delta = 0;

        delta = (rows * that.CurrentGridScale) - that.CurrentGridAmount;

        if (delta > 0) {

            var query = Enumerable
                .From(global_member.Fotos)
                .OrderByDescending(function (x) { return x.PublishDate; })
                .Skip(that.CurrentGridAmount)
                .Take(delta)
                .Select()
                .ToArray();

            that.AppendGridFotos(query);
        }
    };
    // ---------------------------------------------
    this.LoadNextGridFotos = function() {
        var aFotos = that.GetGridFotosPerRow(that.CurrentGridRowIndex + 1);
        if (aFotos.length !== 0) {
            that.AppendGridFotos(aFotos, function() {
                that.TrimFotosToGrid();
            });
        }
    };
    // ---------------------------------------------
    this.SetList = function() {

        var data = {
            FotoCount: global_member.Fotos.length
            },
            sGrid = i18n.translate(fsTemplates.tOverviewList(data)),
            jGrid = $(sGrid);

        $("#content").empty().append(jGrid);

        $("#overview-go-grid").prop("href", global_member.SteamPath + "/overview");

        $.each(global_member.Fotos, function (index, foto) {
            foto.IsNewDim = (foto.IsNew) ? "" : "dim-out";
            foto.IsPrivateDim = (foto.IsPrivate) ? "" : "dim-out";
            foto.IsForStoryOnlyDim = (foto.IsForStoryOnly) ? "" : "dim-out";
            foto.AllowFullSizeDownloadDim = (foto.AllowFullSizeDownload) ? "" : "dim-out";
            foto.AllowCommentingDim = (foto.AllowCommenting) ? "" : "dim-out";
            foto.AllowRatingDim = (foto.AllowRating) ? "" : "dim-out";
            foto.AllowSharingDim = (foto.AllowSharing) ? "" : "dim-out";

            var jFoto = $(i18n.translate(fsTemplates.tOverviewListItem(foto)));
            $("#overview-list ul").append(jFoto);
        });

        setTooltips($(".page-commands"), "bottom");
        fittingContent();
    };
    // ---------------------------------------------
    that.Init();
}