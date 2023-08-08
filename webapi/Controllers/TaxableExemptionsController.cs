using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi.Models.ContextEF;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxableExemptionsController : ControllerBase
    {
        private readonly TaxablesDBContext _context;

        public TaxableExemptionsController(TaxablesDBContext context)
        {
            _context = context;
        }

        // GET: api/TaxableExemptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxableExemptions>>> GetTaxExemptions()
        {
          if (_context.TaxExemptions == null)
          {
              return NotFound();
          }
            return await _context.TaxExemptions.ToListAsync();
        }

        // GET: api/TaxableExemptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaxableExemptions>> GetTaxableExemptions(int id)
        {
          if (_context.TaxExemptions == null)
          {
              return NotFound();
          }
            var taxableExemptions = await _context.TaxExemptions.FindAsync(id);

            if (taxableExemptions == null)
            {
                return NotFound();
            }

            return taxableExemptions;
        }

        // PUT: api/TaxableExemptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaxableExemptions(int id, TaxableExemptions taxableExemptions)
        {
            if (id != taxableExemptions.Id)
            {
                return BadRequest();
            }

            _context.Entry(taxableExemptions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxableExemptionsExists(id))
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

        // POST: api/TaxableExemptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaxableExemptions>> PostTaxableExemptions(TaxableExemptions taxableExemptions)
        {
          if (_context.TaxExemptions == null)
          {
              return Problem("Entity set 'TaxablesDBContext.TaxExemptions'  is null.");
          }
            _context.TaxExemptions.Add(taxableExemptions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaxableExemptions", new { id = taxableExemptions.Id }, taxableExemptions);
        }

        // DELETE: api/TaxableExemptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxableExemptions(int id)
        {
            if (_context.TaxExemptions == null)
            {
                return NotFound();
            }
            var taxableExemptions = await _context.TaxExemptions.FindAsync(id);
            if (taxableExemptions == null)
            {
                return NotFound();
            }

            _context.TaxExemptions.Remove(taxableExemptions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaxableExemptionsExists(int id)
        {
            return (_context.TaxExemptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
