﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Modelos.Tablas
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(40)]
        [StringLength(40, MinimumLength = 5)]
        [DisplayName("Razón social"), Display(Name = "Razón social")]
        public string RazonSocial { get; set; }

        [Required]
        [MinLength(9)]
        [MaxLength(9)]
        [StringLength(9, MinimumLength = 9)]
        [DisplayName("NIF"), Display(Name = "NIF")]
        public string Nif { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(254)]
        [Index(IsUnique = true)]
        [EmailAddress]
        [DisplayName("Email"), Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength(60)]
        [DisplayName("Observaciones"), Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(60)]
        [DisplayName("Calle"), Display(Name = "Calle")]
        public string Calle { get; set; }

        public int DireccionId { get; set; }

        public int GrupoId { get; set; }

        public int TipoId { get; set; }

        [ForeignKey("DireccionId")]
        public virtual Direccion Direccion { get; set; }

        [ForeignKey("GrupoId")]
        public virtual GrupoCliente GrupoCliente { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoCliente TipoCliente { get; set; }
    }
}