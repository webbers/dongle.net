/*
* WSlider 1.0
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
	$.WSlider = function(element, options)
	{
		var defaults = {
            interval: 5000
        };
		
		var plugin = this;
        plugin.settings = {};
        var $element = $(element);
	
		plugin.focusItem = function (element)
		{
			var description = element.find('.description');
			var height = description.height();
			description.css({ opacity: 0, marginBottom: -height });
			element.fadeIn('slow');
			description.animate({ opacity: 0.85, marginBottom: "30px" }, 500);

			element.addClass('selected');
			element.data('bullet').addClass('selected');
		};

		plugin.move = function (element)
		{
			var sliderItems = $element.find('.items');
			var sElement = sliderItems.find('.selected');

			var sDescription = sElement.find('.description');

			if (!element)
			{
				element = sliderItems.find('.selected + li');
				if (element.length == 0)
				{
					element = sliderItems.find('li').first();
				}
			}

			if (element.is('.selected'))
			{
				return;
			}

			var sHeight = sDescription.height();
			$element.find('li').removeClass('selected');

			sDescription.animate({ opacity: 0, marginBottom: -sHeight }, 500, function ()
			{
				sElement.fadeOut('slow');
				plugin.focusItem(element);
			});
			plugin.start();
		};

		plugin.start = function ()
		{
			if (this.timeout)
			{
				clearTimeout(this.timeout);
			}
			else
			{
				this.focusItem($element.find('.items li').first());
			}
			this.timeout = setTimeout(function ()
			{
				plugin.move();
			}, plugin.settings.interval);
		};
		
		plugin.init = function ()
        {
			plugin.settings = $.extend({}, defaults, options);
		
			var $element = $(element);
			$element.addClass('wslider');
			
			var ulElement = $element.find('ul');
			ulElement.addClass('items');

			ulElement.find('.description').each(function ()
			{
				var content = $(this).html();
				$(this).html('');
				$(this).append('<div>' + content + '</div>');
			});

			var bullets = jQuery('<div class="bullets">');
			var ul = jQuery('<ul/>');
			bullets.append(ul);

			$element.find('.items li').each(function ()
			{
				jQuery(this).hide();
				var el = jQuery(this);
				var li = jQuery('<li></li>');
				var a = jQuery('<div></div>');

				a.click(function ()
				{
					plugin.move(el);
				});

				li.append(a);
				ul.append(li);
				jQuery(this).data('bullet', li);
			});
			$element.append(bullets);
			plugin.start();
		};
		
		plugin.init();		
		return $element;
	};
	
    $.fn.wslider = function (options)
    {
        return this.each(function ()
        {
            if (undefined == $(this).data('wslider'))
            {
                var plugin = new $.WSlider(this, options);
                $(this).data('wslider', plugin);
            }
        });
    };
})(jQuery);
$(document).ready(function ()
{
    $('.wslider').each(function ()
    {
        var opt = {};
        if ($(this).attr('interval'))
        {
            opt.interval = $(this).attr('interval');
        }

        $(this).wslider(opt);
    });
});
