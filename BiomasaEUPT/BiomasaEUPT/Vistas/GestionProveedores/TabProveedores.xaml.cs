using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
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

namespace BiomasaEUPT.Vistas.GestionProveedores
{
    /// <summary>
    /// Lógica de interacción para TabProveedores.xaml
    /// </summary>
    public partial class TabProveedores : UserControl
    {
        public TabProveedores()
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
    }
}
