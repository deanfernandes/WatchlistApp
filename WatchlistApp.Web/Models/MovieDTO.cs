namespace WatchlistApp.Web.Models
{
    public class MovieDTO
    {
        public string Title { get; set; } = string.Empty;
        public bool Watched { get; set; }
        public string Genre { get; set; } = string.Empty;
    }
}
