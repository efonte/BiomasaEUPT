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
    /// <summary>
    /// Códigos postales, nombre, latitud y longitud de los municipios de un país
    /// </summary>
    [Table("Municipios")]
    public class Municipio
    {
        [Key]
        public int MunicipioId { get; set; }

        [Required]
        [StringLength(80)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        [DisplayName("Código Postal"), Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        [StringLength(10)]
        [DisplayName("Latitud"), Display(Name = "Latitud")]
        public string Latitud { get; set; }

        [StringLength(10)]
        [DisplayName("Longitud"), Display(Name = "Longitud")]
        public string Longitud { get; set; }

        public int ProvinciaId { get; set; }

        [ForeignKey("ProvinciaId")]
        public virtual Provincia Provincia { get; set; }

        public virtual List<Cliente> Clientes { get; set; }

        public virtual List<Proveedor> Proveedores { get; set; }
    }
}
