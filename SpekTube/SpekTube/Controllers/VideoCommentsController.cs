using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoCommentsController : Controller
    {
        private readonly SpekTubeDbContext _dbContext;

        public VideoCommentsController(SpekTubeDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public ActionResult Getvideo_Comment()
        {
            if (_dbContext.Video_Comments == null)
            {
                return NotFound();
            }
            return Ok(_dbContext.Video_Comments.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoComment>> GetvideoComment(int id)
        {
            if (_dbContext.Video_Comments == null)
            {
                return NotFound();
            }
            var obj = await _dbContext.Video_Comments.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }
            return obj;
        }

        //[HttpGet("getVideoComments/{videoID}")]
        //public IActionResult getVideoComments(int videoID)
        //{
        //    var videoComments = _dbContext.Video_Comments.Where(v => v.VideoID_FK == videoID).OrderByDescending(v => v.Created_At).ToList();

        //    if (videoComments.Count > 0)
        //    {
        //        return Ok(videoComments);
        //    }
        //    else
        //    {
        //        return NotFound(); 
        //    }
        //}



        [HttpPut]
        public IActionResult PutVideoComment(VideoComment vCommentDetails)
        {
            if (_dbContext.Video_Comments == null)
            {
                return Problem("null.");
            }

            vCommentDetails.Created_At = DateTime.Now; // Assigning the current date to the created_at property

            _dbContext.Video_Comments.Add(vCommentDetails);
            _dbContext.SaveChanges();

            return Ok(new { message = "Comment added." });
        }
        
        
        [HttpGet("getVideoComments/{videoID}")]
        public IActionResult GetVideoComments(int videoID)
        {
            try
            {
                List<VideoCommentModel> userComments = _dbContext.VideoCommentModel
                    .FromSqlRaw("EXEC GetVideoComments @videoID", new SqlParameter("@videoID", videoID))
                    .ToList();

                return Ok(userComments);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}
