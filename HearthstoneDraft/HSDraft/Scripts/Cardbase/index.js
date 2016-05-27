$(function () {

    $("#newcard").hide();
    $("#add-new-card-link").on("click", function (e) {
        e.preventDefault();
        $("#newcard").show();
    });
});