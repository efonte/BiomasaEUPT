using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections;
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
using Z.EntityFramework.Plus;

namespace BiomasaEUPT.Vistas.GestionTrazabilidad
{
    /// <summary>
    /// Lógica de interacción para TrazabilidadCodigos.xaml
    /// </summary>
    public partial class TrazabilidadCodigos : UserControl
    {
        public ObservableCollection<Proveedor> ArbolRecepcion { get; set; }
        public ObservableCollection<ProductoTerminado> ArbolElaboracion { get; set; }
        public ObservableCollection<Proveedor> ArbolVenta { get; set; }


        public TrazabilidadCodigos()
        {
            InitializeComponent();
            ArbolRecepcion = new ObservableCollection<Proveedor>();
            ArbolElaboracion = new ObservableCollection<ProductoTerminado>();
            ArbolVenta = new ObservableCollection<Proveedor>();
            DataContext = this;
        }

    }
}
