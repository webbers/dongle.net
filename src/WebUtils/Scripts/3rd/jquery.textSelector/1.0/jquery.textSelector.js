/**
 * jQuery TextSelector Plugin
 * version: 1.0 (2012-03-08)
 *
 * This document is licensed as free software under the terms of the
 * MIT License: http://www.opensource.org/licenses/mit-license.php
 *
 * Author: Afonso França
 */

(function (jQuery)
{
    jQuery.fn.extend(
	{
		getSelection: function()
		{
			var e = this.jquery ? this[0] : this;
			
			if( isDom30(e) )
			{
				var l = e.selectionEnd - e.selectionStart;
				return { start: e.selectionStart, end: e.selectionEnd, length: l, text: e.value.substr(e.selectionStart, l) };
			}
			if( isIe() )
			{
				e.focus();

				var r = document.selection.createRange();
				if (r == null) {
					return { start: 0, end: e.value.length, length: 0 }
				}

				var re = e.createTextRange();
				var rc = re.duplicate();
				re.moveToBookmark(r.getBookmark());
				rc.setEndPoint('EndToStart', re);

				return { start: rc.text.length, end: rc.text.length + r.text.length, length: r.text.length, text: r.text };
			}
			return { start: 0, end: e.value.length, length: 0 };
		},
		
		setSelection: function(selection)
		{
			selection.end = (selection.end == null? selection.start: selection.end);
			var e = this.jquery ? this[0] : this;
			
			if( isDom30(e) )
			{
				e.selectionStart = selection.start;
				e.selectionEnd = selection.end;
				return true;
			}
			if( isIe() )
			{
				e.focus();
				
				var range = e.createTextRange();
				range.collapse(true);
				range.moveStart('character', selection.start);
				range.moveEnd('character', selection.end - selection.start);
				range.select();
				return true;
			}
			return false;
		},

		replaceSelection: function()
		{
			var e = this.jquery ? this[0] : this;
			var text = arguments[0] || '';

			if( isDom30(e) )
			{
				e.value = e.value.substr(0, e.selectionStart) + text + e.value.substr(e.selectionEnd, e.value.length);
				return this;
			}
			if( isIe() )
			{
				e.focus();
				document.selection.createRange().text = text;
				return this;
			}
			e.value += text;
			return this;
	    }
	});
})(jQuery);

function isIe()
{
	return document.selection;
}

function isDom30(e)
{
	return 'selectionStart' in e;
}
