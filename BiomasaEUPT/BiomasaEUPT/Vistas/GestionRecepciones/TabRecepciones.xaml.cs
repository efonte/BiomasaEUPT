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
        private CollectionViewSource recepcionesViewSource;
        private CollectionViewSource tiposMateriasPrimasViewSource;

        public TabRecepciones()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = BaseDeDatos.Instancia.biomasaEUPTContext;
                recepcionesViewSource = ((CollectionViewSource)(ucTablaRecepciones.FindResource("recepcionesViewSource")));
                tiposMateriasPrimasViewSource = ((CollectionViewSource)(ucTablaRecepciones.FindResource("tiposMateriasPrimasViewSource")));
                context.Recepciones.Load();
                context.TiposMateriasPrimas.Load();
                recepcionesViewSource.Source = context.Recepciones.Local;
                tiposMateriasPrimasViewSource.Source = context.TiposMateriasPrimas.Local;

                ucFiltroTabla.lbFiltro.SelectionChanged += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbFechaRecepcion.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbFechaRecepcion.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbMes.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbMes.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbAno.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbAno.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbNumeroAlbaran.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.cbNumeroAlbaran.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaRecepciones.bAnadirRecepcion.Click += BAnadirRecepcion_Click;
            }
        }

        private async void BAnadirRecepcion_Click(object sender, RoutedEventArgs e)
        {
            /*var formEntrada = new FormEntrada();

            if ((bool)await DialogHost.Show(formEntrada, "RootDialog"))
            {
                context.clientes.Add(new clientes()
                {
                    fecha_recepcion = formEntrada.tbFechaRecepcion.Text,
                    mes = formEntrada.tbMes.Text,
                    ano = formEntrada.tbAno.Text,
                    numero_albaran = formEntrada.tbNumeroAlbaran.Text,
                });
            }*/
        }

        public void FiltrarTabla()
        {
            recepcionesViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaRecepciones.tbBuscar.Text.ToLower();
            var entrada = e.Item as Recepcion;
            string fechaRecepcion = entrada.FechaRecepcion.ToLongDateString();
            //string mes = entrada.mes.ToString();
            //string ano = entrada.ano.ToString();
            string numeroAlbaran = entrada.NumeroAlbaran.ToLower();
            var tipos = context.MateriasPrimas.Where(m => m.RecepcionId == entrada.RecepcionId).Select(m => m.TipoId.ToString().ToLower()).Distinct().ToList();

            var condicion = (ucTablaRecepciones.cbFechaRecepcion.IsChecked == true ? fechaRecepcion.Contains(textoBuscado) : false) ||
                         //(ucTablaEntradas.cbMes.IsChecked == true ? mes.Contains(textoBuscado) : false) ||
                         //(ucTablaEntradas.cbAno.IsChecked == true ? ano.Contains(textoBuscado) : false) ||
                         (ucTablaRecepciones.cbNumeroAlbaran.IsChecked == true ? numeroAlbaran.Contains(textoBuscado) : false);

            // Filtra todos
            if (ucFiltroTabla.lbFiltro.SelectedItems.Count == 0)
            {
                e.Accepted = condicion;
            }
            else
            {
                foreach (TipoMateriaPrima tipoMateriaPrima in ucFiltroTabla.lbFiltro.SelectedItems)
                {
                    if (tipos.Where(t => t == tipoMateriaPrima.Nombre.ToLower()).Count() > 0)
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = condicion;
                        break;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
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
                        param => BorrarEntrada(),
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

        private async void BorrarEntrada()
        {
            string pregunta = ucTablaRecepciones.dgRecepciones.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar la recepción " + (ucTablaRecepciones.dgRecepciones.SelectedItem as Recepcion).NumeroAlbaran + "?"
                : "¿Está seguro de que desea borrar las recepciones seleccionadas?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.Recepciones.RemoveRange(ucTablaRecepciones.dgRecepciones.SelectedItems.Cast<Recepcion>().ToList());
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




    }
}
