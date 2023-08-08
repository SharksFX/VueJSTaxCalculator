using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using webapi.Models;
using webapi.Models.ContextEF;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxTablesController : ControllerBase
    {
        private readonly TaxablesDBContext _context;

        public TaxTablesController(TaxablesDBContext context)
        {
            _context = context;
        }

        // GET: api/TaxTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxTables>>> GetTaxTable()
        {
          if (_context.TaxTable == null)
          {
              return NotFound();
          }

            return await _context.TaxTable.ToListAsync();
        }

        // GET: api/TaxTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaxTables>> GetTaxTables(int id)
        {
          if (_context.TaxTable == null)
          {
              return NotFound();
          }
            var taxTables = await _context.TaxTable.FindAsync(id);

            if (taxTables == null)
            {
                return NotFound();
            }

            return taxTables;
        }

        // PUT: api/TaxTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaxTables(int id, TaxTables taxTables)
        {
            if (id != taxTables.Id)
            {
                return BadRequest();
            }

            _context.Entry(taxTables).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxTablesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaxTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaxTables>> PostTaxTables(TaxTables taxTables)
        {
          if (_context.TaxTable == null)
          {
              return Problem("Entity set 'TaxablesDBContext.TaxTable'  is null.");
          }
            _context.TaxTable.Add(taxTables);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaxTables", new { id = taxTables.Id }, taxTables);
        }

        // DELETE: api/TaxTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxTables(int id)
        {
            if (_context.TaxTable == null)
            {
                return NotFound();
            }
            var taxTables = await _context.TaxTable.FindAsync(id);
            if (taxTables == null)
            {
                return NotFound();
            }

            _context.TaxTable.Remove(taxTables);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaxTablesExists(int id)
        {
            return (_context.TaxTable?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
