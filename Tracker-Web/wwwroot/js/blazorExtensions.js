
function writeCookie(name, value, days) {

    var expires;
    if (days) {
        var date = new Date;
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    else {
        expires = "";
    }

    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(cookieName) {

    var name = cookieName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieList = decodedCookie.split(";");

    for (var i = 0; i < cookieList.length; i++) {
        var cookie = cookieList[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }

        if (cookie.indexOf(name) == 0) {
            return cookie.substring(name.length, cookie.length);
        }
    }
    return "";
}