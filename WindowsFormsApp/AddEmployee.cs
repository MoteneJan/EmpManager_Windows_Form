using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

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

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;                
            }
            displayEmployeeData();
        }

        public void displayEmployeeData()
        {
            EmployeeData empData = new EmployeeData();
            List<EmployeeData> listdata = empData.employeeListData();

            grdAddEmployee.DataSource = listdata;
        }

        public void clearFields()
        {
            txtEmpID.Text = "";
            txtFullNames.Text = "";
            cmbGender.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            txtPhoneNum.Text = "";
            txtAddress.Text = "";
            cmbStatus.SelectedIndex = -1;
            pictureBox.Image = null;
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
                        string checkID = "SELECT COUNT(*) FROM Employees WHERE empID = @emID AND deleteDate IS NULL ";

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
                                                     "(empID, fullNames, gender, position, contactNum, phyAddress, photo, salary, insertDate, status)" +
                                                     "VALUES(@empID, @fullNames, @gender, @position, @contactNum, @phyAddress, @photo, @salary, @insertDate, @status)";

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
                                    cmd.Parameters.AddWithValue("@fullNames", txtFullNames.Text.Trim());
                                    cmd.Parameters.AddWithValue("@gender", cmbGender.Text.Trim());
                                    cmd.Parameters.AddWithValue("@position", cmbPosition.Text.Trim());
                                    cmd.Parameters.AddWithValue("@contactNum", txtPhoneNum.Text.Trim());
                                    cmd.Parameters.AddWithValue("@phyAddress", txtAddress.Text.Trim());
                                    cmd.Parameters.AddWithValue("@photo", path);
                                    cmd.Parameters.AddWithValue("@salary", 0);
                                    cmd.Parameters.AddWithValue("@insertDate", today);
                                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text.Trim());

                                    cmd.ExecuteNonQuery();

                                    displayEmployeeData();

                                    MessageBox.Show("Employee Added Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    clearFields();
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

        private void grdAddEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridViewRow row = grdAddEmployee.Rows[e.RowIndex];
                txtEmpID.Text = row.Cells[1].Value.ToString();
                txtFullNames.Text = row.Cells[2].Value.ToString();
                cmbGender.Text = row.Cells[3].Value.ToString();
                cmbPosition.Text = row.Cells[4].Value.ToString();
                txtPhoneNum.Text = row.Cells[5].Value.ToString();
                txtAddress.Text = row.Cells[6].Value.ToString();

                string imagePath = row.Cells[7].Value.ToString();
                if(imagePath != null)
                {
                    pictureBox.Image = Image.FromFile(imagePath);
                }
                else
                {
                    pictureBox.Image = null;
                }
                              
                cmbStatus.Text = row.Cells[9].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {        

            if (txtEmpID.Text == "" || txtFullNames.Text == "" || cmbGender.Text == "" || cmbPosition.Text == "" || cmbStatus.Text == "" || txtPhoneNum.Text == "" || txtAddress.Text == "" || pictureBox.Image == null)
            {
                MessageBox.Show("Please fill all the required fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DateTime today = DateTime.Today;
                DialogResult check = MessageBox.Show("Are you sure you want to UPDATE Employee ID " + txtEmpID.Text.Trim() + " ? ", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
               
                if(check == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();

                        string updateData = "UPDATE Employees " +
                                            "SET fullNames = @fullNames, gender = @gender, position = @position, contactNum = @contactNum, phyAddress = @phyAddress, photo = @photo, updateDate = @updateDate, status = @status " +
                                            "WHERE empID = @empID";

                       string path = Path.Combine(@"C:\Users\moten\Documents\TUTORIALS\PROJECTS\EmpManager_Windows_Form\WindowsFormsApp\Directory\" + txtEmpID.Text.Trim() + ".jpg");

                        string directoryPath = Path.GetDirectoryName(path);

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        File.Copy(pictureBox.ImageLocation, path, true);

                        using (SqlCommand cmd = new SqlCommand(updateData, con))
                        {
                            cmd.Parameters.AddWithValue("@empID", txtEmpID.Text.Trim());
                            cmd.Parameters.AddWithValue("@fullNames", txtFullNames.Text.Trim());
                            cmd.Parameters.AddWithValue("@gender", cmbGender.Text.Trim());
                            cmd.Parameters.AddWithValue("@position", cmbPosition.Text.Trim());
                            cmd.Parameters.AddWithValue("@contactNum", txtPhoneNum.Text.Trim());
                            cmd.Parameters.AddWithValue("@phyAddress", txtAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@photo", path);                            
                            cmd.Parameters.AddWithValue("@updateDate", today);
                            cmd.Parameters.AddWithValue("@status", cmbStatus.Text.Trim());

                            cmd.ExecuteNonQuery();

                            displayEmployeeData();

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
                else
                {
                    MessageBox.Show("Cancelled. ", " Information Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtEmpID.Text == "" || txtFullNames.Text == "" || cmbGender.Text == "" || cmbPosition.Text == "" || cmbStatus.Text == "" || txtPhoneNum.Text == "" || txtAddress.Text == "" || pictureBox.Image == null)
            {
                MessageBox.Show("Please fill all the required fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DateTime today = DateTime.Today;
                DialogResult check = MessageBox.Show("Are you sure you want to DELETE Employee ID " + txtEmpID.Text.Trim() + " ? ", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();

                        string updateData = "UPDATE Employees " +
                                            "SET  deleteDate = @deleteDate " +
                                            "WHERE empID = @empID";                                               

                        using (SqlCommand cmd = new SqlCommand(updateData, con))
                        {
                            cmd.Parameters.AddWithValue("@empID", txtEmpID.Text.Trim());                         
                            cmd.Parameters.AddWithValue("@deleteDate", today);
                           
                            cmd.ExecuteNonQuery();

                            displayEmployeeData();

                            MessageBox.Show("Employee  Deleted Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                else
                {
                    MessageBox.Show("Cancelled. ", " Information Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}