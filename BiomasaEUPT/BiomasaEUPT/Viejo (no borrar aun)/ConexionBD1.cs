using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiomasaEUPT.Modelo
{
    class ConexionBD1 : Form
    {
        private SqlConnection conn;
        private SqlDataAdapter daCustomers;

        private DataSet dsCustomers;
        private DataGrid dgCustomers;

        private const string tableName = "usuarios";

        // initialize form with DataGrid and Button
        public ConexionBD1()
        {
            // fill dataset
            Initdata();

            // set up datagrid
            dgCustomers = new DataGrid();
            dgCustomers.Location = new Point(5, 5);
            dgCustomers.Size = new Size(
                ClientRectangle.Size.Width - 10,
                ClientRectangle.Height - 50);
            dgCustomers.DataSource = dsCustomers;
            dgCustomers.DataMember = tableName;

            // create update button
            Button btnUpdate = new Button();
            btnUpdate.Text = "Update";
            btnUpdate.Location = new Point(
                ClientRectangle.Width / 2 - btnUpdate.Width / 2,
                ClientRectangle.Height - (btnUpdate.Height + 10));
            btnUpdate.Click += new EventHandler(btnUpdateClicked);

            // make sure controls appear on form
            Controls.AddRange(new Control[] { dgCustomers, btnUpdate });
        }

        // set up ADO.NET objects
        public void Initdata()
        {
            // instantiate the connection
            conn = new SqlConnection(
                "Data Source=155.210.68.124,49170;Initial Catalog=BiomasaEUPT;User Id=usuario;Password=usuario");

            // 1. instantiate a new DataSet
            dsCustomers = new DataSet();

            // 2. init SqlDataAdapter with select command and connection
            daCustomers = new SqlDataAdapter(
                "select id_usuario, nombre from usuarios", conn);

            // 3. fill in insert, update, and delete commands
            SqlCommandBuilder cmdBldr = new SqlCommandBuilder(daCustomers);

            // 4. fill the dataset
            daCustomers.Fill(dsCustomers, tableName);
        }

        // Update button was clicked
        public void btnUpdateClicked(object sender, EventArgs e)
        {
            // write changes back to DataBase
            daCustomers.Update(dsCustomers, tableName);
        }

        // start the Windows form
        static void Main11()
        {
            Application.Run(new ConexionBD1());
        }
    }
}
