let current_request = 0
var pressedKeys = {};

$(function() {
    window.onkeyup = function(e) { pressedKeys[e.keyCode] = false; }
    window.onkeydown = function(e) { pressedKeys[e.keyCode] = true; }
    window.addEventListener('message', function (event) {
        var item = event.data;

        if (item.show) {
            $("#main_text").val("");
            $(".main").fadeIn( "swing" );
            $("#main_text").focus();
        }

        if (item.request) {
            current_request = item.request
        }

        if (item.maxlength) {
            $("#main_text").attr('maxlength',item.maxlength);
           
            $("#title_").html("Wpisz treść (max " + item.maxlength + "):");
        }

        if (item.hide) {
            $(".main").fadeOut( "swing" );
        }

    });

    $("#send").click(function() {
        let text = $("#main_text").val();
        test.replace(/([\u2700-\u27BF]|[\uE000-\uF8FF]|\uD83C[\uDC00-\uDFFF]|\uD83D[\uDC00-\uDFFF]|[\u2011-\u26FF]|\uD83E[\uDD10-\uDDFF])/g, '');
        $(".main").fadeOut( "swing" );
        $.post("https://mmkeyboard/response", JSON.stringify({
            request: current_request,
            value: text
        }));
    });

    $("textarea").focusin(function() {
        $.post("https://mmkeyboard/allowmove", JSON.stringify({
            allowmove: false
        }));
    });

    $("textarea").focusout(function() {
        $.post("https://mmkeyboard/allowmove", JSON.stringify({
            allowmove: true
        }));
    });

    jQuery(document).on("keydown", function (evt) {
        if (evt.keyCode == 27) {
            $.post("https://mmkeyboard/response", JSON.stringify({
                request: current_request,
                value: ""
            }));
            $(".main").fadeOut( "swing" );
        }
        if(!pressedKeys[16] && evt.keyCode == 13) {
            document.getElementById('main_text').disabled=true;
            let text = $("#main_text").val();
            text.replace(/([\u2700-\u27BF]|[\uE000-\uF8FF]|\uD83C[\uDC00-\uDFFF]|\uD83D[\uDC00-\uDFFF]|[\u2011-\u26FF]|\uD83E[\uDD10-\uDDFF])/g, '');
            $(".main").fadeOut( "swing" );
            $.post("https://mmkeyboard/response", JSON.stringify({
                request: current_request,
                value: text
            }));
            document.getElementById('main_text').disabled=false;
        }
    });
});