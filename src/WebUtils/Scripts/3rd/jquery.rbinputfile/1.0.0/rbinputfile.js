/*
Copyright (c) 2012 Renan Borges - http://www.renanborges.com
Version: 1.0.1

- 1.0.1: Fix da altura do campo input para a mesma altura do botão;
- 1.0.2: Fix no IE 7
- 1.0.3: Fix da aplicação dupla do plugin

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
function getInternetExplorerVersion()
{
    var rv = -1;
    if (navigator.appName == 'Microsoft Internet Explorer')
    {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}
(function ($) {
    $.fn.rbinputfile = function (options) {
		if (getInternetExplorerVersion() == "7")
		{
			return false;
		}
        var defaults = {
            inputText: 'Selecione o arquivo'
        };

        var options = $.extend(defaults, options);
        
        $(this).each(function () {
			
			if (!$(this).hasClass('rbinputfield'))
			{
				$inputDiv = $('<div class="rbinputfile-panel" index="' + $(this).index(this) + '"><div class="rbinputfile-text">' + options.inputText + '</div><div class="rbinputfile-bg3"></div><div class="rbinputfile-file-name"></div></div>');
                $(this).eq($(this).index(this)).change(function () {
                    $(this).eq($(this).index(this)).next().find('.rbinputfile-file-name').text('').text($(this).val().split('\\').pop());
                });

                $(this).after($inputDiv);
			    $(this).css('position', 'absolute').fadeTo(0, 0).css('height', '26px').css('opacity', '0').css('alpha', '(opacity=00)').css('margin-left','-1px');
					
                $(this).hover(function () {
                    $(this).next().addClass('rbinputfile-panel-hover');
                },
                function () {
                    $(this).next().removeClass('rbinputfile-panel-hover');
                });
			
			    $(this).addClass('rbinputfield');
			}

        });

    }
})(jQuery);