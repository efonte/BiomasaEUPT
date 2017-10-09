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
    /// Tipología de los estados del proceso de envasado (Nuevo, Procesado y Finalizado)
    /// </summary>
    [Table("EstadosEnvasados")]
    public class EstadoEnvasado
    {

        [Key]
        public int EstadoEnvasadoId { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_NOMBRE_ESTADO_ENVASADO, MinimumLength = Constantes.LONG_MIN_NOMBRE_ESTADO_ENVASADO)]
        [Index(IsUnique = true)]
        [DisplayName("Nombre"), Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(Constantes.LONG_MAX_DESCRIPCION_ESTADO_ENVASADO, MinimumLength = Constantes.LONG_MIN_DESCRIPCION_ESTADO_ENVASADO)]
        [DisplayName("Descripción"), Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public virtual List<OrdenEnvasado> OrdenesEnvasados { get; set; }

    }
}
