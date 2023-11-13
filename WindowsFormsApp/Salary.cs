using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApp
{
    public partial class Salary : UserControl
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True");
        public Salary()
        {
            InitializeComponent();

            displaySalaryData();
            disableFields();
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }
            displaySalaryData();
            disableFields();
        }

        public void disableFields()
        {
            txtEmpID.Enabled = false;
            txtFullNames.Enabled = false;
            txtPosition.Enabled = false;           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Salary_Load(object sender, EventArgs e)
        {

        }
        public void displaySalaryData()
        {
            SalaryData empData = new SalaryData();
            List<SalaryData> listdata = empData.salaryEmployeeListData();

            grdAddEmployee.DataSource = listdata;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(txtEmpID.Text == "" || txtFullNames.Text == "" || txtPosition.Text == "" || txtSalary.Text == "")
            {
                MessageBox.Show("Please fill all the required fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DateTime today = DateTime.Today;
                DialogResult check = MessageBox.Show("Are you sure you want to UPDATE Employee ID " + txtEmpID.Text.Trim() + " ? ", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (check == DialogResult.Yes)
                {
                    if(con.State == ConnectionState.Closed)
                    {

                        try
                        {
                            con.Open();

                            string updateData = "UPDATE Employees " +
                                                "SET salary = @salary, updateDate = @updateDate " +
                                                "WHERE empID = @empID";

                            using (SqlCommand cmd = new SqlCommand(updateData, con))
                            {
                                cmd.Parameters.AddWithValue("@empID", txtEmpID.Text.Trim());
                                cmd.Parameters.AddWithValue("@salary", txtSalary.Text.Trim());
                                cmd.Parameters.AddWithValue("@updateDate", today);

                                cmd.ExecuteNonQuery();

                                displaySalaryData();

                                MessageBox.Show("Employee Updated Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                clearFields();
                            }
                             
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error " + ex, " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            con.Close();
                        } 
                    }
                }
                else
                {
                    MessageBox.Show("Cancelled. ", " Information Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void grdAddEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = grdAddEmployee.Rows[e.RowIndex];
                txtEmpID.Text = row.Cells[0].Value.ToString();
                txtFullNames.Text = row.Cells[1].Value.ToString();                
                txtPosition.Text = row.Cells[4].Value.ToString();
                txtSalary.Text = row.Cells[5].Value.ToString();
            }
        }
        public void clearFields()
        {
            txtEmpID.Text = "";
            txtFullNames.Text = "";          
            txtPosition.Text = "";
            txtSalary.Text = "";           
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearFields();
        }
    }
}
