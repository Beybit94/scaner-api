$(document).ready(function () {
    var $body = $("body");
    $(document).on({
        ajaxStart: function () { $body.addClass("loading"); },
        ajaxStop: function () {
            $body.removeClass("loading");
        },
        ajaxError: function (e, jqXHR, ajaxSettings, thrownError) {
            $body.removeClass("loading");
        }
    });
});