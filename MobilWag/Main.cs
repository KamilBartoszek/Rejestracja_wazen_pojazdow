using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using System.IO.Ports;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace MobilWag {
    public partial class Main : Form {

        public string miernik;
        FileStream fs;
        Document document;
        PdfWriter writer;
        public String[] ports;
        public string dataPatch;
        public SQLiteConnection connect = new SQLiteConnection();
        public SimplerAES AES = new SimplerAES();
        public SQLiteDataAdapter dataAdapter;
        public DataSet dataSet;
        public DataTable DataTable;
        public SQLiteCommandBuilder commandBuilder;
        public DateTime dt; //= new DateTime();
        public bool stan;
        public string uzytkownik;
        public int waga;

        public Main() {

            InitializeComponent();

            ports = SerialPort.GetPortNames();

            dt = DateTime.Now;

            textBox5.Text = vScrollBar1.Value.ToString();
            TextBox.CheckForIllegalCrossThreadCalls = false;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMMM yyyy  |  HH:mm:ss";

        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            Connect form2 = new Connect(this);
            form2.Owner = this;
            form2.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            if (miernik == "Rhewa82") { Rhewa82(); }
            if (miernik == "rinstrumR320") { rinstrumR320(); }
            if (miernik == "Soehnle3010") { Soehnle3010(); }
            if (miernik == "IT3000") { IT3000(); }
            if (miernik == "DiniArgeo") { DiniArgeo(); }
        }

        private void DiniArgeo() {
            int a = 0;
            int res = 0;
            while (true) {
                if (serialPort1.IsOpen == true) {
                    if (serialPort1.BytesToRead > 0) {
                        try {
                            string dane = serialPort1.ReadLine();
                            string wagaString = dane[9].ToString() + dane[10].ToString() + dane[11].ToString() +
                                                dane[12].ToString() + dane[13].ToString();
                            string TrimString = wagaString.Trim();
                            waga = Convert.ToInt32(TrimString);
                            textBox1.Text = waga.ToString();

                            res += waga;
                            a++;
                            if (a >= 4) {
                                int wynik = res/a;
                                if (wynik == waga) {
                                    textBox1.ForeColor = System.Drawing.Color.Green;
                                    textBox2.Text = waga.ToString();
                                    int tara = int.Parse(textBox4.Text);
                                    if ((waga - tara) < 0) { textBox3.Text = "0"; }
                                    double netto = waga - tara;
                                    double percent = (Convert.ToDouble(vScrollBar1.Value)/100)*(netto);
                                    textBox3.Text = Convert.ToInt32(netto - percent).ToString();
                                }
                                else {
                                    textBox1.ForeColor = System.Drawing.Color.Red;
                                    textBox2.Text = "";
                                    textBox3.Text = "";
                                }
                                a = 0;
                                res = 0;
                            }
                        }
                        catch {
                            // ignored
                        }
                    }
                }
            }
        }

        private void IT3000() {
            int a = 0;
            int res = 0;
            while (true) {
                if (serialPort1.IsOpen == true) {
                    if (serialPort1.BytesToRead > 0) {
                        try {
                            string dane = serialPort1.ReadLine();
                            string wagaString = dane[6].ToString() + dane[7].ToString() + dane[8].ToString() +
                                                dane[9].ToString() + dane[10].ToString() + dane[11].ToString();
                            string TrimString = wagaString.Trim();
                            waga = Convert.ToInt32(TrimString);
                            textBox1.Text = waga.ToString();

                            res += waga;
                            a++;
                            if (a >= 3) {
                                int wynik = res/a;
                                if (wynik == waga) {
                                    textBox1.ForeColor = System.Drawing.Color.Green;
                                    textBox2.Text = waga.ToString();
                                    int tara = int.Parse(textBox4.Text);
                                    if ((waga - tara) < 0) { textBox3.Text = "0"; }
                                    double netto = waga - tara;
                                    double percent = (Convert.ToDouble(vScrollBar1.Value)/100)*(netto);
                                    textBox3.Text = Convert.ToInt32(netto - percent).ToString();
                                }
                                else {
                                    textBox1.ForeColor = System.Drawing.Color.Red;
                                    textBox2.Text = "";
                                    textBox3.Text = "";
                                }
                                a = 0;
                                res = 0;
                            }
                        }
                        catch {
                            // ignored
                        }
                    }
                }
            }
        }

        private void Soehnle3010() {
            int a = 0;
            int res = 0;
            while (true) {
                if (serialPort1.IsOpen == true) {
                    if (serialPort1.BytesToRead > 0) {
                        try {
                            string wagaString;
                            string dane = serialPort1.ReadLine().ToString();
                            if (dane[7].ToString() == "0") {
                                wagaString = "0";
                            }
                            else {
                                wagaString = dane[14].ToString() + dane[15].ToString() + dane[16].ToString() +
                                             dane[17].ToString() + dane[18].ToString();
                            }
                            string TrimString = wagaString.Trim();
                            waga = Convert.ToInt32(TrimString);
                            textBox1.Text = waga.ToString();

                            res += waga;
                            a++;
                            if (a >= 3) {
                                int wynik = res/a;
                                if (wynik == waga) {
                                    textBox1.ForeColor = System.Drawing.Color.Green;
                                    textBox2.Text = waga.ToString();
                                    int tara = int.Parse(textBox4.Text);
                                    if ((waga - tara) < 0) { textBox3.Text = "0"; }
                                    double netto = waga - tara;
                                    double percent = (Convert.ToDouble(vScrollBar1.Value)/100)*(netto);
                                    textBox3.Text = Convert.ToInt32(netto - percent).ToString();
                                }
                                else {
                                    textBox1.ForeColor = System.Drawing.Color.Red;
                                    textBox2.Text = "";
                                    textBox3.Text = "";
                                }
                                a = 0;
                                res = 0;
                            }
                        }
                        catch {
                            // ignored
                        }
                    }
                }
            }
        }

        private void rinstrumR320() {
            int a = 0;
            int res = 0;
            while (true) {
                if (serialPort1.IsOpen == true) {
                    if (serialPort1.BytesToRead > 0) {
                        try {
                            string dane = serialPort1.ReadExisting();
                            string wagaString = dane[3].ToString() + dane[4].ToString() + dane[5].ToString() +
                                                dane[6].ToString() + dane[7].ToString();
                            string trimString = wagaString.Trim();
                            waga = Convert.ToInt32(trimString);
                            textBox1.Text = waga.ToString();

                            res += waga;
                            a++;
                            if (a >= 3) {
                                int wynik = res/a;
                                if (wynik == waga) {
                                    textBox1.ForeColor = System.Drawing.Color.Green;
                                    textBox2.Text = waga.ToString();
                                    int tara = int.Parse(textBox4.Text);
                                    if ((waga - tara) < 0) { textBox3.Text = "0"; }
                                    double netto = waga - tara;
                                    double percent = (Convert.ToDouble(vScrollBar1.Value)/100)*(netto);
                                    textBox3.Text = Convert.ToInt32(netto - percent).ToString();
                                }
                                else {
                                    textBox1.ForeColor = System.Drawing.Color.Red;
                                    textBox2.Text = "";
                                    textBox3.Text = "";
                                }
                                a = 0;
                                res = 0;
                            }
                        }
                        catch {
                            // ignored
                        }
                    }
                }
            }
        }

        private void Rhewa82() {
            int a = 0;
            int res = 0;
            while (true) {
                textBox1.ReadOnly = false;
                if (serialPort1.IsOpen == true) {
                    if (serialPort1.BytesToRead > 0) {
                        try {
                            string dane = serialPort1.ReadLine();
                            string wagaString = dane[3].ToString() + dane[4].ToString() + dane[5].ToString() +
                                                dane[6].ToString() + dane[7].ToString();
                            string TrimString = wagaString.Trim();
                            waga = Convert.ToInt32(TrimString);
                            textBox1.Text = waga.ToString();

                            res += waga;
                            a++;
                            if (a >= 3) {
                                int wynik = res/a;
                                if (wynik == waga) {
                                    textBox1.ForeColor = System.Drawing.Color.Green;
                                    textBox2.Text = waga.ToString();
                                    int tara = int.Parse(textBox4.Text);
                                    if ((waga - tara) < 0) { textBox3.Text = "0"; }
                                    double netto = waga - tara;
                                    double percent = (Convert.ToDouble(vScrollBar1.Value)/100)*(netto);
                                    textBox3.Text = Convert.ToInt32(netto - percent).ToString();
                                }
                                else {
                                    textBox1.ForeColor = System.Drawing.Color.Red;
                                    textBox2.Text = "";
                                    textBox3.Text = "";
                                }
                                a = 0;
                                res = 0;
                            }
                        }
                        catch {
                            // ignored
                        }
                    }
                }
                textBox1.ReadOnly = false;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if (backgroundWorker1.WorkerSupportsCancellation == true) {
                serialPort1.Close();
                this.backgroundWorker1.CancelAsync();
                textBox1.Text = "";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            if (checkBox2.Checked == true) {
                textBox4.ReadOnly = false;
            }
            else {
                textBox4.ReadOnly = true;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            try {
                dataPatch = File.ReadAllText(@"DBpatch.txt");
                connect.ConnectionString = "Data Source=" + dataPatch;
                connect.Open();
                MessageBox.Show("Połączono");

            }
            catch (Exception ex) {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e) {
            try {
                connect.Close();
                MessageBox.Show("Rozłączono");

            }
            catch (Exception ex) {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            switch (comboBox1.Text) {
                case "Kierowcy":
                    Kierowca form4 = new Kierowca(this);
                    form4.Owner = this;
                    form4.ShowDialog();
                    break;
                case "Pojazdy":
                    Pojazdy form6 = new Pojazdy(this);
                    form6.Owner = this;
                    form6.ShowDialog();
                    break;
                case "Kontrahenci":
                    Kontrachenci form5 = new Kontrachenci(this);
                    form5.Owner = this;
                    form5.ShowDialog();
                    break;
                case "Towary":
                    Towary form7 = new Towary(this);
                    form7.Owner = this;
                    form7.ShowDialog();
                    break;
            }
        } // Baza.Dodaj

        private void button1_Click_1(object sender, EventArgs e) {
            if (MessageBox.Show("Czy napewno chcesz usunąć wybrane pozycje?", "Ostrzeżenie", MessageBoxButtons.YesNo) ==
                System.Windows.Forms.DialogResult.Yes) {
                try {
                    foreach (DataGridViewRow oneRow in this.dataGridView1.SelectedRows) {
                        dataGridView1.Rows.RemoveAt(oneRow.Index);
                    }
                    UpDateBase();
                }
                catch {

                }
            }
        } // Baza.Usuń

        public void UpDateBase() {
            try {
                commandBuilder = new SQLiteCommandBuilder(dataAdapter);
                dataAdapter.Update(dataSet);
                dataShow();
            }
            catch {
                MessageBox.Show("UpDateBase error");
            }

        }

        public void dataShow() {
            try {
                dataAdapter = new SQLiteDataAdapter("select * from " + comboBox1.Text, connect);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch {
                MessageBox.Show("DataShow error");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            dataShow();
            textBox11.Text = "";

            if (comboBox1.Text == "Kierowcy") {
                comboBox16.Items.Clear();
                comboBox16.Items.Add("Nazwisko");
                comboBox16.Items.Add("Imie");
                comboBox16.SelectedIndex = 0;
                button16.Enabled = false;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;

            }
            if (comboBox1.Text == "Kontrahenci") {
                comboBox16.Items.Clear();
                comboBox16.Items.Add("Nazwa");
                comboBox16.Items.Add("SkroconaNazwa");
                comboBox16.Items.Add("NIP");
                comboBox16.Items.Add("Ulica");
                comboBox16.Items.Add("KodPocztowy");
                comboBox16.Items.Add("Miejscowosc");
                comboBox16.Items.Add("REGON");
                comboBox16.SelectedIndex = 0;
                button16.Enabled = false;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;

            }
            if (comboBox1.Text == "Pojazdy") {
                comboBox16.Items.Clear();
                comboBox16.Items.Add("NrPojazdu");
                comboBox16.Items.Add("NrNaczepy");
                comboBox16.Items.Add("Tara");
                comboBox16.SelectedIndex = 0;
                button16.Enabled = false;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;

            }
            if (comboBox1.Text == "Towary") {
                comboBox16.Items.Clear();
                comboBox16.Items.Add("Nazwa");
                comboBox16.Items.Add("Cena");
                comboBox16.Items.Add("Kod");
                comboBox16.SelectedIndex = 0;
                button16.Enabled = false;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;

            }
            if (comboBox1.Text == "Wazenie") {
                comboBox16.Items.Clear();
                comboBox16.Items.Add("Nazwa");
                comboBox16.Items.Add("SkroconaNazwa");
                comboBox16.Items.Add("NrPojazdu");
                comboBox16.Items.Add("NrNaczepy");
                comboBox16.Items.Add("Kierowca");
                comboBox16.Items.Add("Towar");
                comboBox16.Items.Add("Rodzaj");
                comboBox16.Items.Add("Ważył");
                comboBox16.SelectedIndex = 0;
                button16.Enabled = true;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;

            }
        }

        private
            void Form1_Load(object sender, EventArgs e) {
            string startPatch = Application.StartupPath;
            string DataBaseStartPatch = startPatch + "\\" + @"Config\baza.sqlite";
            if (File.Exists(@"Config\\DBpatch.txt") == true) {
                File.WriteAllText(@"Config\\DBpatch.txt", DataBaseStartPatch);
            }
            else {
                File.Create(@"Config\\DBpatch.txt");
                }
            Login form9 = new Login(this);
            form9.Owner = this;
            form9.ShowDialog();
            try {
                textBox8.Text = "Zalogowano jako " + uzytkownik.ToString();
                dataAdapter = new SQLiteDataAdapter("select * from Ustawienia", connect);
                DataTable = new DataTable();
                dataAdapter.Fill(DataTable);
                string leg = Convert.ToString((AES.Decrypt(DataTable.Rows[0][0].ToString())));
                DateTime legDateTime = Convert.ToDateTime(leg);
                textBox10.Text = "Legalizacja ważna do: " + legDateTime.ToLongDateString();
                if (legDateTime.Date >= DateTime.Now.Date) {
                    textBox10.BackColor = System.Drawing.Color.LightGreen;
                }
                else {
                    textBox10.Text = "UWAGA: Brak legalizacji!  |  Waga wymaga ponownej legalizacji  |  tel. 793 071 071";
                    textBox10.BackColor = System.Drawing.Color.IndianRed;
                }
            }
            catch {
            }
            if (uzytkownik == "mobilwag") {
                toolStripButton4.Visible = true;
                toolStripButton6.Visible = true;
                toolStripButton4.Enabled = true;
                toolStripButton6.Enabled = true;
            }
            if (uzytkownik == "admin") {
                toolStripButton4.Visible = true;
                toolStripButton4.Enabled = true;

            }
            if (stan == false) { System.Diagnostics.Process.GetCurrentProcess().Kill(); }
            try {
                if (File.Exists(@"Config\\Miernik.txt") == true) { miernik = File.ReadAllText(@"Config\\Miernik.txt"); }
            }
            catch {
            }
            timer1.Start();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e) {
            textBox5.Text = vScrollBar1.Value.ToString() + " %";
        }

        private void button5_Click_1(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand();
            if (textBox1.ForeColor == System.Drawing.Color.Green || textBox1.Text != "") {
                try {
                    cmd = connect.CreateCommand();
                    cmd.CommandText =
                        "insert into Usluga(NrRej, Towar, Waga, Data, Godzina, Ważył)  values (@NrRej, @Towar, @Waga, @Data, @Godzina, @Ważył)";
                    cmd.Parameters.AddWithValue("@NrRej", textBox6.Text);
                    cmd.Parameters.AddWithValue("@Towar", textBox7.Text);
                    if (textBox1.Text == "") {
                        cmd.Parameters.AddWithValue("@Waga", 0);
                    }
                    else {
                        cmd.Parameters.AddWithValue("@Waga", textBox2.Text);
                    }
                    cmd.Parameters.AddWithValue("@Data", dt.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Godzina", dt.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@Ważył", uzytkownik);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "");
                }
                try {
                    dataAdapter = new SQLiteDataAdapter("select * from Usluga", connect);
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView2.DataSource = dataSet.Tables[0];
                }
                catch {
                    MessageBox.Show("Problem z bazą");
                }
            }
            else {
                MessageBox.Show("1)Waga niestabilna\n2)Wartość ważenia pusta", "error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void button6_Click_1(object sender, EventArgs e) {
            if (MessageBox.Show("Czy napewno chcesz usunąć wybrane pozycje?", "Ostrzeżenie", MessageBoxButtons.YesNo) ==
                System.Windows.Forms.DialogResult.Yes) {
                try {
                    foreach (DataGridViewRow oneRow in this.dataGridView2.SelectedRows) {
                        dataGridView2.Rows.RemoveAt(oneRow.Index);
                    }
                }
                catch {
                    
                }
                try {
                    commandBuilder = new SQLiteCommandBuilder(dataAdapter);
                    dataAdapter.Update(dataSet);
                    dataAdapter = new SQLiteDataAdapter("select * from Usluga;", connect);
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView2.DataSource = dataSet.Tables[0];

                }
                catch {

                    }
                }
            }
        private void button8_Click(object sender, EventArgs e) {
            try {
                dataAdapter = new SQLiteDataAdapter("select * from Usluga", connect);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView2.DataSource = dataSet.Tables[0];
            }
            catch {
                MessageBox.Show("Problem z bazą");
            }
        }

        private void button9_Click(object sender, EventArgs e) {

            dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
            DataTable = new DataTable();
            dataAdapter.Fill(DataTable);


                if (dataGridView2.SelectedRows.Count > 0) {
                    try {
                        fs =
                            new FileStream(
                                @"Raporty_Uslugi\\Raport_nr_" + dataGridView2.SelectedRows[0].Cells["ID"].Value + ".pdf",
                                FileMode.Create);
                        document = new Document(PageSize.A4, 25, 25, 30, 30);
                        writer = PdfWriter.GetInstance(document, fs);
                    }
                    catch {
                        MessageBox.Show("Zamknij dokument .pdf");
                    }
                    PdfPTable table = new PdfPTable(2);
                    PdfPCell cell =
                        new PdfPCell(
                            new Phrase(Environment.NewLine + "Dowód wazenia nr: " +
                                       dataGridView2.SelectedRows[0].Cells["ID"].Value + Environment.NewLine + " "));
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    table.DefaultCell.HorizontalAlignment = 1;

                    dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                    DataTable = new DataTable();
                    dataAdapter.Fill(DataTable);
                    if (Convert.ToString(DataTable.Rows[0]["REGON"]) == "tak") {
                        dataAdapter = new SQLiteDataAdapter("select * from DaneFirmy", connect);
                        DataTable = new DataTable();
                        dataAdapter.Fill(DataTable);
                        PdfPCell FirmaPdfPCell =
                            new PdfPCell(
                                new Phrase('\n' + Convert.ToString(DataTable.Rows[0]["Nazwa"]) + '\n' +
                                           Convert.ToString(DataTable.Rows[0]["Ulica"]) + " " +
                                           Convert.ToString(DataTable.Rows[0]["Nr"]) + '\n' +
                                           Convert.ToString(DataTable.Rows[0]["Miasto"]) + ", " +
                                           Convert.ToString(DataTable.Rows[0]["KodPocztowy"]) + '\n' + "NIP: " +
                                           Convert.ToString(DataTable.Rows[0]["NIP"]) + "\n " + "REGON: " +
                                           Convert.ToString(DataTable.Rows[0]["REGON"]) + "\n "));
                        FirmaPdfPCell.Colspan = 2;
                        FirmaPdfPCell.HorizontalAlignment = 1;
                        table.AddCell(FirmaPdfPCell);

                    }
                    else {
                        dataAdapter = new SQLiteDataAdapter("select * from DaneFirmy", connect);
                        DataTable = new DataTable();
                        dataAdapter.Fill(DataTable);
                        PdfPCell FirmaPdfPCell =
                            new PdfPCell(
                                new Phrase('\n' + Convert.ToString(DataTable.Rows[0]["Nazwa"]) + '\n' +
                                           Convert.ToString(DataTable.Rows[0]["Ulica"]) + " " +
                                           Convert.ToString(DataTable.Rows[0]["Nr"]) + '\n' +
                                           Convert.ToString(DataTable.Rows[0]["Miasto"]) + ", " +
                                           Convert.ToString(DataTable.Rows[0]["KodPocztowy"]) + '\n' + "NIP: " +
                                           Convert.ToString(DataTable.Rows[0]["NIP"]) + "\n "));
                        FirmaPdfPCell.Colspan = 2;
                        FirmaPdfPCell.HorizontalAlignment = 1;
                        table.AddCell(FirmaPdfPCell);
                    }

                    if (dataGridView2.SelectedRows[0].Cells["NrRej"].Value.ToString() == "") { }
                    else {
                        table.AddCell(Environment.NewLine + "Nr. rejestracyjny:" + Environment.NewLine + " ");
                        table.AddCell(
                            new PdfPCell(
                                new Phrase(
                                    Environment.NewLine + dataGridView2.SelectedRows[0].Cells["NrRej"].Value.ToString(),
                                    FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12))));
                    }
                    if (dataGridView2.SelectedRows[0].Cells["Towar"].Value.ToString() == "") { }
                    else {
                        table.AddCell(Environment.NewLine + "Towar:" + Environment.NewLine + " ");
                        table.AddCell(
                            new PdfPCell(
                                new Phrase(
                                    Environment.NewLine + dataGridView2.SelectedRows[0].Cells["Towar"].Value.ToString(),
                                    FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12))));
                    }
                    if (textBox10.BackColor == System.Drawing.Color.IndianRed || textBox10.Text == "") {
                        table.AddCell(Environment.NewLine + "Waga: \n (BRAK LEGALIZACJI)" + Environment.NewLine +
                                      " ");
                    }
                    else { table.AddCell(Environment.NewLine + "Waga:" + Environment.NewLine + " "); }
                    table.AddCell(
                        new PdfPCell(
                            new Phrase(
                                Environment.NewLine + dataGridView2.SelectedRows[0].Cells["Waga"].Value.ToString() +
                                " kg",
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12))));
                    table.AddCell(Environment.NewLine + "Data:" + Environment.NewLine + " ");
                    table.AddCell(
                        new PdfPCell(
                            new Phrase(
                                Environment.NewLine + dataGridView2.SelectedRows[0].Cells["Data"].Value.ToString(),
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12))));
                    table.AddCell(Environment.NewLine + "Godzina:" + Environment.NewLine + " ");
                    table.AddCell(
                        new PdfPCell(
                            new Phrase(
                                Environment.NewLine + dataGridView2.SelectedRows[0].Cells["Godzina"].Value.ToString(),
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12))));
                    try {
                        document.Open();
                        document.Add(table);
                        document.Close();
                        writer.Close();
                        fs.Close();
                        System.Diagnostics.Process.Start(@"Raporty_Uslugi\\Raport_nr_" +
                                                         dataGridView2.SelectedRows[0].Cells["ID"].Value.ToString() +
                                                         ".pdf");
                    }
                    catch {

                    }
                }
            }
        

        private
            void button10_Click(object sender, EventArgs e) {
            try {
                System.Diagnostics.Process.Start(@"Raporty_Uslugi\\Raport_nr_" +
                                                 dataGridView2.SelectedRows[0].Cells["ID"].Value.ToString() + ".pdf");
            }
            catch {

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 1) { comboBox1.SelectedIndex = 0; }
            if (tabControl1.SelectedIndex == 2) {
                try {
                    dataAdapter = new SQLiteDataAdapter("select * from Usluga", connect);
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView2.DataSource = dataSet.Tables[0];
                }
                catch {

                }
            }
        }

        private void button11_Click(object sender, EventArgs e) {
            if (dataGridView1.SelectedRows.Count > 0) {
                if (comboBox1.SelectedItem.ToString() == "Pojazdy") {
                    comboBox2.Text = "";
                    comboBox17.Text = "";
                    textBox4.Text = "";
                    comboBox2.SelectedText = dataGridView1.SelectedRows[0].Cells["NrPojazdu"].Value.ToString();
                    comboBox17.SelectedText = dataGridView1.SelectedRows[0].Cells["NrNaczepy"].Value.ToString();
                    comboBox14.SelectedText = dataGridView1.SelectedRows[0].Cells["Tara"].Value.ToString();
                    textBox4.Text = dataGridView1.SelectedRows[0].Cells["Tara"].Value.ToString();
                }
                if (comboBox1.SelectedItem.ToString() == "Towary") {
                    comboBox4.Text = "";
                    comboBox11.Text = "";
                    comboBox4.SelectedText = dataGridView1.SelectedRows[0].Cells["Nazwa"].Value.ToString();
                    comboBox11.SelectedText = dataGridView1.SelectedRows[0].Cells["Cena"].Value.ToString();
                }
                if (comboBox1.SelectedItem.ToString() == "Kierowcy") {
                    comboBox10.Text = "";
                    comboBox10.SelectedText = dataGridView1.SelectedRows[0].Cells["Imie"].Value.ToString() + " " +
                                              dataGridView1.SelectedRows[0].Cells["Nazwisko"].Value.ToString();
                }
                if (comboBox1.SelectedItem.ToString() == "Kontrahenci") {
                    comboBox3.Text = "";
                    comboBox5.Text = "";
                    comboBox6.Text = "";
                    comboBox7.Text = "";
                    comboBox8.Text = "";
                    comboBox9.Text = "";
                    comboBox12.Text = "";
                    comboBox13.Text = "";
                    comboBox13.SelectedText = dataGridView1.SelectedRows[0].Cells["REGON"].Value.ToString();
                    comboBox12.SelectedText = dataGridView1.SelectedRows[0].Cells["SkroconaNazwa"].Value.ToString();
                    comboBox3.SelectedText = dataGridView1.SelectedRows[0].Cells["Nazwa"].Value.ToString();
                    comboBox5.SelectedText = dataGridView1.SelectedRows[0].Cells["NIP"].Value.ToString();
                    comboBox6.SelectedText = dataGridView1.SelectedRows[0].Cells["Ulica"].Value.ToString();
                    comboBox7.SelectedText = dataGridView1.SelectedRows[0].Cells["Nr"].Value.ToString();
                    comboBox8.SelectedText = dataGridView1.SelectedRows[0].Cells["KodPocztowy"].Value.ToString();
                    comboBox9.SelectedText = dataGridView1.SelectedRows[0].Cells["Miejscowosc"].Value.ToString();
                }
                if (comboBox1.SelectedItem.ToString() == "Wazenie") {
                    comboBox2.Text = "";
                    comboBox17.Text = "";
                    textBox4.Text = "";
                    comboBox2.SelectedText = dataGridView1.SelectedRows[0].Cells["NrPojazdu"].Value.ToString();
                    comboBox17.SelectedText = dataGridView1.SelectedRows[0].Cells["NrNaczepy"].Value.ToString();
                    textBox4.Text = dataGridView1.SelectedRows[0].Cells["Tara"].Value.ToString();
                    comboBox4.Text = "";
                    comboBox11.Text = "";
                    comboBox4.SelectedText = dataGridView1.SelectedRows[0].Cells["Towar"].Value.ToString();
                    comboBox11.SelectedText = dataGridView1.SelectedRows[0].Cells["Cena"].Value.ToString();
                    comboBox10.Text = "";
                    comboBox10.SelectedText = dataGridView1.SelectedRows[0].Cells["Kierowca"].Value.ToString();
                    comboBox3.Text = "";
                    comboBox5.Text = "";
                    comboBox6.Text = "";
                    comboBox7.Text = "";
                    comboBox8.Text = "";
                    comboBox9.Text = "";
                    comboBox12.Text = "";
                    comboBox13.Text = "";
                    comboBox13.SelectedText = dataGridView1.SelectedRows[0].Cells["REGON"].Value.ToString();
                    comboBox12.SelectedText = dataGridView1.SelectedRows[0].Cells["SkroconaNazwa"].Value.ToString();
                    comboBox3.SelectedText = dataGridView1.SelectedRows[0].Cells["Nazwa"].Value.ToString();
                    comboBox5.SelectedText = dataGridView1.SelectedRows[0].Cells["NIP"].Value.ToString();
                    comboBox6.SelectedText = dataGridView1.SelectedRows[0].Cells["Ulica"].Value.ToString();
                    comboBox7.SelectedText = dataGridView1.SelectedRows[0].Cells["Nr"].Value.ToString();
                    comboBox8.SelectedText = dataGridView1.SelectedRows[0].Cells["KodPocztowy"].Value.ToString();
                    comboBox9.SelectedText = dataGridView1.SelectedRows[0].Cells["Miasto"].Value.ToString();

                }
            }
            if (comboBox1.SelectedIndex == 3 || comboBox1.SelectedIndex == 4) {
                tabControl1.SelectedIndex = 0;
            }
            else {
                comboBox1.SelectedIndex = comboBox1.SelectedIndex + 1;
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand();
            if (textBox1.ForeColor == System.Drawing.Color.Green || textBox1.Text != "") {
                try {
                    cmd = connect.CreateCommand();
                    cmd.CommandText =
                        "insert into Wazenie(NrPojazdu, NrNaczepy, Towar, Kierowca, Nazwa, NIP, Ulica, Nr, KodPocztowy, Miasto, Data, Czas, WagaBrutto, WagaNetto, Tara, Cena, Rodzaj, Zanieczyszczenie, Ważył, REGON, SkroconaNazwa)  values (@NrPojazdu, @NrNaczepy, @Towar, @Kierowca, @Nazwa, @NIP, @Ulica, @Nr, @KodPocztowy, @Miasto, @Data, @Czas, @WagaBrutto, @WagaNetto, @Tara, @Cena, @Rodzaj, @Zanieczyszczenie, @Wazyl, @REGON, @SkroconaNazwa)";
                    cmd.Parameters.AddWithValue("@NrPojazdu", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@NrNaczepy", comboBox17.Text);
                    cmd.Parameters.AddWithValue("@Towar", comboBox4.Text);
                    cmd.Parameters.AddWithValue("@Kierowca", comboBox10.Text);
                    cmd.Parameters.AddWithValue("@Nazwa", comboBox3.Text);
                    cmd.Parameters.AddWithValue("@NIP", comboBox5.Text);
                    cmd.Parameters.AddWithValue("@Ulica", comboBox6.Text);
                    cmd.Parameters.AddWithValue("@Nr", comboBox7.Text);
                    cmd.Parameters.AddWithValue("@KodPocztowy", comboBox8.Text);
                    cmd.Parameters.AddWithValue("@Miasto", comboBox9.Text);
                    cmd.Parameters.AddWithValue("@Data", dt.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Czas", dt.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@WagaBrutto", textBox2.Text);
                    cmd.Parameters.AddWithValue("@WagaNetto", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Tara", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Cena", comboBox11.Text);
                    cmd.Parameters.AddWithValue("@Zanieczyszczenie", vScrollBar1.Value.ToString());
                    cmd.Parameters.AddWithValue("@Wazyl", uzytkownik);
                    if (checkBox1.Checked == true) { cmd.Parameters.AddWithValue("@Rodzaj", "Otrzymano"); }
                    if (checkBox3.Checked == true) { cmd.Parameters.AddWithValue("@Rodzaj", "Wydano"); }
                    cmd.Parameters.AddWithValue("@REGON", comboBox13.Text);
                    cmd.Parameters.AddWithValue("@SkroconaNazwa", comboBox12.Text);
                    textBox9.Text = "Dodano";
                    textBox9.BackColor = System.Drawing.Color.LightGreen;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception) {
                    //MessageBox.Show(ex.ToString());
                    textBox9.Text = "database error";
                    textBox9.BackColor = System.Drawing.Color.LightCoral;
                }
            }
            else {
                textBox9.Text = "Waga niestabilna lub wartość ważenia pusta";
                textBox9.BackColor = System.Drawing.Color.LightCoral;
            }
        } // Wazenia.Dodaj do bazy

        private void button14_Click(object sender, EventArgs e) {
            dataAdapter = new SQLiteDataAdapter("select * from " + comboBox1.Text, connect);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button12_Click(object sender, EventArgs e) {
            if (comboBox1.Text == "Wazenie") {
                if (dataGridView1.SelectedRows.Count > 0) {
                    try {
                        fs =
                            new FileStream(
                                @"Raporty_Wazenia\\Raport_nr_" + dataGridView1.SelectedRows[0].Cells["ID"].Value +
                                ".pdf", FileMode.Create);
                        document = new Document(PageSize.A4, 25, 25, 30, 30);
                        writer = PdfWriter.GetInstance(document, fs);
                    }
                    catch {
                        MessageBox.Show("Zamknij dokument .pdf");
                    }
                    PdfPTable table = new PdfPTable(2);

                    PdfPCell cell =
                        new PdfPCell(
                            new Phrase(Environment.NewLine + "Dowód wazenia nr: " +
                                       dataGridView1.SelectedRows[0].Cells["ID"].Value + Environment.NewLine + " ",
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    table.DefaultCell.HorizontalAlignment = 1;

                    if (dataGridView1.SelectedRows[0].Cells["Rodzaj"].Value.ToString() == "Wydano") {


                        dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                        DataTable = new DataTable();
                        dataAdapter.Fill(DataTable);
                        if (Convert.ToString(DataTable.Rows[0]["REGON"]) == "tak") {
                            dataAdapter = new SQLiteDataAdapter("select * from DaneFirmy", connect);
                            DataTable = new DataTable();
                            dataAdapter.Fill(DataTable);
                            PdfPCell FirmaPdfPCell =
                                new PdfPCell(
                                    new Phrase("\n Dostawca: \n \n" + " " + Convert.ToString(DataTable.Rows[0]["Nazwa"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Ulica"]) + " " +
                                              " " + Convert.ToString(DataTable.Rows[0]["Nr"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Miasto"]) + ", " +
                                              " " + Convert.ToString(DataTable.Rows[0]["KodPocztowy"]) + "\n " + "NIP: " +
                                                Convert.ToString(DataTable.Rows[0]["NIP"]) + "\n " + "REGON: " +
                                                Convert.ToString(DataTable.Rows[0]["REGON"]) + "\n "));
                            //FirmaPdfPCell.Colspan = 2;
                            FirmaPdfPCell.HorizontalAlignment = 0;
                            table.AddCell(FirmaPdfPCell);

                        }
                        else {
                            dataAdapter = new SQLiteDataAdapter("select * from DaneFirmy", connect);
                            DataTable = new DataTable();
                            dataAdapter.Fill(DataTable);
                            PdfPCell FirmaPdfPCell =
                                new PdfPCell(
                                    new Phrase("\n Dostawca: \n \n" + " " + Convert.ToString(DataTable.Rows[0]["Nazwa"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Ulica"]) + " " +
                                              " " + Convert.ToString(DataTable.Rows[0]["Nr"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Miasto"]) + ", " +
                                               Convert.ToString(DataTable.Rows[0]["KodPocztowy"]) + "\n " + "NIP: " +
                                               Convert.ToString(DataTable.Rows[0]["NIP"]) + "\n "));
                            //FirmaPdfPCell.Colspan = 2;
                            FirmaPdfPCell.HorizontalAlignment = 0;
                            table.AddCell(FirmaPdfPCell);
                        }
                        dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                        DataTable = new DataTable();
                        dataAdapter.Fill(DataTable);
                        if (Convert.ToString(DataTable.Rows[0]["REGON"]) == "tak") {

                            PdfPCell odbiorcaCell = new PdfPCell(new Phrase("\n Odbiorca: \n \n " +
                                                                            dataGridView1.SelectedRows[0].Cells["Nazwa"]
                                                                                .Value.ToString() +
                                                                            Environment.NewLine + " " +
                                                                            dataGridView1.SelectedRows[0].Cells["Ulica"]
                                                                                .Value.ToString() +
                                                                            " " +
                                                                            dataGridView1.SelectedRows[0].Cells["Nr"]
                                                                                .Value.ToString() +
                                                                            Environment.NewLine + " " +
                                                                            dataGridView1.SelectedRows[0].Cells[
                                                                                "KodPocztowy"].Value.ToString() + ", " +
                                                                            dataGridView1.SelectedRows[0].Cells["Miasto"
                                                                                ].Value.ToString() +
                                                                            Environment.NewLine + " " +
                                                                            "NIP: " +
                                                                            dataGridView1.SelectedRows[0].Cells["NIP"]
                                                                                .Value.ToString() +
                                                                            Environment.NewLine + " REGON: " + dataGridView1.SelectedRows[0].Cells["REGON"].Value.ToString(),
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                            odbiorcaCell.HorizontalAlignment = 0;
                            table.AddCell(odbiorcaCell);
                        }
                        else {
                            PdfPCell odbiorcaCell = new PdfPCell(new Phrase("\n Odbiorca: \n \n " +
                                                dataGridView1.SelectedRows[0].Cells["Nazwa"]
                                                    .Value.ToString() +
                                                Environment.NewLine + " " +
                                                dataGridView1.SelectedRows[0].Cells["Ulica"]
                                                    .Value.ToString() +
                                                " " +
                                                dataGridView1.SelectedRows[0].Cells["Nr"]
                                                    .Value.ToString() +
                                                Environment.NewLine + " " +
                                                dataGridView1.SelectedRows[0].Cells[
                                                    "KodPocztowy"].Value.ToString() + ", " +
                                                dataGridView1.SelectedRows[0].Cells["Miasto"
                                                    ].Value.ToString() +
                                                Environment.NewLine + " " +
                                                "NIP: " +
                                                dataGridView1.SelectedRows[0].Cells["NIP"]
                                                    .Value.ToString() +
                                                Environment.NewLine + " ",
    FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                            odbiorcaCell.HorizontalAlignment = 0;
                            table.AddCell(odbiorcaCell);

                        }

                    }
                    else {
                        dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                        DataTable = new DataTable();
                        dataAdapter.Fill(DataTable);
                        if (Convert.ToString(DataTable.Rows[0]["REGON"]) == "tak") {

                            PdfPCell odbiorcaCell = new PdfPCell(new Phrase("\n Dostawca: \n \n " +
                                                                            dataGridView1.SelectedRows[0].Cells["Nazwa"]
                                                                                .Value.ToString() +
                                                                            Environment.NewLine + " " +
                                                                            dataGridView1.SelectedRows[0].Cells["Ulica"]
                                                                                .Value.ToString() +
                                                                            " " +
                                                                            dataGridView1.SelectedRows[0].Cells["Nr"]
                                                                                .Value.ToString() +
                                                                            Environment.NewLine + " " +
                                                                            dataGridView1.SelectedRows[0].Cells[
                                                                                "KodPocztowy"].Value.ToString() + ", " +
                                                                            dataGridView1.SelectedRows[0].Cells["Miasto"
                                                                                ].Value.ToString() +
                                                                            Environment.NewLine + " " +
                                                                            "NIP: " +
                                                                            dataGridView1.SelectedRows[0].Cells["NIP"]
                                                                                .Value.ToString() +
                                                                            Environment.NewLine + " REGON: " + dataGridView1.SelectedRows[0].Cells["REGON"]
                                                                                .Value.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                            odbiorcaCell.HorizontalAlignment = 0;
                            table.AddCell(odbiorcaCell);
                        }
                        else {
                            PdfPCell odbiorcaCell = new PdfPCell(new Phrase("\n Dostawca: \n \n " +
                                                dataGridView1.SelectedRows[0].Cells["Nazwa"]
                                                    .Value.ToString() +
                                                Environment.NewLine + " " +
                                                dataGridView1.SelectedRows[0].Cells["Ulica"]
                                                    .Value.ToString() +
                                                " " +
                                                dataGridView1.SelectedRows[0].Cells["Nr"]
                                                    .Value.ToString() +
                                                Environment.NewLine + " " +
                                                dataGridView1.SelectedRows[0].Cells[
                                                    "KodPocztowy"].Value.ToString() + ", " +
                                                dataGridView1.SelectedRows[0].Cells["Miasto"
                                                    ].Value.ToString() +
                                                Environment.NewLine + " " +
                                                "NIP: " +
                                                dataGridView1.SelectedRows[0].Cells["NIP"]
                                                    .Value.ToString() +
                                                Environment.NewLine + " ",
    FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                            odbiorcaCell.HorizontalAlignment = 0;
                            table.AddCell(odbiorcaCell);

                        }
                        dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                        DataTable = new DataTable();
                        dataAdapter.Fill(DataTable);
                        if (Convert.ToString(DataTable.Rows[0]["REGON"]) == "tak") {
                            dataAdapter = new SQLiteDataAdapter("select * from DaneFirmy", connect);
                            DataTable = new DataTable();
                            dataAdapter.Fill(DataTable);
                            PdfPCell FirmaPdfPCell =
                                new PdfPCell(
                                    new Phrase("\n Odbiorca: \n \n" + " " + Convert.ToString(DataTable.Rows[0]["Nazwa"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Ulica"]) + " " +
                                              " " + Convert.ToString(DataTable.Rows[0]["Nr"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Miasto"]) + ", " +
                                              " " + Convert.ToString(DataTable.Rows[0]["KodPocztowy"]) + "\n " + "NIP: " +
                                                Convert.ToString(DataTable.Rows[0]["NIP"]) + "\n " + "REGON: " +
                                                Convert.ToString(DataTable.Rows[0]["REGON"]) + "\n "));
                            //FirmaPdfPCell.Colspan = 2;
                            FirmaPdfPCell.HorizontalAlignment = 0;
                            table.AddCell(FirmaPdfPCell);

                        }
                        else {
                            dataAdapter = new SQLiteDataAdapter("select * from DaneFirmy", connect);
                            DataTable = new DataTable();
                            dataAdapter.Fill(DataTable);
                            PdfPCell FirmaPdfPCell =
                                new PdfPCell(
                                    new Phrase("\n Dostawca: \n \n" + " " + Convert.ToString(DataTable.Rows[0]["Nazwa"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Ulica"]) + " " +
                                              " " + Convert.ToString(DataTable.Rows[0]["Nr"]) + '\n' +
                                              " " + Convert.ToString(DataTable.Rows[0]["Miasto"]) + ", " +
                                               Convert.ToString(DataTable.Rows[0]["KodPocztowy"]) + "\n " + "NIP: " +
                                               Convert.ToString(DataTable.Rows[0]["NIP"]) + "\n "));
                            //FirmaPdfPCell.Colspan = 2;
                            FirmaPdfPCell.HorizontalAlignment = 0;
                            table.AddCell(FirmaPdfPCell);
                        }

                    }

                    dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                    DataTable = new DataTable();
                    dataAdapter.Fill(DataTable);
                    if (Convert.ToString(DataTable.Rows[0]["Kierowca"]) == "tak") {
                        table.AddCell(Environment.NewLine + "Kierowca:" + Environment.NewLine + " ");
                        PdfPCell numerNaczepyCell =
                        new PdfPCell(
    new Phrase(Environment.NewLine +
               dataGridView1.SelectedRows[0].Cells["Kierowca"].Value.ToString() +
               Environment.NewLine + " ",
        FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                        numerNaczepyCell.HorizontalAlignment = 0;
                        table.AddCell(numerNaczepyCell);
                    }





                    table.AddCell(Environment.NewLine + "Numer rejestracyjny pojazdu:" + Environment.NewLine + " ");

                    PdfPCell numerCell =
                        new PdfPCell(
                            new Phrase(Environment.NewLine +
                                       dataGridView1.SelectedRows[0].Cells["NrPojazdu"].Value.ToString() +
                                       Environment.NewLine + " ",
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                    numerCell.HorizontalAlignment = 0;
                    table.AddCell(numerCell);
                    dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                    DataTable = new DataTable();
                    dataAdapter.Fill(DataTable);
                    if (Convert.ToString(DataTable.Rows[0]["NrNaczepy"]) == "tak") {
                        table.AddCell(Environment.NewLine + "Numer rejestracyjny naczepy:" + Environment.NewLine + " ");
                        PdfPCell numerNaczepyCell =
                        new PdfPCell(
    new Phrase(Environment.NewLine +
               dataGridView1.SelectedRows[0].Cells["NrNaczepy"].Value.ToString() +
               Environment.NewLine + " ",
        FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                        numerNaczepyCell.HorizontalAlignment = 0;
                        table.AddCell(numerNaczepyCell);
                    }
                    table.AddCell(Environment.NewLine + "Towar:" + Environment.NewLine + " ");

                    PdfPCell towarCell =
                        new PdfPCell(
                            new Phrase(
                                Environment.NewLine + dataGridView1.SelectedRows[0].Cells["Towar"].Value.ToString() +
                                Environment.NewLine + " ",
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                    towarCell.HorizontalAlignment = 0;
                    table.AddCell(towarCell);
                    dataAdapter = new SQLiteDataAdapter("select * from Druk", connect);
                    DataTable = new DataTable();
                    dataAdapter.Fill(DataTable);
                    if (Convert.ToString(DataTable.Rows[0]["Cena"]) == "tak") {
                        table.AddCell("\n Cena: ");
                        double cena = 0;
                        try {
                             cena = double.Parse(dataGridView1.SelectedRows[0].Cells["Cena"].Value.ToString());
                        }
                        catch {
                            
                        }
                        double netto = double.Parse(dataGridView1.SelectedRows[0].Cells["WagaNetto"].Value.ToString());
                        PdfPCell CenaCell =
    new PdfPCell(
        new Phrase(
            Environment.NewLine + String.Format("{0:0.##}", (cena * netto) / 1000) + " zł \n ",
            FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                        CenaCell.HorizontalAlignment = 0;
                        table.AddCell(CenaCell);

                    }
                    if (textBox10.BackColor == System.Drawing.Color.IndianRed || textBox10.Text == "") {
                        table.AddCell(Environment.NewLine + "Waga: \n (BRAK LEGALIZACJI)" + Environment.NewLine +
                                      " ");
                    }
                    else {
                        table.AddCell(Environment.NewLine + "Waga:" + Environment.NewLine + " ");
                    }
                    //table.AddCell(Environment.NewLine + "Waga:" + Environment.NewLine + " ");

                    PdfPCell wagaZanieczyszczonaCell =
                        new PdfPCell(
                            new Phrase(Environment.NewLine + "Brutto: " +
                                       dataGridView1.SelectedRows[0].Cells["WagaBrutto"].Value.ToString() + " kg" +
                                       Environment.NewLine + "Netto:  " +
                                       dataGridView1.SelectedRows[0].Cells["WagaNetto"].Value.ToString() + " kg" + '\n' +
                                       "(Zanieczyszczenie: " +
                                       dataGridView1.SelectedRows[0].Cells["Zanieczyszczenie"].Value.ToString() + "%)" +
                                       Environment.NewLine + "Tara:   " +
                                       dataGridView1.SelectedRows[0].Cells["Tara"].Value.ToString() + " kg" +
                                       Environment.NewLine + " ",
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                    wagaZanieczyszczonaCell.HorizontalAlignment = 0;

                    PdfPCell wagaCell =
                        new PdfPCell(
                            new Phrase(Environment.NewLine + "Brutto: " +
                                       dataGridView1.SelectedRows[0].Cells["WagaBrutto"].Value.ToString() + " kg" +
                                       Environment.NewLine + "Netto:  " +
                                       dataGridView1.SelectedRows[0].Cells["WagaNetto"].Value.ToString() + " kg" +
                                       Environment.NewLine + "Tara:   " +
                                       dataGridView1.SelectedRows[0].Cells["Tara"].Value.ToString() + " kg" +
                                       Environment.NewLine + " ",
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                    wagaCell.HorizontalAlignment = 0;

                    if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Zanieczyszczenie"].Value) > 0) {
                        table.AddCell(wagaZanieczyszczonaCell);
                    }
                    else {
                        table.AddCell(wagaCell);
                    }
                    table.AddCell(" ");
                    PdfPCell pomiarCell =
                        new PdfPCell(
                            new Phrase(Environment.NewLine + "data: " +
                                       dataGridView1.SelectedRows[0].Cells["Data"].Value.ToString() +
                                       Environment.NewLine + "godzina: " +
                                       dataGridView1.SelectedRows[0].Cells["Czas"].Value.ToString() +
                                       Environment.NewLine + " ",
                                FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 12)));
                    pomiarCell.HorizontalAlignment = 0;
                    table.AddCell(pomiarCell);
                    try {
                        document.Open();
                        document.Add(table);
                        document.Close();
                        writer.Close();
                        fs.Close();
                    }
                    catch{
                        
                    }
                }
                System.Diagnostics.Process.Start(@"Raporty_Wazenia\\Raport_nr_" +
                                                 dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() + ".pdf");
            }
            else {
                MessageBox.Show("Raporty .PDF dostępne tylko dla ważeń");
            }

        }

        private void button13_Click(object sender, EventArgs e) {
            if (comboBox1.Text == "Wazenie") {
                try {
                    System.Diagnostics.Process.Start(@"Raporty_Wazenia\\Raport_nr_" +
                                                     dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString() + ".pdf");
                }
                catch {
                    // ignored
                }
            }
            else {
                MessageBox.Show("Raporty PDF dostępne tylko dla ważeń");
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            if (checkBox4.Checked == false) {
                try {
                    //serialPort1.Close();
                    textBox1.Text = "";
                    serialPort1.Open();
                    if (serialPort1.IsOpen == true) {
                        if (backgroundWorker1.IsBusy != true) { backgroundWorker1.RunWorkerAsync(); }
                    }
                }
                catch {
                    checkBox4.Checked = false;
                    //MessageBox.Show("Connetion error");
                }
            }
            else {
                try {
                    if (backgroundWorker1.WorkerSupportsCancellation == true) {
                        serialPort1.Close();
                        backgroundWorker1.CancelAsync();
                        textBox1.Text = "";
                    }
                    textBox1.ForeColor = System.Drawing.Color.Green;
                }
                catch {
                    checkBox4.Checked = true;
                    // ignored
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            try {
                int brutto = Convert.ToInt32(textBox1.Text);
                textBox2.Text = brutto.ToString();
                if (textBox4.Text == "") { textBox4.Text = "0"; }
                int tara = Convert.ToInt32(textBox4.Text);
                textBox4.Text = tara.ToString();
                int netto = brutto - tara;
                double percent = (Convert.ToDouble(vScrollBar1.Value)/100)*(netto);
                if ((brutto - tara) < 0) {
                    textBox3.Text = "0";
                    tara = 0;
                }
                else {
                    textBox3.Text = (netto - percent).ToString();
                }
            }
            catch {
                // ignored
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox1.Checked == true) { checkBox3.Checked = false; }
            if (checkBox1.Checked == false) { checkBox3.Checked = true; }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            if (checkBox3.Checked == true) { checkBox1.Checked = false; }
            if (checkBox3.Checked == false) { checkBox1.Checked = true; }
        }

        private void button3_Click_1(object sender, EventArgs e) {
            checkBox4.Checked = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            string tara = textBox4.Text;
            textBox4.Text = "";
            textBox5.Text = "0";
            checkBox3.Checked = false;
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            textBox4.ReadOnly = true;
            vScrollBar1.Value = 0;
            try {
                serialPort1.Close();
            }
            catch {
                // ignored
            }
            textBox4.Text = tara;
            try {
                serialPort1.Open();
            }
            catch {
                // ignored
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            Users form10 = new Users(this);
            form10.Owner = this;
            form10.ShowDialog();

        }

        private void timer1_Tick(object sender, EventArgs e) {
            dt = DateTime.Now;
            dateTimePicker1.Value = dt;
            //dateTimePicker1.Value = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, dateTimePicker1.Value.Day, dt.Hour, dt.Minute, dt.Second);
        }

        private void button7_Click_1(object sender, EventArgs e) {
            tabControl1.SelectedIndex = 1;
            comboBox1.SelectedIndex = 0;
        }

        private void toolStripButton6_Click(object sender, EventArgs e) {
            mobilwagUser form11 = new mobilwagUser(this);
            form11.Owner = this;
            form11.ShowDialog();
        }

        private void button14_Click_1(object sender, EventArgs e) {
            try {
                // creating Excel Application
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                // creating new WorkBook within Excel application
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                // creating new Excelsheet in workbook
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                // see the excel sheet behind the program
                app.Visible = true;
                // get the reference of first sheet. By default its name is Sheet1.
                // store its reference to worksheet
                //worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                // changing the name of active sheet
                worksheet.Name = comboBox1.Text;
                // storing header part in Excel
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++) {
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }
                // storing Each row and column value to excel sheet
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++) {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                // save the application
                var _path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                //_path + "\\Config\\" + textBox2.Text + ".sqlite"
                workbook.SaveAs(_path + "\\Raporty_zbiorcze\\" + comboBox1.Text + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xls", Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing,
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                // Exit from the application
                //app.Quit();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        public class SimplerAES {
            private static byte[] key = {
                13, 217, 19, 11, 24, 26, 85, 45, 114, 114, 27, 162, 34, 112,
                222, 29, 241, 24, 175, 144, 174, 53, 196, 29, 24, 66, 17, 218, 131, 236, 53, 209
            };

            private static byte[] vector = {
                146, 61, 191, 111, 23, 8, 113, 119, 251, 121, 221, 12, 79,
                32, 114, 156
            };

            private ICryptoTransform encryptor, decryptor;
            private UTF8Encoding encoder;

            public SimplerAES() {
                RijndaelManaged rm = new RijndaelManaged();
                encryptor = rm.CreateEncryptor(key, vector);
                decryptor = rm.CreateDecryptor(key, vector);
                encoder = new UTF8Encoding();
            }

            public string Encrypt(string unencrypted) {
                return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
            }

            public string Decrypt(string encrypted) {
                return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
            }

            public byte[] Encrypt(byte[] buffer) {
                return Transform(buffer, encryptor);
            }

            public byte[] Decrypt(byte[] buffer) {
                return Transform(buffer, decryptor);
            }

            protected byte[] Transform(byte[] buffer, ICryptoTransform transform) {
                MemoryStream stream = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write)) {
                    cs.Write(buffer, 0, buffer.Length);
                }
                return stream.ToArray();
            }
        }

        private void tabPage3_Click(object sender, EventArgs e) {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if ((e.KeyChar <= 57 && e.KeyChar >= 48) || e.KeyChar == 13 || e.KeyChar == 8) {
            }
            else {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e) {
            if ((e.KeyChar <= 57 && e.KeyChar >= 48) || e.KeyChar == 13 || e.KeyChar == 8) {
            }
            else {
                e.Handled = true;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e) {
            Ustawienia ustawienia = new Ustawienia(this);
            ustawienia.Owner = this;
            ustawienia.ShowDialog();
        }

        private void button15_Click_1(object sender, EventArgs e) {
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            comboBox8.Text = "";
            comboBox9.Text = "";
            comboBox10.Text = "";
            comboBox11.Text = "";
            comboBox12.Text = "";
            comboBox13.Text = "";
            comboBox17.Text = "";
            comboBox14.Text = "";

        }

        private void button8_Click_1(object sender, EventArgs e) {
            try {
                // creating Excel Application
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                // creating new WorkBook within Excel application
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                // creating new Excelsheet in workbook
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                // see the excel sheet behind the program
                app.Visible = true;
                // get the reference of first sheet. By default its name is Sheet1.
                // store its reference to worksheet
                //worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                // changing the name of active sheet
                worksheet.Name = "Usługa";
                // storing header part in Excel
                for (int i = 1; i < dataGridView2.Columns.Count + 1; i++) {
                    worksheet.Cells[1, i] = dataGridView2.Columns[i - 1].HeaderText;
                }
                // storing Each row and column value to excel sheet
                for (int i = 0; i < dataGridView2.Rows.Count; i++) {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++) {
                        worksheet.Cells[i + 2, j + 1] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                    }
                }
                // save the application
                var _path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                //_path + "\\Config\\" + textBox2.Text + ".sqlite"
                workbook.SaveAs(_path + "\\Raporty_zbiorcze\\Usługi" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xls", Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing,
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                // Exit from the application
                //app.Quit();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                //ignore
            }
        }

        private void textBox11_TextChanged_1(object sender, EventArgs e) {
            //dataAdapter = new SQLiteDataAdapter("select * from " + comboBox1.Text + " where Nazwa ='" + textBox11.Text + "'", connect);
            //DataTable = new DataTable();
            //dataAdapter.Fill(DataTable);
            //dataGridView1.DataSource = DataTable;


            DataTable dt = (DataTable)dataGridView1.DataSource;
            try { dt.DefaultView.RowFilter = comboBox16.Text + " like '%" + textBox11.Text + "%'"; }
            catch {
                
            }

        }

        private void button16_Click(object sender, EventArgs e) {
            DateTime OdKiedyDate = dateTimePicker2.Value;
            DateTime DoKiedyDate = dateTimePicker3.Value;
            string OdKiedyDateString = OdKiedyDate.ToShortDateString();
            string DoKiedyDateString = DoKiedyDate.ToShortDateString();
            DataTable dt = (DataTable)dataGridView1.DataSource;
            dt.DefaultView.RowFilter = "(Data >= #" + OdKiedyDateString + "# and Data <=#" + DoKiedyDateString + "#)";
        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e) {
            textBox11.Text = "";
        }

        private void button10_Click_1(object sender, EventArgs e) {
            UpDateBase();

        }

        private void button13_Click_1(object sender, EventArgs e) {
            textBox4.Text = "";
            textBox4.Text = comboBox14.Text;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            Licencja LicencjaForm = new Licencja(this);
            LicencjaForm.Owner = this;
            LicencjaForm.ShowDialog();
        }
    }
}

 



    


