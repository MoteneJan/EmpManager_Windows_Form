using System; 
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp
{
    public partial class Dashboard : UserControl
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True");

        public Dashboard()
        {
            InitializeComponent();

            displayTE();
            displayAE();
            displayIE();
        }

        public void RefreshData()
        {
            if(InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;                
            }
            displayTE();
            displayAE();
            displayIE();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        //COUNT TOTAL NUMBER OF EMPLOYEES
        public void displayTE()
        {
            if (con.State != ConnectionState.Open)
            {
                try
                {
                    con.Open();

                    string selectData = "SELECT COUNT(id) FROM Employees WHERE deleteDate IS NULL";
                    using (SqlCommand cmd = new SqlCommand(selectData, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if(reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            lblDashboard_TE.Text = count.ToString();
                        }
                        reader.Close();
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

        //COUNT TOTAL NUMBER OF ACTIVE EMPLOYEES
        public void displayAE()
        {
            if (con.State != ConnectionState.Open)
            {
                try
                {
                    con.Open();

                    string selectData = "SELECT COUNT(id) FROM Employees WHERE status = @status AND deleteDate IS NULL";
                    using (SqlCommand cmd = new SqlCommand(selectData, con))
                    {
                        cmd.Parameters.AddWithValue("@status", "Active");
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            lblDashboard_AE.Text = count.ToString();
                        }
                        reader.Close();
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

        //COUNT TOTAL NUMBER OF INACTIVE EMPLOYEES
        public void displayIE()
        {
            if (con.State != ConnectionState.Open)
            {
                try
                {
                    con.Open();

                    string selectData = "SELECT COUNT(id) FROM Employees WHERE status = @status AND deleteDate IS NULL";
                    using (SqlCommand cmd = new SqlCommand(selectData, con))
                    {
                        cmd.Parameters.AddWithValue("@status", "Inactive");
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            lblDashboard_IE.Text = count.ToString();
                        }
                        reader.Close();
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
}
