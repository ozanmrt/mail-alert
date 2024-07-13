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

namespace MailUyarı
{
    public partial class Form_Filtreleme : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=MERT; Initial Catalog=MailUyari; Integrated Security=True;MultipleActiveResultSets=true;MultipleActiveResultSets=true;");



        public Form_Filtreleme()
        {
            InitializeComponent();
        }

        private void Form_Filtreleme_Load(object sender, EventArgs e)
        {
            try
            {
                radioButton1.Checked = true;
                pictureBox1.Visible = true;
                panel1.Visible = true;
                VerileriGetir();

            }                   
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /*
            foreach (var item in Form1.KritikKelimeler)
            {
                listBox1.Items.Add(item);
            }

            foreach (string item in Form1.OnemliKelimeler)
            {
                listBox2.Items.Add(item);
            }*/

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;

                panel1.Visible = true;
                panel2.Visible = false;
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;

                panel1.Visible = false;
                panel2.Visible = true;
            }
        }


        private void RenkleriSirala()
        {
            try
            {
                Form1.MailOkunmayanlar++;
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void VerileriGetir()
        {
            try
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();



                SqlCommand komut = new SqlCommand("Select * from kritik", baglanti); // Kritik Kelimeler İçin
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    listBox1.Items.Add(dr["kritikKelime"]);
                }

                dr.Close();

                 SqlCommand komut2 = new SqlCommand("Select * from onemli", baglanti); // Önemli Kelimeler İçin
                 SqlDataReader dr2 = komut2.ExecuteReader();
                 while (dr2.Read())
                 {
                     listBox2.Items.Add(dr2["onemliKelime"]);
                 }

                dr2.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Butonlar
        private void button1_Click(object sender, EventArgs e) // Kritik Kelimeler Ekleme
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (textBox1.Text.Trim() != "")
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();

                    SqlCommand komut = new SqlCommand("insert into kritik(kritikKelime) values (@kritikKelime)", baglanti);
                    komut.Parameters.AddWithValue("@kritikKelime", textBox1.Text);
                    komut.ExecuteNonQuery();

                    Form1.KritikKelimeler.Add(textBox1.Text);

                    textBox1.Clear();
                    RenkleriSirala();

                    VerileriGetir();
                }
            }           
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;

        }

        private void button4_Click(object sender, EventArgs e) // Önemli Kelimeler Ekleme
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (textBox2.Text.Trim() != "")
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();

                    SqlCommand komut = new SqlCommand("insert into onemli(onemliKelime) values (@onemliKelime)", baglanti);
                    komut.Parameters.AddWithValue("@onemliKelime", textBox2.Text);
                    komut.ExecuteNonQuery();

                    Form1.OnemliKelimeler.Add(textBox2.Text);

                    textBox2.Clear();
                    RenkleriSirala();

                    VerileriGetir();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e) // Kritik Kelimeler Silme
        {
            try
            {
                // Form1.KritikKelimeler.Remove(listBox1.SelectedItem);
                //  listBox1.Items.Remove(listBox1.SelectedItem);

                if (listBox1.SelectedItem != null)
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();

                    SqlCommand komut = new SqlCommand("delete from kritik where kritikKelime=@kritikKelime", baglanti);
                    komut.Parameters.AddWithValue("@kritikKelime", listBox1.SelectedItem);
                    komut.ExecuteNonQuery();
                    Form1.KritikKelimeler.Remove(listBox1.SelectedItem);
                    RenkleriSirala();
                    VerileriGetir();
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void button3_Click(object sender, EventArgs e) // Önemli Kelimeler Silme
        {
            try
            {
                // Form1.OnemliKelimeler.Remove(listBox2.SelectedItem);
                // listBox2.Items.Remove(listBox2.SelectedItem);

                if (listBox2.SelectedItem != null)
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();

                    SqlCommand komut = new SqlCommand("delete from onemli where onemliKelime=@onemliKelime", baglanti);
                    komut.Parameters.AddWithValue("@onemliKelime", listBox2.SelectedItem);
                    komut.ExecuteNonQuery();
                    Form1.OnemliKelimeler.Remove(listBox2.SelectedItem);
                    RenkleriSirala();
                    VerileriGetir();
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
