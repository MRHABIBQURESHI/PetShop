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

namespace PetShop
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        SqlConnection Conn = new SqlConnection(@"Data Source=LAB1A\MSSQLSERVER1122;Initial Catalog=PetShopDb;Integrated Security=True");

        private void LoginBtn_Click(object sender, EventArgs e)
        {

            if (UsernameTB.Text == "" || PasswordTB.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Conn.Open();
                    string enteredUsername = UsernameTB.Text.Trim();
                    string enteredPassword = PasswordTB.Text.Trim();

                    SqlCommand cmd = new SqlCommand("SELECT EmpPass FROM EmployeeTbl WHERE EmpUsername = @Username", Conn);
                    cmd.Parameters.AddWithValue("@Username", enteredUsername);

                    object passwordFromDBObj = cmd.ExecuteScalar();
                    if (passwordFromDBObj != null)
                    {
                        string passwordFromDB = passwordFromDBObj.ToString().Trim();

                        if (enteredPassword == passwordFromDB)
                        {
                            MessageBox.Show("Login successful");
                            Conn.Close();
                            Home Obj = new Home();
                            Obj.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                }
            }
        }

    }
}
