'use strict';

controllers.controller('loginController', function ($scope, userService, cookieService) {
    $scope.username = '';
    $scope.password = '';
    $scope.rememberMe = true;

    $scope.login = function (username, password, rememberMe) {
        if (rememberMe) {
            var login = cookieService.setCookie('login', username)
            var pwd = cookieService.setCookie('pwd', password)
        }
        userService.login(username, password);
    };

    userService.onLoginFailed(function () {
        toastr.error('Invalid username or password.', 'Login Error!')
    });
})