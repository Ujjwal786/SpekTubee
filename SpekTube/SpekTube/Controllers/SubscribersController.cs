using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribersController : Controller
    {
        private readonly SpekTubeDbContext _dbContext;

        public SubscribersController(SpekTubeDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public ActionResult Get_Subscribers()
        {
            if (_dbContext.Subscribers == null)
            {
                return NotFound();
            }
            return Ok(_dbContext.Subscribers.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscriber>> GetSubscribersbyId(int id)
        {
            if (_dbContext.Subscribers == null)
            {
                return NotFound();
            }
            var obj = await _dbContext.Subscribers.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }
            return obj;
        }


        [HttpPut]
        public IActionResult PutSubscribers(Subscriber sDetails)
        {
            if (_dbContext.Subscribers == null)
            {
                return Problem("null");
            }

            var vobj = _dbContext.Subscribers.FirstOrDefault(v => v.UserID_FK == sDetails.UserID_FK && v.ChannelID_FK == sDetails.ChannelID_FK);
            if (vobj != null)
            {
                _dbContext.Remove(vobj);
                _dbContext.SaveChanges();
                return Ok(new { message = "Subscriber Deleted."});
            }

            _dbContext.Subscribers.Add(sDetails);
            _dbContext.SaveChanges();
            return Ok(new { message = "Subscriber added." });
        }
    }
}
