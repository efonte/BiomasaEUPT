using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace BiomasaEUPT.Vistas.GestionVentas
{
    /// <summary>
    /// Lógica de interacción para TabVentas.xaml
    /// </summary>
    public partial class TabVentas : UserControl
    {
        public TabVentas()
        {
            InitializeComponent();
            IsVisibleChanged += new DependencyPropertyChangedEventHandler(MyControl_IsVisibleChanged);
        }

        private void MyControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Se añade el foco del teclado al UserControl para que funcionen los atajos de teclado
            if (!(bool)(e.NewValue))
                return;
            Focusable = true;
            Keyboard.Focus(this);
        }

        private void bMasOpciones_Click(object sender, RoutedEventArgs e)
        {
            transicion.SelectedIndex = 1;
            (DataContext as TabVentasViewModel).MasOpcionesVentasViewModel.Inicializar();
        }

        private void bVolver_Click(object sender, RoutedEventArgs e)
        {
            transicion.SelectedIndex = 0;
            (DataContext as TabVentasViewModel).Inicializar();
        }
    }
}
