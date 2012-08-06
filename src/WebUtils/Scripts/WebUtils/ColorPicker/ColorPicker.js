/*
* JQuery Color Picker 1.0
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
    $.fn.colorPicker = function ()
    {
        if (this.length > 0)
        {
            buildSelector();
        }
        return this.each(function ()
        {
            buildPicker(this);
        });
    };

    var selectorOwner;
    var selectorShowing = false;

    var buildPicker = function (element)
    {
        if ($(element).css('display') == 'none')
        {
            return;
        }
        var control = $('<div class="color_picker label-color"><div class="color-' + $(element).val() + '">&nbsp;</div></div>');
        control.click(toggleSelector);
        $(element).after(control);

        //hide the input box
        $(element).hide();
    };

    var buildSelector = function ()
    {
        if ($('#color_selector').length > 0)
        {
            return;
        }
        var selector = $('<div id="color_selector" class="label-color"></div>');

        //add color pallete
        for (var i = 1; i <= 16; i++)
        {
            $('<div class="color_swatch color-' + i + '"  value="' + i + '">&nbsp;</div>')
				.click(function ()
				{
				    changeColor($(this).attr('value'));
				})
				.mouseover(function ()
				{
				    $(this).addClass('hover');
				})
				.mouseout(function ()
				{
				    $(this).removeClass('hover');
				})
				.appendTo(selector);
        };

        $("body").append(selector);
        selector.hide();
    };

    var checkMouse = function (event)
    {
        var selector = "div#color_selector";
        var selectorParent = $(event.target).parents(selector).length;
        if (event.target == $(selector)[0] || event.target == selectorOwner || selectorParent > 0)
        {
            return;
        }
        hideSelector();
    };

    var hideSelector = function ()
    {
        var selector = $("div#color_selector");

        $(document).unbind("mousedown", checkMouse);
        selector.hide();
        selectorShowing = false;
    };

    var showSelector = function ()
    {
        var selector = $("div#color_selector");

        selector
			.css(
			{
			    top: $(selectorOwner).offset().top + ($(selectorOwner).outerHeight()),
			    left: $(selectorOwner).offset().left
			})
			.show();

        //bind close event handler
        $(document).bind("mousedown", checkMouse);
        selectorShowing = true;
    };

    var toggleSelector = function ()
    {
        selectorOwner = this;
        selectorShowing ? hideSelector() : showSelector();
    };

    var changeColor = function (value)
    {
        $(selectorOwner).children()
			.removeClass()
			.addClass('color-' + value);
        $(selectorOwner).prev("input").val(value).change();

        //close the selector
        hideSelector();
    };

})(jQuery);


