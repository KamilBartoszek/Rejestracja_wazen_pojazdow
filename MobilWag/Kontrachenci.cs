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
    public partial class Kontrachenci : Form {
        Main form1;
        public Kontrachenci(Main form1) {
            this.form1 = form1;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand();
            try {
                cmd = form1.connect.CreateCommand();
                cmd.CommandText = "insert into Kontrahenci(Nazwa, SkroconaNazwa, NIP, Ulica, Nr, KodPocztowy, Miejscowosc, REGON)  values (@Nazwa, @SkroconaNazwa, @NIP, @Ulica, @Nr, @KodPocztowy, @Miejscowosc, @REGON)";
                cmd.Parameters.AddWithValue("@Nazwa", textBox1.Text);
                cmd.Parameters.AddWithValue("@SkroconaNazwa", textBox2.Text);
                cmd.Parameters.AddWithValue("@NIP", textBox3.Text);
                cmd.Parameters.AddWithValue("@Ulica", textBox4.Text);
                cmd.Parameters.AddWithValue("@Nr", textBox5.Text);
                cmd.Parameters.AddWithValue("@KodPocztowy", textBox6.Text);
                cmd.Parameters.AddWithValue("@Miejscowosc", textBox7.Text);
                cmd.Parameters.AddWithValue("@REGON", textBox8.Text);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            form1.dataShow();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char) Keys.Enter) {
                e.KeyChar = (char)' ';
                textBox1.AppendText(Environment.NewLine);
            }
        }
    }
}
    
