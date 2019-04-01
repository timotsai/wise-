using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace wise4051irda
{
    class clsMysql
    {
        static string M_str_sqlcon = Properties.Settings.Default.ConnectionString;

        private static MySqlConnection getConnection()
        {
            MySqlConnection mycon = new MySqlConnection();
            try
            {
                mycon = new MySqlConnection(M_str_sqlcon);
                mycon.Open();
                return mycon;
            }
            catch (Exception ex)
            { return mycon; }
            finally
            { mycon = null; }
        }
        public static void checkTableExists()
        {
            MySqlConnection mycon = getConnection();
            MySqlCommand myCommand = new MySqlCommand(); ;
            string sqlCommand = "";
            try
            {
                sqlCommand = "CREATE TABLE IF NOT EXISTS rfid_log (";
                sqlCommand += "id INT(11) UNSIGNED AUTO_INCREMENT PRIMARY KEY,";
                sqlCommand += "rfid_tag VARCHAR(30) NOT NULL,";
                sqlCommand += "channel VARCHAR(2) NOT NULL,";
                sqlCommand += "rcvtime TIMESTAMP);";
                myCommand = new MySqlCommand(sqlCommand, mycon);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (mycon.State == ConnectionState.Open) mycon.Close();
                myCommand.Dispose();
                mycon.Dispose();
            }
        }
        public static void insertadamdata(string sqlCommand)
        {
            MySqlConnection mycon = getConnection();
            MySqlCommand myCommand = new MySqlCommand();
            try
            {
                myCommand = new MySqlCommand(sqlCommand, mycon);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (mycon.State == ConnectionState.Open) mycon.Close();
                myCommand.Dispose();
                mycon.Dispose();
            }

        }
    }
}
