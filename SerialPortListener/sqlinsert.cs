using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SerialPortListener.Serial;
using System.IO;
using System.IO.Ports;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;
namespace SerialPortListener
{
    class sqlinsert
    {
        public void insert(String a)
        {
            try
            {
                string MyConnection2 = "datasource=localhost;port=3306;username=root;password=1qazXSW2";
                string Query = a;  
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);           
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataReader MyReader2;
                MyConn2.Open();
                MyReader2 = MyCommand2.ExecuteReader();   
              
                while (MyReader2.Read())
                {
                }
                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
