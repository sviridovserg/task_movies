services.factory('moviesService', ['$http', '$q', function ($http, $q) {
	function getMovies(/*sortField, sortDirection*/) {
		var deferred = $q.defer();
		//var fieldParam = sortField == null || sortField == undefined ? "" : sortField;
		//var directionParam = sortDirection == null || sortDirection == undefined ? "" : sortDirection;
		$http.get('/Movies/AllMovies/').success(function (response) {
			deferred.resolve(parseItemsList(response));
		});
		return deferred.promise;
	}

	function searchMovies(field, expression) {
		var deferred = $q.defer();
		field = getServerField(field);
		var fieldParam = field == null || field == undefined ? "" : field;
		var expressionParam = expression == null || expression == undefined ? "" : expression;
		$http.get('/Movies/Search?field=' + fieldParam + '&expression=' + expressionParam).success(function (response) {
			deferred.resolve(parseItemsList(response));
		});
		return deferred.promise;
	}

	function addMovie(movieItem, savedCallback) {
		$http.post('/Movies/AddMovie', getServerItem(movieItem)).then(function (response) {
			if (savedCallback) {
				savedCallback();
			}
		});
	}

	function updateMovie(movieItem, savedCallback) {
	    $http.post('/Movies/UpdateMovie', getServerItem(movieItem)).then(function (response) {
			if (savedCallback) {
				savedCallback();
			}
		});
	}

	function getMovie(id, callback) {
		var deferred = $q.defer();
		var idParam = id == null || id == undefined ? "" : id;
		$http.get('/Movies/GetMovie?id=' + idParam).success(function (response) {
			deferred.resolve(parseItem(response));
		});
		return deferred.promise;
	}

	function getServerItem(item) {
		return {
			Id: item.id == undefined ? 0 : item.id,
			CacheId: item.cacheId == undefined ? "" : item.cacheId,
			Title: item.title == undefined ? "" : item.title,
			Rating: item.rating == undefined ? 0 : item.rating,
			ReleaseYear: item.releaseYear == undefined ? 0 : item.releaseYear,
			Classification: item.classification == undefined ? "" : item.classification,
			Genre: item.genre == undefined ? "" : item.genre,
			Cast: item.cast == undefined ? [] : item.cast
		}
	}

	function getServerField(field) {
		switch (field) {
			case 'id': return "Id";
			case 'title': return "Title";
			case 'rating': return "Rating";
			case 'releaseYear': return "ReleaseYear";
			case 'classification': return "Classification";
			case 'genre': return "Genre";
			case 'cast': return "Cast";
		}
		return null;
	}

	function parseItemsList(list) {
		var result = [];
		var i = 0;
		for (i = 0; i < list.length; i++) {
			result.push(parseItem(list[i]));
		}
		return result;
	}

	function parseItem(serverItem) {
		return new movies.models.movieItem({
			id: serverItem.Id,
			cacheId: serverItem.CacheId,
			title: serverItem.Title,
			rating: serverItem.Rating,
			releaseYear: serverItem.ReleaseYear,
			classification: serverItem.Classification,
			genre: serverItem.Genre,
			cast: serverItem.Cast
		})
	}
	return {
		getMovies: getMovies,
		searchMovies: searchMovies,
		addMovie: addMovie,
		getMovie: getMovie,
		updateMovie: updateMovie
	};
} ]);