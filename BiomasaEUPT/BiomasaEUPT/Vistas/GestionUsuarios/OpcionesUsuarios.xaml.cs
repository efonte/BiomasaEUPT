using BiomasaEUPT.Clases;
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

namespace BiomasaEUPT.Vistas.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para OpcionesUsuarios.xaml
    /// </summary>
    public partial class OpcionesUsuarios : UserControl
    {
        private BiomasaEUPTEntidades context;
        public OpcionesUsuarios()
        {
            InitializeComponent();
            //DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
        }
        /*
        #region ConfirmarCambios
        private ICommand _confirmarCambiosComando;

        public ICommand ConfirmarCambiosComando
        {
            get
            {
                if (_confirmarCambiosComando == null)
                {
                    _confirmarCambiosComando = new RelayComando(
                        param => ConfirmarCambios(),
                        param => CanConfirmarCambios()
                    );
                }
                return _confirmarCambiosComando;
            }
        }

        private bool CanConfirmarCambios()
        {
            return context != null && context.ChangeTracker.HasChanges(); ;
        }

        private void ConfirmarCambios()
        {
            //biomasaEUPTDataSet.AcceptChanges();
            //biomasaEUPTDataSetusuariosTableAdapter.Fill(biomasaEUPTDataSet.usuarios);
            //biomasaEUPTDataSetusuariosTableAdapter.Update(biomasaEUPTDataSet.usuarios); // Automáticamente hace AcceptChanges()
            context.SaveChanges();
            //custViewSource.View.Refresh();
        }
        #endregion
        */

    }
}
