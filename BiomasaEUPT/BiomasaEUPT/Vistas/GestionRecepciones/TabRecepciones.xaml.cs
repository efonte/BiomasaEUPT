using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    /// <summary>
    /// Lógica de interacción para TabRecepciones.xaml
    /// </summary>
    public partial class TabRecepciones : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource materiasPrimasViewSource;
        private CollectionViewSource tiposMateriasPrimasViewSource;
        private CollectionViewSource gruposMateriasPrimasViewSource;
        //private CollectionViewSource sitiosRecepcionesViewSource;
        private CollectionViewSource procedenciasViewSource;
        private CollectionViewSource recepcionesViewSource;
        private CollectionViewSource proveedoresViewSource;
        private CollectionViewSource estadosRecepcionesViewSource;

        public TabRecepciones()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                materiasPrimasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("materiasPrimasViewSource")));
                tiposMateriasPrimasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("tiposMateriasPrimasViewSource")));
                gruposMateriasPrimasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("gruposMateriasPrimasViewSource")));
                procedenciasViewSource = ((CollectionViewSource)(ucTablaMateriasPrimas.FindResource("procedenciasViewSource")));
                recepcionesViewSource = ((CollectionViewSource)(ucTablaRecepciones.FindResource("recepcionesViewSource")));
                proveedoresViewSource = ((CollectionViewSource)(ucTablaRecepciones.FindResource("proveedoresPrimasViewSource")));
                estadosRecepcionesViewSource = ((CollectionViewSource)(ucTablaRecepciones.FindResource("estadosRecepcionesViewSource")));

                context.TiposMateriasPrimas.Load();
                context.GruposMateriasPrimas.Load();
                context.Procedencias.Load();
                context.Recepciones.Load();
                context.Proveedores.Load();
                context.EstadosRecepciones.Load();

                tiposMateriasPrimasViewSource.Source = context.TiposMateriasPrimas.Local;
                gruposMateriasPrimasViewSource.Source = context.GruposMateriasPrimas.Local;
                procedenciasViewSource.Source = context.Procedencias.Local;
                recepcionesViewSource.Source = context.Recepciones.Local;
                proveedoresViewSource.Source = context.Proveedores.Local;
                estadosRecepcionesViewSource.Source = context.EstadosRecepciones.Local;

                ucTablaRecepciones.dgRecepciones.SelectionChanged += DgRecepciones_SelectionChanged;
                ucTablaRecepciones.cbNumeroAlbaran.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
                ucTablaRecepciones.cbNumeroAlbaran.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };
                ucTablaRecepciones.cbFechaRecepcion.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
                ucTablaRecepciones.cbFechaRecepcion.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };
                ucTablaRecepciones.cbEstado.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
                ucTablaRecepciones.cbEstado.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };
                ucTablaRecepciones.cbProveedor.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
                ucTablaRecepciones.cbProveedor.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };

                ucTablaRecepciones.bAnadirRecepcion.Click += BAnadirRecepcion_Click;
                ucTablaMateriasPrimas.bAnadirMateriaPrima.Click += BAnadirMateriaPrima_Click;

                Style rowStyle = new Style(typeof(DataGridRow));
                rowStyle.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(RowRecepciones_DoubleClick)));
                ucTablaRecepciones.dgRecepciones.RowStyle = rowStyle;
            }
        }

        private void DgRecepciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var recepcion = (sender as DataGrid).SelectedItem as Recepcion;
            if (recepcion != null)
            {
                Console.WriteLine(recepcion.NumeroAlbaran);
                materiasPrimasViewSource.Source = context.MateriasPrimas.Where(mp => mp.RecepcionId == recepcion.RecepcionId).ToList();
            }
        }

        private async void BAnadirRecepcion_Click(object sender, RoutedEventArgs e)
        {
            var formRecepcion = new FormRecepcion();

            if ((bool)await DialogHost.Show(formRecepcion, "RootDialog"))
            {
                context.Recepciones.Add(new Recepcion()
                {
                    NumeroAlbaran = formRecepcion.NumeroAlbaran,
                    FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second),
                    ProveedorId = (formRecepcion.cbProveedores.SelectedItem as Proveedor).ProveedorId,
                    EstadoId = (formRecepcion.cbEstadosRecepciones.SelectedItem as EstadoRecepcion).EstadoRecepcionId
                });
                context.SaveChanges();
            }
        }

        private async void BAnadirMateriaPrima_Click(object sender, RoutedEventArgs e)
        {
            var formRecepcion = new FormMateriaPrima();

            if ((bool)await DialogHost.Show(formRecepcion, "RootDialog"))
            {
               /* context.MateriasPrimas.Add(new MateriaPrima()
                {
                    NumeroAlbaran = formRecepcion.NumeroAlbaran,
                    FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second),
                    ProveedorId = (formRecepcion.cbProveedores.SelectedItem as Proveedor).ProveedorId,
                    EstadoId = (formRecepcion.cbEstadosRecepciones.SelectedItem as EstadoRecepcion).EstadoRecepcionId
                });
                context.SaveChanges();*/
            }
        }

        public void FiltrarTablaRecepciones()
        {
            recepcionesViewSource.Filter += new FilterEventHandler(FiltroTablaRecepciones);
        }

        private void FiltroTablaRecepciones(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaRecepciones.tbBuscar.Text.ToLower();
            var recepcion = e.Item as Recepcion;
            string fechaRecepcion = recepcion.FechaRecepcion.ToString();
            string numeroAlbaran = recepcion.NumeroAlbaran.ToLower();
            string proveedor = recepcion.Proveedor.RazonSocial.ToLower();
            string estado = recepcion.EstadoRecepcion.Nombre.ToLower();

            e.Accepted = (ucTablaRecepciones.cbFechaRecepcion.IsChecked == true ? fechaRecepcion.Contains(textoBuscado) : false) ||
                         (ucTablaRecepciones.cbNumeroAlbaran.IsChecked == true ? numeroAlbaran.Contains(textoBuscado) : false) ||
                         (ucTablaRecepciones.cbProveedor.IsChecked == true ? proveedor.Contains(textoBuscado) : false) ||
                         (ucTablaRecepciones.cbEstado.IsChecked == true ? estado.Contains(textoBuscado) : false);

        }

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
            return context != null && context.HayCambios<Cliente>();
        }

        private async void ConfirmarCambios()
        {
            try
            {
                context.GuardarCambios<Cliente>();
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
            //clientesViewSource.View.Refresh();
        }
        #endregion       

        #region Borrar
        private ICommand _borrarComando;

        public ICommand BorrarComando
        {
            get
            {
                if (_borrarComando == null)
                {
                    _borrarComando = new RelayComando(
                        param => BorrarRecepcion(),
                        param => CanBorrar()
                    );
                }
                return _borrarComando;
            }
        }

        private bool CanBorrar()
        {
            if (ucTablaRecepciones.dgRecepciones.SelectedIndex != -1)
            {
                Recepcion recepcionSeleccionada = ucTablaRecepciones.dgRecepciones.SelectedItem as Recepcion;
                return recepcionSeleccionada != null;
            }
            return false;
        }

        private async void BorrarRecepcion()
        {
            string pregunta = ucTablaRecepciones.dgRecepciones.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar la recepción " + (ucTablaRecepciones.dgRecepciones.SelectedItem as Recepcion).NumeroAlbaran + "?"
                : "¿Está seguro de que desea borrar las recepciones seleccionadas?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                context.Recepciones.RemoveRange(ucTablaRecepciones.dgRecepciones.SelectedItems.Cast<Recepcion>().ToList());
                context.SaveChanges();
            }
        }
        #endregion


        /*  #region AñadirCliente
          private ICommand _anadirClienteComando;

          public ICommand AnadirClienteComando
          {
              get
              {
                  if (_anadirClienteComando == null)
                  {
                      _anadirClienteComando = new RelayComando(
                          param => AnadirCliente(),
                          param => true
                      );
                  }
                  return _anadirClienteComando;
              }
          }


          private void AnadirCliente()
          {

          }

      }
      #endregion*/


        private async void RowRecepciones_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fila = sender as DataGridRow;
            var recepcionSeleccionada = ucTablaRecepciones.dgRecepciones.SelectedItem as Recepcion;
            var formRecepcion = new FormRecepcion("Editar Recepción")
            {
                NumeroAlbaran = recepcionSeleccionada.NumeroAlbaran,
                Fecha = recepcionSeleccionada.FechaRecepcion,
                Hora = recepcionSeleccionada.FechaRecepcion
            };
            var albaranViejo = formRecepcion.NumeroAlbaran;
            formRecepcion.vAlbaranUnico.NombreActual = recepcionSeleccionada.NumeroAlbaran;
            if ((bool)await DialogHost.Show(formRecepcion, "RootDialog"))
            {
                recepcionSeleccionada.NumeroAlbaran = formRecepcion.NumeroAlbaran;
                recepcionSeleccionada.FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second);
                recepcionSeleccionada.ProveedorId = (formRecepcion.cbProveedores.SelectedItem as Proveedor).ProveedorId;
                recepcionSeleccionada.EstadoId = (formRecepcion.cbEstadosRecepciones.SelectedItem as EstadoRecepcion).EstadoRecepcionId;
                recepcionesViewSource.View.Refresh();
                context.SaveChanges();
                /* using (var context = new BiomasaEUPTContext())
                 {
                     var recepcion = context.Recepciones.Single(tc => tc.NumeroAlbaran == albaranViejo);
                     recepcion.NumeroAlbaran = formTipo.Nombre;
                     tipoCliente.Descripcion = formTipo.Descripcion;
                     context.SaveChanges();
                 }*/
            }
        }

    }
}
