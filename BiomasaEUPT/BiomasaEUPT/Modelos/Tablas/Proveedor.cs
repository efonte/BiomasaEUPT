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
    /// Proveedor que suministra las materias primas
    /// </summary>
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        public int ProveedorId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_RAZON_SOCIAL, MinimumLength = Constantes.LONG_MIN_RAZON_SOCIAL)]
        [Index(IsUnique = true)]
        [DisplayName("Razón social"), Display(Name = "Razón social")]
        public string RazonSocial { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NIF, MinimumLength = Constantes.LONG_MIN_NIF)]
        [Index(IsUnique = true)]
        [DisplayName("NIF"), Display(Name = "NIF")]
        public string Nif { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_EMAIL, MinimumLength = Constantes.LONG_MIN_EMAIL)]
        [Index(IsUnique = true)]
        [EmailAddress]
        [DisplayName("Email"), Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(Constantes.LONG_MAX_OBSERVACIONES, MinimumLength = Constantes.LONG_MIN_OBSERVACIONES)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_CALLE, MinimumLength = Constantes.LONG_MIN_CALLE)]
        [DisplayName("Calle"), Display(Name = "Calle")]
        public string Calle { get; set; }

        public int MunicipioId { get; set; }

        public int TipoId { get; set; }

        [ForeignKey("MunicipioId")]
        public virtual Municipio Municipio { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoProveedor TipoProveedor { get; set; }

        public virtual List<Recepcion> Recepciones { get; set; }
    }
}
