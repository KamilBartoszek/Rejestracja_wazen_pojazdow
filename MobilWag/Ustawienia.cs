using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace MobilWag {
    public partial class Ustawienia : Form {
        Main form1;

        public Ustawienia(Main form1) {
            this.form1 = form1;
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e) {
            try {
                form1.dataAdapter = new SQLiteDataAdapter("select * from DaneFirmy", form1.connect);
                form1.DataTable = new DataTable();
                form1.dataAdapter.Fill(form1.DataTable);
                textBox1.Text = Convert.ToString(form1.DataTable.Rows[0]["Nazwa"].ToString());
                textBox2.Text = Convert.ToString(form1.DataTable.Rows[0]["NIP"].ToString());
                textBox3.Text = Convert.ToString(form1.DataTable.Rows[0]["REGON"].ToString());
                textBox4.Text = Convert.ToString(form1.DataTable.Rows[0]["Ulica"].ToString());
                textBox5.Text = Convert.ToString(form1.DataTable.Rows[0]["Nr"].ToString());
                textBox6.Text = Convert.ToString(form1.DataTable.Rows[0]["Miasto"].ToString());
                textBox7.Text = Convert.ToString(form1.DataTable.Rows[0]["KodPocztowy"].ToString());

                form1.dataAdapter = new SQLiteDataAdapter("select * from Druk", form1.connect);
                form1.DataTable = new DataTable();
                form1.dataAdapter.Fill(form1.DataTable);
                if (Convert.ToString(form1.DataTable.Rows[0]["REGON"].ToString()) == "tak") {
                    checkBox1.Checked = true;
                }
                if (Convert.ToString(form1.DataTable.Rows[0]["Cena"].ToString()) == "tak") { checkBox2.Checked = true; }
                if (Convert.ToString(form1.DataTable.Rows[0]["NrNaczepy"].ToString()) == "tak") {
                    checkBox4.Checked = true;
                }
                if (Convert.ToString(form1.DataTable.Rows[0]["Kierowca"].ToString()) == "tak") {
                    checkBox3.Checked = true;
                }
            }
            catch {

            }
        }

        private void button1_Click(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand();
            try {
                cmd = form1.connect.CreateCommand();
                cmd.CommandText = "DELETE FROM DaneFirmy;";
                cmd.ExecuteReader();
                cmd.Dispose();
                cmd = form1.connect.CreateCommand();
                cmd.CommandText =
                    "insert into DaneFirmy(Nazwa, NIP, Ulica, Nr, KodPocztowy, Miasto, REGON)  values (@Nazwa, @NIP, @Ulica, @Nr, @KodPocztowy, @Miasto, @REGON)";
                cmd.Parameters.AddWithValue("@Nazwa", textBox1.Text);
                cmd.Parameters.AddWithValue("@NIP", textBox2.Text);
                cmd.Parameters.AddWithValue("@Ulica", textBox4.Text);
                cmd.Parameters.AddWithValue("@Nr", textBox5.Text);
                cmd.Parameters.AddWithValue("@KodPocztowy", textBox7.Text);
                cmd.Parameters.AddWithValue("@Miasto", textBox6.Text);
                cmd.Parameters.AddWithValue("@REGON", textBox3.Text);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            cmd = form1.connect.CreateCommand();
            cmd.CommandText = "DELETE FROM Druk;";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = form1.connect.CreateCommand();
            cmd.CommandText = "insert into Druk(REGON, Cena, NrNaczepy, Kierowca) values (@REGON, @Cena, @NrNaczepy, @Kierowca);";
            if (checkBox1.Checked == true) {
                cmd.Parameters.AddWithValue("@REGON", "tak");
            }
            else {
                cmd.Parameters.AddWithValue("@REGON", "nie");
            }
            if (checkBox2.Checked == true) {
                cmd.Parameters.AddWithValue("@Cena", "tak");
            }
            else {
                cmd.Parameters.AddWithValue("@Cena", "nie");
            }
            if (checkBox3.Checked == true) {
                cmd.Parameters.AddWithValue("@Kierowca", "tak");
            }
            else {
                cmd.Parameters.AddWithValue("@Kierowca", "nie");
            }
            if (checkBox4.Checked == true) {
                cmd.Parameters.AddWithValue("@NrNaczepy", "tak");
            }
            else {
                cmd.Parameters.AddWithValue("@NrNaczepy", "nie");
            }
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char) Keys.Enter) {
                e.KeyChar = (char) ' ';
                textBox1.AppendText(Environment.NewLine);
            }
        }
    }
}
