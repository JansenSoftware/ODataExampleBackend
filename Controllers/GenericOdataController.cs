using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace MyNamespace.Controllers
{
    [Route("odata/[controller]")]
    public abstract class GenericODataController<TEntity> : ODataController where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected GenericODataController(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        // GET: odata/[Entity]
        [EnableQuery]
        public virtual IActionResult Get()
        {
            return Ok(_dbSet);
        }

        // GET: odata/[Entity](key)
        [EnableQuery]
        public virtual IActionResult Get([FromODataUri] int key)
        {
            var entity = _dbSet.Find(key);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        // POST: odata/[Entity]
        [EnableQuery]
        public virtual IActionResult Post([FromBody] TEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            _dbSet.Add(entity);
            _context.SaveChanges();

            return Created(entity);
        }

        // PUT: odata/[Entity](key)
        [EnableQuery]
        public virtual IActionResult Put([FromODataUri] int key, [FromBody] TEntity updatedEntity)
        {
            if (updatedEntity == null)
            {
                return BadRequest();
            }

            var existingEntity = _dbSet.Find(key);
            if (existingEntity == null)
            {
                return NotFound();
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            _context.SaveChanges();

            return Updated(existingEntity);
        }

        // DELETE: odata/[Entity](key)
        [EnableQuery]
        public virtual IActionResult Delete([FromODataUri] int key)
        {
            var entity = _dbSet.Find(key);
            if (entity == null)
            {
                return NotFound();
            }

            _dbSet.Remove(entity);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
