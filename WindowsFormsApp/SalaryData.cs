using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp
{
    internal class SalaryData
    {
      
        public string EmployeeID { set; get; }
        public string Name { set; get; }
        public string Gender { set; get; }
        public string Contact { set; get; }
        public string Position { set; get; }      
        public int Salary { set; get; }
       
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True");

        //SALARY
        public List<SalaryData> salaryEmployeeListData()
        {
            List<SalaryData> listdata = new List<SalaryData>();
            if (con.State != ConnectionState.Open)
            {
                try
                {
                    con.Open();

                    string selectData = "SELECT * FROM Employees WHERE status = 'Active' AND deleteDate IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            SalaryData salaryData = new SalaryData();
                            salaryData.EmployeeID = reader["empID"].ToString();
                            salaryData.Name = reader["fullNames"].ToString();
                            salaryData.Gender = reader["gender"].ToString();
                            salaryData.Position = reader["position"].ToString();
                            salaryData.Contact = reader["contactNum"].ToString();                           
                            salaryData.Salary = (int)reader["salary"];


                            listdata.Add(salaryData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
                finally
                {
                    con.Close();
                }
            }
            return listdata;
        }
    }
}
