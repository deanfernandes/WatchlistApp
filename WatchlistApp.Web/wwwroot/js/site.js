const TICK = '\u2705';  // ✅
const CROSS = '\u274C'; // ❌

function fetchMovies() {
    $.get('https://localhost:7152/api/Movies', function (movies) {
        displayMovies(movies);
    }).fail(function (error) {
        console.error('Error fetching movies:', error);
    });
}

function displayMovies(movies) {
    movies.forEach(function (movie) {
        addMovieTableRow(movie);
    });
}

function addMovieTableRow(movie) {
    var $tr = $("<tr></tr>");

    var $tdTitle = $("<td></td>").text(movie.title);
    $tr.append($tdTitle);

    var $tdGenre = $("<td></td>").text(movie.genre);
    $tr.append($tdGenre);

    var $tdWatched = $("<td></td>")
        .text(movie.watched ? TICK : CROSS)
        .css("text-align", "center");
    $tr.append($tdWatched);

    var $tdEditButton = $("<td></td>");
    var $editButton = $("<a></a>")
        .text("Edit")
        .addClass("btn btn-info")
        .attr("href", "/Movies/Edit?id=" + movie.id);
    $tdEditButton.append($editButton);
    $tr.append($tdEditButton);

    var $tdDeleteButton = $("<td></td>");
    var $deleteButton = $("<a></a>")
        .text("Delete")
        .addClass("btn btn-danger")
        .attr("href", "/Movies/Delete?id=" + movie.id);
    $tdDeleteButton.append($deleteButton);
    $tr.append($tdDeleteButton);

    $("#moviesTableBody").append($tr);
}