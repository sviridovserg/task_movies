'use strict'

controllers.controller('indexController', function ($scope, $location, userService, cookieService) {
    $scope.loggedIn = userService.getLoggedInUser() != null;
    var loginCookie = cookieService.getCookie('login');
    var pwdCookie = cookieService.getCookie('pwd');
    if (loginCookie != null && loginCookie != undefined && pwdCookie != null && pwdCookie != undefined) {
        userService.login(loginCookie, pwdCookie);
    }

    userService.onLogin(function () {
        $scope.loggedIn = userService.getLoggedInUser() != null;
    });

    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };

    $scope.logout = function () {
        userService.logout();
        window.location.href = 'index.htm';

    }
});