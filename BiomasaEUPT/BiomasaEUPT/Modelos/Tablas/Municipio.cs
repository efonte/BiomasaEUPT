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
        [StringLength(Constantes.LONG_MAX_NOMBRE_MUNICIPIO, MinimumLength = Constantes.LONG_MIN_NOMBRE_MUNICIPIO)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_CODIGO_POSTAL, MinimumLength = Constantes.LONG_MIN_CODIGO_POSTAL)]
        [DisplayName("Código Postal"), Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        [StringLength(Constantes.LONG_MAX_LATITUD)]
        [DisplayName("Latitud"), Display(Name = "Latitud")]
        public string Latitud { get; set; }

        [StringLength(Constantes.LONG_MAX_LONGITUD)]
        [DisplayName("Longitud"), Display(Name = "Longitud")]
        public string Longitud { get; set; }

        public int ProvinciaId { get; set; }

        [ForeignKey("ProvinciaId")]
        public virtual Provincia Provincia { get; set; }

        public virtual List<Cliente> Clientes { get; set; }

        public virtual List<Proveedor> Proveedores { get; set; }
    }
}
