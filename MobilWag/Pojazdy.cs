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
    public partial class Pojazdy : Form {

        Main form1;

        public Pojazdy(Main form1) {

            this.form1 = form1;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand();
            try {
                cmd = form1.connect.CreateCommand();
                cmd.CommandText = "insert into Pojazdy(NrPojazdu, NrNaczepy, Tara)  values (@NrPojazdu, @NrNaczepy, @Tara)";
                cmd.Parameters.AddWithValue("@NrPojazdu", textBox1.Text);
                cmd.Parameters.AddWithValue("@NrNaczepy", textBox2.Text);
                cmd.Parameters.AddWithValue("@Tara", textBox3.Text);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch {
                MessageBox.Show("Problem z bazą");
            }
            form1.dataShow();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e) {
            if ((e.KeyChar <= 57 && e.KeyChar >= 48) || e.KeyChar == 13 || e.KeyChar == 8) {
            }
            else {
                e.Handled = true;
            }
        }
    }
}
