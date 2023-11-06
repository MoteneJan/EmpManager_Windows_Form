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

namespace WindowsFormsApp
{
    public partial class RegistrationForm : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True");
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void chkPassword_CheckedChanged(object sender, EventArgs e)
        {
            //chkPassword.PasswordChar = chkPassword.Checked ? '\0': '*';
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            if(txtSignUpUsername.Text.Length < 0 || txtSignUpPassword.Text.Length < 0)
            {
                MessageBox.Show("Please fill all the required fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(con.State != ConnectionState.Open)
                {
                    try
                    {
                        con.Open();

                        //Checks if the user already exist
                        string selectUsername = "SELECT COUNT(id) FROM users WHERE username = @user";

                        using(SqlCommand checkUser = new SqlCommand(selectUsername, con))
                        {
                            checkUser.Parameters.AddWithValue("@user", txtSignUpUsername.Text.Trim());
                            int count = (int)checkUser.ExecuteScalar();

                            if(count >= 1)
                            {
                                MessageBox.Show(txtSignUpUsername.Text.Trim() + " is already registered", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;
                                string query = "INSERT INTO Users (username, password, dateRegistered) VALUES(@username, @password, @dateRegistered)";

                                using (SqlCommand cmd = new SqlCommand(query, con))
                                {
                                    cmd.Parameters.AddWithValue("@username", txtSignUpUsername.Text.Trim());
                                    cmd.Parameters.AddWithValue("@password", txtSignUpPassword.Text.Trim());
                                    cmd.Parameters.AddWithValue("@dateRegistered", today);

                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Successfully Registered!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    LoginForm loginForm = new LoginForm();
                                    loginForm.Show();
                                    this.Hide();
                                }
                            }
                        }

                                    
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error " + ex , " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

    }
}
