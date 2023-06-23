using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosDescriptionController : Controller
    {
        public readonly SpekTubeDbContext _dbContext;

        public VideosDescriptionController(SpekTubeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult addVideoDescription(VideoDescription vdetail)
        {
            _dbContext.VideosDescription.Add(vdetail);
            _dbContext.SaveChanges();
            return Ok(new { id = vdetail.Id });
        }

    }
}
