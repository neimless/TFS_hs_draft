$(function () {
    $(".draft-history-link").on("click", function (e) {
        e.preventDefault();
        if ($(this).next(".draft-history-picks").is(":visible")) {
            $(this).next(".draft-history-picks").hide();
        }
        else {
            $(this).next(".draft-history-picks").show();
        }
    });
});

