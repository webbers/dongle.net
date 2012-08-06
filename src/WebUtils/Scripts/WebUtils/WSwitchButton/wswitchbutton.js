/*
* WSwitchButton 1.1
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
(function ($) {
    $.WSwitchbutton = function (element, options) {
        var defaults = {
            yes: 'sim',
            no: 'não',
            speed: 0.125
        };

        var plugin = this;
        plugin.settings = {};
        var $element = $(element);

        var oldOnSelectStart = null;

        var checkedPos = parseInt(selectStyle('.wswitchbutton-checked')['background-position'], 0);
        checkedPos = checkedPos ? checkedPos : 0;

        var uncheckedPos = parseInt(selectStyle('.wswitchbutton-unchecked')['background-position'], 0);
        uncheckedPos = uncheckedPos ? uncheckedPos : -25;
        
        var checkedStyle;
        var uncheckedStyle;
        
        if (navigator.appName != 'Microsoft Internet Explorer') {
            checkedStyle = { 'background-position': checkedPos };
            uncheckedStyle = { 'background-position': uncheckedPos };
        }
        else {
            checkedStyle = { 'background-position-x': checkedPos };
            uncheckedStyle = { 'background-position-x': uncheckedPos };
        }
        

        var startControl = function (el, button, animate) {
            var style = el.is(':checked') ? checkedStyle : uncheckedStyle;
            if (animate) {
                button.animate(style, Math.abs(uncheckedPos) / plugin.settings.speed);
            }
            else {
                button.css(style, Math.abs(uncheckedPos) / plugin.settings.speed);
            }
        };

        plugin.init = function () {
            plugin.settings = $.extend({}, defaults, options);
            var $control = $('<div class="wswitchbutton-control"></div>');
            var $noLabel = $('<div class="left" unselectable="on">' + plugin.settings.no + '</div>');

            var $yesLabel = $('<div class="right" unselectable="on">' + plugin.settings.yes + '</div>');
            var disabledButton = $element.attr('disabled');
            if (disabledButton) {
                var $button = $('<div class="button button-disabled" disabled="disabled"></div>');
            }
            else {
                var $button = $('<div class="button"></div>');
            }

            $control.append($noLabel);
            $control.append($button);
            $control.append($yesLabel);

            var dragging;
            var downPos = 0;
            var initBgPos = 0;
            var moved = false;

            function setChecked(checked) {
                if (!disabledButton) {
                    if (checked && $element.attr('checked') == false) {
                        $element.attr('checked', true);
                        $element.triggerHandler('change');
                    }
                    else if (!checked && $element.attr('checked') == true) {
                        $element.attr('checked', false);
                        $element.triggerHandler('change');
                    }
                }
            }

            function check() {
                setChecked(true);
            }

            function uncheck() {
                setChecked(false);
            }

            $element.change(function () {
                startControl($element, $button, true);
            });

            $yesLabel.click(function () {
                check();
                startControl($element, $button, true);
            });

            $noLabel.click(function () {
                uncheck();
                startControl($element, $button, true);
            });
            var initCss;
            var splited;
            $(document).mousedown(function (e) {
                if (e.target == $button[0]) {
                    downPos = e.pageX - $button[0].offsetLeft;
                    if (navigator.appName != 'Microsoft Internet Explorer') {
                        initCss = $button.css('background-position');
                        splited = initCss.split(" ", 1);
                    }
                    else {
                        initCss = $button.css('background-position-x');
                        splited = initCss;
                    }

                    initBgPos = parseInt(splited, 0);
                    dragging = e.target;

                    oldOnSelectStart = document.onselectstart;
                    document.onselectstart = function () {
                        return false;
                    };
                }

            });

            $(document).mousemove(function (e) {

                if (dragging && !disabledButton) {
                    var offset = e.pageX - $button[0].offsetLeft;
                    moved = (offset - downPos) != 0;
                    var pos = initBgPos + offset - downPos;

                    pos = pos < uncheckedPos ? uncheckedPos : pos;
                    pos = pos > 0 ? 0 : pos;
                    $button.css('background-position', pos + 'px');

                    if (pos > uncheckedPos / 2) {
                        check();
                    }
                    else {
                        uncheck();
                    }
                    $button.css('cursor', 'pointer');
                }
            });

            $(document).mouseup(function () {
                document.onselectstart = oldOnSelectStart;

                if (dragging == $button[0]) {
                    if (!moved) {
                        if ($element.is(':checked')) {
                            uncheck();
                        }
                        else {
                            check();
                        }
                    }
                    moved = false;
                    if (navigator.appName != 'Microsoft Internet Explorer') {
                        initBgPos = parseInt($button.css('background-position').split(' '), 0);
                    }
                    else {
                        initBgPos = parseInt($button.css('background-position-x'), 0);
                    }
                    
                    if (!$element.is(':checked')) {
                        $button.animate(uncheckedStyle, Math.abs(initBgPos + Math.abs(uncheckedPos)) / plugin.settings.speed);
                    }
                    else {
                        $button.animate(checkedStyle, Math.abs(initBgPos) / plugin.settings.speed);
                    }
                }
                dragging = null;
            });

            startControl($element, $button);
            $element.after($control);
        };

        plugin.init();

        return $element;
    };

    $.fn.wswitchbutton = function (options) {
        return this.each(function () {
            if (undefined == $(this).data('WSwitchButton')) {
                var plugin = new $.WSwitchbutton(this, options);
                $(this).data('WSwitchButton', plugin);
            }
        });
    };

})(jQuery);


function selectStyle(sel)
{
    if (sel.substr(0, 1) != ".")
    {
        sel = "." + sel;
    }
    var attrClass;

    $(document.styleSheets).each(function (i, v)
    {
        var attrClassTest;
        if (attrClassTest = selectAttr(sel, v))
        {
            attrClass = attrClassTest;
        }
    });

    if (!attrClass)
    {
        attrClass = Array();
    }

    var objStyle = { };

    if (attrClass == "")
    {
        return false;
    }

    if (attrClass.match(";"))
    {
        attrClass = attrClass.split(";");
    }
    else
    {
        attrClass = [attrClass];
    }

    $(attrClass).each(function (i, v)
    {
        if (v != "")
        {
            v = v.split(":");
            if (v[1])
            {
                objStyle[v[0]] = $.trim(v[1].replace(' 50%', ''));
            }
        }
    });
    return objStyle;
}

function selectAttr(sel, v)
{
    var attrClass = false;

    if ($.browser.msie)
    {
        if (v.rules.length > 0)
        {
            $(v.rules).each(function (i2, v2)
            {
                if (sel == v2.selectorText)
                {
                    attrClass = v2.style.cssText;
                }
            });
        }
        else if (v.imports.length > 0)
        {
            $(v.imports).each(function (i2, v2)
            {
                if (sel == v2.selectorText)
                {
                    attrClass = v2.style.cssText;
                }
                else if (v2 == "[object]" || v2 == "[Object CSSStyleSheet]" || v2 == "[object CSSImportRule]")
                {
                    return selectAttr(sel, v2);
                }
            });
        }
    }
    else
    {
        $(v.cssRules).each(function (i2, v2)
        {
            if (sel == v2.selectorText)
            {
                attrClass = v2.style.cssText;
            }
            else if (v2 == "[object CSSImportRule]")
            {
                return selectAttr(sel, v2.styleSheet);
            }
        });
    }

    return attrClass;
}
