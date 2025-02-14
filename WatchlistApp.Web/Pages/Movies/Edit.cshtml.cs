using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WatchlistApp.Web.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace WatchlistApp.Web.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Movie Movie { get; set; }
        public List<SelectListItem> GenreList { get; set; } = new();


        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var genreResponse = await _httpClient.GetAsync("https://localhost:7152/api/Genres");
            if (genreResponse.IsSuccessStatusCode)
            {
                var genreJson = await genreResponse.Content.ReadAsStringAsync();
                var genres = JsonSerializer.Deserialize<List<string>>(genreJson);

                GenreList = genres.ConvertAll(g => new SelectListItem { Value = g, Text = g });
            }

            var response = await _httpClient.GetAsync($"https://localhost:7152/api/Movies/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            Movie = JsonSerializer.Deserialize<Movie>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Movie.Id = id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var movieDto = new MovieDTO
            {
                Title = Movie.Title,
                Genre = Movie.Genre,
                Watched = Movie.Watched
            };

            var json = JsonSerializer.Serialize(movieDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7152/api/Movies/{Movie.Id}", content);
            if (!response.IsSuccessStatusCode) return Page();

            return RedirectToPage("/Index");
        }
    }
}
