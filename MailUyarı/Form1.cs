using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Media;
using System.Collections;
using System.Net.Mail;
using System.Threading;

namespace MailUyarı
{
    public partial class Form1 : Form
    {
        #region Tanımlamalar
            Outlook.Application oApp;
            Outlook.NameSpace oNS;
            Outlook.MAPIFolder oInbox;
            public DataTable tablo;
            public BodyForm WBBody;
            public Form_Filtreleme FormFiltre;
            SoundPlayer alarm = new SoundPlayer();
            TimeSpan otuzdk = new TimeSpan(0, 1, 0);

            string[] MailSubject;
            string[] MailSenderName;
            public string[] MailBody;
            string[] MailDate;
            bool[] MailRead;
            string[] MailDurum;
            dynamic[] Mailler;
            dynamic SonGovde;

            public static ArrayList KritikKelimeler = new ArrayList();
            public static ArrayList OnemliKelimeler = new ArrayList();   
           
            int DiziIndex, MailSayisi, MailOlmayanlar;
            public static int MailOkunmayanlar;
            SqlConnection baglanti = new SqlConnection("Data Source=MERT; Initial Catalog=MailUyari; Integrated Security=True;MultipleActiveResultSets=true;MultipleActiveResultSets=true;");
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            try
            { 
                
                baglanti.Open();
                

                oApp = new Outlook.Application(); // Outlook uygulamasını oluşturun.ou
                oNS = oApp.GetNamespace("mapi"); // MAPI ad alanını alın.
                oInbox = oNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox); // Gelen Kutusu klasörünü al. 
                oNS.SendAndReceive(true);

                tablo = new DataTable("oInbox");
             

                tablo.Columns.Add("okunduBilgisi", typeof(bool));
                tablo.Columns.Add("Konu", typeof(string));
                tablo.Columns.Add("Gonderen", typeof(string));
                tablo.Columns.Add("Body", typeof(string));
                tablo.Columns.Add("Date", typeof(DateTime));
                tablo.Columns.Add("durum", typeof(string));

                dataGridView1.DataSource = tablo;

                alarm.SoundLocation = Application.StartupPath + "\\Sounds\\School_Fire_Alarm-Cullen_Card-202875844.wav";

                // this.label1 = new System.Windows.Forms.Label();


                comboBox1.SelectedIndex = 0;


                SqlCommand komut = new SqlCommand("Select * from kritik", baglanti);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    KritikKelimeler.Add(dr["kritikKelime"].ToString());
                }

                dr.Close();


                SqlCommand komut2 = new SqlCommand("Select * from onemli", baglanti);
                SqlDataReader dr2 = komut2.ExecuteReader();
                while (dr2.Read())
                {
                    OnemliKelimeler.Add(dr2["onemliKelime"].ToString());
                }

                dr2.Close();



                //tablo.Clear();
                // dataGridView1.Refresh();

                DiziyiDoldur();
                MailIslem();
            
                timer1.Start();
                timer2.Start();

                dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
                timer4.Start();
                timer3.Start();
                               
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void DiziyiDoldur()
        {
            try
            {
               
                MailSubject = new string[oInbox.Items.Count];
                MailSenderName = new string[oInbox.Items.Count];
                MailBody = new string[oInbox.Items.Count];
                MailDate = new string[oInbox.Items.Count];
                MailRead = new bool[oInbox.Items.Count];
                MailDurum = new string[oInbox.Items.Count];
                Mailler = new dynamic[oInbox.Items.Count];
               

                DiziIndex = 0;
                MailOlmayanlar = 0;
                MailOkunmayanlar = 0;
                MailSayisi = 0;

                foreach (dynamic item in oInbox.Items)
                {
                    if (item is Outlook.MailItem)
                    {
                        MailSubject[DiziIndex] = item.Subject;
                       
                        MailSenderName[DiziIndex] = item.SenderName;
                        MailBody[DiziIndex] = item.HTMLBody;
                        MailDate[DiziIndex] = item.SentOn.ToLongDateString() + " " + item.SentOn.ToLongTimeString();
                        Mailler[DiziIndex] = item;
                     

                        if (item.UnRead == true) // Okunmamış mailleri al
                        {
                            MailRead[DiziIndex] = false;
                            MailOkunmayanlar++;

                        }
                        else
                        {
                            MailRead[DiziIndex] = true;
                        }

                        


                        // Reklendirme

                        FormFiltre = new Form_Filtreleme();
                        MailDurum[DiziIndex] = "1basit";

                        foreach (string kelime in OnemliKelimeler)
                        {
                            if (MailSubject[DiziIndex].ToLower().IndexOf(kelime.ToLower()) != -1)
                            {
                                MailDurum[DiziIndex] = "2orta";

                            }
                        }
                        foreach (string kelime in KritikKelimeler)
                        {
                            if (MailSubject[DiziIndex].ToLower().IndexOf(kelime.ToLower()) != -1)
                            {
                                MailDurum[DiziIndex] = "3kritik";

                            }
                        }

                      
                        DiziIndex++;
                        MailSayisi++;
                      
                    }
                    else
                    {
                        MailOlmayanlar++;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void MailIslem()
        {
            for (int i = 0; i < MailSayisi; i++)
            {
                tablo.Rows.Add(MailRead[i], MailSubject[i], MailSenderName[i], MailBody[i], MailDate[i], MailDurum[i]);
                dataGridView1.DataSource = tablo;



                /* if (MailSubject[i].IndexOf("Kritik") != -1)
                 {
                     DataGridViewCellStyle renk = new DataGridViewCellStyle();
                     renk.BackColor = Color.Red;
                     renk.ForeColor = Color.White;
                     dataGridView1.Rows[i].DefaultCellStyle = renk;
                 }*/

            }

            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
        }
        public void MailGonder(string Konu,string Govde)
        {
            try
            {


                MailMessage ePosta = new MailMessage();
                ePosta.From = new MailAddress("example@gmail.com");
                ePosta.To.Add("example2@gmail.com");
                ePosta.Subject = Konu;
                ePosta.Body = Govde;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential("example@gmail.com", "password");
                smtp.Port = 25;
                smtp.Host = "10.49.1.8";
                smtp.EnableSsl = false;
                //smtp.SendAsync(ePosta, (object)ePosta);
                smtp.Send(ePosta);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Menü - GridView - Rapor Bölümü
        private void FiltrelemeToolStripMenuItem_Click(object sender, EventArgs e) // Kelimelerin ayarlandığı menü
        {
            Form_Filtreleme filtreForm = new Form_Filtreleme();
            filtreForm.ShowDialog();

        }

        private void gelişmişFiltrelemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gelismis_Grid gelismisGrid = new Gelismis_Grid();
            gelismisGrid.Show();

        }

        private void raporlarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < tablo.Rows.Count && e.RowIndex >= 0)
            {

                WBBody = new BodyForm();
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;


                foreach (dynamic item in oInbox.Items)
                {
                    if (item is Outlook.MailItem && dataGridView1.CurrentRow.Cells["Body"].Value.ToString() == item.HTMLBody && item.UnRead == true)
                    {                                              
                        try
                        {
                            item.Display(false);

                            foreach (string kelime in KritikKelimeler)
                            {
                                if (item.Subject.ToLower().IndexOf(kelime.ToLower()) != -1)
                                {

                                    if (baglanti.State == ConnectionState.Closed)
                                        baglanti.Open();


                                        SqlCommand komut = new SqlCommand("update Mails set OkunduBilgisi=@OkunduBilgisi,Calisma_Durumu=@CalismaDurumu,OtuzDK=@OtuzDK where govde=@govde", baglanti);
                                        komut.Parameters.AddWithValue("@OkunduBilgisi", true);
                                        komut.Parameters.AddWithValue("@govde", item.HTMLBody);
                                        komut.Parameters.AddWithValue("@CalismaDurumu", "Çalışılıyor");
                                        komut.Parameters.AddWithValue("@OtuzDK", true);
                                        komut.ExecuteNonQuery();
                                
                                    
                                    break;
                                }

                            }

                                                    
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        

                        break;
                    }
                    else if (item is Outlook.MailItem && dataGridView1.CurrentRow.Cells["Body"].Value.ToString() == item.HTMLBody)
                    {
                        item.Display(false);


                        if (baglanti.State == ConnectionState.Closed)
                            baglanti.Open();


                            SqlCommand komut = new SqlCommand("Select OkunduBilgisi from Mails where Govde=@govde", baglanti);
                            komut.Parameters.AddWithValue("@govde", item.HTMLBody);

                            SqlDataReader dr = komut.ExecuteReader();
                            if(dr.Read())
                            {
                                if (bool.Parse(dr["OkunduBilgisi"].ToString()) == false)
                                {
                                    SqlCommand kmt = new SqlCommand("update Mails set OkunduBilgisi=@OkunduBilgisi,Calisma_Durumu=@CalismaDurumu where govde=@govde", baglanti);
                                    kmt.Parameters.AddWithValue("@OkunduBilgisi", true);
                                    kmt.Parameters.AddWithValue("@govde", item.HTMLBody);
                                    kmt.Parameters.AddWithValue("@CalismaDurumu", "Çalışılıyor");
                                    komut.Parameters.AddWithValue("@OtuzDK", true);
                                    kmt.ExecuteNonQuery();
                                 }
                            }


                            dr.Close();


                    }
                }

                /*
                WBBody.Size = WBBody.webBrowser1.Size;
                WBBody.Show();
                WBBody.webBrowser1.DocumentText = dataGridView1.CurrentRow.Cells["Body"].Value.ToString();
                */

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < tablo.Rows.Count && e.RowIndex >= 0)
            {
                comboBox2.Text = null;
                comboBox1.Text = "";
                richTextBox1.Text = "";

                WBBody = new BodyForm();
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;


                foreach (dynamic item in oInbox.Items)
                {
                    if (item is Outlook.MailItem && dataGridView1.CurrentRow.Cells["Body"].Value.ToString() == item.HTMLBody)
                    {
                        try
                        {

                            if (baglanti.State == ConnectionState.Closed)
                                baglanti.Open();


                            SqlCommand kmt = new SqlCommand("Select * from Mails where Govde=@govde", baglanti);
                            kmt.Parameters.AddWithValue("@govde", item.HTMLBody);
                            SqlDataReader sdr = kmt.ExecuteReader();


                            if (sdr.Read())
                            {
                                    
                                        groupBox1.Visible = true;

                                        // Label Bilgi
                                        label13.Text = item.Subject;
                                        label12.Text = item.SenderName;
                                        label11.Text = item.SentOn.ToLongDateString() + " " + item.SentOn.ToLongTimeString();

                                        SqlCommand komut = new SqlCommand("Select Calisma_Durumu,Ilgili_Kisi,Aciklama from Mails where Govde=@govde", baglanti); // Kritik Kelimeler İçin
                                        komut.Parameters.AddWithValue("@govde", item.HTMLBody);
                                        SonGovde = item.HTMLBody;

                                        SqlDataReader dr = komut.ExecuteReader();
                                        while (dr.Read())
                                        {
                                            comboBox1.Text = dr["Calisma_Durumu"].ToString();
                                            comboBox2.Text = dr["Ilgili_Kisi"].ToString();
                                            richTextBox1.Text = dr["Aciklama"].ToString();
                                        }

                                        if (comboBox1.Text == "Okunmadı" || comboBox1.Text == "Bitti")
                                        {
                                            comboBox1.Enabled = false;
                                        }

                                        else
                                        {
                                            comboBox1.Enabled = true;
                                        }


                                        dr.Close();


                            }
                            else
                            {
                                groupBox1.Visible = false;
                            }
                            sdr.Close();



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }



                        break;
                    }
                }


            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 2)
            {
                comboBox2.Enabled = true;
                richTextBox1.Enabled = true;
                button1.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
                comboBox2.Text = null;
                richTextBox1.Enabled = false;
                richTextBox1.Text = "";
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox1.Text.Trim() != "" && comboBox2.Text != "")
                {
                    DialogResult secenek = MessageBox.Show("Raporu veritabanına kaydetmek istiyor musunuz?", "Bilgilendirme Penceresi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (secenek == DialogResult.Yes)
                    {
                        if (baglanti.State == ConnectionState.Closed)
                            baglanti.Open();

                        SqlCommand komut = new SqlCommand("update Mails set Ilgili_Kisi=@Ilgili_Kisi,Calisma_Durumu=@CalismaDurumu,Aciklama=@Aciklama where govde=@govde", baglanti);
                        komut.Parameters.AddWithValue("@Ilgili_Kisi", comboBox2.Text);
                        komut.Parameters.AddWithValue("@CalismaDurumu", "Bitti");
                        komut.Parameters.AddWithValue("@Aciklama", richTextBox1.Text);
                        komut.Parameters.AddWithValue("@govde", SonGovde);
                        komut.ExecuteNonQuery();

                        comboBox1.Enabled = false;

                        MessageBox.Show("Rapor Kaydedildi", "Bilgilendirme Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (secenek == DialogResult.No)
                    {
                        MessageBox.Show("Rapor Kaydedilmedi", "Bilgilendirme Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                    



                }
                else
                {
                    MessageBox.Show("İlgilenen Kişi ve Açıklama Satırı Boş Bırakılamaz!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }           
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Sıralama
        private void rengeGöreSıralaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
        }

        private void kritikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tablo.Clear();
            dataGridView1.Refresh();

            for (int i = 0; i < MailSayisi; i++)
            {

                foreach (string kelime in KritikKelimeler)
                {
                    if (MailSubject[i].ToLower().IndexOf(kelime.ToLower()) != -1)
                    {
                        tablo.Rows.Add(MailRead[i], MailSubject[i], MailSenderName[i], MailBody[i], MailDate[i], MailDurum[i]);
                        dataGridView1.DataSource = tablo;

                    }
                }

            }
            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
        }

        private void onemliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tablo.Clear();
            dataGridView1.Refresh();

            for (int i = 0; i < MailSayisi; i++)
            {

                foreach (string kelime in OnemliKelimeler)
                {
                    if (MailSubject[i].ToLower().IndexOf(kelime.ToLower()) != -1)
                    {
                        tablo.Rows.Add(MailRead[i], MailSubject[i], MailSenderName[i], MailBody[i], MailDate[i], MailDurum[i]);
                        dataGridView1.DataSource = tablo;

                    }
                }

            }
            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tablo.Clear();
            dataGridView1.Refresh();

            for (int i = 0; i < MailSayisi; i++)
            {
                int sayac = 0;

                foreach (string kelime in OnemliKelimeler)
                {
                    if (MailSubject[i].ToLower().IndexOf(kelime.ToLower()) != -1)
                    {
                        sayac++;
                    }
                }
                foreach (string kelime in KritikKelimeler)
                {
                    if (MailSubject[i].ToLower().IndexOf(kelime.ToLower()) != -1)
                    {
                        sayac++;        
                    }
                }

                if (sayac == 0)
                {
                    tablo.Rows.Add(MailRead[i], MailSubject[i], MailSenderName[i], MailBody[i], MailDate[i], MailDurum[i]);
                    dataGridView1.DataSource = tablo;
                }

            }
            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Descending);
        }

        private void tümünüSiralaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MailOkunmayanlar++;
        }
        #endregion

        #region Timer
        private void timer1_Tick(object sender, EventArgs e) // Diziyi dolduran ve Maile İşlem yapan Timer
        {
            try
            {
                int newMailCount = oInbox.Items.Count - MailOlmayanlar;
                int unreadMailCount = oInbox.UnReadItemCount;

                if (MailSayisi != newMailCount || MailOkunmayanlar != unreadMailCount)
                {
                    DiziyiDoldur();

                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();

                    Outlook.Items oItems = oInbox.Items;
                    Outlook.MailItem lastMail = (Outlook.MailItem)oItems.GetLast();

                    foreach (string kelime in KritikKelimeler)
                    {
                        if (lastMail.Subject.ToLower().IndexOf(kelime.ToLower()) != -1 && lastMail.UnRead == true)
                        {
                            SqlCommand komut = new SqlCommand("insert into Mails(OkunduBilgisi,Konu,Gonderen,Govde,Tarih,Calisma_Durumu,OtuzDK) values (@OkunduBilgisi,@Konu,@Gonderen,@Govde,@Tarih,@CalismaDurumu,@OtuzDK)", baglanti);
                            komut.Parameters.AddWithValue("@OkunduBilgisi", false);
                            komut.Parameters.AddWithValue("@Konu", lastMail.Subject);
                            komut.Parameters.AddWithValue("@Gonderen", lastMail.SenderName);
                            komut.Parameters.AddWithValue("@Govde", lastMail.HTMLBody);
                            komut.Parameters.AddWithValue("@Tarih", lastMail.SentOn.ToLongDateString() + " " + lastMail.SentOn.ToLongTimeString());
                            komut.Parameters.AddWithValue("@CalismaDurumu", "Okunmadı");
                            komut.Parameters.AddWithValue("@OtuzDK", false);

                            komut.ExecuteNonQuery();
                            break; // Tek bir kritik kelime için ekleme yeterli
                        }
                    }

                    tablo.Clear();
                    MailIslem();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void timer2_Tick(object sender, EventArgs e) // Renk Ayarlamasını Yapan Timer
        {
            try
            {
                DataGridViewCellStyle normalStyle = new DataGridViewCellStyle() { BackColor = Color.Green, ForeColor = Color.White };
                DataGridViewCellStyle onemliStyle = new DataGridViewCellStyle() { BackColor = Color.Orange, ForeColor = Color.White };
                DataGridViewCellStyle kritikStyle = new DataGridViewCellStyle() { BackColor = Color.Red, ForeColor = Color.White };

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string konu = dataGridView1.Rows[i].Cells["Konu"].Value.ToString().ToLower();
                    bool isOnemli = false;
                    bool isKritik = false;

                    foreach (string item in OnemliKelimeler)
                    {
                        if (konu.Contains(item.ToLower()))
                        {
                            dataGridView1.Rows[i].DefaultCellStyle = onemliStyle;
                            isOnemli = true;
                            break;
                        }
                    }

                    if (!isOnemli)
                    {
                        foreach (string item in KritikKelimeler)
                        {
                            if (konu.Contains(item.ToLower()))
                            {
                                dataGridView1.Rows[i].DefaultCellStyle = kritikStyle;
                                isKritik = true;
                                break;
                            }
                        }
                    }

                    if (!isOnemli && !isKritik)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle = normalStyle;
                    }
                }

                label1.Text = "Mail Sayısı: " + dataGridView1.RowCount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e) // Mail 30 Dakika Kontrolü Yapan Timer
        {
            try
            {
                SqlCommand komut = new SqlCommand("Select * from Mails where OtuzDK = 0", baglanti); // Otuz dakika kontrol edilmemiş mailleri seç

                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dt = DateTime.Parse(dr["Tarih"].ToString());
                    TimeSpan ts = DateTime.Now - dt;
                    int id = int.Parse(dr["ID"].ToString());
                    string konu = dr["Konu"].ToString();
                    string gonderen = dr["Gonderen"].ToString();

                    if (ts > otuzdk)
                    {
                        MailGonder("30 DAKİKA UYARISI!", $"Konu: {konu}\nGönderen: {gonderen}\nTarih: {dt.ToString()}\n\nİlgili Maile 30 Dakika Boyunca Bakılmadı!");

                        SqlCommand komut2 = new SqlCommand("update Mails set OtuzDK = 1 where ID = @ID", baglanti);
                        komut2.Parameters.AddWithValue("@ID", id);
                        komut2.ExecuteNonQuery();
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void timer4_Tick(object sender, EventArgs e) // Alarm Çalan Timer
        {
            try
            {
                int kritikMailSayisi = 0;

                for (int i = 0; i < MailSayisi; i++)
                {
                    if (MailDurum[i] == "3kritik" && MailRead[i] == false)
                    {
                        kritikMailSayisi++;
                    }
                }

                if (kritikMailSayisi > 0)
                {
                    alarm.Play();
                    dataGridView1.Size = new Size(843, 389);
                    pictureBox1.Visible = true;
                }
                else
                {
                    alarm.Stop();
                    pictureBox1.Visible = false;
                    dataGridView1.Size = new Size(843, 468);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion
    }
}
