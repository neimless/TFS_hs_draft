$(function () {

    var joining = false;

    $("#start-draft").on("click", function (e) {
        e.preventDefault();
        joining = true;
        if ($("#new-draft-form").valid())
        {        
            $.ajax({
                type: "POST",
                url: "/Draft/Start",
                data: {
                    "players": $("#PlayerCount").val(),
                },
                success: function(result) {
                    window.location.href = result.Url;
                }
            })
        }
    });

    $(".join-draft").on("click", function (e) {
        e.preventDefault();
        joining = true;
        $.ajax({
            type: "POST",
            url: "/Draft/Join",
            data: {
                "id": $(this).attr("draftid")
            },
            success: function (result) {
                window.location.href = result.Url;
            }
        })
    });

    var drafthubConnection = $.connection.draftHub;
    drafthubConnection.client.refreshLobby = function () {
        if (!joining)
        {
            location.reload();
        }        
    };

    $.connection.hub.start().done(function () {
    });
});

