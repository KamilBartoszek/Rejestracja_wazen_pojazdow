using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.IO;

namespace MobilWag {
    public partial class Users : Form {

        public string dataPatch;

        private Main form1;

        public Users(Main form1) {
            this.form1 = form1;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e) {
            string haslo = textBox1.Text;
            string hashHaslo = SzyfrujHaslo(haslo);
            textBox2.Text = hashHaslo;
        }

        public static string SzyfrujHaslo(string haslo) {
            if (string.IsNullOrEmpty(haslo)) { return string.Empty; }
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(haslo));
            return Convert.ToBase64String(hash);
        }

        private void button2_Click(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand();
            try {
                cmd = form1.connect.CreateCommand();
                cmd.CommandText =
                    "insert into Uzytkownicy(Login, Haslo)  values (@Login, @Haslo)";
                if (textBox1.Text == "admin" || textBox1.Text == "" || textBox1.Text == "mobilwag") {
                    MessageBox.Show("1)Pole Login jest puste" + '\n' + "2)użytkownik już istnieje");
                }
                else {
                    cmd.Parameters.AddWithValue("@Login", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Haslo", SzyfrujHaslo(textBox2.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("Dodano użytkownika: " + textBox1.Text);
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            }
            catch (Exception) {
                MessageBox.Show("1)Pole Login jest puste" + '\n' + "2)użytkownik już istnieje");
            }
            form1.dataAdapter = new SQLiteDataAdapter(@"Select * from Uzytkownicy", form1.connect);
            form1.dataSet = new DataSet();
            form1.dataAdapter.Fill(form1.dataSet);
            dataGridView1.DataSource = form1.dataSet.Tables[0];
        }

        private void button1_Click_1(object sender, EventArgs e) {
            foreach (DataGridViewCell oneCell in dataGridView1.SelectedCells) {
                if (oneCell.Selected)
                    try {
                        dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
                        form1.commandBuilder = new SQLiteCommandBuilder(form1.dataAdapter);
                        form1.dataAdapter.Update(form1.dataSet);
                        form1.dataAdapter = new SQLiteDataAdapter("select * from Uzytkownicy", form1.connect);
                        form1.dataSet = new DataSet();
                        form1.dataAdapter.Fill(form1.dataSet);
                        dataGridView1.DataSource = form1.dataSet.Tables[0];
                    }
                    catch {
                        // ignored
                    }
                form1.dataAdapter = new SQLiteDataAdapter(@"Select * from Uzytkownicy", form1.connect);
                form1.dataSet = new DataSet();
                form1.dataAdapter.Fill(form1.dataSet);
                dataGridView1.DataSource = form1.dataSet.Tables[0];
            }
        }

        private void Form10_Load(object sender, EventArgs e) {
            form1.dataAdapter = new SQLiteDataAdapter(@"Select * from Uzytkownicy", form1.connect);
            form1.dataSet = new DataSet();
            form1.dataAdapter.Fill(form1.dataSet);
            dataGridView1.DataSource = form1.dataSet.Tables[0];
        }
    }
}

