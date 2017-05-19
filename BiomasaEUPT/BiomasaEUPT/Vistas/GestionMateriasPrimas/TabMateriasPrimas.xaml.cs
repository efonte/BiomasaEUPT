using BiomasaEUPT.Clases;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.GestionMateriasPrimas
{
    /// <summary>
    /// Lógica de interacción para TabMateriasPrimas.xaml
    /// </summary>
    public partial class TabMateriasPrimas : UserControl
    {
        private BiomasaEUPTEntidades context;
        private CollectionViewSource materiasPrimasViewSource;
        private CollectionViewSource tipos_materiasPrimasViewSource;
        private CollectionViewSource grupos_materiasPrimasViewSource;
        private CollectionViewSource entradasViewSource;
        private CollectionViewSource sitiosDescargasViewSource;
        private CollectionViewSource proveedoresViewSource;
        private CollectionViewSource procedenciasViewSource;


        public TabMateriasPrimas()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
                materiasPrimasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("materiasPrimasViewSource")));
                grupos_materiasPrimasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("grupos_materiasPrimasViewSource")));
                tipos_materiasPrimasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("tipos_materiasPrimasViewSource")));
                entradasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("entradasViewSource")));
                sitiosDescargasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("sitiosDescargasViewSource")));
                proveedoresViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("proveedoresViewSource")));
                procedenciasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("procedenciasViewSource")));

                context.materias_primas.Load();
                context.tipos_materias_primas.Load();
                context.grupos_materias_primas.Load();
                context.entradas.Load();
                context.sitios_descargas.Load();
                context.proveedores.Load();
                context.procedencias.Load();
                materiasPrimasViewSource.Source = context.materias_primas.Local;
                tipos_materiasPrimasViewSource.Source = context.tipos_materias_primas.Local;
                grupos_materiasPrimasViewSource.Source = context.grupos_materias_primas.Local;
                entradasViewSource.Source = context.entradas.Local;
                sitiosDescargasViewSource.Source = context.sitios_descargas.Local;
                proveedoresViewSource.Source = context.proveedores.Local;
                procedenciasViewSource.Source = context.procedencias.Local;

                /*ucFiltroTabla.lbFiltro.SelectionChanged += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbRazonSocial.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbRazonSocial.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbNif.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbNif.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbEmail.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbEmail.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCalle.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCalle.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCodigoPostal.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCodigoPostal.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbMunicipio.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbMunicipio.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.bAnadirCliente.Click += BAnadirCliente_Click;*/
            }
        }

        /*private async void BAnadirMateriaPrima_Click(object sender, RoutedEventArgs e)
        {
            var formCliente = new FormMateriaPrima();

            if ((bool)await DialogHost.Show(formCliente, "RootDialog"))
            {
                context.materias_primas.Add(new materias_primas()
                {
                    fecha_alta = FormMateriaPrima.tbMateriaPrima.Text,
                    fecha_baja = FormMateriaPrima.tbMateriaPrima.Text,
                    tipos_materias_primas = FormMateriaPrima.cbMateriasPrimas.SelectedItem as tipos_materias_primas,
                    grupos_materias_primas = FormMateriaPrima.cbMateriasPrimas.SelectedItem as grupos_materias_primas,
                    peso = FormMateriaPrima.tbPeso.Text,
                    volumen = FormMateriaPrima.tbVolumen.Text,
                    unidades = FormMateriaPrima.tbUnidades.Text,
                    entradas = FormMateriaPrima.cbEntradas.SelectedItem as entradas,
                    sitios_descargas = FormMateriaPrima.cbSitioDescarga.SelectedItem as entradas,
                    proveedores = FormMateriaPrima.cbProveedor.SelectedItem as proveedores,
                    procedencias = FormMateriaPrima.tbProcedencia.SelectedItem as procedencias,
                    codigo = FormMateriaPrima.tbCodigo.Text,
                    observaciones = FormMateriaPrima.tbObservaciones.Text
                });
            }
        }*/

        /*public void FiltrarTabla()
        {
            materiasPrimasViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }*/

        /*private void FiltroTabla(object sender, FilterEventArgs e)
        {

            string textoBuscado = ucTablaMateriasPrimas.tbBuscar.Text.ToLower();
            var cliente = e.Item as clientes;
            string razonSocial = cliente.razon_social.ToLower();
            string nif = cliente.nif.ToLower();
            string email = cliente.email.ToLower();
            string calle = cliente.calle.ToLower();
            string codigoPostal = cliente.direcciones.codigo_postal.ToLower();
            string municipio = cliente.direcciones.municipio.ToLower();
            string tipo = cliente.tipos_clientes.nombre.ToLower();

            // Filtra todos
            if (ucFiltroTabla.lbFiltro.SelectedItems.Count == 0)
            {
                e.Accepted = (ucTablaClientes.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbMunicipio.IsChecked == true ? municipio.Contains(textoBuscado) : false);
            }
            else
            {
                foreach (tipos_clientes tipoCliente in ucFiltroTabla.lbFiltro.SelectedItems)
                {
                    if (tipoCliente.nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = (ucTablaClientes.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbMunicipio.IsChecked == true ? municipio.Contains(textoBuscado) : false);
                        break;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }

        }*/

        #region ConfirmarCambios
        private ICommand _confirmarCambiosComando;

        public ICommand ConfirmarCambiosComando
        {
            get
            {
                if (_confirmarCambiosComando == null)
                {
                    _confirmarCambiosComando = new RelayComando(
                        param => ConfirmarCambios(),
                        param => CanConfirmarCambios()
                    );
                }
                return _confirmarCambiosComando;
            }
        }

        private bool CanConfirmarCambios()
        {
            return context != null && context.HayCambios<materias_primas>();
        }

        private async void ConfirmarCambios()
        {
            try
            {
                context.GuardarCambios<materias_primas>();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("\n", errorMessages);

                await DialogHost.Show(new MensajeInformacion("No pueden guardar los cambios:\n\n" + fullErrorMessage), "RootDialog");
                //Console.WriteLine(fullErrorMessage);               
            }
        }
        #endregion

        private bool asd(DbEntityEntry x)
        {
            foreach (var prop in x.OriginalValues.PropertyNames)
            {
                if (x.OriginalValues[prop].ToString() != x.CurrentValues[prop].ToString())
                {
                    return true;
                }
            }
            return false;
        }


        #region Borrar
        private ICommand _borrarComando;

        public ICommand BorrarComando
        {
            get
            {
                if (_borrarComando == null)
                {
                    _borrarComando = new RelayComando(
                        param => BorrarMateriaPrima(),
                        param => CanBorrar()
                    );
                }
                return _borrarComando;
            }
        }

        private bool CanBorrar()
        {
            if (ucTablaMateriasPrimas.dgMateriasPrimas.SelectedIndex != -1)
            {
                materias_primas materiaPrimaSeleccionada = ucTablaMateriasPrimas.dgMateriasPrimas.SelectedItem as materias_primas;
                return materiaPrimaSeleccionada != null;
            }
            return false;
        }

        private async void BorrarMateriaPrima()
        {
            string pregunta = ucTablaMateriasPrimas.dgMateriasPrimas.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar la materia prima " + (ucTablaMateriasPrimas.dgMateriasPrimas.SelectedItem as materias_primas).codigo + "?"
                : "¿Está seguro de que desea borrar la materia prima seleccionada?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.materias_primas.RemoveRange(ucTablaMateriasPrimas.dgMateriasPrimas.SelectedItems.Cast<materias_primas>().ToList());
            }
        }
        #endregion


        /*  #region AñadirMateriaPrima
          private ICommand _anadirMateriaPrimaComando;

          public ICommand AnadirMateriaPrimaComando
          {
              get
              {
                  if (_anadirMateriaPrimaComando == null)
                  {
                      _anadirMateriaPrimaComando = new RelayComando(
                          param => AnadirMateriaPrima(),
                          param => true
                      );
                  }
                  return _anadirMateriaPrimaComando;
              }
          }


          private void AnadirMateriaPrima()
          {

          }

      }
      #endregion*/




    }
}
