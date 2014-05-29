﻿'use strict'

services.factory('cookieService', function () {
    function setUser(user) {
        setCookie('username', user.name, new Date(2099, 1,1));
        setCookie('login', user.email, new Date(2099, 1, 1));
    }

    function getUser() {
        var username = getCookie('username');
        var login = getCookie('login');

        return {  name:username, email:login };
    }

    function removeUser() {
        deleteCookie('username');
        deleteCookie('login');
    }

    function setCookie (name, value, expires, path, domain, secure) {
        document.cookie = name + "=" + escape(value) +
          ((expires) ? "; expires=" + expires : "") +
          ((path) ? "; path=" + path : "") +
          ((domain) ? "; domain=" + domain : "") +
          ((secure) ? "; secure" : "");
    }

    function getCookie (name) {
        var cookie = " " + document.cookie;
        var search = " " + name + "=";
        var setStr = null;
        var offset = 0;
        var end = 0;
        if (cookie.length > 0) {
            offset = cookie.indexOf(search);
            if (offset != -1) {
                offset += search.length;
                end = cookie.indexOf(";", offset)
                if (end == -1) {
                    end = cookie.length;
                }
                setStr = unescape(cookie.substring(offset, end));
            }
        }
        return (setStr);
    }

    function deleteCookie(name) {
        setCookie(name, '', new Date(1971, 1, 1));
    }

    return {
        setUser: setUser,
        getUser: getUser,
        removeUser: removeUser,
        getCookie: getCookie,
        setCookie: setCookie,
        deleteCookie: deleteCookie
    };
});