$(function () {

    //function cardinfo(id) {
    //    $.ajax({
    //        type: "POST",
    //        url: "/Draft/ShowCardInfo",
    //        data: {
    //            "id": id
    //        },
    //        success: function (result) {
    //            $("#draft-cardinfo-container").html(result);
    //        }
    //    })
    //}

    function picks() {
        $.ajax({
            type: "POST",
            url: "/Draft/RefreshPicks",
            data: {
                "name": $("#Playername").val()
            },
            success: function (result) {
                $("#draft-picks-container").html(result);
            }
        })
    }

    function manacurve() {
        $.ajax({
            type: "POST",
            url: "/Draft/RefreshManacurve",
            data: {
                "name": $("#Playername").val()
            },
            success: function (result) {
                $("#draft-manacurve-container").html(result);
            }
        })
    }

    function status() {
        $.ajax({
            type: "POST",
            url: "/Draft/RefreshStatus",
            data: {
                "name": $("#Playername").val()
            },
            success: function (result) {
                $("#draft-status-container").html(result);
            }
        })
    }

    function draftpicks() {
        $.ajax({
            type: "POST",
            url: "/Draft/RefreshDraft",
            data: {
                "name": $("#Playername").val()
            },
            success: function (result) {
                $("#draft-container").html(result);
            }
        })
    }
    
    $("#draft-container").on("click", "a.card-info-link", function (e) {
        e.preventDefault();
        var cardid = $(this).attr("cardid");
        var infohtml = $("#all-cardinfo-container").find("#cardid-"+cardid).html();
        $("#draft-cardinfo-container").html(infohtml);
    });

    $("#draft-container").on("click", "a.pick-card-link", function (e) {
        e.preventDefault();
        var cardid = $(this).attr("cardid");
        var packnumber = $(this).attr("packnro");
        $.ajax({
            type: "POST",
            url: "/Draft/Pick",
            data: {
                "name": $("#Playername").val(),
                "packnro": packnumber,
                "id": cardid
            },
            success: function () {
                picks();
                draftpicks();
                manacurve();
            }
        })
    });

    status();
    picks();
    draftpicks();

    var draft = $.connection.draftHub;
    draft.client.refreshDraft = function () {
        draftpicks();
    };

    draft.client.refreshPicks = function () {
        picks();
        manacurve();
    };

    draft.client.refreshStatus = function () {
        status();
    };

    $.connection.hub.start().done(function () {
    });
});