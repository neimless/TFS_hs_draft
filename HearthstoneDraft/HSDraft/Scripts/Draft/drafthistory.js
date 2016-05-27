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

    $("#draft-history-container").on("click", "a.card-info-link", function (e) {
        e.preventDefault();
        var cardid = $(this).attr("cardid");
        var infohtml = $("#all-cardinfo-container").find("#cardid-" + cardid).html();
        $("#draft-cardinfo-container").html(infohtml);
    });
});

