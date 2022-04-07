using System;
using System.Collections.Generic;

#nullable disable

namespace CuentasXPagar1.Models
{
    public partial class Entradadocumento
    {
        public int NumeroDocumento { get; set; }
        public int NumeroFactura { get; set; }
        public DateTime FechaDocumento { get; set; }
        public double Monto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int ProveedorId { get; set; }
        public string Estado { get; set; }

        public virtual Proveedore Proveedor { get; set; }
    }
}
