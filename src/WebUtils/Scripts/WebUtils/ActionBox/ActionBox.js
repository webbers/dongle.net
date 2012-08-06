/*
* ActionBox 1.0
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
    var HashInfo =
	{
	    fromString: function (hash)
	    {
	        hash = hash.replace('#', '');
	        hash = hash.replace('%23', '');

	        var firstBar = hash.indexOf('/');
	        var name;

	        if (firstBar > -1)
	        {
	            name = hash.substr(0, firstBar);
	        }
	        else
	        {
	            var firstQuestion = hash.indexOf('?');
	            if (firstQuestion > -1)
	            {
	                name = hash.substr(0, firstQuestion);
	            }
	            else
	            {
	                name = hash;
	            }
	        }
	        var params = hash.substr(name.length + 1).replace(/[\/\?]/g, '&');
	        var paramsObj = {};

	        var parameterList = params.split('&');

	        jQuery(parameterList).each(function ()
	        {
	            if (this == null)
	            {
	                return;
	            }
	            var data = this.split('=');
	            if (data.length == 1)
	            {
	                return;
	            }
	            paramsObj[data[0]] = data[1];
	        });

	        var id = null;
	        if (parameterList.length > 0 && parameterList[0].split('=').length == 1)
	        {
	            id = parameterList[0];
	            paramsObj.id = id;
	        }

	        return { id: id, name: name, params: paramsObj };
	    },

	    toString: function (hashInfo)
	    {
	        var hash = hashInfo.name;

	        if (hashInfo.id)
	        {
	            hash += "/" + hashInfo.id;
	        }

	        if (hashInfo.params != 0)
	        {
	            hash += "?";

	            for (var key in hashInfo.params)
	            {
	                var obj = hashInfo.params[key];
	                hash += key + "=" + obj + "&";
	            }
	            hash = hash.substr(0, hash.length - 1);
	        }

	        return hash;
	    }
	};

    $.actionbox =
	{
	    actions: {},

	    init: function (defaultHash)
	    {
	        $.history.init(function (hash)
	        {
	            hash = hash ? hash : defaultHash;
	            var hashInfo = HashInfo.fromString(hash);
	            var action = $.actionbox.actions[hashInfo.name];

	            if (action != null)
	            {
	                if (action.ajax != null && action.dataHandler != null)
	                {
	                    action.ajax.data = action.dataHandler(hashInfo);
	                    jQuery.ajax(action.ajax);
	                }
	                if (action.execute)
	                {
	                    action.execute(hashInfo);
	                }
	            }
	        });

	        this.run(location.hash.replace('#', ''));
	    },

	    add: function (action)
	    {
	        $.actionbox.actions[action.name] = action;
	    },

	    run: function (hash)
	    {
	        if (typeof (hash) == "string")
	        {
	            jQuery.history.load(hash);
	        }
	        else
	        {
	            jQuery.history.load(HashInfo.toString(hash));
	        }
	    },

	    redo: function ()
	    {
	        this.run(location.hash.substr(1));
	    }
	};
})(jQuery);