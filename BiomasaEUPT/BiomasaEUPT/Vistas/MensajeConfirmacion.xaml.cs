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

namespace BiomasaEUPT.Vistas
{
    /// <summary>
    /// Lógica de interacción para MensajeConfirmacion.xaml
    /// </summary>
    public partial class MensajeConfirmacion : UserControl
    {
        public MensajeConfirmacion()
        {
            InitializeComponent();
        }

        public MensajeConfirmacion(string mensaje)
        {
            InitializeComponent();
            this.mensaje.Text = mensaje;
        }
    }
}
