using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosReportController : Controller
    {
        private readonly SpekTubeDbContext _dbContext;

        public VideosReportController(SpekTubeDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public ActionResult Getvideo_report()
        {
            if (_dbContext.VideosReport == null)
            {
                return NotFound();
            }
            return Ok(_dbContext.VideosReport.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoReport>> GetvideoReport(int id)
        {
            if (_dbContext.VideosReport == null)
            {
                return NotFound();
            }
            var obj = await _dbContext.VideosReport.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }
            return obj;
        }


        [HttpPut]
        public IActionResult PutVideoReport(VideoReport reportDetails)
        {
            if (_dbContext.VideosReport == null)
            {
                return Problem("Entity set 'YoutubeeContext.video_like' is null.");
            }

            var vobj = _dbContext.VideosReport.FirstOrDefault(v => v.UserID_FK == reportDetails.UserID_FK && v.VideoID_FK == reportDetails.VideoID_FK);
            if (vobj != null)
            {
                vobj.Problem = reportDetails.Problem;
                _dbContext.SaveChanges();
                return Ok(new { message = "Report status updated." });
            }

            _dbContext.VideosReport.Add(reportDetails);
            _dbContext.SaveChanges();
            return Ok(new { message = "Report added." });
        }

    }
}
