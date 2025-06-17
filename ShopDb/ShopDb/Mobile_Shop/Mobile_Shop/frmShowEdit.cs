using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mobile_Shop
{
    public partial class frmShowEdit : Form
    {
        public frmShowEdit()
        {
            InitializeComponent();
        }

        private void frmShowEdit_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }
        private void LoadGridData()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(@"select se.sellerId,se.sellerName,se.sellerEmail,se.sellerContact,se.picture,de.departmentName from sellers se inner join department de on se.departmentId=de.departmentId", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                this.dataGridView1.DataSource = dt;
                con.Close();

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM sellers WHERE sellerId=@id", con))
                    {

                        using (SqlDataAdapter sda = new SqlDataAdapter("SELECT DISTINCT * FROM department", con))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            cmbDep.DataSource = ds.Tables[0];
                            cmbDep.DisplayMember = "departmentName";
                            cmbDep.ValueMember = "departmentId";
                        }


                        con.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            txtId.Text = dr.GetInt32(0).ToString();
                            txtId.Enabled = false;
                            txtName.Text = dr.GetString(1).ToString();
                            txtContact.Text = dr.GetString(2).ToString();
                            txtEmail.Text = dr.GetString(3).ToString();

                            MemoryStream ms = new MemoryStream((byte[])dr[4]);
                            Image img = Image.FromStream(ms);
                            pictureBox1.Image = img;

                            cmbDep.SelectedValue = dr.GetInt32(5).ToString();
                        }

                        con.Close();


                    }
                }
            }
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog1.FileName);
                this.pictureBox1.Image = img;
                txtPicturePath.Text = openFileDialog1.FileName;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string picUrl = txtPicturePath.Text;
            if (picUrl == "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
                {
                    Image img = Image.FromFile(txtPicturePath.Text);
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Bmp);

                    using (SqlCommand cmd = new SqlCommand("UPDATE sellers SET sellerName=@n, sellerContact=@c,sellerEmail=@e, departmentId=@s WHERE sellerId=@id ", con))
                    {
                        cmd.Parameters.AddWithValue("@id", txtId.Text);
                        cmd.Parameters.AddWithValue("@n", txtName.Text);
                        cmd.Parameters.AddWithValue("@c", txtContact.Text);
                        cmd.Parameters.AddWithValue("@e", txtEmail.Text);

                        cmd.Parameters.AddWithValue("@s", cmbDep.SelectedValue);
                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Data Updated Successfully!!!");
                            LoadGridData();
                        }
                        con.Close();
                    }

                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
                {
                    Image img = Image.FromFile(txtPicturePath.Text);
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Bmp);

                    using (SqlCommand cmd = new SqlCommand("UPDATE sellers SET sellerName=@n, sellerContact=@c,sellerEmail=@e,picture=@p, departmentId=@s WHERE sellerId=@id ", con))
                    {
                        cmd.Parameters.AddWithValue("@id", txtId.Text);
                        cmd.Parameters.AddWithValue("@n", txtName.Text);
                        cmd.Parameters.AddWithValue("@c", txtContact.Text);
                        cmd.Parameters.AddWithValue("@e", txtEmail.Text);
                        cmd.Parameters.Add(new SqlParameter("@p", SqlDbType.VarBinary) { Value = ms.ToArray() });
                        cmd.Parameters.AddWithValue("@s", cmbDep.SelectedValue);
                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Data Updated Successfully!!!");
                            LoadGridData();
                        }

                        con.Close();
                    }

                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM sellers WHERE sellerId=@id ", con))
                {
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data Deleted Successfully!!!");
                        LoadGridData();
                        ClearAll();
                    }
                    con.Close();
                }
            }
        }
        private void ClearAll()
        {
            txtId.Clear();
            txtName.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtPicturePath.Clear();
            cmbDep.SelectedIndex = -1;
            pictureBox1.Image = null;
        }
    }
}
