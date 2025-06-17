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

namespace Mobile_Shop
{
    public partial class frm_CRUDwith_SP : Form
    {
        string conString = "Data Source=PC\\SQLEXPRESS;Initial Catalog=shop_db;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand cmd;
        string accountantId = "";
        public frm_CRUDwith_SP()
        {
            InitializeComponent();
            sqlCon = new SqlConnection(conString);
            sqlCon.Open();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter accountant name!!");
                txtName.Select();
            }
            else if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("Enter city!!");
                txtCity.Select();
            }
            else if (string.IsNullOrWhiteSpace(txtDepartment.Text))
            {
                MessageBox.Show("Enter department!!");
                txtDepartment.Select();
            }
            else if (comboBox1.SelectedIndex <= -1)
            {
                MessageBox.Show("Select gender!!");
                comboBox1.Select();
            }
            else
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    DataTable dtData = new DataTable();
                    cmd = new SqlCommand("spAccountant", sqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@actionType", "SaveData");
                    cmd.Parameters.AddWithValue("@actId", accountantId);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@city", txtCity.Text);
                    cmd.Parameters.AddWithValue("@department", txtDepartment.Text);
                    cmd.Parameters.AddWithValue("@gender", comboBox1.Text);

                    int numRes = cmd.ExecuteNonQuery();
                    if (numRes > 0)
                    {
                        MessageBox.Show("Data saved successfully!!!");
                        LoadGrid();
                        ClearAll();
                    }


                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        private DataTable ShowAllEmployeeData()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            DataTable dtData = new DataTable();
            cmd = new SqlCommand("spAccountant", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@actionType", "ShowAllData");
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dtData);
            return dtData;
        }

        private void ClearAll()
        {
            btnSave.Text = "Save";
            txtName.Clear();
            txtCity.Clear();
            txtDepartment.Clear();
            comboBox1.SelectedIndex = -1;
            accountantId = "";
            LoadGrid();
        }

        private void frm_CRUDwith_SP_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }
        private void LoadGrid()
        {
            dataGridView1.DataSource = ShowAllEmployeeData();
        }
        private DataTable ShowEmpRecordById(string actId)
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            DataTable dtData = new DataTable();
            cmd = new SqlCommand("spAccountant", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@actionType", "ShowAllDataById");
            cmd.Parameters.AddWithValue("@actId", actId);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dtData);
            return dtData;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnSave.Text = "Update";
                accountantId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                DataTable dtData = ShowEmpRecordById(accountantId);
                if (dtData.Rows.Count > 0)
                {
                    accountantId = dtData.Rows[0][0].ToString();
                    txtName.Text = dtData.Rows[0][1].ToString();
                    txtCity.Text = dtData.Rows[0][2].ToString();
                    txtDepartment.Text = dtData.Rows[0][3].ToString();
                    comboBox1.Text = dtData.Rows[0][4].ToString();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(accountantId))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    DataTable dtData = new DataTable();
                    cmd = new SqlCommand("spAccountant", sqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@actionType", "DeleteData");
                    cmd.Parameters.AddWithValue("@actId", accountantId);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    int numRes = cmd.ExecuteNonQuery();
                    if (numRes > 0)
                    {
                        MessageBox.Show("Data deleted successfully!!!");
                        LoadGrid();
                        ClearAll();
                    }
                    else
                    {
                        MessageBox.Show("Please try again!!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: - " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a record!!");
            }
        }
    }
}
