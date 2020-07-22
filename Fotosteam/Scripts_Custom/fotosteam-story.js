
var LedgeTemplates = {
    S: 0,
    LL: 1,
    PL: 2,
    LP: 3,
    LPP: 4,
    PLP: 5,
    PPL: 6
}; Object.freeze(LedgeTemplates);

// =============================================
function Story(json, member) {
    var that = this;

    for (var property in json) {
        switch (property) {
            case "Chapters":
                this.Chapters = new Chapters(json[property]);
                break;
            default:
                this["Story" + property] = json[property];
        }
    }
    this.StoryKey = MakeUrlSafe(this.StoryName);
    this.StoryUrl = member.SteamPath + "/story/" + this.StoryKey;
    this.StoryFoto = new Foto(that.StoryHeaderPhoto, member);

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
    this.ShowPage = function(chapterKey) {

        //Header
        //var queryF = Enumerable
        //    .From(window.global_member.Fotos)
        //    .Where(function(x) { return x.Name.toLowerCase() == that.StoryHeaderPhotoName.toLowerCase(); })
        //    .Select()
        //    .FirstOrDefault();

        resourceLoader(that.Resources, function () {
            global_member.SetPageStandards("stories", that.StoryFoto);
            appendToTitle(that.StoryName);

            that.SetPage(chapterKey);
        });

    };

    // ---------------------------------------------
    this.SetPage = function(chapterKey) {

        $("body").css("background-color", "rgb(" + that.StoryFoto.Color + ")");
        $("head").append(
            '<style type="text/css">' +
                '#content a { color: ' + "rgb(" + that.StoryFoto.Color + ")" + ' }' +
            '</style>'
        );

        //Header
        $("header").find("h2.title > span").append(that.StoryName);
        $("header").find("h2.title > small").append(i18n.get("story"));

        var chapter = null;
        if (chapterKey === null) {
            chapter = that.Chapters[0];
        } else {
            chapter = Enumerable
                .From(that.Chapters)
                .Where(function(x) { return x.ChapterKey == chapterKey; })
                .Select()
                .FirstOrDefault();
        }

        var prevChapters = Enumerable
            .From(that.Chapters)
            .Where(function(x) { return x.Idx < chapter.Idx; })
            .Select().ToArray();
        var nextChapters = Enumerable
            .From(that.Chapters)
            .Where(function(x) { return x.Idx > chapter.Idx; })
            .Select().ToArray();


        var data = {
            Story: that,
            CurrentChapter: chapter,
            PrevChapter: prevChapters[prevChapters.length -1],
            PrevChapterClass: (prevChapters[prevChapters.length -1]) ? "": "empty",
            PrevChapters : prevChapters,
            NextChapter: nextChapters[0],
            NextChapterClass: (nextChapters[0]) ? "": "empty",
            NextChapters: nextChapters
        };

        var jStory = $(i18n.translate(fsTemplates.tStory(data)));

        if (that.Chapters.length < 2) {
            jStory.find(".chapters").hide();
        }

        if (chapter) {
            var jLedges = jStory.find(".chapter .ledges");

            $.each(chapter.Ledges, function(index, ledge) {
                var jLedge = $('<div class="ledge ledge-template-' + ledge.Template + '" id="L' + ledge.Order + '"></div>');
                var t = ledge.Template.split("");

                $.each(t, function (idx, brickFormat) {
                    var brick = ledge.Bricks[idx];
                    var jBrick = $('<div class="brick brick-' + brick.Type + '" id="L' + ledge.Order + 'B' + brick.Order + '"></div>');
                    switch (brickFormat) {
                        case "L": jBrick.addClass("landscape"); break;
                        case "P": jBrick.addClass("portrait"); break;
                        default:
                    }
                    switch (brick.Type) {
                        case "text":
                            var jContent = $('<div class="content-box"></div>');
                            
                            var mdText = Encoder.htmlDecode(brick.Text),
                                text = markdown.Transform(mdText);
                            jContent.append(text);
                            jBrick.append(jContent);
                            break;

                        case "photo":
                            var photo = null,
                                url = "";
                            if (brick.Photo) {
                                photo = new Foto(brick.Photo, member);
                                switch (global_format) {

                                    case "XL": //ab 1440
                                        url = photo.Url1920; 
                                        switch (t.length) {
                                            case 2:
                                            case 3:
                                                url = photo.Url1440; break;
                                            default:
                                         }
                                        break;

                                    case "L":  //bis 1440                                    
                                        url = photo.Url1440;
                                        switch (t.length) {
                                            case 2:
                                            case 3:
                                                url = photo.Url1024; break;
                                            default:
                                        }
                                        break;

                                    case "M": //bis 1024
                                        url = photo.Url1024;
                                        switch (t.length) {
                                            case 2:
                                            case 3:
                                                url = photo.Url640; break;
                                            default:
                                        }
                                        break;

                                    default: //S = bis 640
                                        url = photo.Url640;
                                        switch(t.length) {
                                            case 2:
                                            case 3:
                                                if (brickFormat === "P") { url = photo.Url1024; } break;
                                            default:
                                        }

                                }
                                jBrick.css({
                                    "background-image": "url(" + url + ")",
                                    "cursor": "pointer"
                                });
                                jBrick.click(function () {
                                    window.location.href = photo.Url;
                                });
                            }
                            break;

                        case "map":
                            loadMap(jBrick, brick.Latitude, brick.Longitude, false);
                            break;

                        default:
                    }
                    jLedge.append(jBrick);
                });
                jLedges.append(jLedge);
            });

        }

        $("#content").empty().append(jStory);

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

}
// =============================================
function Chapter(json) {
    if (json) {
        for (var property in json) {
            switch (property) {
                case "Name":
                    this.ChapterName = json[property];
                    break;
                case "Ledges":
                    this.Ledges = new Ledges(json[property]);
                    break;
                default:
                    this[property] = json[property];
            }
        }
        this.ChapterKey = MakeUrlSafe(this.ChapterName);
    }
}
//---
function Chapters(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Chapter(x);
        });
    }
    list = addIndex(list);
    return Enumerable.From(list).OrderBy(function (x) { return x.Order; }).ToArray();
}
// =============================================
function Ledge(json) {
    if (json) {
        for (var property in json) {
            switch (property) {
                case "Bricks":
                    this.Bricks = new Bricks(json[property]);
                    break;
                default:
                    this[property] = json[property];
            }
        }
    }
}
//---
function Ledges(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Ledge(x);
        });
    }
    return Enumerable.From(list).OrderBy(function (x) { return x.Order; }).ToArray();
}
// =============================================
function Brick(json) {
    if (json) {
        for (var property in json) { this[property] = json[property]; }
    }
}
//---
function Bricks(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Brick(x);
        });
    }
    return Enumerable.From(list).OrderBy(function (x) { return x.Order; }).ToArray();
}
