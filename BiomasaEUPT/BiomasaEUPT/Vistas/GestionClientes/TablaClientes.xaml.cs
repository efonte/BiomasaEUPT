using BiomasaEUPT.Clases;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace BiomasaEUPT.Vistas.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para TablaClientes.xaml
    /// </summary>
    public partial class TablaClientes : UserControl
    {
        private TabClientes tabClientes;

        public TablaClientes()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject ucParent = Parent;

            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            tabClientes = (TabClientes)ucParent;
        }

        private void pbDireccion_Opened(object sender, RoutedEventArgs e)
        {
            // Al hacer clic en la columna de Dirección se creará un FromDireccion y será asignado
            // a PopupContent. No se añade en TabClientes.xaml para que así no cargue en memoria cada uno
            // de los PopupBox (para cada fila) hasta que se quiera editar.
            using (new CursorEspera())
            {
                var popupBox = sender as PopupBox;
                var tabClientesViewModel = tabClientes.DataContext as TabClientesViewModel;
                var formDireccionViewModel = new FormDireccionViewModel()
                {
                    Context = tabClientesViewModel.Context,
                    PaisSeleccionado = tabClientesViewModel.ClienteSeleccionado.Municipio.Provincia.Comunidad.Pais,
                    ComunidadSeleccionada = tabClientesViewModel.ClienteSeleccionado.Municipio.Provincia.Comunidad,
                    ProvinciaSeleccionada = tabClientesViewModel.ClienteSeleccionado.Municipio.Provincia,
                    // MunicipioSeleccionado = tabClientesViewModel.ClienteSeleccionado.Municipio
                };
                popupBox.PopupContent = new FormDireccion()
                {
                    DataContext = formDireccionViewModel
                };
                formDireccionViewModel.CargarPaises();
            }
        }

        private void pbDireccion_Closed(object sender, RoutedEventArgs e)
        {
            // Al cerrar el popupBox de la dirección se guarda el municipio seleccionado
            var popupBox = sender as PopupBox;
            ((popupBox.PopupContent as FormDireccion).DataContext as FormDireccionViewModel).Context.SaveChanges();
        }
    }
}
