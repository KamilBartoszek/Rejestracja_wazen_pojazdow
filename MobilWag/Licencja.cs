using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SQLite;

namespace MobilWag {
    public partial class Licencja : Form {
        Main form1;
        public Licencja(Main form1) {
            this.form1 = form1;
            InitializeComponent();
        }

        private void Licencja_Load(object sender, EventArgs e) {
            try {
                form1.dataAdapter = new SQLiteDataAdapter("select * from Licencja", form1.connect);
                form1.DataTable = new DataTable();
                form1.dataAdapter.Fill(form1.DataTable);
                textBox1.Text = Convert.ToString((form1.AES.Decrypt(form1.DataTable.Rows[0][0].ToString())));
            }
            catch {
                
            }
        }
    }
}
