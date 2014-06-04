controllers.controller('moviesController', function ($scope, $location, moviesService) {
    moviesService.getMovies().then(function (movies) {
        $scope.movies = movies;
    }, function () {
        toastr.error("Service unavailable", "Error");
    });

    $scope.addMovie = function () {
        $location.path('/movies/add');
    };
    $scope.editMovie = function (movie) {
        $location.path('/movies/update/' + movie.cacheId);
    };
    $scope.fields = [
        { id: "id", value: "Id" },
        { id: "title", value: "Title" },
        { id: "releaseYear", value: "Year" },
        { id: "rating", value: "Rating" },
        { id: "genre", value: "Genre" },
        { id: "classification", value: "Classification" },
        { id: "cast", value: "Actor" },
    ];

    $scope.search = function (field, expression) {
        if (!field || !expression) {
            alert('Search field and expression should be entered');
        }
        moviesService.searchMovies(field.id, expression).then(function (movies) {
            $scope.movies = movies;
        });
    };

    $scope.sort = function (field, asc) {
        if ($scope.sortField != field) {
            $scope.asc = undefined;
        }
        $scope.sortField = field;
        if ($scope.asc == undefined) {
            $scope.asc = false;
        } else if (!$scope.asc) {
            $scope.asc = true;
        } else {
            $scope.asc = undefined;
            $scope.sortField = undefined;
        }
    };
});