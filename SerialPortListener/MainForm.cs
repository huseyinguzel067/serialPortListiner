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
using System.Globalization;

namespace SerialPortListener
{
    public partial class MainForm : Form
    {
        SerialPortManager _spManager;
        public MainForm()
        {
            InitializeComponent();
            UserInitialization();
            CheckForIllegalCrossThreadCalls = false;
        }
        private void UserInitialization()
        {
            _spManager = new SerialPortManager();
            SerialSettings mySerialSettings = _spManager.CurrentSerialSettings;
            serialSettingsBindingSource.DataSource = mySerialSettings;
            serialSettingsBindingSource.DataSource = mySerialSettings;                   
            portNameComboBox.DataSource = mySerialSettings.PortNameCollection;         
            baudRateComboBox.DataSource = mySerialSettings.BaudRateCollection;
            dataBitsComboBox.DataSource = mySerialSettings.DataBitsCollection;
            parityComboBox.DataSource = Enum.GetValues(typeof(System.IO.Ports.Parity));
            stopBitsComboBox.DataSource = Enum.GetValues(typeof(System.IO.Ports.StopBits));

            _spManager.NewSerialDataRecieved += new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved);
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _spManager.Dispose();
        }
        void _spManager_NewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {

            //    //str = Encoding.ASCII.GetString(e.Data);
            //    ////A:İÇ SİCAKLİK
            //    ////B:İÇ NEM
            //    ////C:DİŞ SİCAKLİK
            //    ////D:DİŞ NEM
            //    ////E:TOPRAK NEM
            //    ////F:SU DEPOSU
            //    ////G:IŞIK SİDDETİ
            //    ////H:HAREKET
            //    ////K:YAĞMUR

        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if(portNameComboBox.Text!="")
            {      
            seriportum.PortName =portNameComboBox.Text;
            seriportum.BaudRate = Convert.ToInt32(baudRateComboBox.Text);
            seriportum.DataBits = Convert.ToInt32(dataBitsComboBox.Text);
           

            btnStart.ForeColor = Color.Red;
            btnStop.ForeColor = Color.Black;
            try
            {
                seriportum.Open();
                
            }
            catch (Exception)
            {
                MessageBox.Show("com port");
            }

            }
        }

  
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.ForeColor = Color.Black;
            btnStop.ForeColor = Color.Red;

            seriportum.Close();
            
           
            

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void tbData_TextChanged(object sender, EventArgs e)
        {

        }

        private void portNameLabel_Click(object sender, EventArgs e)
        {

        }
        private void dur(String x)
        {
            seriportum.Close();
            seriportum.PortName = portNameComboBox.Text;
            seriportum.BaudRate = Convert.ToInt32(baudRateComboBox.Text);
            seriportum.DataBits = Convert.ToInt32(dataBitsComboBox.Text);
            seriportum.Open();
            seriportum.Write(x);
            seriportum.Close();
            seriportum.Open();
        }

     

        private void button5_Click(object sender, EventArgs e)
        {
            button5.ForeColor = Color.Red;

            string[] tablo = { "icsicaklik", "icnem", "dissicaklik", "disnem", "topraknem", "hareket", "isiksiddeti", "yagmur", "sudeposu" };
            int sira = 0;
            for (int i = 0; i < 9; i++)
            {


                string den = tablo[sira];

                sqlinsert ekle = new sqlinsert();
                string b = "delete from arduino." + den + " where id>=0;";
                ekle.insert(b);

                sira++;
            }

        }



        public static string sqlbaglanti { get; set; }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void seriportum_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DateTime time = DateTime.Now;// Use current time.
            string format = "yyyy-MM-dd HH:mm:ss";   // Use this format.
            String tarih = time.ToString(format);
            string st = "";
            string kod = "";
            string deger = "";
            try
            {
                st = seriportum.ReadLine();
                kod = st.Substring(0, 1);
                deger = st.Substring(1, st.Length - 1);
                listBox1.Items.Insert(0, kod+ deger);
                hareketLabel.Text=listBox1.Items.Count.ToString();
                if(listBox1.Items.Count>=100)
                {

                    listBox1.Items.Clear();
                }

            }
            catch (Exception)
            {
                kod = "Z";
            }

            if (kod != "Z")
            {

                if (kod == "A")
                {
                    try
                    {
                        icSicaklik.Text = deger;
                        int extis = Convert.ToInt32(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.icsicaklik(icsicaklikde,tarih) values('" + extis + "','" + tarih + "');";
                        sql.insert(b);
                        //ast = extis;
                    }
                    catch (Exception )
                    {

                    }
                }
                if (kod == "G")
                {
                    try
                    {
                        isikSiddeti.Text = deger;
                        int extisik = Convert.ToInt16(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.isiksiddeti(isiksiddetide,tarih) values('" + extisik + "','" + tarih + "');";
                        sql.insert(b);
                    }
                    catch (Exception)
                    {

                    }   
                }


                if (kod == "B")
                {
                    try
                    {

                        icNem.Text = deger;
                        int extin = Convert.ToInt32(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.icnem(icnemde,tarih) values('" + extin + "','" + tarih + "');";
                        sql.insert(b);
                    }

                    catch (Exception)
                    {

                    }
                }
        






                if (kod == "H")
                {
                    try
                    {

                        icNem.Text = deger;
                        int exthd = Convert.ToInt32(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.hareket(hareketde,tarih) values('" + exthd + "','" + tarih + "');";
                        sql.insert(b);

                    }

                    catch (Exception)
                    {

                    }
                }




                if (kod == "D")
                {
                    try
                    {

                        disNem.Text = deger;
                        int extdn = Convert.ToInt16(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.disnem(disnemde,tarih) values('" + extdn + "','" + tarih + "');";
                        sql.insert(b);
                    }

                    catch (Exception)
                    {

                    }
                }

                if (kod == "C")
                {
                    try
                    {
                        disSicklik.Text = deger;
                        int extds = Convert.ToInt16(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.dissicaklik(dissicaklikde,tarih) values('" + extds + "','" + tarih + "');";
                        sql.insert(b);
                    }

                    catch (Exception)
                    {

                    }
                }




                if (kod == "E")
                {
                    try
                    {
                        toprakNem.Text = deger;
                        int exttn = Convert.ToInt16(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.topraknem(topraknemde,tarih) values('" + exttn + "','" + tarih + "');";
                        sql.insert(b);
                    }

                    catch (Exception)
                    {

                    }
                }







                if (kod == "F")
                {
                    try
                    {
                        depoSuSeviyesi.Text = deger;
                        int extsd = Convert.ToInt16(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.sudeposu(sudeposude,tarih) values('" + extsd + "','" + tarih + "');";
                        sql.insert(b);
                    }

                    catch (Exception)
                    {

                    }
                }




                if (kod == "K")
                {
                    try
                    {
                        yagmurSiddeti.Text = deger;
                        int extys = Convert.ToInt16(deger);
                        sqlinsert sql = new sqlinsert();
                        string b = "insert into arduino.yagmur(yagmurde,tarih) values('" + extys + "','" + tarih + "');";
                        sql.insert(b);
                    }

                    catch (Exception)
                    {

                    }
                }





            }
        }
    }
}

