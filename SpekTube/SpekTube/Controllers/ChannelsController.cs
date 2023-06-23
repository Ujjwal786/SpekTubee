using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpekTube.Models;
using System.Threading.Channels;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly SpekTubeDbContext _dbContext;

        public ChannelsController(SpekTubeDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("getAllChannels")]
        public IActionResult GetAllChannels()
        {
            var obj = _dbContext.Channel.ToList();

            return Ok(obj);
        }


        [HttpGet("getChannelById/{id}")]
        public  IActionResult GetChannelById(int id)
        {
            if (_dbContext.Channel == null)
                return NotFound();
            var channelObj = _dbContext.Channel.FirstOrDefault(c => c.UserID_FK == id);

            if (channelObj == null)
                return NotFound();

            return Ok(channelObj);
        }

        [HttpPost("addChannel")]
        public IActionResult addChannelDetails()
        {
            //var channelOjb = _dbContext.Channel.FirstOrDefault(c => c.Channel_Name == cdetails.Channel_Name);
            //if (channelOjb != null)
            //    return Conflict("already exist");
            //_dbContext.Channel.Add(cdetails);
            //_dbContext.SaveChanges();
            //return CreatedAtAction(nameof(GetChannelById), new { id = cdetails.Id }, cdetails);

            var file = Request.Form.Files[0];
            string newFileName=null;

            if (file != null && file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                newFileName = GetUniqueFileName(fileExtension);
                string filePath = Path.Combine("C://Users/csp/SpektraGroupProject/YouTube/src/assets/channel_logo", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                file.CopyTo(stream);                
            }
            
                var obj = new Channels
                {
                    UserID_FK = int.Parse(Request.Form["userID"]),
                    Channel_Name = Request.Form["channelName"],
                    Channel_Description = Request.Form["channelDescription"],
                    Channel_Logo_Url = newFileName
                };


                _dbContext.Channel.Add(obj);
                _dbContext.SaveChanges();
                return CreatedAtAction(nameof(GetChannelById), new { id = obj.Id }, obj);



        }
        private string GetUniqueFileName(string fileExtension)
        {
            string newFileName = Guid.NewGuid().ToString("N");
            if (newFileName.Length > 40)
                newFileName = newFileName.Substring(0, 40);
            newFileName += fileExtension;
            return newFileName;
        }
    }
    public class ChannelDetails
    {
        public int Id { get; set; }
        public int UserId_Fk { get; set; }

        public string? Channel_Name { get; set; }
        public string? Channel_Description { get; set; }
    }

}
