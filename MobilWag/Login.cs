using MobilWag;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;

namespace MobilWag {
    public partial class Login : Form {
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteDataReader rdr;
        Main form1;
        public Login(Main form1) {
            this.form1 = form1;
            try {
                if (File.Exists(@"Config\\DBpatch.txt") == true) {
                    string dataPatch = File.ReadAllText(@"Config\\DBpatch.txt");
                    form1.connect.ConnectionString = "Data Source=" + dataPatch;
                    form1.connect.Open();
                }
            }
            catch (Exception ex) {

                MessageBox.Show(ex.ToString());
            }

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            form1.stan = false;
            bool login = false;
            if ((textBox1.Text == "mobilwag" && textBox2.Text == "mobilw@g1234") || (textBox1.Text == "admin" && textBox2.Text == "admin1234")) {
                form1.stan = true;
                form1.uzytkownik = textBox1.Text;
                this.Close();
            }
            else {
                cmd = form1.connect.CreateCommand();
                cmd.CommandText =
                    "SELECT IFNULL(Login,'') AS Login, IFNULL(Haslo,'') AS Haslo FROM Uzytkownicy WHERE Login='" +
                    textBox1.Text.ToString() + "'" + " AND Haslo='" + SzyfrujHaslo(textBox2.Text.ToString()) + "'";
                rdr = cmd.ExecuteReader();
                while (rdr.Read() != false) {
                    if (this.Porownaj(rdr["Login"].ToString(), textBox1.Text) &&
                        this.Porownaj(rdr["Haslo"].ToString(), SzyfrujHaslo(textBox2.Text))) {
                        login = true;
                    }
                }
                if (login == true) {
                    form1.stan = true;
                    form1.uzytkownik = textBox1.Text;
                    cmd.Dispose();
                    rdr.Dispose();
                    this.Close();
                }
                else {
                    MessageBox.Show("Błędne login lub hasło");
                }
            }
        }
        private bool Porownaj(string string1, string string2) {
            return String.Compare(string1, string2, true, System.Globalization.CultureInfo.InvariantCulture) == 0 ? true : false;
        }
        public static string SzyfrujHaslo(string haslo) {
            if (string.IsNullOrEmpty(haslo)) { return string.Empty; }
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(haslo));
            return Convert.ToBase64String(hash);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                form1.stan = false;
                bool login = false;
                if ((textBox1.Text == "mobilwag" && textBox2.Text == "mobilw@g1234") || (textBox1.Text == "admin" && textBox2.Text == "admin1234")) {
                    form1.stan = true;
                    form1.uzytkownik = textBox1.Text;
                    this.Close();
                }
                else {
                    cmd = form1.connect.CreateCommand();
                    cmd.CommandText =
                        "SELECT IFNULL(Login,'') AS Login, IFNULL(Haslo,'') AS Haslo FROM Uzytkownicy WHERE Login='" +
                        textBox1.Text.ToString() + "'" + " AND Haslo='" + SzyfrujHaslo(textBox2.Text.ToString()) + "'";
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read() != false) {
                        if (this.Porownaj(rdr["Login"].ToString(), textBox1.Text) &&
                            this.Porownaj(rdr["Haslo"].ToString(), SzyfrujHaslo(textBox2.Text))) {
                            login = true;
                        }
                    }
                    if (login == true) {
                        form1.stan = true;
                        form1.uzytkownik = textBox1.Text;
                        cmd.Dispose();
                        rdr.Dispose();
                        this.Close();
                    }
                    else {
                        MessageBox.Show("Błędne login lub hasło");
                    }
                }
            }
        }
    }
}
