$(function () {
    
    //var timer = 60;
    //var timerenabled = true;
    //var myVar = setInterval(function () { myTimer() }, 1000);

    //function myTimer() {
    //    if ($("#State").val() == "Started" && timerenabled) {
    //        timer--;
    //        if (timer >= 0) {
    //            document.getElementById("time-left-to-pick").innerHTML = timer;
    //        }
    //        if (timer < -2) {
    //            $("#draft-container").find("a.pick-card-link")[0].click();
    //            timerenabled = false;
    //        }
    //    }
    //}

    function picks() {
        $.ajax({
            type: "POST",
            url: "/Draft/RefreshPicks",
            data: {
                "id": $("#Id").val()
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
                "id": $("#Id").val()
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
                "id": $("#Id").val()
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
                "id": $("#Id").val()
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

    $("#draft-picks-container").on("click", "a.card-info-link", function (e) {
        e.preventDefault();
        var cardid = $(this).attr("cardid");
        var infohtml = $("#all-cardinfo-container").find("#cardid-" + cardid).html();
        $("#draft-cardinfo-container").html(infohtml);
    });

    $("#draft-container").on("click", "a.pick-card-link", function (e) {
        e.preventDefault();
        timerenabled = false;
        $("#draft-container-to-hide").hide();
        var cardid = $(this).attr("cardid");
        var packnumber = $(this).attr("packnro");
        $.ajax({
            type: "POST",
            url: "/Draft/Pick",
            data: {
                "packnro": packnumber,
                "cardId": cardid,
                "id": $("#Id").val()
            },
            success: function () {                
                draftpicks();
                manacurve();
                picks();
            }
        })
    });

    status();
    picks();
    draftpicks();

    var draft = $.connection.draftHub;
    draft.client.refreshDraft = function () {
        draftpicks();
        timerenabled = true;
        timer = 0;
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