
var ResolutionUnits = [
    "",
    "none",
    "inch",
    "centimeter"
];

var ExposureModes = [
    "exposure-mode-auto",
    "exposure-mode-manual",
    "exposure-mode-bracket"
];

var ExposurePrograms = [
    "unknown",
    "manual",
    "exposure-program-automatic",
    "exposure-program-aperture-priority",
    "exposure-program-shutter-speed-priority",
    "exposure-program-creative",
    "exposure-program-action",
    "exposure-program-portrait",
    "exposure-program-landscape",
    "exposure-program-bulb"
];

var MeteringModes = [
    "unknown",
    "metering-mode-average",
    "metering-mode-center-weighted-average",
    "metering-mode-spot",
    "metering-mode-multi-spot",
    "metering-mode-pattern",
    "metering-mode-partial"
];

var Licenses = [ //see Enums.LicenseType
    { Value: 0, Caption: "NONE" },
    { Value: 16, Caption: "CC-ZERO" },
    { Value: 33, Caption: "CC-BY 3.0" },
    { Value: 65, Caption: "CC-BY 4.0" },
    { Value: 35, Caption: "CC-BY-SA 3.0" },
    { Value: 67, Caption: "CC-BY-SA 4.0" },
    { Value: 37, Caption: "CC-BY-NC 3.0" },
    { Value: 69, Caption: "CC-BY-NC 4.0" },
    { Value: 41, Caption: "CC-BY-ND 3.0" },
    { Value: 73, Caption: "CC-BY-ND 4.0" },
    { Value: 39, Caption: "CC-BY-NC-SA 3.0" },
    { Value: 71, Caption: "CC-BY-NC-SA 4.0" },
    { Value: 45, Caption: "CC-BY-NC-ND 3.0" },
    { Value: 77, Caption: "CC-BY-NC-ND 4.0" }
];

function getLicenseArray(licValue) {
    var lt = parseFloat(licValue),
        slt = [],
        ver = "";
    switch (lt) {
        case 0:
            slt.push("NONE");
            break;
        case 16:
            slt.push("ZERO");
            break;
        default:
            slt.push("CC");
            if ((lt & 1) == 1) { slt.push("BY"); }
            if ((lt & 4) == 4) { slt.push("NC"); }
            if ((lt & 8) == 8) { slt.push("ND"); }
            if ((lt & 2) == 2) { slt.push("SA"); }
            if ((lt & 32) == 32) { ver = "3.0"; }
            if ((lt & 64) == 64) { ver = "4.0"; }
    }
    return [slt, ver];
}
function getLicenseName(licArray, licVersion) {
    var sln = [];
    $.each(licArray, function (index, value) {
        if (value.toLowerCase() == "cc") {
            sln.push(i18n.get("license-" + value.toLowerCase(), " " + licVersion));
        } else {
            sln.push(i18n.get("license-" + value.toLowerCase()));
        }
    });
    return sln.join(", ");
}
// =============================================
function Foto(json, member) {
    var that = this;
    var markdown = new MarkdownDeep.Markdown();

    this.SetCaption = function() {
        this.Caption = (this.Title) ? this.Title : this.Name;
    };
    this.SetColor = function (value) {
        this.Color = value;
        var arrColor = this.Color.split(",");
        this.ColorR = arrColor[0];
        this.ColorG = arrColor[1];
        this.ColorB = arrColor[2];
        this.ColorHex = "#" + rgbToHex(this.ColorR, this.ColorG, this.ColorB);
    };
    this.SetLicense = function (value) {
        this.LicenseValue = value;
        var licArray = getLicenseArray(this.LicenseValue);
        this.LicenseArray = licArray[0];
        this.LicenseVersion = licArray[1];
        this.LicenseName = getLicenseName(this.LicenseArray, this.LicenseVersion);
        this.License = this.LicenseArray.join("-") + " " + this.LicenseVersion;
    };

    for (var property in json) {
        switch (property) {
            case "Id":
                this.Id = json[property];
                this.fId = "f" + json[property];
                break;
            case "Color":
                this.SetColor((json[property]) ? json[property] : "34,34,34");
                break;
            case "CaptureDate":
                if (json[property] != "0001-01-01T00:00:00") {
                    this.CaptureDate = json[property];
                    this.CaptureDate_Formatted = moment(json[property]).locale(global_lang).format("LLLL");
                    this.CaptureDate_Short = moment(json[property]).locale(global_lang).format("L");
                } else {
                    this.CaptureDate = "";
                    this.CaptureDate_Formatted = "";
                    this.CaptureDate_Short = "";
                }
                break;
            case "PublishDate":
                this[property] = json[property];
                this.PublishDate_Formatted = moment(json[property]).locale(global_lang).format("LLLL");
                this.PublishDate_Short = moment(json[property]).locale(global_lang).format("L");
                break;
            case "AspectRatio":
                this.AspectRatio = parseFloat(json[property]).toFixed(2);
                break;
            case "Orientation":
                this.Orientation = json[property];
                this.OrientationLocale = i18n.get("orientation-" + json[property]);
                break;
            case "License":
                this.SetLicense(json[property]);
                break;
            case "DirectLinks":
                this.DirectLinks = json[property];
                $.each(this.DirectLinks, function (index, value) {
                    switch (value.Size) {
                        case 0: that.UrlFull = value.Url; break;
                        case 100: that.Url100 = value.Url; break;
                        case 200: that.Url200 = value.Url; break;
                        case 400: that.Url400 = value.Url; break;
                        case 640: that.Url640 = value.Url; break;
                        case 1024: that.Url1024 = value.Url; break;
                        case 1440: that.Url1440 = value.Url; break;
                        case 1920: that.Url1920 = value.Url; break;
                    }
                });
                break;
            case "Categories":
                this.CategoryList = new Categories(json[property], member);
                break;
            case "Topics":
                this.TopicList = new Topics(json[property], member);
                break;
            case "Location":
                this.LocationOrg = json[property];
                break;
            case "LocationId":
                if (member.Locations) {
                    this.Location = Enumerable
                        .From(member.Locations)
                        .Where(function (x) { return x.LocationId == json[property]; })
                        .Select()
                        .FirstOrDefault();
                }
                break;
            case "Event":
                this.EventOrg = json[property];
                break;
            case "EventId":
                if (member.Events) {
                    this.Event = Enumerable
                        .From(member.Events)
                        .Where(function (x) { return x.EventId == json[property]; })
                        .Select()
                        .FirstOrDefault();
                }
                break;
            case "Stories":
                //TODO: Mir ist nicht klar was hier passiert. Es gibt zwei Konstruktoren
                //und es wurde die von fotosteam-objects herangezogen, obwohl der member-Parameter fehlte
                this.StoryList = new Stories(json[property], member);
                break;
            case "Exif":
                this.Exif = new Exif(json.Exif);
                break;
            case "Description":
                var desc = json[property];
                if (desc === null) desc = "";
                this.Description = desc;
                if (desc !== "") {
                    this.MDescription = markdown.Transform(desc);
                } else {
                    this.MDescripn = "&nbsp;";
                }
                break;
            default:
                this[property] = json[property];
        }

    }
    this.SetCaption();
    this.Url = member.SteamPath + "/foto/" + this.Name;
    this.CubicleUrl = function() {
        switch (global_format) {
            case "L":
            case "XL":
                return that.Url400;
            default:
                return that.Url200;
        }
    };
    this.DummyUrl = function () {
        switch (global_format) {
            case "L":
            case "XL":
                return "/Content/Images/dummy400.png";
            default:
                return "/Content/Images/dummy200.png";
        }
    };

    if (this.AllowFullSizeDownload) {
        //this.UrlDownload = member.SteamPath + "/" + this.Name + ".jpg";
        this.UrlDownload = this.UrlFull;
    } 

    // ---------------------------------------------
    this.HasCategory = function (value) {
        var i;
        for (i in that.CategoryList) {
            if (that.CategoryList[i].CategoryType == value) {
                return true;
            }
        }
        return false;
    };
    // ---------------------------------------------
    this.HasTopic = function (value) {
        var i;
        for (i in that.TopicList) {
            if (that.TopicList[i].TopicKey == value) {
                return true;
            }
        }
        return false;
    };
    // ---------------------------------------------
    this.HasLocation = function (value) {
        var retVal = (value === null && that.Location);
        if (value) {
            if (that.Location) {
                retVal = (that.Location.LocationKey == value);
            } else {
                retVal = (value.toUpperCase() === "NOTSET");
            }
        }
        return retVal;
    };
    // ---------------------------------------------
    this.HasEvent = function(value) {
        var retVal = (value === null && that.Event);
        if (value) {
            if (that.Event) {
                retVal = (that.Event.EventKey == value);
            } else {
                retVal = (value.toUpperCase() === "NOTSET");
            }
        }
        return retVal;
    };
    // =============================================

    this.Resources = [
        {
            "Key": "gmap",
            "LocalUrl": "/Scripts/gmap3.min.js",
            "NeedOnline": true
        },
        {
            "Key": "fullsizable",
            "LocalUrl": "/Scripts/jquery.fullsizable" + global_min + ".js"
        }
    ];
    // ---------------------------------------------
    this.ShowPage = function() {

        if (global_auth_member && global_auth_member.Steam == member.Steam) {
            that.Resources.push({
                "Key": "chosen",
                "LocalUrl": "/Scripts/chosen/chosen.jquery" + global_min + ".js"
            }, {
                "Key": "chosen-css",
                "LocalUrl": "/Scripts/chosen/chosen.css"
            }, {
                "Key": "imgareaselect",
                "LocalUrl": "/Scripts/jquery.imgareaselect-0.9.10/scripts/jquery.imgareaselect" + global_min + ".js"
            }, {
                "Key": "imgareaselect-css",
                "LocalUrl": "/Scripts/jquery.imgareaselect-0.9.10/css/imgareaselect-animated.css"
            }, {
                "Key": "fotosteam-fotoedit",
                "LocalUrl": (global_env === "P") ? "/Scripts/Build/" + global_version + "/fotosteam-fotoedit.min.js"
                                                 : "/Scripts_Custom/Resource/fotosteam-fotoedit.js"
            });
        }

        resourceLoader(that.Resources, function () {
            that.SetPage();
        });
    };

    // ---------------------------------------------
    this.SetPageStyles = function (urlSuffix) {

        var url640 = (!urlSuffix) ? that.Url640 : that.Url640 + "?" + urlSuffix,
            url1024 = (!urlSuffix) ? that.Url640 : that.Url1024 + "?" + urlSuffix,
            url1440 = (!urlSuffix) ? that.Url640 : that.Url1440 + "?" + urlSuffix,
            url1920 = (!urlSuffix) ? that.Url640 : that.Url1920 + "?" + urlSuffix;

        $("head").remove("#foto-style").append(
            '<style id="foto-style" type="text/css">' +
                'body { background-color: rgb(' + that.Color + ')}' +
                '.new-comment-cmds button, .search-choice, .fotocolor:not(.force), .dropit-submenu > li > a:hover { background-color: rgba(' + that.Color + ', 0.25) !important }' +
                '.search-choice:hover, .search-choice:hover .search-choice-close:before, .fotocolor.force, .fotocolor:hover, .fotocolor:focus { background-color: rgb(' + that.Color + ') !important; }' +
            '</style>'

            //'@media only screen and (max-width: 40.063em) {div.foto {background-image: url(' + url640 + ');}}' +
            //'@media only screen and (min-width: 40.063em) and (max-width: 64em) {div.foto {background-image: url(' + url1024 + ');}}' +
            //'@media only screen and (min-width: 64.063em) and (max-width: 90em) {div.foto {background-image: url(' + url1440 + ');}}' +
            //'@media only screen and (min-width: 90.063em) {div.foto {background-image: url(' + url1920 + ');}}' +
        );
        $("body").css("background-color", "rgb(" + that.Color + ")");
    };
    // ---------------------------------------------
    this.SetPage = function () {

        member.SetPageStandards("foto", that);
        appendToTitle(that.Caption);

        that.SetPageStyles();

        var index = window.global_member.Fotos.indexOf(that),
            nextFoto, prevFoto;

        if (index >= 0) {
            nextFoto = window.global_member.Fotos[index - 1];
        }
        if (index < window.global_member.Fotos.length - 1) {
            prevFoto = window.global_member.Fotos[index + 1];
        }

        var data = {
                Foto: that,
                Member: (global_member) ? global_member : null,
                AuthMember: (global_auth_member) ? global_auth_member : null
            },
            jFoto;

        data.Foto.PLeft = data.Foto.Left * 100;
        data.Foto.PTop = data.Foto.Top * 100;
        var isEditMode =false;
        if (global_auth_member && global_auth_member.Steam == member.Steam) {
            isEditMode = true;
            //Daten vom voll geladenen Member anhängen
            data.Categories = global_member.Categories;
            data.Topics = global_member.Topics;
            data.Locations = global_member.Locations;
            data.Events = global_member.Events;
            //Foto im Edit-Modus öffnen
            jFoto = initFotoEdit(data);
        } else {
            jFoto = $(i18n.translate(fsTemplates.tFoto(data)));            

            //License
            if (that.LicenseArray.length > 0) {
                var jL = jFoto.find(".info-license span");
                $.each(that.LicenseArray, function (index, value) {
                    jL.append('<i class="icon icon-' + value + '"></i>');
                });
            }
        }

        //jFoto.find(".foto").height((Math.ceil(window.innerHeight)) + "px");

        //Header
        $("header").find("h2.title > span").empty().append(that.Caption);

        var jTitleCommands = $("header").find("h2.title > .title-commands");
        jTitleCommands.empty();

        //window.history.pushState({ fotourl: that.Url }, "", that.Url);
        //--
        if (prevFoto) {
            jTitleCommands.append(
                $('<a href="#" class="tooltip-left" title="' + i18n.get("go-prev") + '"><i class="icon icon-left-dir"></i></a>').click(function () {
                    window.history.pushState({ fotourl: prevFoto.Url }, "", prevFoto.Url);
                    prevFoto.SetPage();
                    return false;
                })
            );
        }
        //--
        if (nextFoto) {
            jTitleCommands.append(
                $('<a href="#" class="tooltip-left" title="' + i18n.get("go-next") + '"><i class="icon icon-right-dir"></i></a>').click(function () {
                    window.history.pushState({ fotourl: nextFoto.Url }, "", nextFoto.Url);
                    nextFoto.SetPage();
                    return false;
                })
            );
        }

        window.onpopstate = function (event) {
            var foto = Enumerable
                .From(window.global_member.Fotos)
                .Where(function (x) { return x.Url == window.location.href; })
                .Select()
                .FirstOrDefault();

            if (foto) { foto.SetPage(); }
        };

        $("#content").empty().append(jFoto);

        //Map
        if (that.HasLocation(null)) {
            loadMap(
                jFoto.find(".location-map").show(),
                that.Location.LocationLatitude,
                that.Location.LocationLongitude,
                false);
        }

        $.when(
            //Comments
            loadComments(that)
        ).then(function() {
            //Ratings
            getSteamData("ratings", { id: that.Id, skip: 0, take: 100 }, function (resR) {
                var dataR = {
                    Ratings: new Ratings(resR.Data)
                },
                jRList = $(i18n.translate(fsTemplates.tRatingList(dataR)));
                $(".foto-ratings").append(jRList);
            });
        }).then(function () {
            fittingContent();
            setTooltips($("header"), "left");
            setTooltips($(".foto-meta-sub"), "bottom");
        });
        
        //Resize-Binding
        //$(window).on('resize', function () {
        //    
        //});

        //Scroll-Binding
        //$(window).on('scroll', function() {
        //    
        //});

        //Key-Bindings
        global_keyBindings.clear();
        // --------------------
        global_keyBindings.add("f", i18n.get("key-full-screen"), function () {
            $(".foto-meta-sub .btn-show").click();
        });
        // --------------------
        global_keyBindings.add("r", i18n.get("key-rate"), function () {
            $(".foto-meta-sub .btn-rating").click();
        });
        // --------------------
        global_keyBindings.add("s", i18n.get("key-share"), function () {
            $(".foto-meta-sub .btn-share").click();
        });
        // --------------------
        if (prevFoto) {
            global_keyBindings.add("left", i18n.get("key-prev-photo"), function () {
                window.history.pushState({}, "", prevFoto.Url);
                prevFoto.SetPage();
                return false;
            });
        }
        // --------------------
        if (nextFoto) {
            global_keyBindings.add("right", i18n.get("key-next-photo"), function() {
                window.history.pushState({}, "", nextFoto.Url);
                nextFoto.SetPage();
                return false;
            });
        }
        // --------------------
        global_keyBindings.showCollection();

        //Popouts
        $("a.btn-popout").popout();

        //Fullsizable
        doFullsizable($("a.btn-show"));

        if (isEditMode === false) {
            $(".foto-meta-pic").click(function() {
                $("a.btn-show").click();
            });
        }
    };  
}
//----------------------------------------------
function replaceFoto(id, newFoto) {
    for (var i = 0; i < global_member.Fotos.length; i++) {
        if (global_member.Fotos[i].Id === id) {
            global_member.Fotos[i] = newFoto;
            break;
        }
    }
}
//----------------------------------------------
function updateFoto(id, fieldName, fieldValue, fieldKey) {    
    updateModel("Photo", id, fieldName, fieldValue, fieldKey, function () {
        updateLocalFoto(id, fieldName, fieldValue);
    });
}
//---
function updateLocalFoto(id, fieldName, fieldValue) {

    for (var i = 0; i < global_member.Fotos.length; i++) {
        if (global_member.Fotos[i].Id === id) {
            break;
        }
    }

    if (i !== -1) {
        switch (fieldName) {
            case "IsPrivate":
                window.global_member.Fotos[i].IsPrivate = fieldValue;
                break;
            case "AllowFullSizeDownload":
                window.global_member.Fotos[i].AllowFullSizeDownload = fieldValue;
                break;
            case "AllowCommenting":
                window.global_member.Fotos[i].AllowCommenting = fieldValue;
                break;
            case "AllowRating":
                window.global_member.Fotos[i].AllowRating = fieldValue;
                break;
            case "AllowSharing":
                window.global_member.Fotos[i].AllowSharing = fieldValue;
                break;
            case "IsForStoryOnly":
                window.global_member.Fotos[i].IsForStoryOnly = fieldValue;
                break;
            case "AllowPromoting":
                window.global_member.Fotos[i].AllowPromoting = fieldValue;
                break;
            case "Title":
                window.global_member.Fotos[i].Title = fieldValue;
                window.global_member.Fotos[i].SetCaption();
                break;
            case "Description":
                window.global_member.Fotos[i].Description = fieldValue;
                break;
            case "Color":
                window.global_member.Fotos[i].SetColor(fieldValue);
                break;
            case "Category":
                var cats = Enumerable.From(window.global_member.Categories)
                    .Where(function (x) { return x.CategoryTypeValue !== 0 && (fieldValue & x.CategoryTypeValue) === x.CategoryTypeValue; })
                    .Select()
                    .ToArray();
                window.global_member.Fotos[i].CategoryList = cats;
                break;
            case "Topics_ResetWithIdList":
                window.global_member.Fotos = [];
                $.each(fieldValue, function(index, value) {
                    window.global_member.Fotos.push(Enumerable.From(window.global_member.Topics).Where(function (x) { return x.TopicId === value; }).FirstOrDefault());
                });
                break;
            case "Topics":
                //fieldValue = positive oder negative Id > Add/Remove
                if (fieldValue > 0) {
                    for (var j = 0; j < window.global_member.Topics.length; j++) {
                        if (window.global_member.Topics[j].TopicId === Math.abs(fieldValue)) {
                            window.global_member.Fotos[i].TopicList.push(window.global_member.Topics[j]);
                            break;
                        }
                    }
                } else {
                    for (var k = 0; k < window.global_member.Fotos[i].TopicList.length; k++) {
                        if (window.global_member.Fotos[i].TopicList[k].TopicId === Math.abs(fieldValue)) {
                            window.global_member.Fotos[i].TopicList.splice(k, 1);
                            break;
                        }
                    }
                }
                break;
            case "LocationId":
                window.global_member.Fotos[i].LocationId = fieldValue;
                window.global_member.Fotos[i].Location = Enumerable
                    .From(window.global_member.Locations)
                    .Where(function (x) { return x.LocationId == fieldValue; })
                    .Select()
                    .FirstOrDefault();
                break;
            case "EventId":
                window.global_member.Fotos[i].EventId = fieldValue;
                window.global_member.Fotos[i].Event = Enumerable
                    .From(window.global_member.Events)
                    .Where(function (x) { return x.EventId == fieldValue; })
                    .Select()
                    .FirstOrDefault();
                break;
            case "License":
                window.global_member.Fotos[i].SetLicense(parseInt(fieldValue));
                break;
            default:
        }
    }
}
// =============================================
function Exif(json) {

    for (var property in json) {
        switch (property) {
            case "CaptureDate":
                if (json[property] != "0001-01-01T00:00:00") {
                    this.CaptureDate = json[property];
                    this.CaptureDate_Formatted = moment(json[property]).locale(global_lang).format("LLLL");
                    break;
                } else {
                    this.CaptureDate = "";
                    this.CaptureDate_Formatted = "";
                }
                break;
            case "Artist":
            case "Copyright": 
            case "Software":
            case "Description":
                this[property] = !json[property] ? "" : json[property];
                break;
            case "FocalLength":
                if (json[property] !== 0) {
                    this.FocalLength = Math.ceil(json[property]) + " mm";
                }
                break;
            case "FNumber":
                if (json[property] !== 0) {
                    this.FNumber = "f/" + json[property];
                }
                break;
            case "ApertureValue":
                if (json[property] !== 0) {
                    this.ApertureValue = parseFloat(json[property]).toFixed(2) + " Av";
                }
                break;
            case "ISOSpeedRatings":
                if (json[property] !== 0) {
                    this.ISOSpeedRatings = "ISO " + json[property];
                }
                break;
            case "ExposureTime":
                if (json[property] !== 0) {
                    var et = parseFloat(json[property]);
                    if (et > 1) {
                        this.ExposureTime = et + " " + i18n.get("seconds-short");
                    } else {
                        this.ExposureTime = "1/" + 1 / et + " " + i18n.get("seconds-short");
                    }
                }
                break;
            case "ExposureBiasValue":
                if (json[property] !== 0) {
                    this.ExposureBiasValue = Math.ceil(json[property]) + " EV";
                }
                break;
            case "XResolution":
                if (json[property] !== 0) {
                    this.XResolution = Math.ceil(json[property]);
                }
                break;
            case "YResolution":
                if (json[property] !== 0) {
                    this.YResolution = Math.ceil(json[property]);
                }
                break;
            case "ResolutionUnit":
                if (parseInt(json[property]) === 0) {
                    this.ResolutionUnit = "";
                } else {
                    this.ResolutionUnit = i18n.get(ResolutionUnits[parseInt(json[property])]);
                }
                break;
            case "ExposureProgram":
                if (json[property] !== 0) {
                    this.ExposureProgram = i18n.get(ExposurePrograms[parseInt(json[property])]);
                }
                break;
            case "ExposureMode":
                if (json[property] !== 0) {
                    this.ExposureMode = i18n.get(ExposureModes[parseInt(json[property])]);
                }
                break;
            case "MeteringMode":
                if (json[property] !== 0) {
                    this.MeteringMode = i18n.get(MeteringModes[parseInt(json[property])]);
                }
                break;
            case "Latitude":
                this.Latitude = parseFloat(json[property]);
                this.LatitudeDeg = convertDecDeg(this.Latitude);
                break;
            case "Longitude":
                this.Longitude = parseFloat(json[property]);
                this.LongitudeDeg = convertDecDeg(this.Longitude);
                break;
            default:
                this[property] = json[property];
                break;
        }
        this.Aperture = this.FNumber + (this.ApertureValue) ? " (" + this.ApertureValue + ")" : "";
        this.Resolution = (this.XResolution) ? this.XResolution + " x " + this.YResolution : "";
    }    
}

// =============================================
function Fotos(json, member) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Foto(x, member);
        });
    }
    return list;
}
// =============================================
function MemberFoto(json) {
    var that = this;

    for (var property in json) {
        switch (property) {
            case "Color":
                this.Color = (json[property]) ? json[property] : "34,34,34";
                var arrColor = this.Color.split(",");
                this.ColorR = arrColor[0];
                this.ColorG = arrColor[1];
                this.ColorB = arrColor[2];
                this.ColorHex = "#" + rgbToHex(this.ColorR, this.ColorG, this.ColorB);
                break;
            case "PublishDate":
                this[property] = json[property];
                this.PublishDate_Formatted = moment(json[property]).locale(global_lang).format("LLLL");
                break;
            case "DirectLinks":
                this.DirectLinks = json[property];
                $.each(this.DirectLinks, function (index, value) {
                    switch (value.Size) {
                        case 0: that.UrlFull = value.Url; break;
                        case 100: that.Url100 = value.Url; break;
                        case 200: that.Url200 = value.Url; break;
                        case 400: that.Url400 = value.Url; break;
                        case 640: that.Url640 = value.Url; break;
                        case 1024: that.Url1024 = value.Url; break;
                        case 1440: that.Url1440 = value.Url; break;
                        case 1920: that.Url1920 = value.Url; break;
                    }
                });
                break;
            default:
                this[property] = json[property];
        }
    }
    this.IsPrivate = false;
    this.Caption = (this.Title) ? this.Title : this.Name;
    this.Url = "/" + this.Alias + "/foto/" + this.Name;
    this.CubicleUrl = function () {
        switch (global_format) {
            case "L":
            case "XL":
                return that.Url400;
            default:
                return that.Url200;
        }
    };
    this.DummyUrl = function () {
        switch (global_format) {
            case "L":
            case "XL":
                return "/Content/Images/dummy400.png";
            default:
                return "/Content/Images/dummy200.png";
        }
    };

}
// =============================================
function MemberFotos(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new MemberFoto(x);
        });
    }
    return list;
}
// =============================================
function showExtendedExif() {
    $("#ExifExtendedDetailsButtons").hide();
    $("#ExifExtendedDetails").show();
}


