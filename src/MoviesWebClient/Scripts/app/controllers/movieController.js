'use strict';
controllers.controller('movieController', function($scope, moviesService, $location, $routeParams) {
    if ($routeParams.hasOwnProperty('cacheId')) {
        $scope.isUpdating = true;
        moviesService.getMovie($routeParams.cacheId).then(function(movie) {
            $scope.newItem = movie;
        });
    } else {
        $scope.newItem = new movies.models.movieItem({});
    }
    $scope.submitMovieChanges = function(newItem) {
        if (!$scope.isUpdating) {
            moviesService.addMovie(newItem, function() {
                $location.path('/movies');
                ;
            });
        } else {
            moviesService.updateMovie(newItem, function() {
                $location.path('/movies');
                ;
            });
        }
    };
    $scope.cancel = function() {
        $location.path('/movies');
        ;
    };
    $scope.getYears = function() {
        var years = [];
        var curYear = new Date().getFullYear();
        for (var i = curYear; i > curYear - 50; i--) {
            years.push(i);
        }
        return years;
    };
    $scope.addActor = function(actor) {
        if (!$scope.newItem.cast) {
            $scope.newItem.cast = [];
        }
        $scope.newItem.cast.push(actor);
        $scope.actor = '';
    };
});