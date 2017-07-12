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

            ucFiltroTabla.lbFiltroTipo.SelectionChanged += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbNombre.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbNombre.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbEmail.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbEmail.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbBaneado.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbBaneado.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbTipo.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbTipo.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucOpciones.bAnadir.Command = viewModel.AnadirComando;
            ucOpciones.bRefrescar.Click += (s, e1) => { viewModel.CargarUsuarios(); };
            ucTablaUsuarios.bRefrescar.Click += (s, e1) => { viewModel.CargarUsuarios(); };

            /*   Style style = new Style(typeof(CheckBox));
               style.Setters.Add(new EventSetter(CheckBox.CheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               style.Setters.Add(new EventSetter(CheckBox.UncheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               ucTablaUsuarios.baneadoColumna.CellStyle = style;*/
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region FiltroTabla
        public void FiltrarTabla()
        {
            //usuariosViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaUsuarios.tbBuscar.Text.ToLower();
            var usuario = e.Item as Usuario;
            string nombre = usuario.Nombre.ToLower();
            string email = usuario.Email.ToLower();
            string tipo = usuario.TipoUsuario.Nombre.ToLower();

            var condicion = (ucTablaUsuarios.cbNombre.IsChecked == true ? nombre.Contains(textoBuscado) : false) ||
                           (ucTablaUsuarios.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                           (ucTablaUsuarios.cbBaneado.IsChecked == true ? usuario.Baneado == true : false);


            // Filtra todos
            if (ucFiltroTabla.lbFiltroTipo.SelectedItems.Count == 0)
            {
                e.Accepted = condicion;
            }
            else
            {
                foreach (TipoUsuario tipoUsuario in ucFiltroTabla.lbFiltroTipo.SelectedItems)
                {
                    if (tipoUsuario.Nombre.ToLower().Equals(tipo))
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
        #endregion

    }
}
