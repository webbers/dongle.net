/*
* WButton 1.0
* Copyright (c) 2011 Webers
*
* Depends:
*   - jQuery 1.4.2+
*
* Dual licensed under MIT or GPLv2 licenses
*   http://en.wikipedia.org/wiki/MIT_License
*   http://en.wikipedia.org/wiki/GNU_General_Public_License
*
*/
(function ($)
{
    $.Wbutton = function (element, options)
    {
        var plugin = this;
        plugin.settings = {};
        var $element = $(element);

        if (options == "disable")
        {
            $element.removeAttr('disabled');
            $element.addClass('disabled');
            $element.addClass('wbutton-disabled');

            $element.find('.content').addClass('disabled-content');
            $element.find('.final-content').addClass('disabled-final-content');

            return $element;
        }

        if (options == "enable")
        {
            $element.removeAttr('disabled');
            $element.removeClass('disabled');
            $element.removeClass('wbutton-disabled');

            $element.find('.content').removeClass('disabled-content');
            $element.find('.final-content').removeClass('disabled-final-content');
            return null;
        }

        var defaults = {
            icon: '',
            onClick: function () { },
            size: "normal"
        };

        plugin.init = function ()
        {
            if ($element.data('wbuttoned'))
            {
                return false;
            }
            plugin.settings = $.extend({}, defaults, options);

            $element
                .click(function (event)
                {
                    if ($element.is('.wbutton-disabled'))
                    {
                        event.stopImmediatePropagation();
                    }

                    if ($element.not(".disabled"))
                    {
                        plugin.settings.onClick();
                    }
                })
                .css('-moz-user-select', 'none')
                .css('-webkit-user-select', 'none')
                .css('user-select', 'none')
                .data('wbuttoned', true);

            var buttonType = $element.is('.wbutton-alert') ? 'alert' : $element.is('.wbutton-action') ? 'action' : $element.is('.wbutton-left') ? 'left' : $element.is('.wbutton-rigth') ? 'rigth' : 'normal';

            $element.hover(function ()
            {
                $element.addClass('wbutton-' + buttonType + '-hover');
            },
            function ()
            {
                $element.removeClass('wbutton-' + buttonType + '-hover');
            });

            $element.mousedown(function ()
            {
                $element.addClass('wbutton-' + buttonType + '-active');
            });

            $element.bind('mouseout mouseup', function ()
            {
                $element.removeClass('wbutton-' + buttonType + '-active');
            });

            if ($element.is('.submit'))
            {
                var form = $element.closest('form');

                form.find('input').each(function ()
                {
                    if (!$(this).data('is-form-sender'))
                    {
                        $(this)
                            .data('is-form-sender', true)
                            .keypress(function (e)
                            {
                                if (e.which == 10 || e.which == 13)
                                {
                                    form.trigger('submit');
                                }
                            });
                    }
                });

                $element.bind('click', function ()
                {
                    if ($element.not(".disabled"))
                    {
                        form.trigger('submit');
                    }
                });
            }

            var buttonContent = $('<span class="content"></span>');
            var buttonEnd = $('<span class="final-content"></span>');

            if ($element.attr('disabled'))
            {
                $element.removeAttr('disabled');
                $element.addClass('disabled');
                $element.addClass('wbutton-disabled');
                buttonContent.addClass('disabled-content');
                buttonEnd.addClass('disabled-final-content');

                $element.click(function ()
                {
                    return false;
                });
            }

            var isDisabled = "";
            if ($element.hasClass('disabled'))
            {
                isDisabled = "-disabled";
            }

            var icon = plugin.settings.icon ? 'icon icon-' + plugin.settings.icon + isDisabled : '';

            buttonContent.append("<span style='float:left;display:block;' class='" + icon + "'>" + $element.html() + "</span>");
            $element.html("");
            $element.append(buttonContent);
            $element.append(buttonEnd);
        };

        plugin.init();

        return $element;
    };

    $.fn.wbutton = function (options)
    {
        return this.each(function ()
        {
            if ($(this).data('wbutton') == undefined || typeof (options) == "string")
            {
                var plugin = new $.Wbutton(this, options);
                $(this).data('wbutton', plugin);
            }
        });
    };

})(jQuery);
$(document).ready(function ()
{
    $('.wbutton').each(function ()
    {
        var opt = {};
        if ($(this).attr('icon'))
        {
            opt.icon = $(this).attr('icon');
        }

        $(this).wbutton(opt);
    });
});