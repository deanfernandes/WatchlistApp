using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchlistApp.Api.Models;

namespace WatchlistApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetGenres()
        {
            return Ok(Enum.GetNames(typeof(Genre)));
        }
    }

    enum Genre
    {
        Action,
        Comedy,
        Horror,
        Animation
    }
}
