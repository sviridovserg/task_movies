'use strict';
controllers.controller('movieController', function ($scope, moviesService, $location, $routeParams) {
    if ($routeParams.hasOwnProperty('cacheId')) {
        $scope.isUpdating = true;
        moviesService.getMovie($routeParams.cacheId).then(function (movie) {
            $scope.newItem = movie;
        }, function () {
            toastr.error("Service unavailable", "Error");
        });
    } else {
        $scope.newItem = new movies.models.movieItem({});
    }
    $scope.submitMovieChanges = function (newItem) {
        var saveFailed = function () {
            toastr.error("Failed to save movie information", "Error");
        };
        if (!$scope.isUpdating) {
            moviesService.addMovie(newItem).then(function () {
                $location.path('/movies');
                ;
            }, saveFailed);
        } else {
            moviesService.updateMovie(newItem).then(function () {
                $location.path('/movies');
                ;
            }, saveFailed);
        }
    };
    $scope.cancel = function () {
        $location.path('/movies');
    };
    $scope.getYears = function () {
        var years = [];
        var curYear = new Date().getFullYear();
        for (var i = curYear; i > curYear - 50; i--) {
            years.push(i);
        }
        return years;
    };
    $scope.addActor = function (actor) {
        if (!$scope.newItem.cast) {
            $scope.newItem.cast = [];
        }
        $scope.newItem.cast.push(actor);
        $scope.actor = '';
    };
    $scope.removeActor = function (actor) {
        $scope.newItem.cast.splice($scope.newItem.cast.indexOf(actor), 1);
    }
});