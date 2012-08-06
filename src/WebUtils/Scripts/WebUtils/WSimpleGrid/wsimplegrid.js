/*
* WSimple Grid 1.0.0
* Copyright (c) 2012 Webers
*
* Depends:
*   - jQuery 1.4.2+
*
* The MIT License
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
(function (jQuery) {

    jQuery.fn.wsimplegrid = function (options) {
        var $element = $(this);
        var defaults = {
            columnClass: 'wsimpleGrid',
            filterButtonValue: "Filtrar",
            removeFilterButtonValue: "Remover filtro"
        };
        var remove;
        var removed;
        var allRowsTable = $element.find('tbody tr');
        var data = jQuery.extend(defaults, options);
        var $columnClass = $("." + data.columnClass);
        var indexClassArray;
        var appendIcon = function () {
            $columnClass.append('<div class="wsimplegrid-filter-icon wsimplegrid-item wsimplegrid-filter-icon-fix"></div>').click(function (e) {
                var filterInput = $('<input type="text" name="filter" class="wsimplegrid-item" />');
                var filterButton = $('<div name="filter-button" class="wbutton wbutton-normal">' + data.filterButtonValue + '</div>');
                var removeFilterButton = $('<div name="remove-filter-button" class="wbutton wbutton-alert">' + data.removeFilterButtonValue + '</div>');
                if ($(e.target).hasClass('wsimplegrid-filter-icon-fix')) {
                    e.stopPropagation();
                    var indexColumn = $(this).index();
                    var i = 0;
                    $columnClass.each(function () {
                        if (this.cellIndex == indexColumn) {
                            indexClassArray = i;
                        }
                        i++;
                    });
                    var changeIcon = function (to, indexColumnRemove, toRemove) {
                        var $filterIconToChange = $columnClass.eq(indexColumnRemove);
                        var $filterIconToChangeRemove = $columnClass.eq(toRemove);
                        if (to == true) {
                            $filterIconToChange.find('.wsimplegrid-filter-icon').addClass('wsimplegrid-filter-icon-active');
                            $filterIconToChange.find('.wsimplegrid-filter-icon').removeClass('wsimplegrid-filter-icon');
                            remove = true;
                        }
                        else {
                            $filterIconToChangeRemove.find('.wsimplegrid-filter-icon-active').addClass('wsimplegrid-filter-icon');
                            $filterIconToChangeRemove.find('.wsimplegrid-filter-icon-active').removeClass('wsimplegrid-filter-icon-active');
                            remove = false;
                            removed = toRemove;
                        }
                    };

                    $('.wsimplegrid-filter-panel').remove();
                    var filterPanel = $('<div class="wsimplegrid-filter-panel wsimplegrid-item" index-filter="' + $(e.target).parent().index('.' + data.columnClass) + '"></div>')
                        .css('top', (e.pageY + 18) + 'px')
                        .css('left', (e.pageX + 5) + 'px');

                    filterButton.click(function () {
                        if (filterInput.val() != '') {
                            $(".wsimplegrid-filter-icon-fix").eq(indexClassArray).parent().removeAttr('filter-key');
                            if ($($element).find('thead th[filter-key]').length == 0) {
                                allRowsTable.show();
                            }
                            $(".wsimplegrid-filter-icon-fix").eq(indexClassArray).parent().attr('filter-key', filterInput.val());
                            $($element).find('thead th[filter-key]').each(function () {
                                $($element).find('tbody tr:visible').each(function () {
                                    if ($(this).find('td:eq(' + indexColumn + ')').text().indexOf(filterInput.val()) < 0) {
                                        $(this).find('td:eq(' + indexColumn + ')').parent().hide();
                                    }
                                });
                            });
                            changeIcon(true, indexClassArray);
                        }
                    });
                    removeFilterButton.click(function () {
                        var indexToRemove = $(this).parent().attr('index-filter');
                        if (filterInput.attr('filter-input-key') != '') {
                            $('.' + data.columnClass + ':eq(' + $('.wsimplegrid-item').eq(indexToRemove).index('.wsimplegrid-item') + ')').removeAttr('filter-key');
                            var p = 0;
                            if ($($element).find('thead th[filter-key]').length == 0) {
                                allRowsTable.show();
                            }
                            else {
                                $($element).find('thead th[filter-key]').each(function () {
                                    var indexColumnRemove = $(this).index();
                                    var filterKeyToRefilter = $(this).attr('filter-key');
                                    if (p == 0) {
                                        $($element).find('tbody tr:hidden').each(function () {
                                            if ($(this).find('td:eq(' + indexColumnRemove + ')').text().indexOf(filterKeyToRefilter) >= 0) {
                                                $(this).find('td:eq(' + indexColumnRemove + '):contains(' + filterKeyToRefilter + ')').parent().show();
                                            }
                                        });
                                    }
                                    else {
                                        $($element).find('tbody tr:visible').each(function () {
                                            if ($(this).find('td:eq(' + indexColumnRemove + ')').text().indexOf(filterKeyToRefilter) >= 0) {
                                                $(this).find('td:eq(' + indexColumnRemove + '):contains(' + filterKeyToRefilter + ')').parent().show();
                                            }
                                            else {
                                                $(this).find('td:eq(' + indexColumnRemove + ')').parent().hide();
                                            }
                                        });
                                    }
                                    p++;
                                });
                            }
                            changeIcon(false, indexClassArray, indexToRemove);
                        }
                    });


                    var filterValue = $(".wsimplegrid-filter-icon-fix").eq(indexClassArray).parent().attr('filter-key');
                    if (filterValue) {
                        filterPanel.append(filterInput).append(filterButton).append(removeFilterButton);
                        filterInput.val(filterValue);
                        filterInput.attr('filter-input-key', filterValue);

                    }
                    else {
                        filterPanel.append(filterInput).append(filterButton);
                        filterInput.val(filterValue);
                        filterInput.attr('filter-input-key', filterValue);
                    }
                    $('body').append(filterPanel);
                    filterInput.focus();
                    filterInput.keypress(function () {
                        if (event.keyCode == 13) {
                            filterButton.trigger('click');
                        }
                    });
                }
            });
        };

        appendIcon();

    };


})(jQuery);
jQuery(document).click(function (e) {
    if (!$(e.target).hasClass('wsimplegrid-item')) {
        $('.wsimplegrid-filter-panel').remove();
    }
});