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

namespace BiomasaEUPT.Vistas
{
    /// <summary>
    /// Lógica de interacción para GestionUsuarios.xaml
    /// </summary>
    public partial class GestionUsuarios : UserControl
    {

        BiomasaEUPTDataSet biomasaEUPTDataSet;
        BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter biomasaEUPTDataSetusuariosTableAdapter;
        BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter biomasaEUPTDataSettipos_usuariosTableAdapter;

        public GestionUsuarios()
        {
            InitializeComponent();

            DataContext = this;
            //DataContext="{Binding RelativeSource={RelativeSource Self}}"
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            biomasaEUPTDataSet = ((BiomasaEUPTDataSet)(FindResource("biomasaEUPTDataSet")));

            // No cargue datos en tiempo de diseño.
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Cargue los datos aquí y asigne el resultado a CollectionViewSource.
                // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
                // 	myCollectionViewSource.Source = your data
                using (new CursorEspera())
                {
                    //dataGrid1.ItemsSource = source;
                    // Carga datos de la tabla usuarios.
                    biomasaEUPTDataSetusuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter();
                    biomasaEUPTDataSetusuariosTableAdapter.Fill(biomasaEUPTDataSet.usuarios);
                    CollectionViewSource usuariosViewSource = ((CollectionViewSource)(FindResource("usuariosViewSource")));
                    usuariosViewSource.View.MoveCurrentToFirst();

                    // Carga datos de la tabla tipos_usuarios.
                    biomasaEUPTDataSettipos_usuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter();
                    biomasaEUPTDataSettipos_usuariosTableAdapter.Fill(biomasaEUPTDataSet.tipos_usuarios);
                    CollectionViewSource tipos_usuariosViewSource = ((CollectionViewSource)(FindResource("tipos_usuariosViewSource")));
                    tipos_usuariosViewSource.View.MoveCurrentToFirst();
                }
               
            }

        }

        private void bAnadirUsuario_Click(object sender, RoutedEventArgs e)
        {

        }

        private void usuariosDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

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
            return biomasaEUPTDataSet!=null && biomasaEUPTDataSet.usuarios.GetChanges() != null;
        }

        private void ConfirmarCambios()
        {
            if (biomasaEUPTDataSet.usuarios.GetChanges() != null)
            {
                Console.WriteLine(biomasaEUPTDataSet.usuarios.GetChanges().Rows.Count + "+++");
            }
            Console.WriteLine(biomasaEUPTDataSet.HasChanges() + "---");

            //biomasaEUPTDataSet.AcceptChanges();
            //biomasaEUPTDataSetusuariosTableAdapter.Fill(biomasaEUPTDataSet.usuarios);
            biomasaEUPTDataSetusuariosTableAdapter.Update(biomasaEUPTDataSet.usuarios); // Automáticamente hace AcceptChanges()
        }
#endregion ConfirmarCambios


    }
}
