using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VideoLikesDisLikeController : Controller
    {
        private readonly SpekTubeDbContext _dbContext;

        public VideoLikesDisLikeController(SpekTubeDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public ActionResult Getvideo_like()
        {
            if (_dbContext.Video_Like == null)
            {
                return NotFound();
            }
            return Ok(_dbContext.Video_Like.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoLike>> GetvideoLike(int id)
        {
            if (_dbContext.Video_Like == null)
            {
                return NotFound();
            }
            var obj = await _dbContext.Video_Like.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }
            return obj;
        }


        [HttpPut]
        public IActionResult PostVideoLike(VideoLike likeDislikeObj)
        {
            if (_dbContext.Video_Like == null)
            {
                return Problem("Entity set 'YoutubeeContext.video_like' is null.");
            }

            var vobj = _dbContext.Video_Like.FirstOrDefault(v => v.UserID_FK == likeDislikeObj.UserID_FK && v.VideoID_FK == likeDislikeObj.VideoID_FK);
            if (vobj != null)
            {
                vobj.IsLikeDislike = likeDislikeObj.IsLikeDislike;
                _dbContext.SaveChanges();
                return Ok(new { message = "Like status updated." });
            }

            _dbContext.Video_Like.Add(likeDislikeObj);
            _dbContext.SaveChanges();
            return Ok(new { message = "Like added." });
        }




    }
}
