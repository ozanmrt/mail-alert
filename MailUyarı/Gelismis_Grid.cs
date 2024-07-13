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

namespace MailUyarı
{
    public partial class Gelismis_Grid : Form
    {

        #region Tanımlamalar
        Outlook.Application oApp;
        Outlook.NameSpace oNS;
        Outlook.MAPIFolder oInbox;
        public DataTable tablo;
        public BodyForm WBBody;
        public Form_Filtreleme FormFiltre;
        SoundPlayer alarm = new SoundPlayer();
        string[] MailSubject;
        string[] MailSenderName;
        public string[] MailBody;
        string[] MailDate;
        bool[] MailRead;
        string[] MailDurum;
        dynamic[] Mailler;
        public static ArrayList KritikKelimeler = new ArrayList();
        public static ArrayList OnemliKelimeler = new ArrayList();
        int DiziIndex, MailSayisi, MailOlmayanlar;
        public static int MailOkunmayanlar;
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog=MailUyari; User Id=sa; password=qwer789+;");
        #endregion

        public Gelismis_Grid()
        {
            InitializeComponent();
        }

        private void Gelismis_Grid_Load(object sender, EventArgs e)
        {

        }
    }
}
