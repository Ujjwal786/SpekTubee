using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphDataController : Controller
    {
        private readonly SpekTubeDbContext _dbContext;
        public GraphDataController(SpekTubeDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpGet("For User/{userID}")]
        public IActionResult getUserGraphData(int userID)
        {
            try
            {
                List<GraphData> graphData = _dbContext.GraphData
                    .FromSqlRaw("EXEC GetGraphDataUsers @userID_FK", new SqlParameter("userID_FK", userID))
                    .ToList();

                return Ok(graphData);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
