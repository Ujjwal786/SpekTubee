using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayListsController : Controller
    {
        private readonly SpekTubeDbContext _dbContext;
        public PlayListsController(SpekTubeDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        [HttpGet("playListNames/{id}")]
        public IActionResult GetPlaylistNames(int id)
        {
            var playlists = _dbContext.Playlists
                .Where(p => p.UserID_FK == id)
                .Select(p => new { p.ID, p.PlayList_Name })
                .ToList();

            if (playlists == null || playlists.Count == 0)
                return NotFound();

            return Ok(playlists);
        }




        [HttpPost("addPlayList")]
        public IActionResult addPlayList(PlayList pdetails)
        {
            var playListObj = _dbContext.Playlists.FirstOrDefault(p => p.UserID_FK == pdetails.UserID_FK && p.PlayList_Name == pdetails.PlayList_Name);
            if (playListObj != null)
                return Conflict("already exist");
            _dbContext.Playlists.Add(pdetails);
            _dbContext.SaveChanges();
            return Ok(playListObj);
        }



    }
}
