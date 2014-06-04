var moviesModule = angular.module('movies', ['ngRoute', 'movies.controllers', 'movies.services', 'movies.directives'])
    .config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/movies', { templateUrl: 'app_partials/movies.html', controller: 'moviesController' });
        $routeProvider.when('/movies/add', { templateUrl: 'app_partials/movie.html', controller: 'movieController' });
        $routeProvider.when('/movies/update/:cacheId', { templateUrl: 'app_partials/movie.html', controller: 'movieController' });

        $routeProvider.otherwise({ templateUrl: 'app_partials/movies.html', controller: 'moviesController' });
    }]).config(['$compileProvider', function($compileProvider) {
        $compileProvider.imgSrcSanitizationWhitelist(/^\s*(https?|ftp|blob):/);
    }]);