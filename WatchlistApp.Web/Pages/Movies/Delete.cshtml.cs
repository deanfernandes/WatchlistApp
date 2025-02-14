using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WatchlistApp.Web.Models;
using System.Text.Json;

namespace WatchlistApp.Web.Pages.Movies
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        [BindProperty]
        public Movie Movie { get; set; }
        public bool DeleteSuccess { get; set; } = false;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7152/api/Movies/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            Movie = JsonSerializer.Deserialize<Movie>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Movie.Id = id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7152/api/movies/{id}");

            if (response.IsSuccessStatusCode)
            {
                DeleteSuccess = true;

                return Page();
            }

            return Page();
        }
    }
}
