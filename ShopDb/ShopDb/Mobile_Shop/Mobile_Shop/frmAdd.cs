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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Mobile_Shop
{
    public partial class frmAdd : Form
    {
        string currentFile = string.Empty;
        List<ProductDetails> details = new List<ProductDetails>();
        public frmAdd()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFile = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(currentFile);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            details.Add(new ProductDetails
            {
                productName = txtProductName.Text,
                manufactureDate = dptManufacture.Value,
                sellDate = dptSellDate.Value,
                price = numericUpDown1.Value
            });
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = details;
        }
        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                con.Open();
                using (SqlTransaction trx = con.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.Transaction = trx;
                        //for image
                        string ext = Path.GetExtension(currentFile);
                        string f = Path.GetFileNameWithoutExtension(DateTime.Now.Ticks.ToString()) + ext;

                        string savePath = @"..\..\Pictured\" + f;
                        MemoryStream ms = new MemoryStream(File.ReadAllBytes(currentFile));
                        byte[] bytes = ms.ToArray();
                        FileStream fs = new FileStream(savePath, FileMode.Create);
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();

                        //for data
                        cmd.CommandText = "insert into customer(customerName,dateOfBirth,insideDhaka,picture)values(@cn,@dob,@isd,@pic); SELECT SCOPE_IDENTITY()";
                        cmd.Parameters.AddWithValue("@cn", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@dob", dptDob.Value);
                        cmd.Parameters.AddWithValue("@isd", checkBox1.Checked);
                        cmd.Parameters.AddWithValue("@pic", f);

                        try
                        {
                            var sid = cmd.ExecuteScalar();
                            foreach (var s in details)
                            {
                                cmd.CommandText = @"insert into products(productName,manufactureDate,sellDate,price,customerId)values(@pn,@mdt,@sdt,@pp,@i)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@pn", s.productName);
                                cmd.Parameters.AddWithValue("@mdt", s.manufactureDate);
                                cmd.Parameters.AddWithValue("@sdt", s.sellDate);
                                cmd.Parameters.AddWithValue("@pp", s.price);
                                cmd.Parameters.AddWithValue("@i", sid);
                                cmd.ExecuteNonQuery();
                            }
                            trx.Commit();
                            MessageBox.Show("Data Saved successfully!!");
                            details.Clear();
                        }
                        catch
                        {
                            trx.Rollback();
                        }



                    }
                }
                con.Close();
            }
        }
    }
}


