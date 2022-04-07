using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuentasXPagar1.Models
{
    public class TransaccionesPost
    {

        public string descripcion { get; set; }
        public int idSistemaAuxiliar { get; set; }
        public int idCuentaCredito { get; set; }
        public int idCuentDebito { get; set; }
        public double monto { get; set; }


    }
}