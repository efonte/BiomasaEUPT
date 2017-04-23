using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Modelo
{
    class ConexionBD
    {

        SqlConnection conn;

        public ConexionBD()
        {
            // Instantiate the connection
            // https://msdn.microsoft.com/es-es/library/system.data.sqlclient.sqlconnection.connectionstring
            conn = new SqlConnection("Data Source=155.210.68.124,49170;Initial Catalog=BiomasaEUPT;User Id=usuario;Password=usuario");
        }

        // call methods that demo SqlCommand capabilities
        static void Main11()
        {
            ConexionBD scd = new ConexionBD();

            Console.WriteLine();
            Console.WriteLine("Categories Before Insert");
            Console.WriteLine("------------------------");

            // use ExecuteReader method
            scd.ReadData();

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
        }

        /// <summary>
        /// use ExecuteReader method
        /// </summary>
        public void ReadData()
        {
            SqlDataReader rdr = null;

            try
            {
                // Open the connection
                conn.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand("select * from usuarios", conn);

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while (rdr.Read())
                {
                    Console.WriteLine(rdr["nombre"]);
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// use ExecuteNonQuery method for Insert
        /// </summary>
        public void Insertdata()
        {
            try
            {
                // Open the connection
                conn.Open();

                // prepare command string
                string insertString = @"
                 insert into Categories
                 (CategoryName, Description)
                 values ('Miscellaneous', 'Whatever doesn''t fit elsewhere')";

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand(insertString, conn);

                // 2. Call ExecuteNonQuery to send command
                cmd.ExecuteNonQuery();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// use ExecuteNonQuery method for Update
        /// </summary>
        public void UpdateData()
        {
            try
            {
                // Open the connection
                conn.Open();

                // prepare command string
                string updateString = @"
                update Categories
                set CategoryName = 'Other'
                where CategoryName = 'Miscellaneous'";

                // 1. Instantiate a new command with command text only
                SqlCommand cmd = new SqlCommand(updateString);

                // 2. Set the Connection property
                cmd.Connection = conn;

                // 3. Call ExecuteNonQuery to send command
                cmd.ExecuteNonQuery();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// use ExecuteNonQuery method for Delete
        /// </summary>
        public void DeleteData()
        {
            try
            {
                // Open the connection
                conn.Open();

                // prepare command string
                string deleteString = @"
                 delete from Categories
                 where CategoryName = 'Other'";

                // 1. Instantiate a new command
                SqlCommand cmd = new SqlCommand();

                // 2. Set the CommandText property
                cmd.CommandText = deleteString;

                // 3. Set the Connection property
                cmd.Connection = conn;

                // 4. Call ExecuteNonQuery to send command
                cmd.ExecuteNonQuery();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// use ExecuteScalar method
        /// </summary>
        /// <returns>number of records</returns>
        public int GetNumberOfRecords()
        {
            int count = -1;

            try
            {
                // Open the connection
                conn.Open();

                // 1. Instantiate a new command
                SqlCommand cmd = new SqlCommand("select count(*) from Categories", conn);

                // 2. Call ExecuteScalar to send command
                count = (int)cmd.ExecuteScalar();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return count;
        }
    }
}
