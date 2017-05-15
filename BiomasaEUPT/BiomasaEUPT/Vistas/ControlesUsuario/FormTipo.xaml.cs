using BiomasaEUPT.Clases;
using BiomasaEUPT.Vistas.GestionClientes;
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

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para FormTipo.xaml
    /// </summary>
    public partial class FormTipo : UserControl
    {
        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        private string _descripcion;
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public FormTipo()
        {
            InitializeComponent();
            DataContext = this;
        }

    }
}
