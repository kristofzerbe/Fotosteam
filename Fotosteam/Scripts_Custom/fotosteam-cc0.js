
function CC0() {
    var that = this;

    this.Title = i18n.get("license-free-photos");
    this.BackgroundImagePath = "/Content/Background/back01";

    this.Resources = [
        {
            "Key": "jquery_visible",
            "LocalUrl": "/Scripts/jquery.visible" + global_min + ".js"
        }
    ];

    // ---------------------------------------------
    this.Init = function () {

        resourceLoader(that.Resources, function () {

            appendToTitle(that.Title);
            that.SetGrid();
            that.SetPage();
        });
    };

    this.SetPage = function () {
        new Common(null);
    };

    // ---------------------------------------------
    this.AppendRunning = false;
    this.CurrentGridRowIndex = 0;
    this.CurrentGridAmount = 0;
    this.CurrentGridScale = 0;
    this.autoGridScale = true;

    this.showAutoGridScale = function (fComplete) {

        if (that.autoGridScale === true) {
            var scale = 0;
            switch (global_format) {
                case "XL":
                    scale = 6;
                    break;
                case "L":
                    scale = 5;
                    break;
                case "M":
                    scale = 4;
                    break;
                default:
                    scale = 3;
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
        if (fComplete !== undefined) {
            fComplete();
        }
    };
    // ---------------------------------------------
    this.setGridScale = function (dir) {

        var oldScale = that.CurrentGridScale;

        if (dir === 0) {
            if (that.autoGridScale === true) {
                return false;
            }
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
                } else {
                    return false;
                }
            }
            if (dir === 1) {
                if (that.CurrentGridScale < 10) {
                    that.CurrentGridScale += 1;
                    $("#overview-scale-up").css("opacity", "");
                    if (that.CurrentGridScale === 10) {
                        $("#overview-scale-down").css("opacity", "0.4");
                    }
                    if ($("footer").visible(true)) {
                        that.loadNextGridFotos();
                    }
                } else {
                    return false;
                }
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
    this.TrimFotosToGrid = function () {
        var w = $("#content .collection-container").width(),
            x = w / that.CurrentGridScale,
            y = Math.ceil(x),
            z = y * that.CurrentGridScale;

        $("#content .collection-wrapper").width(z);
    };

    // ---------------------------------------------
    this.SetGrid = function() {

        var photoPanel = i18n.translate(fsTemplates.tCC0Overview());
        $("#content").empty().append(photoPanel);
       
         $("#SearchCC0Fotos").click(function() {
            that.loadNextGridFotos();
        });

        //Commands
        that.showAutoGridScale();
        $("#overview-scale-auto").click(function() { return that.setGridScale(0); });
        $("#overview-scale-down").click(function() { return that.setGridScale(+1); });
        $("#overview-scale-up").click(function() { return that.setGridScale(-1); });
        $("#overview-go-list").hide();


        //die ersten Bilder holen
        that.CurrentGridAmount = -1;
        that.loadNextGridFotos();

        setTooltips($("header"), "left");
        setTooltips($(".page-commands"), "bottom");
        fittingContent();

        //Resize-Binding
        $(window).on('resize', function() {
            that.showAutoGridScale(function() {
                that.FillGridFotoGaps();
                that.TrimFotosToGrid();
            });
        });

        //Scroll-Binding
        $(window).on('scroll', function() {
            if ($("footer").visible(true)) {
                that.loadNextGridFotos();
            }
        });

        //Key-Bindings
        global_keyBindings.add("plus", i18n.get("key-scale-up"), function() {
            that.setGridScale(-1);
        });
        // --------------------
        global_keyBindings.add("-", i18n.get("key-scale-down"), function() {
            that.setGridScale(+1);
        });
        // --------------------
        global_keyBindings.showCollection();
    };

    // ---------------------------------------------
    this.AppendGridFotos = function (aFotos, fComplete) {

        $.each(aFotos, function (index, foto) {

            foto.SteamPath = foto.Alias;
            var data = {
                Foto: foto
            },
            jFoto = $(i18n.translate(fsTemplates.tCubicle(data)));

            jFoto.find(".details").css("background-color", "rgba(" + foto.Color + ", 0.4)");
            jFoto.find(".title").css("background-color", "rgba(" + foto.Color + ", 1)");

            if (that.autoGridScale === false) {
                jFoto.addClass("scale-" + that.CurrentGridScale);
            }

            $("#content .collection-wrapper").append(jFoto);

            if (fComplete !== undefined) { fComplete(); }

        });

        doFullsizable($("#content .collection-wrapper .cubicle").find("a.btn-show"), {
            detailButton: true,
            avatarButton: true
        });

        that.CurrentGridAmount = $("#content .collection-wrapper .cubicle").length;
        that.CurrentGridRowIndex = that.CurrentGridAmount / that.CurrentGridScale;
    };


    // ---------------------------------------------
    that.FillGridFotoGaps = function () {

        var rows = Math.ceil(that.CurrentGridAmount / that.CurrentGridScale),
            delta = 0;

        delta = (rows * that.CurrentGridScale) - that.CurrentGridAmount;

        if (delta > 0) {

            var query = Enumerable.From(that.Fotos)
                .Skip(that.CurrentGridAmount)
                .Take(delta)
                .Select()
                .ToArray();

            that.AppendGridFotos(query);
        }
    };
    // ---------------------------------------------
    this.loadNextGridFotos = function () {

        var iSkip = that.CurrentGridAmount + 1;
        var iTake = ((that.CurrentGridRowIndex + 1) * that.CurrentGridScale) - that.CurrentGridAmount;

        getSteamData("fotos-cc0", { skip: iSkip, take: iTake }, function (resNF) {
            var newFotos = new MemberFotos(resNF.Data);
            that.Fotos = Enumerable
            .From(newFotos.Fotos)
                .OrderByDescending(function (x) { return x.PublishDate; })
                .Select()
                .ToArray();

            if (newFotos.length !== 0) {
                $("header").css("background-image", "url('" + newFotos[0].Url1920 + "')");
                that.AppendGridFotos(newFotos, function () {
                    that.TrimFotosToGrid();
                });
            }
        });

    };
    that.Init();
}
