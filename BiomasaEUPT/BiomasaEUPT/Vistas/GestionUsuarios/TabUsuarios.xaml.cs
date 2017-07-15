using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

namespace BiomasaEUPT.Vistas.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para TabUsuarios.xaml
    /// </summary>
    public partial class TabUsuarios : UserControl
    {
        private TabUsuariosViewModel viewModel;

        public TabUsuarios()
        {
            InitializeComponent();
            viewModel = new TabUsuariosViewModel();
            DataContext = viewModel;

            ucFiltroTabla.DataContext = viewModel.FiltroTablaViewModel;

            ucOpciones.bAnadir.Command = viewModel.AnadirUsuarioComando;
            ucOpciones.bEditar.Command = viewModel.ModificarUsuarioComando;
            ucOpciones.bBorrar.Command = viewModel.BorrarUsuarioComando;
            ucOpciones.bRefrescar.Command = viewModel.RefrescarUsuariosComando;

            /*   Style style = new Style(typeof(CheckBox));
               style.Setters.Add(new EventSetter(CheckBox.CheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               style.Setters.Add(new EventSetter(CheckBox.UncheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               ucTablaUsuarios.baneadoColumna.CellStyle = style;*/
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
