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
    /// Lógica de interacción para FormPicking.xaml
    /// </summary>
    public partial class FormPicking : UserControl
    {
        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        private double? _volumenTotal;
        public double? VolumenTotal
        {
            get { return _volumenTotal; }
            set { _volumenTotal = value; }
        }

        private double? _volumenRestante;
        public double? VolumenRestante
        {
            get { return _volumenRestante; }
            set { _volumenRestante = value; }
        }

        private int? _unidadesTotales;
        public int? UnidadesTotales
        {
            get { return _unidadesTotales; }
            set { _unidadesTotales = value; }
        }

        private int? _unidadesRestantes;
        public int? UnidadesRestantes
        {
            get { return _unidadesRestantes; }
            set { _unidadesRestantes = value; }
        }

        public FormPicking()
        {
            InitializeComponent();
            DataContext = this;
        }

        public FormPicking(string _titulo) : this()
        {
            gbTitulo.Header = _titulo;
        }
    }
}
