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
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;

namespace CuentasXPagar1.Controllers
{
    public class ContabilizarController : Controller
    {
        private readonly cuentasporpagarContext _context;

        HttpClientHandler _clientHandler = new HttpClientHandler();

        Transaccione transaccion = new Transaccione();
        List<Transaccione> transacciones = new List<Transaccione>();


        public ContabilizarController(cuentasporpagarContext context)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _context = context;
        }

        // GET: Transacciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transacciones.Where(transaccione => transaccione.AsientoId == null).ToListAsync());
        }

        [HttpGet]
        public async Task<List<Transaccione>> Contabilizar()
        {

            transacciones = new List<Transaccione>();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync($"https://contabilidad-api.azurewebsites.net/asientos_contables?cuenta_cred={transaccion.CuentaCr}&cuenta_cred={transaccion.CuentaCr}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    transacciones = JsonConvert.DeserializeObject<List<Transaccione>>(apiResponse);
                }
            }
            return transacciones;
        }

        //[HttpGet]
        //public async Task<Transaccione> GetById(int transaccionId)
        //{

        //    transaccion = new Transaccione();

        //    using (var httpClient = new HttpClient(_clientHandler))
        //    {
        //        transaccion = new Transaccione();
        //        using (var response = await httpClient.GetAsync("https://contabilidad-api.azurewebsites.net/" + transaccionId))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            transaccion = JsonConvert.DeserializeObject<Transaccione>(apiResponse);
        //        }
        //    }
        //    return transaccion;
        //}

        public async Task<IActionResult> AddUpdateTransaccion()
        {
            using (var httpClient = new HttpClient(_clientHandler))
            {
                IEnumerable<Transaccione> transaccioneList = _context.Transacciones.Where(transaccione => transaccione.AsientoId == null);

                foreach (Transaccione transaccione in transaccioneList)
                {

                    JObject transaccion = new JObject(
                        new JProperty("descripcion", transaccione.Descripcion),
                        new JProperty("idSistemaAuxiliar", transaccione.AuxiliarId),
                        new JProperty("idCuentaCredito", transaccione.CuentaCr),
                        new JProperty("idCuentDebito", transaccione.CuentasDb),
                        new JProperty("monto", transaccione.Monto)
                    );

                    StringContent content = new StringContent(transaccion.ToString(), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync($"https://contabilidad-api.azurewebsites.net/asientos_contables", content))
                    {
                        using (var apiResponse = await response.Content.ReadAsStreamAsync())
                        {
                            StreamReader reader = new StreamReader(apiResponse);
                            JObject asientoContable = JObject.Parse(reader.ReadToEnd());

                            transaccione.AsientoId = (int)asientoContable["id"];

                            try
                            {
                                _context.Update(transaccione);
                            }
                            catch (DbUpdateConcurrencyException)
                            {

                            }
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return View("Index", await _context.Transacciones.Where(transaccione => transaccione.AsientoId == null).ToListAsync());
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

       
    }
}
