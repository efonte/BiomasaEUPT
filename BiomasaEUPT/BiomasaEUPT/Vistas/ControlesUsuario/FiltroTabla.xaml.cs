using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos.Validadores;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionProveedores;
using BiomasaEUPT.Vistas.GestionRecepciones;
using BiomasaEUPT.Vistas.GestionUsuarios;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System.Data.Entity;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para FiltroTabla.xaml
    /// </summary>
    public partial class FiltroTabla : UserControl
    {
        public bool MostrarGrupo { get; set; } = true;
        public bool MostrarMenuGrupo { get; set; } = true;
        public bool MostrarMenuTipo { get; set; } = true;        

        /* public static readonly DependencyProperty MostrarGrupoProperty = DependencyProperty.Register
            (
                 "MostrarGrupo",
                 typeof(bool),
                 typeof(FiltroTabla),
                 new PropertyMetadata(true)
            );

         public bool MostrarGrupo
         {
             get { return (bool)GetValue(MostrarGrupoProperty); }
             set { SetValue(MostrarGrupoProperty, value); }
         }*/


        public FiltroTabla()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!MostrarGrupo)
            {
                czGrupos.Visibility = Visibility.Collapsed;
                MostrarMenuGrupo = false;
            }

            if (!MostrarMenuTipo)
            {
                pbTipo.Visibility = Visibility.Collapsed;
            }
            /*else
            {
                bEditarTipo.Command = ModificarTipoComando;
                bBorrarTipo.Command = BorrarTipoComando;
            }*/

            if (!MostrarMenuGrupo)
            {
                pbGrupo.Visibility = Visibility.Collapsed;
            }
            /*else
            {
                bEditarGrupo.Command = ModificarGrupoComando;
                bBorrarGrupo.Command = BorrarGrupoComando;
            }*/
        }
    }
}
