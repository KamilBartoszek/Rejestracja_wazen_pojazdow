using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MobilWag {  
    public partial class Miernik : Form {
        mobilwagUser form11;
        public Miernik( mobilwagUser form11){
            this.form11 = form11;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            File.WriteAllText(@"Config\\Miernik.txt", comboBox1.Text);
            form11.miernik = File.ReadAllText(@"Config\\Miernik.txt");
            this.Close();
        }

        private void Form8_Load(object sender, EventArgs e) {
            form11.miernik = File.ReadAllText(@"Config\\Miernik.txt");
            comboBox1.SelectedText = form11.miernik;
        }
    }
}
