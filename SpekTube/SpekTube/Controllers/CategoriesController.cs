using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SpekTubeDbContext _dbContext;

        public CategoriesController(SpekTubeDbContext context)
        {
            _dbContext = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _dbContext.Categories.OrderBy(c => c.id).ToListAsync();
            return Ok(categories);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.id == id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            var catObj = _dbContext.Categories.FirstOrDefaultAsync(c => c.category_name == category.category_name);
            if (catObj != null)
                return Conflict("already exist");

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, string name)
        {
        
            var catObj = await _dbContext.Categories.FirstOrDefaultAsync(c => c.id == id);
            if (catObj == null)
                return NotFound();

            catObj.category_name = name;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

      
    }

}
