using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using WatchlistApp.Web.Models;

namespace WatchlistApp.Web.Pages.Movies
{
    public class AddModel : PageModel
    {
        private readonly HttpClient _httpClient;
        [BindProperty]
        public Movie Movie { get; set; }
        public List<SelectListItem> GenreList { get; set; } = new();

        public AddModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7152/api/Genres");
            if (response.IsSuccessStatusCode)
            {
                var contentStr = await response.Content.ReadAsStringAsync();
                var genres = JsonSerializer.Deserialize<List<string>>(contentStr);

                GenreList = genres.ConvertAll(g => new SelectListItem { Value = g, Text = g });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var movieDto = new MovieDTO
            {
                Title = Movie.Title,
                Genre = Movie.Genre,
                Watched = false
            };

            var json = JsonSerializer.Serialize(movieDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://localhost:7152/api/movies/", content);

            return RedirectToPage("/Index");
        }
    }
}
