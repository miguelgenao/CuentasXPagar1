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
    public class EntradaDocumentosController : Controller
    {
        private readonly cuentasporpagarContext _context;

        public EntradaDocumentosController(cuentasporpagarContext context)
        {
            _context = context;
        }

        // GET: EntradaDocumentos
        public async Task<IActionResult> Index()
        {
            var cuentasporpagarContext = _context.Entradadocumentos.Include(e => e.Proveedor);
            return View(await cuentasporpagarContext.ToListAsync());
        }

        // GET: EntradaDocumentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entradadocumento = await _context.Entradadocumentos
                .Include(e => e.Proveedor)
                .FirstOrDefaultAsync(m => m.NumeroDocumento == id);
            if (entradadocumento == null)
            {
                return NotFound();
            }

            return View(entradadocumento);
        }

        // GET: EntradaDocumentos/Create
        public IActionResult Create()
        {
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Cedula");
            return View();
        }

        // POST: EntradaDocumentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumeroDocumento,NumeroFactura,FechaDocumento,Monto,FechaRegistro,ProveedorId,Estado")] Entradadocumento entradadocumento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entradadocumento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Cedula", entradadocumento.ProveedorId);
            return View(entradadocumento);
        }

        // GET: EntradaDocumentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entradadocumento = await _context.Entradadocumentos.FindAsync(id);
            if (entradadocumento == null)
            {
                return NotFound();
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Cedula", entradadocumento.ProveedorId);
            return View(entradadocumento);
        }

        // POST: EntradaDocumentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumeroDocumento,NumeroFactura,FechaDocumento,Monto,FechaRegistro,ProveedorId,Estado")] Entradadocumento entradadocumento)
        {
            if (id != entradadocumento.NumeroDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entradadocumento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradadocumentoExists(entradadocumento.NumeroDocumento))
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
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Cedula", entradadocumento.ProveedorId);
            return View(entradadocumento);
        }

        // GET: EntradaDocumentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entradadocumento = await _context.Entradadocumentos
                .Include(e => e.Proveedor)
                .FirstOrDefaultAsync(m => m.NumeroDocumento == id);
            if (entradadocumento == null)
            {
                return NotFound();
            }

            return View(entradadocumento);
        }

        // POST: EntradaDocumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entradadocumento = await _context.Entradadocumentos.FindAsync(id);
            _context.Entradadocumentos.Remove(entradadocumento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntradadocumentoExists(int id)
        {
            return _context.Entradadocumentos.Any(e => e.NumeroDocumento == id);
        }
    }
}
