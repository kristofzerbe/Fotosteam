/* Inputs 0.9.2
 * http://anjlab.github.io/inputs
 * https://github.com/jquery-boilerplate/jquery-boilerplate
 */
(function ($, window, document, undefined) {
    "use strict";

    // Create the defaults once
    var pluginName = "Inputs",
            defaults = {};

    // The actual plugin constructor
    function Plugin(element, options) {
        this.element = element;
        this.$e = $(element);

        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;

        this.setInputValue = function () {
            var val = this.$e.val();
            val ? this.$e.attr('value', val) : this.$e.removeAttr('value');
        }

        this.setTextareaValue = function () {
            this.$e.text(this.$e.val());
        }

        this.refresh = function() {
            this.setInputValue();
            if (this.$e.is("textarea")) {
                this.setTextareaValue();
            }
        }

        this.init();
    }

    $.extend(Plugin.prototype, {
        init: function () {
            var that = this;

            this.refresh();

            this.$e.change(function () {
                that.refresh();
            });
        },
        refresh: function () {
            this.refresh();
        }
    });

    $.fn[pluginName] = function (options) {
        return this.each(function () {
            if (!$.data(this, "plugin_" + pluginName)) {
                $.data(this, "plugin_" + pluginName, new Plugin(this, options));
            }
        });
    };

})(jQuery, window, document);