using System;
using System.Collections.Generic;

#nullable disable

namespace CuentasXPagar1.Models
{
    public partial class Proveedore
    {
        public Proveedore()
        {
            Entradadocumentos = new HashSet<Entradadocumento>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string TipoPersona { get; set; }
        public string Cedula { get; set; }
        public double Balance { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Entradadocumento> Entradadocumentos { get; set; }
    }
}
