using AutoMapper;
using CRUDJsonTemplate.Models;
using Infrastructuur.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection.Metadata.Ecma335;
using System.Xml;

namespace CRUDJsonTemplate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JsonController : ControllerBase
    {
        private readonly IJsonDatabase<MyEntity> _db;

        public JsonController(IJsonDatabase<MyEntity> db)
        {
            _db = db;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            MyEntity entity = await _db.GetByIdAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
        [HttpGet]
        [ActionName("GetAllItems")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _db.GetAllAsync();
            if (items == null || !items.Any())
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MyEntity model)
        {
            MyEntity entity = new MyEntity();
            entity.Id = (await _db.GetAllAsync()).Max(x => x.Id) + 1;   
            entity.Name = model.Name;   
            bool success = await _db.CreateAsync(entity);
            if (!success)
            {
                return BadRequest();
            }
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MyEntity model)
        {
            var entityToUpdate = await _db.GetByIdAsync(x => x.Id == id);
            if (entityToUpdate is null) return NotFound();
            model.Id = id;
            bool success = await _db.UpdateAsync(x => x.Id == id, model);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            MyEntity entity = await _db.GetByIdAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            bool success = await _db.DeleteAsync(x => x.Id == id);
            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
