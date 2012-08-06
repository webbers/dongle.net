/*
* WRequiredIf 1.0
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
$(function () {
    jQuery.validator.unobtrusive.adapters.add("wrequired", function (options) {
        // jQuery Validate equates "required" with "mandatory" for checkbox elements
        if (options.element.tagName.toUpperCase() !== "INPUT" || options.element.type.toUpperCase() !== "CHECKBOX") {
            options.rules["wrequired"] = true;
            if (options.message) {
                options.messages["wrequired"] = options.message;
            }
        }
    });

    jQuery.validator.addMethod("wrequired", function (value, element, param) {
        return value != null && value != '';
    });
} (jQuery));