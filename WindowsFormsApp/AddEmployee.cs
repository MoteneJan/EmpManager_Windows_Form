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
    public partial class AddEmployee : UserControl
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True");
        public AddEmployee()
        {
            InitializeComponent();

            //This Displays the Data from Database to DataGrid
            displayEmployeeData();
        }

        public void displayEmployeeData()
        {
            EmployeeData empData = new EmployeeData();
            List<EmployeeData> listdata = empData.employeeListData();

            dataGridView1.DataSource = listdata;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtEmpID.Text == "" || txtFullNames.Text == "" || cmbGender.Text == "" || cmbPosition.Text == "" || cmbStatus.Text == "" || txtPhoneNum.Text == "" || txtAddress.Text == "" || pictureBox.Image == null)
            {
                MessageBox.Show("Please fill all the required fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (con.State == ConnectionState.Closed)
                {
                    try
                    {
                        con.Open();
                        string checkID = "SELECT (*) FROM Employees WHERE empID = @emID";

                        using (SqlCommand checkEmp = new SqlCommand(checkID, con))
                        {
                            checkEmp.Parameters.AddWithValue("@emID", txtEmpID.Text.Trim());

                            int count = (int)checkEmp.ExecuteScalar();

                            if (count >= 1)
                            {
                                MessageBox.Show(txtEmpID.Text.Trim() + " is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;
                                string insertData = "INSERT INTO Employees" +
                                                     "(empID, FullNames, gender, position, contactNum, phyAddress, photo, insertDate, status)" +
                                                     "VALUES(@empID, @FullNames, @gender, @position, @contactNum, @phyAddress, @photo, @insertDate, @status)";

                                string path = Path.Combine(@"C:\Users\moten\Documents\TUTORIALS\PROJECTS\EmpManager_Windows_Form\WindowsFormsApp\Directory\" + txtEmpID.Text.Trim() + ".jpg");

                                string directoryPath = Path.GetDirectoryName(path);

                                if(!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                File.Copy(pictureBox.ImageLocation, path, true);

                                using (SqlCommand cmd = new SqlCommand(insertData, con))
                                {
                                    cmd.Parameters.AddWithValue("@empID", txtEmpID.Text.Trim());
                                    cmd.Parameters.AddWithValue("@FullNames", txtFullNames.Text.Trim());
                                    cmd.Parameters.AddWithValue("@gender", cmbGender.Text.Trim());
                                    cmd.Parameters.AddWithValue("@position", cmbPosition.Text.Trim());
                                    cmd.Parameters.AddWithValue("@contactNum", txtPhoneNum.Text.Trim());
                                    cmd.Parameters.AddWithValue("@phyAddress", txtAddress.Text.Trim());
                                    cmd.Parameters.AddWithValue("@photo", path);
                                    cmd.Parameters.AddWithValue("@insertDate", today);
                                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text.Trim());

                                    cmd.ExecuteNonQuery();

                                    displayEmployeeData();

                                    MessageBox.Show("Added Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
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

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png) | *.jpg; *.png";

                string imagePath = "";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    pictureBox.ImageLocation = imagePath;
                } 
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error " + ex, " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                        
        }
    }
}
