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
    public class TaxRebatesController : ControllerBase
    {
        private readonly TaxablesDBContext _context;

        public TaxRebatesController(TaxablesDBContext context)
        {
            _context = context;
        }

        // GET: api/TaxRebates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxRebates>>> GetTaxRebate()
        {
          if (_context.TaxRebate == null)
          {
              return NotFound();
          }
            return await _context.TaxRebate.ToListAsync();
        }

        // GET: api/TaxRebatesByYear/2024
        [HttpGet("TaxRebatesByYear/{year}")]
        public async Task<List<TaxRebates>> TaxRebatesByYear(int year)
        {
            var taxRebates = await _context.TaxRebate.ToListAsync();

            if (taxRebates == null)
            {
                return new List<TaxRebates>();
            }

            return taxRebates.Where(y => y.TaxYear == year).ToList();
        }

        // PUT: api/TaxRebates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaxRebates(int id, TaxRebates taxRebates)
        {
            if (id != taxRebates.Id)
            {
                return BadRequest();
            }

            _context.Entry(taxRebates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxRebatesExists(id))
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

        // POST: api/TaxRebates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaxRebates>> PostTaxRebates(TaxRebates taxRebates)
        {
          if (_context.TaxRebate == null)
          {
              return Problem("Entity set 'TaxablesDBContext.TaxRebate'  is null.");
          }
            _context.TaxRebate.Add(taxRebates);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaxRebates", new { id = taxRebates.Id }, taxRebates);
        }

        // DELETE: api/TaxRebates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxRebates(int id)
        {
            if (_context.TaxRebate == null)
            {
                return NotFound();
            }
            var taxRebates = await _context.TaxRebate.FindAsync(id);
            if (taxRebates == null)
            {
                return NotFound();
            }

            _context.TaxRebate.Remove(taxRebates);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaxRebatesExists(int id)
        {
            return (_context.TaxRebate?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
