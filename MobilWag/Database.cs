using System;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace MobilWag {
    public partial class Database : Form {

        SQLiteConnection SQLconnect = new SQLiteConnection();

        OpenFileDialog file = new OpenFileDialog();

        mobilwagUser form11;

        public Database(mobilwagUser form11) {

            this.form11 = form11;

            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) {
            file.Filter = "data files (*.sqlite) | *.sqlite";
            if (file.ShowDialog() == DialogResult.OK) {
                textBox1.Text = file.FileName;
            }
            File.WriteAllText(@"Config\\DBpatch.txt", textBox1.Text);
            SQLconnect.ConnectionString = "Data Source=" + textBox1.Text;
            try {
                SQLconnect.Open();
                SQLconnect.Close();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Form3_Load(object sender, EventArgs e) {
            if (File.Exists(@"Config\\DBpatch.txt") == true) {
                textBox1.Text = File.ReadAllText(@"Config\\DBpatch.txt");
            }
            else {
                File.Create(@"Config\\DBpatch.txt");
            }
            }
        private void button3_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
