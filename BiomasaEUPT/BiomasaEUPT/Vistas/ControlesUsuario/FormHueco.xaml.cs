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
    /// Lógica de interacción para FormHueco.xaml
    /// </summary>
    public partial class FormHueco : UserControl
    {
        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        private int _unidades;
        public int Unidades
        {
            get { return _unidades; }
            set { _unidades = value; }
        }

        private double _volumen;
        public double Volumen
        {
            get { return _volumen; }
            set { _volumen = value; }
        }

        public FormHueco()
        {
            InitializeComponent();
            DataContext = this;
        }


        public FormHueco(string _titulo) : this()
        {
            gbTitulo.Header = _titulo;
        }

    }
}
