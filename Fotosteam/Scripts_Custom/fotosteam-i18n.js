
// =============================================
function i18n_Translator(lang, callback) {
    var that = this;

    this.language = lang;
    this.Values = null;

    // ---------------------------------------------
    this.Init = function() {

        $.getJSON("/Content/Locales/" + that.language + ".json", function(ldata) {
            that.Values = ldata.Values;
            callback();
        });
    };

    // ---------------------------------------------
    this.get = function(key, args) {
        key = key.toLowerCase();

        if (that.Values) {
            for (var k in that.Values) {
                if (k == key) {

                    var s = that.Values[k];

                    if (args !== undefined && args.length !== 0) {
                        var a = args.split("|");
                        for (var i = 0; i < a.length; i++) {
                            s = s.replace("%" + (i + 1), a[i]);
                        }
                    }

                    return s;
                }
            }
        }
        return null;
    };

    // ---------------------------------------------
    this.translate = function(obj) {

        if (obj instanceof jQuery) {
            return TranslateJqueryObject(obj);
        } else if (typeof obj == 'string' || obj instanceof String) {
            return TranslateString(obj);
        } else {
            return null;
        }

    };
    //---
    function TranslateJqueryObject(jE) {

        jE.find("*[data-i18n]").each(function () {
            var key = $(this).data("i18n").toLowerCase(),
                args = $(this).data("i18n-args"),
                text = that.get(key, args);

            if ($(this).children().length === 0) {
                $(this).html(text);
            } else {
                $(this).contents().filter(function () {
                    return this.nodeType == 3;
                }).each(function () {
                    this.textContent = text;
                });
            }
        });

        //TODO: warum funktioniert das nicht!? (siehe Common > Error)
        jE.find("*[data-i18n-value]").each(function () {
            var key = $(this).data("i18n-value").toLowerCase(),
                args = String($(this).data("i18n-value-args")),
                value = that.get(key, args);
            $(this).val(value);
        });

        jE.find("*[data-i18n-placeholder]").each(function () {
            var key = $(this).data("i18n-placeholder").toLowerCase(),
                args = String($(this).data("i18n-placeholder-args")),
                placeholder = that.get(key, args);
            $(this).prop("placeholder", placeholder);
        });

        jE.find("*[data-i18n-title]").each(function () {
            var key = $(this).data("i18n-title").toLowerCase(),
                args = String($(this).data("i18n-title-args")),
                title = that.get(key, args);
            $(this).prop("title", title);
        });

        return jE;
    }
    //---
    function TranslateString(str) {
        var jE = $("<div></div>").html(str),
            jX = TranslateJqueryObject(jE);
        return jX.html();
    }

// ---------------------------------------------
    that.Init();

}