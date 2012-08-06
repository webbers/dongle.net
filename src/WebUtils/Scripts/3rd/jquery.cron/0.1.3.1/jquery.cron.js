/*
* jQuery gentleSelect plugin
* http://shawnchin.github.com/jquery-cron
*
* Copyright (c) 2010 Shawn Chin. 
* Dual licensed under the MIT or GPL Version 2 licenses.
* 
* Requires:
* - jQuery
* - jQuery gentleSelect plugin 
*
* Usage:
*  (JS)
*
*  // initialise like this
*  var c = $('#cron').cron({
*    initial: '9 10 * * *', # Initial value. default = "* * * * *"
*    url_set: '/set/', # POST expecting {"cron": "12 10 * * 6"}
*  });
* 
*  // you can update values later 
*  c.cron("value", "1 2 3 4 *");
*
* // you can also get the current value using the "value" option
* alert(c.cron("value"));
*
*  (HTML)
*  <div id='cron'></div>
*
* Notes:
* At this stage, we only support a subset of possible cron options. 
* For example, each cron entry can only be digits or "*", no commas
* to denote multiple entries. We also limit the allowed combinations:
* - Every minute : * * * * *
* - Every hour   : ? * * * *
* - Every day    : ? ? * * *
* - Every week   : ? ? * * ?
* - Every month  : ? ? ? * *
* - Every year   : ? ? ? ? *
*/
(function ($)
{
    var defaults = {
        initial: "0 * * * * ?",
        url_set: undefined,
        customValues: undefined,
        onChange: undefined, // callback function each time value changes
        language:
        {
            monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
            timeNames: ['minute', 'hour', 'day', 'week', 'month', 'year'],
            cronWords: ['Every', 'at', 'at', 'on the', 'of', 'minutes past hour']
        }
    };

    // -------  build some static data -------

    // options for minutes in an hour
    var strOptMih = "";
    var i;
    var j;

    for (i = 0; i < 60; i++)
    {
        j = (i < 10) ? "0" : "";
        strOptMih += "<option value='" + i + "'>" + j + i + "</option>\n";
    }

    // options for hours in a day
    var strOptHid = "";
    for (i = 0; i < 24; i++)
    {
        j = (i < 10) ? "0" : "";
        strOptHid += "<option value='" + i + "'>" + j + i + "</option>\n";
    }

    // options for days of month
    var strOptDom = "";
    for (i = 1; i < 32; i++)
    {
        strOptDom += "<option value='" + i + "'>" + i + "</option>\n";
    }

    // options for months
    var getMonthOptions = function (opts)
    {
        var strOptMonth = "";
        var months = opts.language.monthNames;
        for (i = 0; i < months.length; i++)
        {
            strOptMonth += "<option value='" + (i + 1) + "'>" + months[i] + "</option>\n";
        }
        return strOptMonth;
    };

    // options for day of week
    var getDayOfWeekOptions = function (opts)
    {
        var strOptDow = "";
        var days = opts.language.dayNames;
        for (i = 0; i < days.length; i++)
        {
            strOptDow += "<option value='" + i + "'>" + days[i] + "</option>\n";
        }
        return strOptDow;
    };

    // options for period
    var getPeriodOptions = function (opts)
    {
        var strOptPeriod = "";
        var periods = ["minute", "hour", "day", "week", "month", "year"];
        for (i = 0; i < periods.length; i++)
        {
            strOptPeriod += "<option value='" + periods[i] + "'>" + opts.language.timeNames[i] + "</option>\n";
        }

        return strOptPeriod;
    };


    // display matrix
    var toDisplay = {
        seconds: [],
        minute: [],
        hour: ["mins"],
        day: ["time"],
        week: ["dow", "time"],
        month: ["dom", "time"],
        year: ["dom", "month", "time"]
    };

    var combinations = {
        minute: /^0\s(\*\s){3}\*\s\?$/,                 // "0 * * * * ?"
        hour: /^0\s\d{1,2}\s(\*\s){2}\*\s\?$/,          // "0 # * * * ?"
        day: /^0\s(\d{1,2}\s){2}(\*\s){2}\?$/,          // "0 # # * * ?"
        week: /^0\s(\d{1,2}\s){2}(\*\s){2}\d{1,2}$/,    // "0 # # * * #"
        month: /^0\s(\d{1,2}\s){3}\*\s\?$/,             // "0 # # # * ?"
        year: /^0\s(\d{1,2}\s){4}\?$/                   // "0 # # # # ?"
    };

    // ------------------ internal functions ---------------
    function defined(obj)
    {
        if (typeof obj == "undefined") { return false; }
        else { return true; }
    }

    function undefinedOrObject(obj)
    {
        return (!defined(obj) || typeof obj == "object");
    }

    function getCronType(cronStr)
    {
        // check format of initial cron value
        var validCron = /^((\d{1,2}|\*)\s){5}(\d{1,2}|\*|\?)$/;
        if (typeof cronStr != "string" || !validCron.test(cronStr))
        {
            $.error("cron: invalid initial value");
            return undefined;
        }
        // check actual cron values
        var d = cronStr.split(" ");
        //           seg, mm, hh, DD, MM, DOW
        var minval = [0, 0, 0, 1, 1, 0];
        var maxval = [59, 59, 23, 31, 12, 6];
        for (i = 0; i < d.length; i++)
        {
            if (d[i] == "*" || d[i] == "?") continue;
            var v = parseInt(d[i]);
            if (defined(v) && v <= maxval[i] && v >= minval[i]) continue;

            $.error("cron: invalid value found (col " + (i + 1) + ") in " + o.initial);
            return undefined;
        }

        // determine combination
        for (var t in combinations)
        {
            if (combinations[t].test(cronStr)) { return t; }
        }

        // unknown combination
        $.error("cron: valid but unsupported cron format. sorry.");
        return undefined;
    }

    function hasError(c, o)
    {
        if (!defined(getCronType(o.initial))) { return true; }
        if (!undefinedOrObject(o.customValues)) { return true; }
        return false;
    }

    function getCurrentValue(c)
    {
        var b = c.data("block");
        var min = hour = day = month = "*";
        var sec = "0";
        var dow = "?";

        var selectedPeriod = b["period"].find("select").val();
        switch (selectedPeriod)
        {
            case "minute":
                break;

            case "hour":
                min = b["mins"].find("select").val();
                break;

            case "day":
                min = b["time"].find("select.cron-time-min").val();
                hour = b["time"].find("select.cron-time-hour").val();
                break;

            case "week":
                min = b["time"].find("select.cron-time-min").val();
                hour = b["time"].find("select.cron-time-hour").val();
                dow = b["dow"].find("select").val();
                break;

            case "month":
                min = b["time"].find("select.cron-time-min").val();
                hour = b["time"].find("select.cron-time-hour").val();
                day = b["dom"].find("select").val();
                break;

            case "year":
                min = b["time"].find("select.cron-time-min").val();
                hour = b["time"].find("select.cron-time-hour").val();
                day = b["dom"].find("select").val();
                month = b["month"].find("select").val();
                break;

            default:
                // we assume this only happens when customListValues is set
                return selectedPeriod;
        }
        return [sec, min, hour, day, month, dow].join(" ");
    }

    function setCurrentValue(c, cronStr)
    {
        var cronType = getCronType(cronStr);
        if (!defined(cronType) || c == null) { return false; }

        var block = c.data("block");
        var d = cronStr.split(" ");
        var value = { sec: d[0], mins: d[1], hour: d[2], dom: d[3], month: d[4], dow: d[5] };

        // update appropriate select boxes
        var targets = toDisplay[cronType];
        for (i = 0; i < targets.length; i++)
        {
            var target = targets[i];
            if (target == "time")
            {
                block[target]
                        .find("select.cron-time-hour")
                            .val(value.hour)
                            .end()
                        .find("select.cron-time-min")
                            .val(value.mins)
                            .end();
            }
            else
            {
                block[target].find("select").val(value[target]);
            }
        }

        // trigger change event
        block["period"]
                .find("select")
                .val(cronType)
                .trigger("change");

        return c;
    }

    // -------------------  PUBLIC METHODS -----------------

    var methods = {
        init: function (opts)
        {
            // init options
            var options = opts ? opts : {}; /* default to empty obj */
            var defaultLanguages = defaults.language;
            var o = $.extend(defaults, options);
            o.language = $.extend(defaultLanguages, options.language);

            var elementId = this.attr("id");
            var $valueField = $('<input type="hidden" id="' + elementId + '" name="'+ elementId +'"/>');
            this.attr('id', elementId + "-field");
            this.after($valueField);

            if (this.attr("initial") != null)
            {
                o.initial = this.attr("initial");
            }

            $valueField.attr("value", o.initial);

            // error checking
            if (hasError(this, o)) { return this; }

            var block = [];
            var customPeriods = "";
            var cv = o.customValues;

            if (defined(cv))
            { // prepend custom values if specified
                for (var key in cv)
                {
                    customPeriods += "<option value='" + cv[key] + "'>" + key + "</option>\n";
                }
            }
            block.period = $("<span class='cron-period'>"
                    + o.language.cronWords[0] + " <select name='cron-period'>" + customPeriods
                    + getPeriodOptions(o) + "</select> </span>")
                .appendTo(this)
                .find("select")
                    .bind("change.cron", eventHandlers.periodChanged)
                    .data("root", this)
                    .end();

            block.dom = $("<span class='cron-block cron-block-dom'>"
                    + " " + o.language.cronWords[3] + " <select name='cron-dom'>" + strOptDom
                    + "</select> </span>")
                .appendTo(this)
                .data("root", this)
                .find("select")
                    .data("root", this)
                    .end();

            block.month = $("<span class='cron-block cron-block-month'>"
                    + " " + o.language.cronWords[4] + " <select name='cron-month'>" + getMonthOptions(o)
                    + "</select> </span>")
                .appendTo(this)
                .data("root", this)
                .find("select")
                    .data("root", this)
                    .end();

            block.mins = $("<span class='cron-block cron-block-mins'>"
                    + " " + o.language.cronWords[1] + " <select name='cron-mins'>" + strOptMih
                    + "</select> " + o.language.cronWords[5] + " </span>")
                .appendTo(this)
                .data("root", this)
                .find("select")
                    .data("root", this)
                    .end();

            block.dow = $("<span class='cron-block cron-block-dow'>"
                    + ",   <select name='cron-dow'>" + getDayOfWeekOptions(o)
                    + "</select> </span>")
                .appendTo(this)
                .data("root", this)
                .find("select")
                    .data("root", this)
                    .end();

            block.time = $("<span class='cron-block cron-block-time'>"
                    + " " + o.language.cronWords[2] + " <select name='cron-time-hour' class='cron-time-hour'>" + strOptHid
                    + "</select>:<select name='cron-time-min' class='cron-time-min'>" + strOptMih
                    + " </span>")
                .appendTo(this)
                .data("root", this)
                .find("select.cron-time-hour")
                    .data("root", this)
                    .end()
                .find("select.cron-time-min")
                    .data("root", this)
                    .end();

            block.controls = $("<span class='cron-controls'>&laquo; save "
                    + "<span class='cron-button cron-button-save'></span>"
                    + " </span>")
                .appendTo(this)
                .data("root", this)
                .find("span.cron-button-save")
                    .bind("click.cron", eventHandlers.saveClicked)
                    .data("root", this)
                    .end();

            this.find("select").bind("change.cron-callback", eventHandlers.somethingChanged);
            this.data("options", o).data("block", block); // store options and block pointer
            this.data("current_value", o.initial); // remember base value to detect changes

            return methods["value"].call(this, o.initial); // set initial value
        },

        getCron: function (cronStr)
        {
            var options = {};
            var defaultLanguages = defaults.language;
            var o = $.extend(defaults, options);
            o.language = $.extend(defaultLanguages, options.language);

            var cronType = getCronType(cronStr);
            if (!defined(cronType)) { return false; }

            var d = cronStr.split(" ");
            var value = { sec: d[0], mins: d[1], hour: d[2], dom: d[3], month: d[4], dow: d[5] };

            //todo: leading zeros nas horas

            var text = o.language.cronWords[0] + " ";

            if (cronType == "minute")
            {
                //a cada minuto
                return text + o.language.timeNames[0];
            }
            if (cronType == "hour")
            {
                //A cada hora aos 5 minutos
                return text +
                       o.language.timeNames[1] + " " +
                       o.language.cronWords[1] + " " +
                       value.mins + " " +
                       o.language.cronWords[5];
            }
            if (cronType == "day")
            {
                //A cada dia às 01:02"
                text += o.language.timeNames[2] + " ";
            }

            if (cronType == "week")
            {
                //"A cada semana, Segunda-feira às 01:02"
                text += o.language.timeNames[3] + ", " +
                        o.language.dayNames[value.dow];
            }

            if (cronType == "month")
            {
                //A cada mês no dia 1 às 01:02
                text += o.language.timeNames[4] + " " +
                        o.language.cronWords[3] + " " +
                        value.dom;
            }

            if (cronType == "year")
            {
                //A cada ano no dia 4 de Fevereiro às 02:03
                text += o.language.timeNames[5] + " " +
                        o.language.cronWords[3] + " " +
                        value.dom + " " +
                        o.language.cronWords[4] + " " +
                        o.language.monthNames[value.month - 1];
            }

            return text + " " +
                   o.language.cronWords[2] + " " +
                   (value.hour < 10 ? "0" : "") + value.hour + ":" +
                   (value.mins < 10 ? "0" : "") + value.mins;
        },

        value: function (cronStr)
        {
            // when no args, act as getter
            if (!cronStr) { return getCurrentValue(this); }
            return setCurrentValue(this, cronStr);
        }
    };

    var eventHandlers = {
        periodChanged: function ()
        {
            var root = $(this).data("root");
            var block = root.data("block");

            var period = $(this).val();
            root.find("span.cron-block").hide(); // first, hide all blocks
            if (toDisplay.hasOwnProperty(period))
            { // not custom value
                var b = toDisplay[$(this).val()];
                for (i = 0; i < b.length; i++)
                {
                    block[b[i]].show();
                }
            }
        },

        somethingChanged: function ()
        {
            var $inputField = $(this).closest('.cron');
            var $valueField = $inputField.next();
            $valueField.val(methods["value"].call($inputField));

            var root = $(this).data("root");
            // if AJAX url defined, show "save"/"reset" button
            if (defined(root.data("options").url_set))
            {
                if (methods.value.call(root) != root.data("current_value"))
                { // if changed
                    root.addClass("cron-changed");
                    root.data("block")["controls"].fadeIn();
                } else
                { // values manually reverted
                    root.removeClass("cron-changed");
                    root.data("block")["controls"].fadeOut();
                }
            } else
            {
                root.data("block")["controls"].hide();
            }

            // chain in user defined event handler, if specified
            var oc = root.data("options").onChange;
            if (defined(oc) && $.isFunction(oc))
            {
                oc.call(root);
            }
        },

        saveClicked: function ()
        {
            var btn = $(this);
            var root = btn.data("root");
            var cronStr = methods.value.call(root);

            if (btn.hasClass("cron-loading")) { return; } // in progress
            btn.addClass("cron-loading");

            $.ajax({
                type: "POST",
                url: root.data("options").url_set,
                data: { "cron": cronStr },
                success: function ()
                {
                    root.data("current_value", cronStr);
                    btn.removeClass("cron-loading");
                    // data changed since "save" clicked?
                    if (cronStr == methods.value.call(root))
                    {
                        root.removeClass("cron-changed");
                        root.data("block").controls.fadeOut();
                    }
                },
                error: function ()
                {
                    alert("An error occured when submitting your request. Try again?");
                    btn.removeClass("cron-loading");
                }
            });
        }
    };

    $.fn.cron = function (method)
    {
        return this.each(function ()
        {
            if (methods[method])
            {
                methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
            }
            else if (typeof method === 'object' || !method)
            {
                if (!$(this).data('cron'))
                {
                    methods.init.apply(this, arguments);
                    $(this).data('cron', true);
                }
            }
            else
            {
                $.error('Method ' + method + ' does not exist on jQuery.cron');
            }
        });
    };

    $.fn.cron = function (method)
    {
        if (methods[method])
        {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof method === 'object' || !method)
        {
            return methods.init.apply(this, arguments);
        }
        else
        {
            $.error('Method ' + method + ' does not exist on jQuery.cron');
        }
        return false;
    };

    $.cronText = function (cron)
    {
        return methods.getCron(cron);
    };

})(jQuery);