/*
* WMultiSelect 1.0
* Copyright (c) 2011 Webers
*
* Depends:
*   - jQuery 1.4.2+
*   - jQuery UI 1.8 widget factory
*   - jQuery MultiSelect UI Widget 1.11: http://www.erichynds.com/jquery/jquery-ui-multiselect-widget
*
* Dual licensed under MIT or GPLv2 licenses
*   http://en.wikipedia.org/wiki/MIT_License
*   http://en.wikipedia.org/wiki/GNU_General_Public_License
*
*/
(function ($)
{
    $.WMultiSelect = function (element, options)
    {
        var $element = $(element);

        var defaults = {
            selectedText: "# selecionados",
            selectedList: 2,
            header: false
        };

        var plugin = this;

        plugin.settings = {};
        plugin.init = function ()
        {
            plugin.settings = $.extend({}, defaults, options);
            $element.multiselect(plugin.settings);
        };

        plugin.init();
    };

    $.fn.wmultiselect = function (options)
    {
        return this.each(function ()
        {
            if (undefined == $(this).data('wmultiselect'))
            {
                var plugin = new $.WMultiSelect(this, options);
                $(this).data('wmultiselect', plugin);
            }
        });
    };


})(jQuery);
$(document).ready(function () {
    $('.wmultiselect').wmultiselect();
    $('.wmultiselect-opened').wmultiselect({stillOpen:true});
})