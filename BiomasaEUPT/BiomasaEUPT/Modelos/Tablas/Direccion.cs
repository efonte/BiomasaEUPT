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
    [Table("Direcciones")]
    public class Direccion
    {
        [Key]
        public int DireccionId { get; set; }

        [Required]
        [StringLength(2)]
        [DisplayName("País"), Display(Name = "País")]
        public string Pais { get; set; }

        [Required]
        [StringLength(15)]
        [DisplayName("Código Postal"), Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Comunidad"), Display(Name = "Comunidad")]
        public string Comunidad { get; set; }

        [Required]
        [StringLength(80)]
        [DisplayName("Provincia"), Display(Name = "Provincia")]
        public string Provincia { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Municipio"), Display(Name = "Municipio")]
        public string Municipio { get; set; }

        [DisplayName("Latitud"), Display(Name = "Latitud")]
        public float? Latitud { get; set; }

        [DisplayName("Longitud"), Display(Name = "Longitud")]
        public float? Longitud { get; set; }


        public virtual List<Cliente> Clientes { get; set; }

        public virtual List<Proveedor> Proveedores { get; set; }
    }
}
