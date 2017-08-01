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
                    if (Tipo == "Usuario")
                    {
                        if (Atributo == "Nombre" && context.Usuarios.Any(u => u.Nombre == valor && u.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                        if (Atributo == "Email" && context.Usuarios.Any(u => u.Email == valor && u.Email != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "email"));
                    }
                    else if (Tipo == "Cliente")
                    {
                        if (Atributo == "RazonSocial" && context.Clientes.Any(c => c.RazonSocial == valor && c.RazonSocial != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "razón social"));

                        if (Atributo == "Nif" && context.Clientes.Any(c => c.Nif == valor && c.Nif != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "NIF"));

                        if (Atributo == "Email" && context.Clientes.Any(c => c.Email == valor && c.Email != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "email"));
                    }
                    else if (Tipo == "TipoCliente")
                    {
                        if (Atributo == "Nombre" && context.TiposClientes.Any(tc => tc.Nombre == valor && tc.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                    }
                    else if (Tipo == "GrupoCliente")
                    {
                        if (Atributo == "Nombre" && context.GruposClientes.Any(gc => gc.Nombre == valor && gc.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                    }
                    else if (Tipo == "Proveedor")
                    {
                        if (Atributo == "RazonSocial" && context.Proveedores.Any(p => p.RazonSocial == valor && p.RazonSocial != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "razón social"));

                        if (Atributo == "Nif" && context.Proveedores.Any(p => p.Nif == valor && p.Nif != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "NIF"));

                        if (Atributo == "Email" && context.Proveedores.Any(p => p.Email == valor && p.Email != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "email"));
                    }
                    else if (Tipo == "TipoProveedor")
                    {
                        if (Atributo == "Nombre" && context.TiposProveedores.Any(tp => tp.Nombre == valor && tp.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));
                    }
                    else if (Tipo == "Recepcion")
                    {
                        if (Atributo == "NumeroAlbaran" && context.Recepciones.Any(r => r.NumeroAlbaran == valor && r.NumeroAlbaran != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nº albarán"));
                    }
                    else if (Tipo == "GrupoMateriaPrima")
                    {
                        if (Atributo == "Nombre" && context.GruposMateriasPrimas.Any(gmp => gmp.Nombre == valor && gmp.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));
                    }
                    else if (Tipo == "TipoMateriaPrima")
                    {
                        if (Atributo == "Nombre" && context.TiposMateriasPrimas.Any(tmp => tmp.Nombre == valor && tmp.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));
                    }
                    else if (Tipo == "Procedencia")
                    {
                        if (Atributo == "Nombre" && context.Procedencias.Any(p => p.Nombre == valor && p.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));
                    }
                    else if (Tipo == "SitioRecepcion")
                    {
                        if (Atributo == "Nombre" && context.SitiosRecepciones.Any(sr => sr.Nombre == valor && sr.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));
                    }
                    else if (Tipo == "HuecoRecepcion")
                    {
                        if (Atributo == "Nombre" && context.HuecosRecepciones.Any(hr => hr.Nombre == valor && hr.Nombre != NombreActual))
                            return new ValidationResult(false, String.Format(mensajeError, "nombre"));
                    }
                }
            }

            // Offline (no usado en la aplicación ya que sino se comprobarían datos que podrían no corresponder con los que hay en la BD)
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
                else if (Tipo == "TipoCliente")
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
