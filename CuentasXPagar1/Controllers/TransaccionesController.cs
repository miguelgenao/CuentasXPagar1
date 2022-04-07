using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuentasXPagar1.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace CuentasXPagar1.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly cuentasporpagarContext _context;

        HttpClientHandler _clientHandler = new HttpClientHandler();
        public TransaccionesController(cuentasporpagarContext context)
        {
            _context = context;
        }

        // GET: Transacciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transacciones.ToListAsync());
        }

        // GET: Transacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaccione == null)
            {
                return NotFound();
            }

            return View(transaccione);
        }

        // GET: Transacciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,AuxiliarId,CuentasDb,CuentaCr,FechaTransaccion,Monto,AsientoId")] Transaccione transaccione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaccione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaccione);
        }

        // GET: Transacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones.FindAsync(id);
            if (transaccione == null)
            {
                return NotFound();
            }
            return View(transaccione);
        }

        // POST: Transacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,AuxiliarId,CuentasDb,CuentaCr,FechaTransaccion,Monto,AsientoId")] Transaccione transaccione)
        {
            if (id != transaccione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccioneExists(transaccione.Id))
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
            return View(transaccione);
        }

        // GET: Transacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaccione == null)
            {
                return NotFound();
            }

            return View(transaccione);
        }

        // POST: Transacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccione = await _context.Transacciones.FindAsync(id);
            _context.Transacciones.Remove(transaccione);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccioneExists(int id)
        {
            return _context.Transacciones.Any(e => e.Id == id);
        }

        [HttpPost, ActionName("Contabilidad")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contabilizar([Bind("Id,Descripcion,AuxiliarId,CuentasDb,CuentaCr,FechaTransaccion,Monto,AsientoId")] Transaccione transacciones)
        {
           

            if (ModelState.IsValid)
            {
                var transaccionPost = new TransaccionesPost
                {
                    descripcion = transacciones.Descripcion,
                    idSistemaAuxiliar = transacciones.AuxiliarId,
                    idCuentaCredito = transacciones.CuentaCr,
                    idCuentDebito = transacciones.CuentasDb,
                    monto = transacciones.Monto,

                };
                try
                {
                    

                    using (var httpClient = new HttpClient(_clientHandler))
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(transaccionPost), Encoding.UTF8, "application/json");

                        dynamic transaccion;

                        using (var response = await httpClient.PostAsync($"https://contabilidad-api.azurewebsites.net/asientos_contables", content))
                        {

                            string apiResponse = await response.Content.ReadAsStringAsync();
                            transaccion = JsonConvert.DeserializeObject<Transaccione>(apiResponse);
                            transacciones.AsientoId = transaccion["id"];
                            return View(transacciones);
                        }
                    }
                }
                catch (Exception)
                {
                   
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transacciones);
        }
    }
}
