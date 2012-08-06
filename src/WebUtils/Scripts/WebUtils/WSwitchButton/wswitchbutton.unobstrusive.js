$(document).ready(function () {
    $('.wswitchbutton').each(function () {
        var opt = {};
        var yes = $(this).attr('yes');
        var no = $(this).attr('no');

        if (yes) {
            opt.yes = yes;
        }
        if (no) {
            opt.no = no;
        }
        $(this).wswitchbutton(opt);
    });
});