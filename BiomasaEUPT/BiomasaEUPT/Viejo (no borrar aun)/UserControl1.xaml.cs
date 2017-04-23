using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            FillDataGrid();
            //DataContext = new UserControl1ViewModel();

            //ConexionBD scd = new ConexionBD();
            // use ExecuteReader method
            //scd.ReadData();

            /*
                // use ExecuteNonQuery method for Insert
                scd.Insertdata();
                Console.WriteLine();
                Console.WriteLine("Categories After Insert");
                Console.WriteLine("------------------------------");

                scd.ReadData();

                // use ExecuteNonQuery method for Update
                scd.UpdateData();

                Console.WriteLine();
                Console.WriteLine("Categories After Update");
                Console.WriteLine("------------------------------");

                scd.ReadData();

                // use ExecuteNonQuery method for Delete
                scd.DeleteData();

                Console.WriteLine();
                Console.WriteLine("Categories After Delete");
                Console.WriteLine("------------------------------");

                scd.ReadData();

                // use ExecuteScalar method
                int numberOfRecords = scd.GetNumberOfRecords();

                Console.WriteLine();
                Console.WriteLine("Number of Records: {0}", numberOfRecords);
            */

        }


        private void FillDataGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["BiomasaEUPT.Properties.Settings.ConexionBD"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT id_usuario, nombre, email FROM usuarios";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("usuarios");
                sda.Fill(dt);
                dgUsuarios.ItemsSource = dt.DefaultView;
            }
        }
    }
}
