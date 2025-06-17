using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mobile_Shop
{
    public partial class frmEdit : Form
    {
        BindingSource bsS = new BindingSource();
        BindingSource bsC = new BindingSource();
        DataSet ds = new DataSet();

        public frmEdit()
        {
            InitializeComponent();
        }

        public void LoadDataBindingSource()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM customer", con))
                {
                    ds = new DataSet();
                    sda.Fill(ds, "customer");
                    sda.SelectCommand.CommandText = "SELECT * FROM products";
                    sda.Fill(ds, "products");

                    ds.Tables["customer"].Columns.Add(new DataColumn("image", typeof(byte[])));
                    for (int i = 0; i < ds.Tables["customer"].Rows.Count; i++)
                    {
                        ds.Tables["customer"].Rows[i]["image"] = File.ReadAllBytes($@"..\..\Pictured\{ds.Tables["customer"].Rows[i]["picture"]}");
                    }

                    DataRelation rel = new DataRelation("FK_S_C", ds.Tables["customer"].Columns["customerId"], ds.Tables["products"].Columns["customerId"]);
                    ds.Relations.Add(rel);
                    bsS.DataSource = ds;
                    bsS.DataMember = "customer";

                    bsC.DataSource = bsS;
                    bsC.DataMember = "FK_S_C";
                    dataGridView1.DataSource = bsC;
                    AddDataBindings();
                }
            }
        }
        private void AddDataBindings()
        {
            lblId.DataBindings.Clear();
            lblId.DataBindings.Add("Text", bsS, "customerId");

            lblName.DataBindings.Clear();
            lblName.DataBindings.Add("Text", bsS, "customerName");

            lblDob.DataBindings.Clear();
            lblDob.DataBindings.Add("Text", bsS, "dateOfBirth");
            Binding bm = new Binding("Text", bsS, "dateOfBirth", true);
            bm.Format += Bm_Format;
            lblDob.DataBindings.Clear();
            lblDob.DataBindings.Add(bm);

            pictureBox1.DataBindings.Clear();
            pictureBox1.DataBindings.Add(new Binding("Image", bsS, "image", true));

            checkBox1.DataBindings.Clear();
            checkBox1.DataBindings.Add("Checked", bsS, "insideDhaka", true);

        }

        private void Bm_Format(object sender, ConvertEventArgs e)
        {
            DateTime d = (DateTime)e.Value;
            e.Value = d.ToString("dd-MM-yyyy");
        }


        private void frmEdit_Load(object sender, EventArgs e)
        {
            LoadDataBindingSource();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            int v = int.Parse((bsS.Current as DataRowView).Row[0].ToString());
            new EditForm
            {
                TheForm = this,
                IdToEdit = v
            }.ShowDialog();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (bsS.Position < bsS.Count - 1)
            {
                bsS.MoveNext();
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (bsS.Position > 0)
            {
                bsS.MovePrevious();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bsS.MoveFirst();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
             bsS.MoveLast();
        }
    }
}
