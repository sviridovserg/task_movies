'user strict';

services.factory('userService', function ($http, $location, cookieService) {
    var loggedInUser = null;
    var token = null;
    var onLoginCallbacks = [];

    function login(username, pwd) {
        $http.post('/service/AuthService.svc/json/auth', { Login: username, Password: pwd }).then(function (response) {
            var savedToken = cookieService.getCookie('token', token);
            if (savedToken != null && savedToken != undefined) {
                loggedInUser = cookieService.getUser();
                $http.defaults.headers.common['Authorization'] = 'Basic ' + savedToken;
                fireOnLogin();
                return;
            }

            setUser(response.data);
        });
    }
    
    function setUser(data) {
        token = data.Token;
        loggedInUser =  {
            name: data.Name,
            email: data.Login,
        };

        $http.defaults.headers.common['Authorization'] = 'Basic ' + token;
        cookieService.setCookie('token', token, new Date(2099, 1, 1));

        cookieService.setUser(loggedInUser);

        fireOnLogin();

        $location.path('/movies');
    }

    function fireOnLogin() {
        for (var i = 0; i < onLoginCallbacks.length; i++) {
            onLoginCallbacks[i]();
        }
    }

    function logout() {
        cookieService.removeUser();
        cookieService.deleteCookie('token');
        cookieService.deleteCookie('pwd');
        loggedInUser = null;
        token = null;
        $http.defaults.headers.common['Authorization'] = null;
    }

    function saveUser(user) {
        $http.post('/api/Users', { Name: user.name, Login: user.email, Password: user.password, ImageData: user.image.dataURL })
            .success(function (response) {
                setUser(response);
            });
    }

    return {
        login: login,
        logout: logout,
        getLoggedInUser: function() {
            return loggedInUser;
        },
        onLogin: function(callback) {
            if (callback === null || callback === undefined) {
                return;
            }
            onLoginCallbacks.push(callback);
        },
        getToken: function () {
            return token;
        },
        saveUser: saveUser
    };
});