using System;
using System.Collections.Generic;

#nullable disable

namespace CuentasXPagar1.Models
{
    public partial class Transaccione
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int AuxiliarId { get; set; }
        public int CuentasDb { get; set; } 
        public int CuentaCr { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public double Monto { get; set; }
        public int? AsientoId { get; set; }
    }
}
