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
    jQuery.validator.unobtrusive.adapters.add('wrequiredif', ['conditionalproperty', 'conditionalvalue'], function (options) {
        options.rules['wrequiredif'] = options.params;
        if (options.message) {
            options.messages['wrequiredif'] = options.message;
        }
    });

    jQuery.validator.addMethod("wrequiredif", function (value, element, param) {
        if (param.conditionalproperty == null && param.conditionalvalue == null) {
            return true;
        }

        var $form = $(element).closest('form');

        var $element = $form.find('[name=' + param.conditionalproperty + ']');

        var propertyValue;
        if ($element && $element.is('[type=checkbox]')) {
            var checked = $element.attr('checked');
            propertyValue = (checked ? true : false).toString();
        }
        else {
            propertyValue = $element.val();
        }



        if (propertyValue != param.conditionalvalue) {
            return true;
        }

        return value != null && value != '';
    });
} (jQuery));