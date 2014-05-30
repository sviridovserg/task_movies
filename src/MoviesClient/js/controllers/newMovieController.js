'use strict'

controllers.controller('newMovieController', function ($scope, moviesService, $location) {
	$scope.newItem = new movies.models.movieItem({});
	$scope.addNewMovie = function (newItem) {
		moviesService.addMovie(newItem, function () {
			$location.path('/movies'); ;
		});
	}

	$scope.cancel = function () {
		$location.path('/movies'); ;
	}

	$scope.getYears = function () {
		var years = [];
		var curYear = new Date().getFullYear();
		for (var i = curYear; i > curYear - 50; i--) {
			years.push(i);
		}
		return years;
	}

	$scope.addActor = function (actor) {
		if (!$scope.newItem.cast) {
			$scope.newItem.cast = [];
		}
		$scope.newItem.cast.push(actor);
		$scope.actor = '';
	}
});