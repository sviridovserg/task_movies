movies.models.movieItem = function (options) {
    if (options.hasOwnProperty('id')) {
        this.id = options.id;
    }

    if (options.hasOwnProperty('cacheId')) {
        this.cacheId = options.cacheId;
    }

    if (options.hasOwnProperty('title')) {
        this.title = options.title;
    }
    if (options.hasOwnProperty('rating')) {
        this.rating = options.rating;
    }
    if (options.hasOwnProperty('releaseYear')) {
        this.releaseYear = options.releaseYear;
    }

    if (options.hasOwnProperty('classification')) {
        this.classification = options.classification;
    }

    if (options.hasOwnProperty('genre')) {
        this.genre = options.genre;
    }

    if (options.hasOwnProperty('cast')) {
        this.cast = options.cast;
    }
}