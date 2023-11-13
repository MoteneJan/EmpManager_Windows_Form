using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp
{
    internal class EmployeeData
    {
        public int ID { set; get; }
        public string EmployeeID { set; get; }
        public string Name { set; get; }
        public string Gender { set; get; }
        public string Position { set; get; }
        public string Contact { set; get; }
        public string Address { set; get; }
        public string Photo { set; get; }
        public int Salary { set; get; }     
        public string Status { set; get; }

        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True");

        //EMPLOYEE
        public List<EmployeeData> employeeListData()
        {
            List<EmployeeData> listdata = new List<EmployeeData>();
            if(con.State != ConnectionState.Open)
            {
                try
                {
                    con.Open();

                    string selectData = "SELECT * FROM Employees WHERE deleteDate IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while(reader.Read())
                        {
                            EmployeeData empData = new EmployeeData();
                            empData.ID = (int)reader["id"];
                            empData.EmployeeID = reader["empID"].ToString();
                            empData.Name = reader["fullNames"].ToString();
                            empData.Gender = reader["gender"].ToString();
                            empData.Position = reader["position"].ToString();
                            empData.Contact = reader["contactNum"].ToString();
                            empData.Address = reader["phyAddress"].ToString();
                            empData.Photo = reader["photo"].ToString();
                            empData.Salary = (int)reader["salary"];
                            empData.Status = reader["Status"].ToString();

                            listdata.Add(empData);
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
