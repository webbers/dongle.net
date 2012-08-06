/*
* WImagePicker 1.0
* Copyright (c) 2011 Webers
*
* Depends:
*   - jQuery 1.4.2+
*   - Shadowbox 3.0.3+ (http://www.shadowbox-js.com)
*
* Dual licensed under MIT or GPLv2 licenses
*   http://en.wikipedia.org/wiki/MIT_License
*   http://en.wikipedia.org/wiki/GNU_General_Public_License
*
*/
(function ($)
{
    $.WImagePicker = function (element, options)
    {
		var defaults = {
            thumbsFolder: '',
            jsonFileUrl: ''
        };
		
		var plugin = this;
        plugin.settings = {};
        var $element = $(element);
		
		plugin.init = function ()
        {
            plugin.settings = $.extend({}, defaults, options);
		
			var imagePicker = $('<div class="wimagepicker ui-widget ui-state-default ui-corner-all ui-state-hover"></div>');
			var img = $('<img src="" />');
			var defaultIcon = $('<div class="default-icon"/>');

			var divSelector = $('<div><label>Url da Imagem</label></div>');
			var input = $('<input class="text-box single-line" type="text" value="">');
			var button = $('<a href="javascript:void(0)" class="ui-state-default ui-corner-all">Selecionar...</a>');
			var clear = $('<div style="clear: both; float: none"></div>');

			divSelector.append(input);
			divSelector.append(button);

			imagePicker.append(img);
			imagePicker.append(defaultIcon);
			imagePicker.append(divSelector);
			imagePicker.append(clear);
			
			var $element = $(element);

			$element.after(imagePicker);
			$element.hide();

			input.change(function ()
			{
				$element.val(input.val());
				img.hide();
				img.attr('src', plugin.settings.thumbsFolder + '/' + input.val());
			});

			img.bind('error', function ()
			{
				img.hide();
				defaultIcon.show();
			});

			img.bind('load', function ()
			{
				img.show();
				defaultIcon.hide();
			});
			

			input.val($element.val());
			input.trigger('change');

			button.click(function()
				{
				Shadowbox.init(
					{
						skipSetup: true,
						onFinish: function()
						{

							$.getJSON(plugin.settings.jsonFileUrl, function(items)
							{
								$('.image-selector').html('');
								$(items).each(function()
								{
									$('.image-selector').append('<li name=' + this.Name + '><img src="' + this.Thumbnail + '" />' + this.Name + '</li>');
									$('.image-selector-window').fadeIn('fast');
								});
							});
						}
					});

				Shadowbox.open(
					{
						content: '<div class="image-selector-window" style="display: none"><ul  class="image-selector"></ul><button id="okButton">Selecionar</button></div>',
						player: "html",
						title: "título",
						width: 800,
						height: 600
					});
			});

			$('.image-selector>li').live('click', function()
			{
				$('.image-selector li').removeClass('selected');
				$(this).addClass('selected');
			});

			$('.image-selector>li').live('dblclick', function()
			{
				selectImage();
			});

			$('#okButton').live('click', function()
			{
				selectImage();
			});

			function selectImage()
			{
				var name = $('.image-selector li.selected').attr('name');
				input.val(name);
				input.trigger('change');
				Shadowbox.close();
			}
		};
		
		plugin.init();

        return $element;
	};

    $.fn.wimagepicker = function (options)
    {
        return this.each(function ()
        {
            if (undefined == $(this).data('wimagepicker'))
            {
                var plugin = new $.WImagePicker(this, options);
                $(this).data('wimagepicker', plugin);
            }
        });
    };

})(jQuery);

$(document).ready(function ()
{
    $('.wimagepicker').each(function ()
    {
        var opt = {};
        if ($(this).attr('thumbsfolder'))
        {
            opt.thumbsFolder = $(this).attr('thumbsfolder');
        }
		if ($(this).attr('jsonfileurl'))
        {
            opt.jsonFileUrl = $(this).attr('jsonfileurl');
        }

        $(this).wimagepicker(opt);
    });
});