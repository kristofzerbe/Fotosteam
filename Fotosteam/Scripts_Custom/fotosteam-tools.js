// ---------------------------------------------
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) == str;
    };
}
// ---------------------------------------------
if (typeof String.prototype.endsWith != 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) == str;
    };
}
// ---------------------------------------------
if (typeof Array.prototype.remove != 'function') {
    Array.prototype.remove = function(v) {
        this.splice(this.indexOf(v) == -1 ? this.length : this.indexOf(v), 1);
    };
}
// ---------------------------------------------
function createGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
// ---------------------------------------------
function addIndex(the_list) {
    for (var i = 0; i < the_list.length; i++) {
        the_list[i].Idx = (function(in_i) { return in_i + 1; })(i);
    }
    return the_list;
}
// ---------------------------------------------
function trimFotosToGrid(jContainer, jWrapper, fItemsRow) {

    var itemsRow = fItemsRow();

    var w = jContainer.width(),
        x = w / itemsRow,
        y = Math.ceil(x),
        z = y * itemsRow;

    jWrapper.width(z);
}// ---------------------------------------------
function r2d(value) {
    return (Math.round(value * 100) / 100);
}
// ---------------------------------------------
function randomString(max) {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < max; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}
// ---------------------------------------------
function randomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
// ---------------------------------------------
function UrlSafeFileName(value) {
    return MakeUrlSafe(getFileName(value));
}
// ---------------------------------------------
function getFileName(value) {
    return value.substr(0, value.lastIndexOf('.'));
}
// ---------------------------------------------
function MakeUrlSafe(value) {
    //IMPORTANT: Changes MUST transfered to FotosteamService.Extensions.MakeUrlSafe
    if (!value) { return ""; }
    value = value.toLowerCase()
                 .replace(/ä/gi, "ae")
                 .replace(/ö/gi, "oe")
                 .replace(/ü/gi, "ue")
                 .replace(/[^a-zA-Z0-9]+/gi, "-");
    return encodeURIComponent(value);
}
// ---------------------------------------------
function validEmail(elementValue) {
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailPattern.test(elementValue);
}
// ---------------------------------------------
function validUrl(elementValue) {
    var urlPattern = /^\b((?:https?:\/\/|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}\/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()[\]{};:'".,<>?«»“”‘’]))$/;
    return urlPattern.test(elementValue);
}
// ---------------------------------------------
function stripHtml(sText) {
    return sText.replace(/((<|&lt;).*?(>|&gt;))/gi, "");
}
// ---------------------------------------------
function convertContentEditableText(jHtml) {
    //Encode- und Markdown-kompatible Formatierung
    var sText = jHtml.html();

    //BLINK:   xxx<div>yyy</div><div><br></div><div>zzz</div>    
    sText = sText.replace(/<div><br><\/div><div>/gi, "&#10;&#10;"); //Absatz
    sText = sText.replace(/<div>/gi, "  &#10;"); //Linefeed (zwei Leerzeichen vor den LF für Markdown)
    sText = sText.replace(/<\/div>/gi, ""); //aufräumen

    //FIREFOX: xxx<br>yyy<br><br>zzz<br>
    sText = sText.replace(/<br><br>/gi, "&#10;&#10;"); //Absatz
    sText = sText.replace(/<br>/gi, "  &#10;"); //Linefeed (zwei Leerzeichen vor den LF für Markdown)

    //IE-11:   <p>xxx<br>yyy</p><p>zzz</p>
    sText = sText.replace(/<\/p><p>/gi, "&#10;&#10;"); //Absatz
    sText = sText.replace(/(<p>|<\/p>)/gi, ""); //aufräumen

    //=>       xxx&nbsp;&nbsp;&#10;yyy&#10;&#10;zzz

    sText = stripHtml(sText).trim();
    return sText;
}
// ---------------------------------------------
function pad(number, length) {
    //konvertiert eine Zahl in einen String mit führenden Nullen
    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}
// ---------------------------------------------
function getDayNumber() {
    var d = new Date(),
        s = d.getFullYear() + pad(d.getMonth() + 1, 2) + pad(d.getDate(), 2);
    return s;
}
// ---------------------------------------------
function convertDecDeg(v, tipo) {
    if (!tipo) tipo = 'N';
    var deg;
    deg = v;
    if (!deg) {
        return "";
    } else if (deg > 180 || deg < 0) {
        // convert coordinate from north to south or east to west if wrong tipo
        return convertDecDeg(-v, (tipo == 'N' ? 'S' : (tipo == 'E' ? 'W' : tipo)));
    } else {
        var gpsdeg = parseInt(deg);
        var remainder = deg - (gpsdeg * 1.0);
        var gpsmin = remainder * 60.0;
        var D = gpsdeg;
        var M = parseInt(gpsmin);
        var remainder2 = gpsmin - (parseInt(gpsmin) * 1.0);
        var S = parseInt(remainder2 * 60.0);
        return D + "&deg; " + M + "' " + S + "'' " + tipo;
    }
}
// ---------------------------------------------
/* Get The Rendered Style Of An Element
 * http://robertnyman.com/2006/04/24/get-the-rendered-style-of-an-element/
 */
function getStyle(oElm, strCssRule) {
    var strValue = "";
    if (document.defaultView && document.defaultView.getComputedStyle) {
        strValue = document.defaultView.getComputedStyle(oElm, "").getPropertyValue(strCssRule);
    }
    else if (oElm.currentStyle) {
        strCssRule = strCssRule.replace(/\-(\w)/g, function (strMatch, p1) {
            return p1.toUpperCase();
        });
        strValue = oElm.currentStyle[strCssRule];
    }
    return strValue;
}
// colorThief-Hilfsmethoden ---------------------------------------------
//function getDomColor(sImageUrl, fSuccess) {

//    try {
//        var image = new Image();
//        image.crossOrigin = "Anonymous";
//        image.onload = function () {
//            // image has been loaded
//            var colorThief = new ColorThief(),
//                domColor = colorThief.getColor(image);
//            fSuccess(domColor);
//        };
//        image.onerror = function() {
//            fSuccess([34, 34, 34]);
//        };
//        $(image).attr("src", sImageUrl);
//    }
//    catch(err) {
//    }    
//}
//// ---------------------------------------------
//function getDomColorBg(jSource, fSuccess) {

//    var re, ar, bi;

//    try {
//        re = /url\([^)]+\)/;
//        ar = re.exec(jSource.css("background-image"));
//        bi = ar[0].replace(/^url\(["']?/, '').replace(/["']?\)$/, '');

//        getDomColor(bi, fSuccess);
//    }
//    catch(err) {
//    }
//}
//// ---------------------------------------------
//function assignDomColorBg(jSource, jTarget) {
//    getDomColorBg(jSource, function(domColor) {
//        jTarget.css("background-color", "rgb(" + domColor + ")");
//    });
//}
// ---------------------------------------------
function rgbToHex(R, G, B) { return toHex(R) + toHex(G) + toHex(B); }
function toHex(n) {
    n = parseInt(n, 10);
    if (isNaN(n)) return "00";
    n = Math.max(0, Math.min(n, 255));
    return "0123456789ABCDEF".charAt((n - n % 16) / 16) + "0123456789ABCDEF".charAt(n % 16);
}
// ---------------------------------------------
function hexToR(h) { return parseInt((cutHex(h)).substring(0, 2), 16); }
function hexToG(h) { return parseInt((cutHex(h)).substring(2, 4), 16); }
function hexToB(h) { return parseInt((cutHex(h)).substring(4, 6), 16); }
function cutHex(h) { return (h.charAt(0) == "#") ? h.substring(1, 7) : h; }
// ---------------------------------------------
function createGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
// ---------------------------------------------
function getGeoComponent(results, searchType, attribute) {

    //Location
    //https://developers.google.com/maps/documentation/javascript/geocoding?hl=de#GeocodingResponses

    var retVal = "";

    $.each(results, function (i, result) {
        //Suche auf 1. Ebene
        var type1 = Enumerable.From(result.types)
            .Where(function (x) { return x === searchType; })
            .ToArray();

        if (type1.length !== 0) {
            retVal = result.address_components[0][attribute];
            return;
        } else {

            $.each(result.address_components, function (j, component) {
                //Suche auf 2. Ebene
                var type2 = Enumerable.From(component.types)
                    .Where(function (x) { return x === searchType; })
                    .ToArray();

                if (type2.length !== 0) {
                    retVal = component[attribute];
                    return;
                }
            });
        }
    });
    return retVal;
}
// ---------------------------------------------
function loadScript(url, callback) {

    var script = document.createElement("script");
    script.type = "text/javascript";

    if (script.readyState) {  //IE
        script.onreadystatechange = function () {
            if (script.readyState == "loaded" || script.readyState == "complete") {
                script.onreadystatechange = null;
                callback();
            }
        };
    } else {  //Others
        script.onload = function () {
            callback();
        };
    }
    script.src = url;
    document.getElementsByTagName("head")[0].appendChild(script);
}
// ---------------------------------------------
function loadCss(url, callback) {

    var css = document.createElement("link");
    css.type = "text/css";
    css.rel = "stylesheet";

    if (css.readyState) {  //IE
        css.onreadystatechange = function () {
            if (css.readyState == "loaded" || css.readyState == "complete") {
                css.onreadystatechange = null;
                callback();
            }
        };
    } else {  //Others
        css.onload = function () {
            callback();
        };
    }

    css.href = url;
    document.getElementsByTagName("head")[0].appendChild(css);
}
// ---------------------------------------------
function resourceLoader(resources, fDone) {

    function load(i) {
        if (i <= (resources.length - 1)) {

            var res = resources[i];

            if (res.RemoteUrl && res.RemoteUrl.length !== 0 && global_online) {
                res.Url = res.RemoteUrl;
            } else {
                if (!res.NeedOnline || (res.NeedOnline && global_online)) {
                    res.Url = res.LocalUrl + "?r=" + global_config.resourceSuffix();
                }
            }

            if (res.Key.endsWith("css")) {
                loadCss(res.Url, function () { res.Loaded = true; load(i + 1); });
            } else {
                loadScript(res.Url, function () { res.Loaded = true; load(i + 1); });
            }

        } else {
            fDone();
        }
    }

    if (resources) {
        load(0);
    } else {
        fDone();
    }
}
// ---------------------------------------------
function showDialogBar(j) {

    if (!$("#dialogbar").length) {
        var db = $('<div id="dialogbar"></div>');
        db.append(j);
        $("body").append(db);
        setTimeout(function() {
            db.addClass("open");
        }, 100);
        $("#dialogbar").css({ height: db.innerHeight() }).data("init-height", db.innerHeight());
    }
}
// ---------------------------------------------
function removeDialogBar() {
    $("#dialogbar").removeClass("open");
    setTimeout(function() {
        $("#dialogbar").remove();
    }, 100);
    $("html").removeClass("dim");
}
// ---------------------------------------------
function toastMessageText(text) { // http://clifordtube.com/toasty/index.html
    $().toasty({
        message: text,
        position: 'bl',
        autoHide : 3000
    });
}
// ---------------------------------------------
function toastMessage(msgLocale) {
    toastMessageText(i18n.get(msgLocale));
}
// ---------------------------------------------
function toastNoticeText(text) {
    $().toasty({
        message: text,
        position: 'bl'
    });
}
// ---------------------------------------------
function toastNotice(msgLocale) {
    toastNoticeText(i18n.get(msgLocale));
}
// ---------------------------------------------
function toastHideAll() {
    $().toasty("hideAll");
}
// ---------------------------------------------
function toastErrorText(text, titleLocale) {
    var isHtmlResponse = text.startsWith("<!DOCTYPE html");
    if (isHtmlResponse) { text = "Resource Not Found"; }
    toastErrorTextAndTitle(text, (titleLocale !== undefined) ? i18n.get(titleLocale) : i18n.get("error"));
}
// ---------------------------------------------
function toastErrorTextAndTitle(text, title) {
    $().toasty({
        message: text,
        position: 'tc',
        type: 'warning',
        title: (title !== undefined) ? title : i18n.get("error")
    });
}
// ---------------------------------------------
function toastError(msgLocale, titleLocale) {
    toastErrorText(i18n.get(msgLocale), titleLocale);
}
// ---------------------------------------------
function popoutAffirm(affirmLocale, fOK) {

    $("#affirm-popout").remove();

    var data = {
        TitleLocale:    affirmLocale + "-affirm-title",
        MessageLocale:  affirmLocale + "-affirm-message",
        OkLocale:       affirmLocale + "-affirm-ok"
    };

    var sAffirm = i18n.translate(fsTemplates.tPopoutAffirmation(data)),
        jAffirm = $(sAffirm);

    jAffirm.click(function () {
        closePopout(); //wird bei jedem Button mit-getriggert, daher kein Cancel-Binding notwendig
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 27) { closePopout(); }
    });

    function closePopout() {

        if (fOK !== undefined) { fOK(); }

        jAffirm.removeClass("popout-show");
        setTimeout(function () {
            jAffirm.remove();
        }, 1000);
        $("body").removeClass("noscroll");
    }

    $("body").append(jAffirm).addClass("noscroll");
    setTimeout(function () {
        jAffirm.addClass("popout-show");
    }, 100);

}
// ---------------------------------------------
function popoutConfirm(confirmLocale, fOK, fCancel) {

    $("#confirm-popout").remove();

    var data = {
        TitleLocale:    confirmLocale + "-confirm-title",
        MessageLocale:  confirmLocale + "-confirm-message",
        OkLocale:       confirmLocale + "-confirm-ok",
        CancelLocale:   confirmLocale + "-confirm-cancel",
    };

    var sConfirm = i18n.translate(fsTemplates.tPopoutConfirmation(data)),
        jConfirm = $(sConfirm);

    var event = "cancel";

    jConfirm.find("#confirm-ok").click(function () {
        event = "ok";
        fOK();
    });

    jConfirm.click(function () {
        closePopout(); //wird bei jedem Button mit-getriggert, daher kein Cancel-Binding notwendig
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 27) { closePopout(); }
    });

    function closePopout() {

        if (event === "cancel") { if (fCancel !== undefined) { fCancel(); } }

        jConfirm.removeClass("popout-show");
        setTimeout(function () {
            jConfirm.remove();
        }, 1000);
        $("body").removeClass("noscroll");
    }

    $("body").append(jConfirm).addClass("noscroll");
    setTimeout(function () {
        jConfirm.addClass("popout-show");
    }, 100);
}
// ---------------------------------------------
function popoutContent(contentLocale, contentHtml) {

    $("#content-popout").remove();

    var data = {
        TitleLocale: contentLocale + "-content-title",
        Content: contentHtml,
        CloseLocale: "close"
    };

    var sDialog = i18n.translate(fsTemplates.tPopoutContent(data)),
    jDialog = $(sDialog);

    jDialog.find("a.close").click(function () {
        closePopout(); 
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 27) { closePopout(); }
    });

    function closePopout() {
        jDialog.removeClass("popout-show");
        setTimeout(function () {
            jDialog.remove();
        }, 1000);
        $("body").removeClass("noscroll");
    }

    $("body").append(jDialog).addClass("noscroll");
    setTimeout(function () {
        jDialog.addClass("popout-show");
    }, 100);

}
// ---------------------------------------------
(function ($) {
    $.fn.popout = function (fPrepare) {

        return this.each(function() {

            var $this = $(this),
                $popout = $("#" + $this.data("popout"));

            $this.click(function () {

                if (fPrepare !== undefined) { fPrepare(); }

                $popout.addClass("popout-show");
                $("body").addClass("noscroll");
            });

            $popout.click(function() {
                closePopout();
            });

            $(document).keydown(function (e) {
                // ESCAPE key pressed
                if (e.keyCode == 27) {
                    closePopout();
                }
            });

            function closePopout() {
                $popout.removeClass("popout-show");
                $("body").removeClass("noscroll");
            }
        });
    };
})(jQuery);
// ---------------------------------------------
/**
 * iosCheckbox
 * Author: Ron Masas
 * http://www.jqueryscript.net/form/iOS-Style-Checkbox-Plugin-with-jQuery-CSS3-iosCheckbox-js.html
 */
(function ($) {
    $.fn.extend({
        iosCheckbox: function (options) {

            var defaults = {
                change: function(e, checkValue) {}
            };
            options = $.extend(defaults, options);

            $(this).each(function () {
                var org_checkbox = $(this);
                var ios_checkbox = $("<div>", { class: 'ios-ui-select' }).append($("<div>", { class: 'inner' }));

                if (org_checkbox.is(":checked")) {
                    ios_checkbox.addClass("checked");
                }
                org_checkbox.hide().after(ios_checkbox);

                ios_checkbox.click(function () {
                    ios_checkbox.toggleClass("checked");
                    if (ios_checkbox.hasClass("checked")) {
                        org_checkbox.prop('checked', true);
                    } else {
                        org_checkbox.prop('checked', false);
                    }
                    options.change(org_checkbox, org_checkbox.prop('checked'));
                });
            });
        }
    });
})(jQuery);
// ---------------------------------------------
(function ($) {
    'use strict';
    /**
     * Makes contenteditable elements within a container generate change events.
     * https://gist.github.com/tom--/4007222
     * 
     * When you do, e.g. $obj.editable(), all the DOM elements with attribute contenteditable
     * that are children of the DOM element $obj will trigger a change event when their
     * contents is edited and changed.
     *
     * See: http://html5demos.com/contenteditable
     *
     * @return {*}
     */

    $.fn.extend({
        editable: function (options) {

            var defaults = {
                change: function (e) { }
            };
            options = $.extend(defaults, options);

            $(this).each(function () {
                $(this).on('focus', function () {
                    var $this = $(this);
                    $this.data('beforeContentEdit', $this.html());
                });
                $(this).on('blur', function () {
                    var $this = $(this);
                    if ($this.data('beforeContentEdit') !== $this.html()) {
                        $this.removeData('beforeContentEdit');
                        options.change($this);
                    }
                });
            });
        }
    });
}(jQuery));
// ---------------------------------------------
/* Detecting a swipe (left, right, top or down) using touch
 * http://www.javascriptkit.com/javatutors/touchevents2.shtml
 */
function swipedetect(el, threshold, restraint, allowedTime, callback) {

    var touchsurface = el,
        swipedir,
        startX,
        startY,
        distX,
        distY,
        //threshold = 150, //required min distance traveled to be considered swipe
        //restraint = 100, // maximum distance allowed at the same time in perpendicular direction
        //allowedTime = 300, // maximum time allowed to travel that distance
        elapsedTime,
        startTime,
        handleswipe = callback || function() {};

    touchsurface.addEventListener('touchstart', function(e) {
        var touchobj = e.changedTouches[0];
        swipedir = 'none';
        distX = 0;
        distY = 0;
        startX = touchobj.pageX;
        startY = touchobj.pageY;
        startTime = new Date().getTime(); // record time when finger first makes contact with surface
        e.preventDefault();
    }, false);

    touchsurface.addEventListener('touchmove', function(e) {
        e.preventDefault(); // prevent scrolling when inside DIV
    }, false);

    touchsurface.addEventListener('touchend', function(e) {
        var touchobj = e.changedTouches[0];
        distX = touchobj.pageX - startX; // get horizontal dist traveled by finger while in contact with surface
        distY = touchobj.pageY - startY; // get vertical dist traveled by finger while in contact with surface
        elapsedTime = new Date().getTime() - startTime; // get time elapsed
        if (elapsedTime <= allowedTime) { // first condition for awipe met
            if (Math.abs(distX) >= threshold && Math.abs(distY) <= restraint) { // 2nd condition for horizontal swipe met
                swipedir = (distX < 0) ? 'left' : 'right'; // if dist traveled is negative, it indicates left swipe
            } else if (Math.abs(distY) >= threshold && Math.abs(distX) <= restraint) { // 2nd condition for vertical swipe met
                swipedir = (distY < 0) ? 'up' : 'down'; // if dist traveled is negative, it indicates up swipe
            }
        }
        handleswipe(swipedir);
        e.preventDefault();
    }, false);
}

//USAGE:
/*
var el = document.getElementById('someel')
swipedetect(el, function(swipedir){
    //swipedir contains either "none", "left", "right", "top", or "down"
    if (swipedir =='left')
        alert('You just swiped left!')
})
*/