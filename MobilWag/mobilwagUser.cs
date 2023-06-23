using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace MobilWag {
    public partial class mobilwagUser : Form {
        Main form1;

        public string miernik;

        public DateTime Legalizacja;

        public mobilwagUser(Main form1) {
            this.form1 = form1;
            InitializeComponent();
        }

        private void Form11_Load(object sender, EventArgs e) {
            try {
                form1.dataAdapter = new SQLiteDataAdapter("select * from Licencja", form1.connect);
                form1.DataTable = new DataTable();
                form1.dataAdapter.Fill(form1.DataTable);
                textBox1.Text = Convert.ToString((form1.AES.Decrypt(form1.DataTable.Rows[0][0].ToString())));
            }
            catch {

            }
        }

        private void button2_Click(object sender, EventArgs e) {
            Miernik form8 = new Miernik(this);
            form8.Owner = this;
            form8.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e) {
            form1.miernik = miernik;
            //this.Close();
            Legalizacja = dateTimePicker1.Value;
            string stringLegalizacja = Legalizacja.ToString();
            
            SQLiteCommand cmd = new SQLiteCommand();
            try {
                cmd = form1.connect.CreateCommand();
                cmd.CommandText = "DELETE FROM Ustawienia;";
                cmd.ExecuteReader();
                cmd.Dispose();
                cmd = form1.connect.CreateCommand();
                cmd.CommandText = "insert into Ustawienia(Wartosc) values (@Wartosc);";
                cmd.Parameters.AddWithValue("@Wartosc", form1.AES.Encrypt(stringLegalizacja));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch {
                //ignore
            }
            cmd = new SQLiteCommand();
            try {
                cmd = form1.connect.CreateCommand();
                cmd.CommandText = "DELETE FROM Licencja;";
                cmd.ExecuteReader();
                cmd.Dispose();
                cmd = form1.connect.CreateCommand();
                cmd.CommandText = "insert into Licencja(Nazwa) values (@Nazwa);";
                cmd.Parameters.AddWithValue("@Nazwa", form1.AES.Encrypt(textBox1.Text));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch {
                //ignore
            }

        }

        private void button3_Click(object sender, EventArgs e) {
            Database form3 = new Database(this);
            form3.Owner = this;
            form3.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) {
            dateTimePicker1.Enabled = true;
        }
    }
}
