using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Modelos.Tablas
{
    [Table("TiposProductosTerminados")]
    public class TipoProductoTerminado
    {
        [Key]
        public int TipoProductoTerminadoId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Tamaño"), Display(Name = "Tamaño")]
        public string Tamano { get; set; }

        [DisplayName("Humedad"), Display(Name = "Humedad")]
        public double? Humedad { get; set; }

        [DisplayName("Medido en volumen"), Display(Name = "Medido en volumen")]
        public bool? MedidoEnVolumen { get; set; }

        [DisplayName("Medido en unidades"), Display(Name = "Medido en unidades")]
        public bool? MedidoEnUnidades { get; set; }

        public int GrupoId { get; set; }

        [ForeignKey("GrupoId")]
        public virtual GrupoProductoTerminado GrupoProductoTerminado { get; set; }

        public virtual List<ProductoTerminado> ProductosTerminados { get; set; }
    }
}
