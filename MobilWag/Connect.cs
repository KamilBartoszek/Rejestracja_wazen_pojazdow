using System;
using System.Windows.Forms;

namespace MobilWag {
    public partial class Connect : Form {
        Main form1;
        public Connect(Main form1) {
            this.form1 = form1;
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(form1.ports);
        }
        private void button1_Click(object sender, EventArgs e) {
            try {
            if (comboBox1.Text != "") {
                form1.serialPort1.Close();
                form1.serialPort1.PortName = comboBox1.Text;
                form1.serialPort1.Open();
                if (form1.serialPort1.IsOpen == true) {
                    if (form1.backgroundWorker1.IsBusy != true) {
                        form1.backgroundWorker1.RunWorkerAsync();
                    }
                }
            }
            else {
                MessageBox.Show("Port nie został wybrany");
            }
            }
            catch {
                MessageBox.Show("Connection error");
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            try {
                if (form1.backgroundWorker1.WorkerSupportsCancellation == true) {
                    form1.serialPort1.Close();
                    form1.backgroundWorker1.CancelAsync();
                    form1.textBox1.Text = "";
                    this.Close();
                }
            }
            catch {
                // ignored
            }
        }
        
        private void Form2_Load(object sender, EventArgs e) {
           // comboBox1.SelectedIndex = 0;
        }
    }
}
