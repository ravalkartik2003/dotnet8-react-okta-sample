using DotNet8OktaSample.Api.Data;
using DotNet8OktaSample.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet8OktaSample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get() =>
            await _db.Categories.AsNoTracking().ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var entity = await _db.Categories.FindAsync(id);
            if (entity == null) return NotFound();
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category model)
        {
            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Category model)
        {
            if (id != model.Id) return BadRequest();
            var exists = await _db.Categories.AnyAsync(p => p.Id == id);
            if (!exists) return NotFound();
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Categories.FindAsync(id);
            if (entity == null) return NotFound();
            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}