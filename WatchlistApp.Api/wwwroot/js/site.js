const uri = 'api/movies';

const TICK_EMOJI = '\u2705';
const CROSS_EMOJI = '\u274C';

document.getElementById('genreSelect').addEventListener('change', fetchMovies);
document.getElementById('unwatchedOnlyInput').addEventListener('change', fetchMovies);

let movies = [];

function fetchMovies() {
    let url = uri;

    const genreFilterValue = document.getElementById("genreSelect").value;
    if (genreFilterValue !== "All") {
        url += `?genre=${encodeURIComponent(genreFilterValue)}`;
    }

    if (document.getElementById("unwatchedOnlyInput").checked) {
        url += (url.includes("?") ? "&" : "?") + `watched=false`;
    }

    fetch(url).then(response => response.json()).then(data => {
        movies = data;
        _displayMovies();
    });
}

function _displayMovies() {
    console.log(movies);

    const tBody = document.getElementById('moviesTableBody');
    tBody.innerHTML = '';

    let filteredMovies = searchFilterMovies(movies, document.getElementById("searchInput").value.trim());

    filteredMovies.forEach(movie => {
        let tr = tBody.insertRow();

        let textNodeTitle = document.createTextNode(movie.title);
        let td1 = tr.insertCell(0);
        td1.appendChild(textNodeTitle);

        let textNodeGenre = document.createTextNode(movie.genre);
        let td2 = tr.insertCell(1);
        td2.appendChild(textNodeGenre);

        /*
        let watchedCheckbox = document.createElement('input');
        watchedCheckbox.type = 'checkbox';
        watchedCheckbox.disabled = true;
        watchedCheckbox.checked = movie.watched;
        */
        let td3 = tr.insertCell(2);
        //movie.watched ? td2.style.backgroundColor = "green" : td2.style.backgroundColor = "red";
        //td3.appendChild(watchedCheckbox);
        td3.textContent = movie.watched ? TICK_EMOJI : CROSS_EMOJI;

        let editButton = document.createElement('button');
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `_displayEditMovie(${movie.id})`);
        editButton.classList.add('btn-info');
        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteMovie(${movie.id})`);
        deleteButton.classList.add('btn-danger');
        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });
}

function deleteMovie(id) {
    fetch(`${uri}/${id}`, {method: 'DELETE'}).then(() => fetchMovies());
}

function addMovie() {
    let titleInput = document.getElementById('titleInput');
    let genreSelectAdd = document.getElementById('genreSelectAdd');

    if (titleInput.value.trim() === '') {
        alert('Movie title is empty');
        return;
    }

    const movie = { title: titleInput.value.trim(), genre: genreSelectAdd.value };
    fetch(uri, { method: 'POST', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' }, body: JSON.stringify(movie) })
        .then(response => response.json()).then(() => { fetchMovies(); titleInput.value = ''; })
}

function _displayEditMovie(id) {
    const movie = movies.find(movie => movie.id === id);

    document.getElementById('edit-id').value = movie.id;
    document.getElementById('edit-title').value = movie.title;
    document.getElementById('edit-watched').checked = movie.watched;
    document.getElementById('edit-genre').value = movie.genre;
    document.getElementById('editForm').style.display = 'block';
}

function hideEditMovie() {
    document.getElementById('editForm').style.display = 'none';
}

function updateMovie() {
    const movieId = document.getElementById('edit-id').value;

    const movie = {
        title: document.getElementById('edit-title').value.trim(),
        watched: document.getElementById('edit-watched').checked,
        genre: document.getElementById('edit-genre').value
    };

    fetch(`${uri}/${movieId}`, { method: 'PUT', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' }, body: JSON.stringify(movie) })
        .then(() => fetchMovies());

    document.getElementById('editForm').style.display = 'none';
}

function fetchGenres() {
    fetch('/api/genres').then(response => response.json()).then(genres =>
    {
        const genreSelect = document.getElementById('genreSelect');
        const genreSelectAdd = document.getElementById('genreSelectAdd');
        const genreSelectEdit = document.getElementById('edit-genre');

        genres.forEach(genre =>
        {
            const option = document.createElement('option');
            option.value = genre;
            option.text = genre;
            genreSelect.appendChild(option);
            let optionAdd = option.cloneNode(true);
            genreSelectAdd.appendChild(optionAdd);
            let optionEdit = option.cloneNode(true);
            genreSelectEdit.appendChild(optionEdit);
        });
    }).catch(error => {console.error("Error fetching genres:", error);});
}

function searchFilterMovies(movies, searchInput) {
    const regex = new RegExp(searchInput, "i");
    const filteredMovies = movies.filter(movie => regex.test(movie.title));

    return filteredMovies;
}