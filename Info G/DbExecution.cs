using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Info_G
{
    public static class DbExecution
    {

        static SqlConnection? cnn;

        public static string read_names = "SELECT Name FROM Topic";

        static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=info_g_db;Integrated Security=True;";
        public static void ExecuteQuery(string query)
        {
            cnn = new SqlConnection(connectionString);
            try
            {
                //openning connection
                cnn.Open();
                //applying query to the table
                SqlCommand cmd = new SqlCommand(query, cnn);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                adapter.InsertCommand.ExecuteNonQuery();
                //closing connection
                cmd.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        public static IEnumerable<string> ReadRows(string query) 
        {
            cnn = new SqlConnection(connectionString);
            //openning connection
            cnn.Open();

            //applying query to the table
            SqlCommand cmd = new SqlCommand(query, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return reader.GetString(0);
            }
            //closing connection
            reader.Close();
            cmd.Dispose();
            cnn.Close();

        }

       
    }
}
