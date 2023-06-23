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
    public partial class Kierowca : Form {

        Main form1;

        public Kierowca(Main form1) {

            this.form1 = form1;

            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand();

                try {
                    cmd = form1.connect.CreateCommand();
                    cmd.CommandText = "insert into Kierowcy(Imie, Nazwisko)  values (@imie, @nazwisko)";
                    cmd.Parameters.AddWithValue("@imie", textBox1.Text);
                    cmd.Parameters.AddWithValue("@nazwisko", textBox2.Text);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch {
                    MessageBox.Show("Problem z bazą");
                }
                form1.dataShow();
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
