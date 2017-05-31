using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Modelos.Tablas
{
    [Table("OrdenesElaboraciones")]
    public class OrdenElaboracion
    {
        [Key]
        public int OrdenElaboracionId { get; set; }

        public int ProductoTerminadoId { get; set; }

        [ForeignKey("ProductoTerminadoId")]
        public virtual ProductoTerminado ProductoTerminado { get; set; }
    }
}
