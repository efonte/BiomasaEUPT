using BiomasaEUPT.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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
    public partial class GestionUsuarios1 : UserControl
    {
        SqlDataAdapter sda;
        DataSet dsUsuarios;
        public GestionUsuarios1()
        {
            InitializeComponent();
            DataContext = new GestionUsuariosViewModel();
            RellenarDataGrid1();
        }

        private void RellenarDataGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["BiomasaEUPT.Properties.Settings.ConexionBD"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                string CmdString = "SELECT id_usuario, nombre, email, tipo_id, fecha_alta FROM usuarios";
                SqlCommand cmd = new SqlCommand(CmdString, con);

                /* DataTable dt = new DataTable("vista_usuarios");
                 sda.Fill(dt);
                 dgUsuarios.ItemsSource = dt.DefaultView;*/
                sda = new SqlDataAdapter(cmd);
                dsUsuarios = new DataSet();
                sda.Fill(dsUsuarios, "usuarios");
                dgUsuarios.ItemsSource = dsUsuarios.Tables["usuarios"].DefaultView;
                
            }
        }

        private void RellenarDataGrid1()
        {
            BiomasaEUPTDataSet biomasaEUPTDataSet = ((BiomasaEUPTDataSet)(FindResource("biomasaEUPTDataSet")));
            // Cargar datos en la tabla usuarios. Puede modificar este código según sea necesario.
            BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter biomasaEUPTDataSetusuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter();
            biomasaEUPTDataSetusuariosTableAdapter.Fill(biomasaEUPTDataSet.usuarios);
            CollectionViewSource usuariosViewSource = ((CollectionViewSource)(FindResource("usuariosViewSource")));
            usuariosViewSource.View.MoveCurrentToFirst();


            /*   INSERTAR DATOS FUNCIONAAAAAAAAA
             *   
             *   BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter tipos_usuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter();
                tipos_usuariosTableAdapter.Insert("Prueba", "Esto es una prueba");*/
        }


        private void dgUsuarios_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Console.WriteLine("asd");
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var a = e.Row.DataContext;
               // dsUsuarios.AcceptChanges();
                sda.Update(dsUsuarios, "usuarios");
               // dsUsuarios.AcceptChanges();
                /*   usuarios usuario = e.Row.DataContext as usuarios;
                   var usuarioExistente = context.usuarios.Where(u => u.id_usuario == usuario.id_usuario).FirstOrDefault();
                   if (usuarioExistente == null)
                   {
                       usuarios nuevoUsuario = new usuarios();
                       nuevoUsuario.nombre = usuario.nombre;
                       nuevoUsuario.email = usuario.email;
                       nuevoUsuario.tipo_id = usuario.tipo_id;
                       nuevoUsuario.contrasena = usuario.contrasena = "a";

                       context.usuarios.Add(nuevoUsuario);
                       context.SaveChanges();

                       Debug.WriteLine("---");
                   }
                   else
                   {
                       usuarioExistente.nombre = usuario.nombre;
                       usuarioExistente.email = usuario.email;
                       usuarioExistente.tipo_id = usuario.tipo_id;
                       context.SaveChanges();
                       Debug.WriteLine("+++");
                   }*/

            }
        }

        private void dgUsuarios_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var usuario = dgUsuarios.SelectedItem;

            Console.WriteLine(usuario + "---a---");

            if (usuario != null)
            {
                // var matchedUsuario = (from u in context.usuarios
                //                       where u.id_usuario == usuario.id_usuario
                //                       select u).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("¿Está seguro de querer borrar el usuario?",
                        "Confirmar borrado", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        //context.usuarios.Remove(matchedUsuario);
                        //context.SaveChanges();
                    }
                }
            }

        }

        private void bBorrarUsuarios_Click(object sender, RoutedEventArgs e)
        {

        }


        private void bAnadirUsuario_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
