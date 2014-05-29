var moviesModule = angular.module('movies', ['ngRoute', 'movies.controllers', 'movies.services'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/default', { templateUrl: 'partials/default.html', controller: 'defaultPageController' });
    $routeProvider.when('/movies', { templateUrl: 'partials/movies.html', controller: 'moviesController' });
    $routeProvider.when('/movies/add', { templateUrl: 'partials/movie.html', controller: 'newMovieController' });
    $routeProvider.when('/movies/update', { templateUrl: 'partials/movie.html', controller: 'updateMovieController' });
    $routeProvider.when('/login', { templateUrl: 'partials/login.html', controller: 'loginController' });
    
    $routeProvider.when('/signup', { templateUrl: 'partials/signup.html', controller: 'signupController' });
    $routeProvider.otherwise({ redirectTo: '/default' });
} ]).config(['$compileProvider', function ($compileProvider) {
    $compileProvider.imgSrcSanitizationWhitelist(/^\s*(https?|ftp|blob):/);
} ]);
