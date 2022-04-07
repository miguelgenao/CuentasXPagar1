using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuentasXPagar1.Models;

namespace CuentasXPagar1.Controllers
{
    public class ConceptoDePagoController : Controller
    {
        private readonly cuentasporpagarContext _context;

        public ConceptoDePagoController(cuentasporpagarContext context)
        {
            _context = context;
        }

        // GET: ConceptoDePago
        public async Task<IActionResult> Index()
        {
            return View(await _context.Conceptodepagos.ToListAsync());
        }

        // GET: ConceptoDePago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptodepago = await _context.Conceptodepagos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conceptodepago == null)
            {
                return NotFound();
            }

            return View(conceptodepago);
        }

        // GET: ConceptoDePago/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConceptoDePago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,Estado")] Conceptodepago conceptodepago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conceptodepago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conceptodepago);
        }

        // GET: ConceptoDePago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptodepago = await _context.Conceptodepagos.FindAsync(id);
            if (conceptodepago == null)
            {
                return NotFound();
            }
            return View(conceptodepago);
        }

        // POST: ConceptoDePago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Estado")] Conceptodepago conceptodepago)
        {
            if (id != conceptodepago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conceptodepago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConceptodepagoExists(conceptodepago.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conceptodepago);
        }

        // GET: ConceptoDePago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conceptodepago = await _context.Conceptodepagos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conceptodepago == null)
            {
                return NotFound();
            }

            return View(conceptodepago);
        }

        // POST: ConceptoDePago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conceptodepago = await _context.Conceptodepagos.FindAsync(id);
            _context.Conceptodepagos.Remove(conceptodepago);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConceptodepagoExists(int id)
        {
            return _context.Conceptodepagos.Any(e => e.Id == id);
        }
    }
}
