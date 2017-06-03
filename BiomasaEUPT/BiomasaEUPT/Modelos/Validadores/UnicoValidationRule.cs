using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class UnicoValidationRule : ValidationRule
    {
        private bool _online;
        public bool Online
        {
            get { return _online; }
            set { _online = value; }
        }

        private string _tipo;
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        private string _atributo;
        public string Atributo
        {
            get { return _atributo; }
            set { _atributo = value; }
        }

        private string _nombreActual;
        public string NombreActual
        {
            get { return _nombreActual; }
            set { _nombreActual = value; }
        }

        public CollectionViewSource Coleccion { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string mensajeError = "El campo {0} debe ser único.";
            string valor = (string)value;

            if (Online)
            {
                using (var context = new BiomasaEUPTContext())
                {
                    if (Tipo == "Cliente")
                    {
                        if (Atributo == "RazonSocial" && context.Clientes.Any(c => c.RazonSocial == valor && c.RazonSocial != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "razón social"));

                        if (Atributo == "Nif" && context.Clientes.Any(c => c.Nif == valor && c.Nif != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "NIF"));

                        if (Atributo == "Email" && context.Clientes.Any(c => c.Email == valor && c.Email != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "email"));

                    }
                    else if (Tipo == "Recepcion")
                    {
                        if (Atributo == "NumeroAlbaran" && context.Recepciones.Any(r => r.NumeroAlbaran == valor && r.NumeroAlbaran != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nº albarán"));
                    }
                }
            }
            else
            {
                if (Tipo == "Usuario")
                {
                    foreach (var item in (ObservableCollection<Usuario>)Coleccion.Source)
                    {
                        if (Atributo == "Nombre" && item.Nombre == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                        if (Atributo == "Email" && item.Email == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "email"));

                    }
                }
                else if (Tipo == "Cliente")
                {
                    foreach (var item in (ObservableCollection<Cliente>)Coleccion.Source)
                    {
                        if (Atributo == "RazonSocial" && item.RazonSocial == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "razón social"));

                        if (Atributo == "Nif" && item.Nif == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "NIF"));

                        if (Atributo == "Email" && item.Email == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "email"));

                    }
                }
                else if (Tipo == "tipoCliente")
                {
                    foreach (var item in (ObservableCollection<TipoCliente>)Coleccion.Source)
                    {
                        if (Atributo == "Nombre" && item.Nombre != NombreActual && /*item.nombre.GetHashCode() != valor.GetHashCode() &&*/ item.Nombre == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                    }
                }
                else if (Tipo == "GrupoCliente")
                {
                    foreach (var item in (ObservableCollection<GrupoCliente>)Coleccion.Source)
                    {
                        if (Atributo == "Nombre" && item.Nombre != NombreActual && item.Nombre == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                    }
                }
                else if (Tipo == "TipoProveedor")
                {
                    foreach (var item in (ObservableCollection<TipoProveedor>)Coleccion.Source)
                    {
                        if (Atributo == "Nombre" && item.Nombre != NombreActual && item.Nombre == valor)
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                    }
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
