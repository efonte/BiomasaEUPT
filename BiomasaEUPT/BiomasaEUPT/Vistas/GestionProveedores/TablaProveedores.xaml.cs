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

namespace BiomasaEUPT.Vistas.GestionProveedores
{
    /// <summary>
    /// Lógica de interacción para TablaClientes.xaml
    /// </summary>
    public partial class TablaProveedores : UserControl
    {
        private TabProveedores tabProveedores;

        public TablaProveedores()
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

            tabProveedores = (TabProveedores)ucParent;
        }

        private void tbBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            tabProveedores.FiltrarTabla();
        }

        private void pbDireccion_Opened(object sender, RoutedEventArgs e)
        {
            // Al hacer clic en la columna de Dirección se creará un FromDireccion y será asignado
            // a PopupContent. No se añade en TabProveedores.xaml para que así no cargue en memoria cada uno
            // de los PopupBox (para cada fila) hasta que se quiera editar.
            using (new CursorEspera())
            {
                PopupBox popupBox = sender as PopupBox; ;
                popupBox.PopupContent = new FormDireccion(tabProveedores.GetContext());
            }
        }

    }
}
