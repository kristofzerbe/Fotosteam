/* Tabulous
 * http://git.aaronlumsden.com/tabulous.js/
 * optimized by Kristof Zerbe
 */
; (function ($, window, document, undefined) {

    var pluginName = "tabulous",
        defaults = {
            baseUrl: "/",
            firstTab: null,
            effect: 'scale'
            //effect: 'scaleup'
            //effect: 'left'
            //effect: 'flip'
        };

    function Plugin( element, options ) {
        this.element = element;
        this.$elem = $(this.element);
        this.options = $.extend( {}, defaults, options );
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype = {

        init: function() {
            var that = this;

            var tabList = this.$elem.find("ul:first-child"),
                allTabs = tabList.find('a'),
                container = this.$elem.find('.tabulous-container'),
                allShelfs = container.find('div.tabulous-shelf');
                //,firstShelf = container.find('div.tabulous-shelf:first');

            var firstLink = "#" + this.options.firstTab;
            window.history.replaceState({ link: firstLink }, "", this.options.baseUrl + firstLink);

            tabList.find("li a[href='#" + this.options.firstTab + "']")
                .addClass('tabulous-active');
            
            tabList.find('li:last-child').after('<span class="tabulous-clear"></span>');

            //container.css('height', firstShelf.height() + 'px');

            container.find("div.tabulous-shelf#" + this.options.firstTab)
                .css("display", "block");

            container.find("div.tabulous-shelf").not("#" + this.options.firstTab)
                .addClass('tabulous-hide' + this.options.effect);

            var maxHeight = -1;
            allShelfs.each(function() {
                maxHeight = maxHeight > $(this).height() ? maxHeight : $(this).height();
            });
            container.css('height', maxHeight + 'px');
            container.find('div.tabulous-shelf').css('height', maxHeight + 'px');

            // ----------------------------------------------------------
            function goTab(jE, baseUrl, effect, setHistory) {

                var currentTab = jE,
                    wrapper = currentTab.parent().parent().parent(),
                    tabUrl = currentTab.attr('href'),
                    currentShelf = wrapper.find('div' + tabUrl);

                container.addClass('tabulous-transition');
                allTabs.removeClass('tabulous-active');
                currentTab.addClass('tabulous-active');

                if (setHistory) {
                    window.history.pushState({ taburl: tabUrl }, "", baseUrl + tabUrl);
                }

                allShelfs
                    .removeClass('tabulous-show' + effect)
                    .addClass('tabulous-make-transist')
                    .addClass('tabulous-hide' + effect);
                setTimeout(function () {
                    allShelfs.hide();
                    currentShelf
                        .addClass('tabulous-make-transist')
                        .addClass('tabulous-show' + effect);
                    setTimeout(function () {
                        currentShelf.show();
                        //container.css('height', currentShelf.height() + 'px');
                    }, 300);
                }, 300);
            }
            // ----------------------------------------------------------
            function setOriginalTabWidths() {
                tabList.find('li').each(function () {
                    $(this).data("original-width", $(this).width());
                });                
            }
            // ----------------------------------------------------------
            function equalizeTabs() {
                var totalW = 0,
                    totalCurrentW = 0,
                    tabCount = tabList.find('li').length,
                    tabListW = tabList.width();

                tabList.find('li').each(function () {
                    totalW += parseInt($(this).data("original-width"));
                    totalCurrentW += $(this).width();
                });

                var isFullRow = (totalW / tabCount) === tabListW,
                    isTotalRow = (totalW - tabListW) < 0,
                    isFloating = (getStyle(tabList.find('li:first')[0], "float") !== "none");

                var rows = Math.ceil(totalW / tabListW);

                var maxItems = Math.ceil(tabCount / rows);
                var minItems = Math.floor(tabCount / rows);

                console.clear();
                console.log(
                    "tabListW: " + tabListW + "\n" +
                    "totalW: " + totalW + "\n" +
                    "totalCurrentW: " + totalCurrentW + "\n" +
                    "rows: " + rows + "\n" +
                    "tabCount: " + tabCount + "\n" +
                    "maxItems: " + maxItems + "\n" +
                    "minItems: " + minItems + "\n" +
                    "isFullRow: " + isFullRow + "\n" +
                    "isTotalRow: " + isTotalRow + "\n" +
                    "float: " + isFloating
                );

                if (isFloating && !isFullRow && !isTotalRow) {
                    tabList.find('li').each(function(index) {
                        if (index < (tabCount - minItems)) {
                            $(this).css("width", (tabListW / maxItems) + "px");
                        } else {
                            $(this).css("width", (tabListW / minItems) + "px");
                        }
                    });
                } else {
                    tabList.find('li').each(function() {
                        $(this).css("width", "");
                    });                    
                }
            }
            // ----------------------------------------------------------

            allTabs.bind('click', function(e) {
                e.preventDefault();
                goTab($(this), that.options.baseUrl, that.options.effect, true);
            });            

            window.onpopstate = function (event) {
                if (event.state && event.state.taburl) {
                    var tab = tabList.find("li a[href='" + event.state.taburl + "']");
                    goTab(tab, that.options.baseUrl, that.options.effect, false);
                }
            }

            setTimeout(function() {                
                setOriginalTabWidths();
                equalizeTabs();
            }, 200); //time to render

            $(window).on('resize', function () {
                equalizeTabs();
            });
        }
    };

    $.fn[pluginName] = function ( options ) {
        return this.each(function () {
            new Plugin( this, options );
        });
    };

})( jQuery, window, document );


