/*
jQuery fullsizable plugin v2.0.2ex
  - take full available browser space to show images

(c) 2011-2014 Matthias Schmidt <http://m-schmidt.eu/>

Improvements by kristofzerbe (github)
- prevent multiple control adding on multiple calls
- prevent multiple 'click' bindings
- prevent arrow keys on navigation=false
- new option 'keyBindings' to define if key events are available
- image.ratio fix
- options: link titles
- inifityLoad: function for preloading images in the main dom
- slideshow...

Example Usage:
  $('a.fullsizable').fullsizable();

Options:
  **detach_id** (optional, defaults to null) - id of an element that will temporarely be set to ``display: none`` after the curtain loaded.
  **navigation** (optional, defaults to true) - show next and previous links when working with a set of images.
  **keyBindings** (optional, defaults to true) - bind key events to close and move through pictures
  **closeButton** (optional, defaults to true) - show a close link.
  **fullscreenButton** (optional, defaults to true) - show full screen button for native HTML5 fullscreen support in supported browsers.
  **openOnClick** (optional, defaults to true) - set to false to disable default behavior which fullsizes an image when clicking on a thumb.
  **clickBehaviour** (optional, 'next' or 'close', defaults to 'close') - whether a click on an opened image should close the viewer or open the next image.
  **preload** (optional, defaults to true) - lookup selector on initialization, set only to false in combination with ``reloadOnOpen: true`` or ``fullsizable:reload`` event.
  **reloadOnOpen** (optional, defaults to false) - lookup selector every time the viewer opens.
*/


(function () {
    var $,
        $fullsize_holder,
        $command_holder,
        bindCurtainEvents,
        closeFullscreen,
        closeViewer,
        container_id,
        current_image,
        hasFullscreenSupport,
        hideChrome,
        image_holder_id,
        images,
        keyPressed,
        makeFullsizable,
        mouseMovement,
        mouseStart,
        nextImage,
        openViewer,
        options,
        preloadImage,
        prepareCurtain,
        prevImage,
        resizeImage,
        showChrome,
        showImage,
        spinner_class,
        stored_scroll_position,
        stored_display_value,
        toggleFullscreen,
        unbindCurtainEvents,
        slideshowPlay,
        slideshowPause,
        slideshow_running,
        urls,
        sources,
        memberAvatarUrl,
        memberAlias,
        memberPlainName;

    $ = jQuery;

    container_id = '#jquery-fullsizable';

    image_holder_id = '#fullsized_image_holder';

    spinner_class = 'fullsized_spinner';

    $fullsize_holder = $('<div id="jquery-fullsizable"><div id="fullsized_image_holder" class="figure-bar"></div><div id="fullsized_command_holder"></div></div>');

    images = [];
    urls = new Array();
    sources = new Array();
    memberAvatarUrl = "";
    memberAlias = "";
    memberPlainName = "";

    current_image = 0;

    options = null;

    stored_scroll_position = null;

    stored_display_value = null;

    slideshow_running = false;

    resizeImage = function () {

        var image = images[current_image];

        image.ratio = (image.naturalHeight / image.naturalWidth).toFixed(2);

        if ($(window).height() / image.ratio > $(window).width()) {
            $(image).width($(window).width());
            $(image).height($(window).width() * image.ratio);
            return $(image).css('margin-top', ($(window).height() - $(image).height()) / 2);
        } else {
            $(image).height($(window).height());
            $(image).width($(window).height() / image.ratio);
            return $(image).css('margin-top', 0);
        }
    };

    keyPressed = function (e) {
        if (e.keyCode === 27) {
            closeViewer();
        }
        if (options.navigation) {
            if (e.keyCode === 37) {
                prevImage(true);
            }
            if (e.keyCode === 39) {
                return nextImage(true);
            }
        }
    };

    prevImage = function (shouldHideChrome) {
        if (shouldHideChrome == null) {
            shouldHideChrome = false;
        }
        if (current_image > 0) {
            return showImage(images[current_image - 1], -1, shouldHideChrome);
        }
    };

    nextImage = function (shouldHideChrome) {
        if (shouldHideChrome == null) {
            shouldHideChrome = false;
        }
        if ((current_image + 1) === (images.length) && slideshow_running === true) {
            closeViewer();
        }
        if (current_image < images.length - 1) {
            return showImage(images[current_image + 1], 1, shouldHideChrome);
        }
    };

    showImage = function (image, direction, shouldHideChrome) {
        if (direction == null) {
            direction = 1;
        }
        if (shouldHideChrome == null) {
            shouldHideChrome = false;
        }
        current_image = image.index;
        $(image_holder_id).hide();
        $(image_holder_id).html(image);

        $command_holder.find("#fullsized_member_img").attr("src", image.memberAvatarUrl);
        $fullsize_holder.find("#fullsized_member_name").text(image.memberPlainName);

        if (options.navigation) {
            if (shouldHideChrome === true) {
                hideChrome();
            } else {
                showChrome();
            }
        }
        if (image.loaded != null) {
            $(container_id).removeClass(spinner_class);
            resizeImage();
            $(image_holder_id).fadeIn('fast');
            return preloadImage(direction);
        } else {
            $(container_id).addClass(spinner_class);
            image.onload = function () {
                resizeImage();
                $(image_holder_id).fadeIn('slow', function () {
                    return $(container_id).removeClass(spinner_class);
                });
                this.loaded = true;
                return preloadImage(direction);
            };
            return image.src = image.buffer_src;
        }
    };

    preloadImage = function (direction) {
        var preload_image;

        if (direction === 1 && current_image < images.length - 1) {
            preload_image = images[current_image + 1];
        } else if ((direction === -1 || current_image === (images.length - 1)) && current_image > 0) {
            preload_image = images[current_image - 1];
        } else {
            return;
        }

        if (images.length - current_image === 2) {
            options.infinityLoad();
        }

        preload_image.onload = function () {
            return this.loaded = true;
        };
        if (preload_image.src === '') {
            return preload_image.src = preload_image.buffer_src;
        }
    };

    openViewer = function (image, opening_selector) {
        $('body').append($fullsize_holder);        
        $(window).bind('resize', resizeImage);        
        showImage(image);        
        return $(container_id).hide().fadeIn(function () {
            if (options.detach_id != null) {
                stored_scroll_position = $(window).scrollTop();
                stored_display_value = $('#' + options.detach_id).css("display");
                $('#' + options.detach_id).css('display', 'none');
                resizeImage();
            }
            bindCurtainEvents();
            return $(document).trigger('fullsizable:opened', opening_selector);
        });
    };

    closeViewer = function () {

        if (slideshow_running === true) {
            slideshowPause();
        }
        $(container_id).fadeOut(function () {
            return $fullsize_holder.remove();
        });
        closeFullscreen();
        $(container_id).removeClass(spinner_class);
        unbindCurtainEvents();

        if (options.detach_id != null) {
            $('#' + options.detach_id).css('display', stored_display_value);
            $(window).scrollTop(stored_scroll_position);
        }

        return $(window).unbind('resize', resizeImage);
    };

    makeFullsizable = function () {
        //  images.length = 0;

        return $(options.selector).each(function () {

            var source = $(this).attr('href');
            var image = sources[source];
            if (!image) {
                image = new Image;
                image.buffer_src = source;
                image.index = images.length;
                image.detailUrl = $(this).attr("detailUrl");
                image.memberPlainName = $(this).attr("plainName");
                image.memberAvatarUrl = $(this).attr("avatarUrl");
                image.memberAlias = $(this).attr("alias");
                images.push(image);
                sources[source] = image;
            }
            if (options.openOnClick) {
                return $(this).unbind("click").click(function (e) {
                    e.preventDefault();
                    if (options.reloadOnOpen) {
                        makeFullsizable();
                    }
                    return openViewer(image, this);
                });
            }            
        });
    };

    slideshowPlay = function () {
        var button = $command_holder.find("#fullsized_slideshow");
        button.removeClass("fullsized_slideshow-play")
               .addClass("fullsized_slideshow-pause");
        window.refreshIntervalId = setInterval(function () {
            nextImage(true);
        }, options.slideshowDelay);
        slideshow_running = true;
    }

    slideshowPause = function () {
        var button = $command_holder.find("#fullsized_slideshow");
        button.removeClass("fullsized_slideshow-pause")
               .addClass("fullsized_slideshow-play");
        clearInterval(window.refreshIntervalId);
        slideshow_running = false;
    }

    prepareCurtain = function () {       
        if (options.navigation) {
            if ($fullsize_holder.find("#fullsized_go_prev").length == 0) {
                $fullsize_holder.append('<a id="fullsized_go_prev" href="#prev"></a>');
                $(document).on('click', '#fullsized_go_prev', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    return prevImage();
                });
            }
            if ($fullsize_holder.find("#fullsized_go_next").length == 0) {
                $fullsize_holder.append('<a id="fullsized_go_next" href="#next"></a>');
                $(document).on('click', '#fullsized_go_next', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    return nextImage();
                });
            }
        }
        $command_holder = $fullsize_holder.find("#fullsized_command_holder");
        //if (options.closeButton) {
        //    if ($command_holder.find("#fullsized_close").length == 0) {
        //        $command_holder.append('<a id="fullsized_close" class="tooltip-bottom" href="#close" title="' + options.titleClose + '"></a>');
        //        $(document).on('click', '#fullsized_close', function (e) {
        //            e.preventDefault();
        //            e.stopPropagation();
        //            return closeViewer();
        //        });
        //    }
        //}
        if (options.fullscreenButton && hasFullscreenSupport()) {
            if ($command_holder.find("#fullsized_fullscreen").length == 0) {
                $command_holder.append('<a id="fullsized_fullscreen" class="tooltip-bottom" href="#fullscreen" title="' + options.titleFullscreen + '"></a>');
                $(document).on('click', '#fullsized_fullscreen', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    return toggleFullscreen();
                });
            }
        }
        if (options.detailButton) {
            if ($command_holder.find("#fullsized_detail").length === 0) {
                $command_holder.append('<a id="fullsized_detail" class="tooltip-bottom" href="#detail" title="' + options.titleDetail + '"></a>');
                $(document).on('click', '#fullsized_detail', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    window.location = images[current_image].detailUrl;
                });
            }
        } else {
            var fullsize = $command_holder.find("#fullsized_fullscreen");
            if (fullsize) {
                fullsize.css("right", "65px");
            }
        }

        if (options.avatarButton) {
            if ($command_holder.find("#fullsized_member").length === 0) {
                $command_holder.append('<a id="fullsized_member" class="tooltip-bottom" href="#member"><img id="fullsized_member_img" src="" /></a>');
                $(document).on('click', '#fullsized_member', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    window.location = window.location.origin + "/" + images[current_image].memberAlias + "/about";
                });
                $command_holder.append('<span id="fullsized_member_name"></span>');
            }
        }

        if (options.slideshowButton) {
            if ($command_holder.find("#fullsized_slideshow").length == 0) {
                $command_holder.append('<a id="fullsized_slideshow" class="fullsized_slideshow-play tooltip-bottom" href="#slideshow" title="' + options.titleSlideshowStart + '"></a>');
                $(document).on('click', '#fullsized_slideshow', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    if (slideshow_running === false) {
                        return slideshowPlay();
                    } else {
                        return slideshowPause();
                    }
                });
            }
        }        

        switch (options.clickBehaviour) {
            case 'close':
                return $(document).on('click', container_id, closeViewer);
            case 'next':
                return $(document).on('click', container_id, function () {
                    return nextImage(true);
                });
        }
    };

    bindCurtainEvents = function () {        
        if (options.keyBindings) {
            $(document).bind('keydown', keyPressed);
        }
        if (options.navigation) {
            $(document).bind('fullsizable:next', function () {
                return nextImage(true);
            });
            $(document).bind('fullsizable:prev', function () {
                return prevImage(true);
            });
        }
        $(".tooltip-bottom").tooltipster();
        return $(document).bind('fullsizable:close', closeViewer);        
    };

    unbindCurtainEvents = function () {
        if (options.navigation) {
            $(document).unbind('keydown', keyPressed);
            $(document).unbind('fullsizable:next');
            $(document).unbind('fullsizable:prev');
        }
        $(".tooltip-bottom").tooltipster('destroy');
        return $(document).unbind('fullsizable:close');
    };

    hideChrome = function () {
        //var $chrome;

        //$chrome = $fullsize_holder.find('a');
        if ($command_holder.is(':visible') === true) {
            $command_holder.toggle(false);
            $('#fullsized_go_prev').toggle(false);
            $('#fullsized_go_next').toggle(false);
            return $fullsize_holder.bind('mousemove', mouseMovement);
        }
    };

    mouseStart = null;

    mouseMovement = function (event) {
        var distance;

        if (mouseStart === null) {
            mouseStart = [event.clientX, event.clientY];
        }
        distance = Math.round(Math.sqrt(Math.pow(mouseStart[1] - event.clientY, 2) + Math.pow(mouseStart[0] - event.clientX, 2)));
        if (distance >= 10) {
            $fullsize_holder.unbind('mousemove', mouseMovement);
            mouseStart = null;
            return showChrome();
        }
    };

    showChrome = function () {
        //$('#fullsized_close, #fullsized_fullscreen, #fullsized_slideshow,#fullsized_member, #fullsized_detail').toggle(true);
        $command_holder.toggle(true);
        $('#fullsized_go_prev').toggle(current_image !== 0);
        return $('#fullsized_go_next').toggle(current_image !== images.length - 1);
    };

    $.fn.fullsizable = function (opts) {
        options = $.extend({
            selector: this.selector,
            detach_id: null,
            navigation: true,
            keyBindings: true,
            closeButton: false,
            detailButton: false,
            avatarButton: false,
            detailUrl: "",
            slideshowButton: false,
            slideshowDelay: 5000,
            fullscreenButton: true,
            openOnClick: true,
            clickBehaviour: 'close',
            preload: true,
            reloadOnOpen: false,
            infinityLoad: function () { },
            titleGoPrev: "PREVIOUS",
            titleGoNext: "NEXT",
            titleClose: "CLOSE",
            titleFullscreen: "FULLSCREEN",
            titleSlideshowStart: "SLIDESHOW PLAY",
            titleSlideshowPause: "SLIDESHOW PAUSE"
        }, opts || {});

        prepareCurtain();

        if (options.preload) {
            makeFullsizable();
        }

        $(document).bind('fullsizable:reload', makeFullsizable);
        $(document).bind('fullsizable:slideshow', slideshowPlay);
        $(document).bind('fullsizable:open', function (e, target) {
            var image, _i, _len, _results;

            if (options.reloadOnOpen) {
                makeFullsizable();
            }
            _results = [];
            for (_i = 0, _len = images.length; _i < _len; _i++) {
                image = images[_i];
                if (image.buffer_src === $(target).attr('href')) {
                    _results.push(openViewer(image, target));
                } else {
                    _results.push(void 0);
                }
            }
            return _results;
        });

        return this;
    };

    hasFullscreenSupport = function () {
        var fs_dom;

        fs_dom = $fullsize_holder.get(0);
        if (fs_dom.requestFullScreen || fs_dom.webkitRequestFullScreen || fs_dom.mozRequestFullScreen) {
            return true;
        } else {
            return false;
        }
    };

    closeFullscreen = function () {
        return toggleFullscreen(true);
    };

    toggleFullscreen = function (force_close) {
        var fs_dom;

        fs_dom = $fullsize_holder.get(0);
        if (fs_dom.requestFullScreen) {
            if (document.fullScreen || force_close) {
                return document.exitFullScreen();
            } else {
                return fs_dom.requestFullScreen();
            }
        } else if (fs_dom.webkitRequestFullScreen) {
            if (document.webkitIsFullScreen || force_close) {
                return document.webkitCancelFullScreen();
            } else {
                return fs_dom.webkitRequestFullScreen();
            }
        } else if (fs_dom.mozRequestFullScreen) {
            if (document.mozFullScreen || force_close) {
                return document.mozCancelFullScreen();
            } else {
                return fs_dom.mozRequestFullScreen();
            }
        }
    };

}).call(this);
