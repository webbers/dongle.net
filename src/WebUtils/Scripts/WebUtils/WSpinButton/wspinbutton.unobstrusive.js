/*
* WSpinButton 1.0
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
    var calcFloat = {
        get: function (num)
        {
            num = num.toString();
            if (num.indexOf('.') == -1) return [0, eval(num)];
            var nn = num.split('.');
            var po = nn[1].length;
            var st = nn.join('');
            var sign = '';
            if (st.charAt(0) == '-')
            {
                st = st.substr(1);
                sign = '-';
            }
            for (var i = 0; i < st.length; ++i) if (st.charAt(0) == '0') st = st.substr(1, st.length);
            st = sign + st;
            return [po, eval(st)];
        },
        getInt: function (num, figure)
        {
            var d = Math.pow(10, figure);
            var n = this.get(num);
            var v1 = eval('num * d');
            var v2 = eval('n[1] * d');
            if (this.get(v1)[1] == v2) return v1;
            return (n[0] == 0 ? v1 : eval(v2 + '/Math.pow(10, n[0])'));
        },
        sum: function (v1, v2)
        {
            var n1 = this.get(v1);
            var n2 = this.get(v2);
            var figure = (n1[0] > n2[0] ? n1[0] : n2[0]);
            v1 = this.getInt(v1, figure);
            v2 = this.getInt(v2, figure);
            return eval('v1 + v2') / Math.pow(10, figure);
        }
    };
    $.extend({
        spin:
		{
		    step: 1,
		    max: null,
		    min: 0,
		    timestep: 200,
		    timeBlink: 100,
		    locked: false,
		    decimal: null,
		    beforeChange: null,
		    changed: null,
		    buttonUp: null,
		    buttonDown: null
		}
    });
    $.fn.extend({
        wspinbutton: function (o)
        {
            return this.each(function ()
            {

                o = o || {};
                var options = {};
                $.each($.spin, function (k, v)
                {
                    options[k] = (typeof o[k] != 'undefined' ? o[k] : v);
                });

                var $element = $(this);
                if (!$element.val())
                {
                    $element.val(options.min);
                }
                if ($(this).data('wspinned'))
                {
                    return false;
                }
                var $buttonElement = $('<div />');
                $buttonElement.addClass('wspinbutton-button');
                $element.after($buttonElement);
                $element.data('wspinned', true);
                if (options.locked)
                {
                    $element.focus(function ()
                    {
                        $element.blur();
                    });
                }
                function spin(vector)
                {
                    var val = $element.val();
                    var originalVal = val;
                    if (options.decimal)
                    {
                        val = val.replace(options.decimal, '.');
                    }
                    if (!isNaN(val))
                    {
                        val = calcFloat.sum(val, vector * options.step);
                        if (options.min !== null && val < options.min) val = options.min;
                        if (options.max !== null && val > options.max) val = options.max;
                        if (val != $element.val())
                        {
                            if (options.decimal) val = val.toString().replace('.', options.decimal);
                            var ret = ($.isFunction(options.beforeChange) ? options.beforeChange.apply($element, [val, originalVal]) : true);
                            if (ret !== false)
                            {
                                $element.val(val);
                                if ($.isFunction(options.changed))
                                {
                                    options.changed.apply($element, [val]);
                                };
                                $element.change();

                                if (vector > 0)
                                {
                                    $buttonElement.addClass('up');
                                    $buttonElement.removeClass('down');
                                }
                                else
                                {
                                    $buttonElement.addClass('down');
                                    $buttonElement.removeClass('up');
                                }
                                if (options.timeBlink < options.timestep)
                                {
                                    setTimeout(function ()
                                    {
                                        $buttonElement.removeClass('down');
                                        $buttonElement.removeClass('up');
                                    }, options.timeBlink);
                                }
                            }
                        }
                    }
                    if (vector > 0)
                    {
                        if ($.isFunction(options.buttonUp))
                        {
                            options.buttonUp.apply($element, [val]);
                        }
                    }
                    else if ($.isFunction(options.buttonDown))
                    {
                        options.buttonDown.apply($element, [val]);
                    }
                }

                $buttonElement.mousedown(function (e)
                {
                    var pos = e.pageY - $buttonElement.offset().top;
                    var vector = ($buttonElement.height() / 2 > pos ? 1 : -1);
                    (function ()
                    {
                        spin(vector);
                        var timeout = setTimeout(arguments.callee, options.timestep);
                        $(document).one('mouseup', function ()
                        {
                            clearTimeout(timeout);
                            $buttonElement.removeClass('spinDown');
                            $buttonElement.removeClass('spinUp');
                        });
                    })();
                    return false;
                });
            });
        }
    });
})(jQuery);