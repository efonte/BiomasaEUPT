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

namespace BiomasaEUPT.Vistas.GestionEntradas
{
    /// <summary>
    /// Lógica de interacción para TabEntradas.xaml
    /// </summary>
    public partial class TabEntradas : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource entradasViewSource;
        private CollectionViewSource tiposMateriasPrimasViewSource;

        public TabEntradas()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = BaseDeDatos.Instancia.biomasaEUPTContext;
                entradasViewSource = ((CollectionViewSource)(ucTablaEntradas.FindResource("entradasViewSource")));
                tiposMateriasPrimasViewSource = ((CollectionViewSource)(ucTablaEntradas.FindResource("tiposMateriasPrimasViewSource")));
                context.Recepciones.Load();
                context.TiposMateriasPrimas.Load();
                entradasViewSource.Source = context.Recepciones.Local;
                tiposMateriasPrimasViewSource.Source = context.TiposMateriasPrimas.Local;

                ucFiltroTabla.lbFiltro.SelectionChanged += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbFechaRecepcion.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbFechaRecepcion.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbMes.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbMes.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbAno.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbAno.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbNumeroAlbaran.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.cbNumeroAlbaran.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaEntradas.bAnadirEntrada.Click += BAnadirEntrada_Click;
            }
        }

        private async void BAnadirEntrada_Click(object sender, RoutedEventArgs e)
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
            entradasViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaEntradas.tbBuscar.Text.ToLower();
            var entrada = e.Item as Recepcion;
            string fechaRecepcion = entrada.FechaRecepcion.ToLongDateString();
            //string mes = entrada.mes.ToString();
            //string ano = entrada.ano.ToString();
            string numeroAlbaran = entrada.NumeroAlbaran.ToLower();
            var tipos = context.MateriasPrimas.Where(m => m.RecepcionId == entrada.RecepcionId).Select(m => m.TipoId.ToString().ToLower()).Distinct().ToList();

            var condicion = (ucTablaEntradas.cbFechaRecepcion.IsChecked == true ? fechaRecepcion.Contains(textoBuscado) : false) ||
                         //(ucTablaEntradas.cbMes.IsChecked == true ? mes.Contains(textoBuscado) : false) ||
                         //(ucTablaEntradas.cbAno.IsChecked == true ? ano.Contains(textoBuscado) : false) ||
                         (ucTablaEntradas.cbNumeroAlbaran.IsChecked == true ? numeroAlbaran.Contains(textoBuscado) : false);

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
            if (ucTablaEntradas.dgEntradas.SelectedIndex != -1)
            {
                Recepcion entradaSeleccionada = ucTablaEntradas.dgEntradas.SelectedItem as Recepcion;
                return entradaSeleccionada != null;
            }
            return false;
        }

        private async void BorrarEntrada()
        {
            string pregunta = ucTablaEntradas.dgEntradas.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar la entrada " + (ucTablaEntradas.dgEntradas.SelectedItem as Recepcion).NumeroAlbaran + "?"
                : "¿Está seguro de que desea borrar las entradas seleccionadas?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.Recepciones.RemoveRange(ucTablaEntradas.dgEntradas.SelectedItems.Cast<Recepcion>().ToList());
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
