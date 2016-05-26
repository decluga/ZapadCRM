(function ($) {
    var kendo           = window.kendo,
        RangeSlider     = kendo.ui.RangeSlider,
        math            = Math,
        extend          = $.extend,
        parse           = kendo.parseFloat,
        PRECISION       = 3,
        CHANGE          = "change"
    ;


    function round(value) {
        value = parseFloat(value, 10);
        var power = math.pow(10, PRECISION || 0);
        return math.round(value * power) / power;
    }

    var CustomRangeSlider = RangeSlider.extend({
        init: function (element, options) {
            RangeSlider.fn.init.call(this, element, options);
        },

        options: {
            name: "CustomRangeSlider"
        },

        updateValue: function (val, forceEvent) {
            var that = this,
                values = that.value(),
                selectionStart = val[0],
                selectionEnd = val[1],
                change = values[0] != selectionStart || values[1] != selectionEnd
            ;

            change = (forceEvent === true) ? true : change;

            that.value([selectionStart, selectionEnd]);

            if (change) {
                that.trigger(CHANGE, {
                    values: [selectionStart, selectionEnd],
                    value: [selectionStart, selectionEnd]
                });
            }
        },

        reinit: function (newOptions) {
            var that    = this,
                options = that.options
            ;

            /** Update options*/
            $.extend(true, that.options, newOptions);

            /** Update distance */
            that._distance = round(options.max - options.min);

            /** Remove slider items */
            that._trackDiv.prev().remove();

            /** Update slider items */
            that._sliderItemsInit();

            /** Update value */
            that.updateValue([options.min, options.max], true);
        },

        reset: function () {
            var that = this,
                options = that.options
            ;

            /** Update value */
            that.updateValue([options.min, options.max], true);
        }
    });

    kendo.ui.plugin(CustomRangeSlider);
})(jQuery);



(function ($) {
    var kendo = window.kendo,
        Widget = kendo.ui.Widget,
        Draggable = kendo.ui.Draggable,
        isPlainObject = $.isPlainObject,
        activeElement = kendo._activeElement,
        proxy = $.proxy,
        extend = $.extend,
        each = $.each,
        template = kendo.template,
        BODY = "body",
        templates,
        NS = ".kendoWindow",
        // classNames
        KWINDOW = ".k-window",
        KWINDOWTITLE = ".k-window-title",
        KWINDOWTITLEBAR = KWINDOWTITLE + "bar",
        KWINDOWCONTENT = ".k-window-content",
        KWINDOWRESIZEHANDLES = ".k-resize-handle",
        KOVERLAY = ".k-overlay",
        KCONTENTFRAME = "k-content-frame",
        LOADING = "k-loading",
        KHOVERSTATE = "k-state-hover",
        KFOCUSEDSTATE = "k-state-focused",
        MAXIMIZEDSTATE = "k-window-maximized",
        // constants
        VISIBLE = ":visible",
        HIDDEN = "hidden",
        CURSOR = "cursor",
        // events
        OPEN = "open",
        ACTIVATE = "activate",
        DEACTIVATE = "deactivate",
        CLOSE = "close",
        REFRESH = "refresh",
        RESIZE = "resize",
        RESIZEEND = "resizeEnd",
        DRAGSTART = "dragstart",
        DRAGEND = "dragend",
        ERROR = "error",
        OVERFLOW = "overflow",
        ZINDEX = "zIndex",
        MINIMIZE_MAXIMIZE = ".k-window-actions .k-i-minimize,.k-window-actions .k-i-maximize",
        KPIN = ".k-i-pin",
        KUNPIN = ".k-i-unpin",
        PIN_UNPIN = KPIN + "," + KUNPIN,
        TITLEBAR_BUTTONS = ".k-window-titlebar .k-window-action",
        REFRESHICON = ".k-window-titlebar .k-i-refresh",
        isLocalUrl = kendo.isLocalUrl;

    var Window = kendo.ui.Window;

    var CustomWindow = Window.extend({
        init: function (element, options) {
            Window.fn.init.call(this, element, options);
        },

        options: {
            name: "CustomWindow"
        },

        open: function (newOptions) {
            newOptions = newOptions ? newOptions : {};
            defaultOptions = {
                silentMode: false,
            };
            newOptions = extend(defaultOptions, newOptions);

            if (!newOptions.silentMode) {
                /** Default mode */
                Window.fn.open.call(this);
            } else {
                /** Silent mode */
                var that = this,
                    wrapper = that.wrapper,
                    options = that.options,
                    showOptions = options.animation.open,
                    contentElement = wrapper.children(KWINDOWCONTENT),
                    overlay;

                if (!that.trigger(OPEN)) {
                    if (that._closing) {
                        wrapper.kendoStop(true, true);
                    }

                    that._closing = false;

                    that.toFront();

                    if (options.autoFocus) {
                        that.element.focus();
                    }

                    options.visible = true;

                    /**if (options.modal) {
                        overlay = that._overlay(false);
    
                        overlay.kendoStop(true, true);
    
                        if (showOptions.duration && kendo.effects.Fade) {
                            var overlayFx = kendo.fx(overlay).fadeIn();
                            overlayFx.duration(showOptions.duration || 0);
                            overlayFx.endValue(0.5);
                            overlayFx.play();
                        } else {
                            overlay.css("opacity", 0.5);
                        }
    
                        overlay.show();
                    }*/

                    /**
                    if (!wrapper.is(VISIBLE)) {
                        contentElement.css(OVERFLOW, HIDDEN);
                        wrapper.show().kendoStop().kendoAnimate({
                            effects: showOptions.effects,
                            duration: showOptions.duration,
                            complete: proxy(this._activate, this)
                        });
                    }
                    */
                }

                if (options.isMaximized) {
                    that._documentScrollTop = $(document).scrollTop();
                    $("html, body").css(OVERFLOW, HIDDEN);
                }

                return that;
            }
        }
    });

    kendo.ui.plugin(CustomWindow);
})(jQuery);