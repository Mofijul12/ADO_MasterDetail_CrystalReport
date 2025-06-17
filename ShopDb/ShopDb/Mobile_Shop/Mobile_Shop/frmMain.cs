using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mobile_Shop
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdd add= new frmAdd();
            add.Show();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEdit fe = new frmEdit();
            fe.Show();
        }

        private void customerInfoReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReportPage rp = new frmReportPage();
            rp.Show();
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmSellerAdd sf = new frmSellerAdd();
            sf.Show();
            
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmShowEdit se = new frmShowEdit();
            se.Show();
        }

        private void sellerInfoReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSellerInformationReport sf = new frmSellerInformationReport();
            sf.Show();
        }

        private void addEditDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_CRUDwith_SP sp=new frm_CRUDwith_SP();
            sp.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
