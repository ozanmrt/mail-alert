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
    public partial class RaporForm : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=MERT; Initial Catalog=MailUyari; Integrated Security=True;MultipleActiveResultSets=true;MultipleActiveResultSets=true;");

        public RaporForm()
        {
            InitializeComponent();
        }

        private void RaporForm_Load(object sender, EventArgs e)
        {

        }
    }
}
