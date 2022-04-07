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
            return View(await _context.Transacciones.ToListAsync());
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

        [HttpPost]
        public async Task<Transaccione> AddUpdateTransaccion(Transaccione transacciones)
        {

            var transaccionPost = new TransaccionesPost
            {
                descripcion = transacciones.Descripcion,
                idSistemaAuxiliar = transacciones.AuxiliarId,
                idCuentaCredito = transacciones.CuentaCr,
                idCuentDebito = transacciones.CuentasDb,
                monto = transacciones.Monto,

            };

            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(transaccionPost), Encoding.UTF8, "application/json");
                
                dynamic transaccion;

                using (var response = await httpClient.PostAsync($"https://contabilidad-api.azurewebsites.net/asientos_contables", content ))
                {
                   
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    transaccion = JsonConvert.DeserializeObject<Transaccione>(apiResponse);
                }
            }
            Console.WriteLine(transaccion);
            return transaccion;
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
