$(function () {

    $('#player-name').val(prompt('Enter your name:', ''));

    $("#start-draft").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Draft/Start",
            data: {
                "name": $("#player-name").val(),
                "players": $("#player-count").val()
            },
            success: function(result) {
                window.location.href = result.Url;
            }
        })
    });

    $("#join-draft").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Draft/Join",
            data: {
                "name": $("#player-name").val()
            },
            success: function (result) {
                window.location.href = result.Url;
            }
        })
    });
});

