using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Data;

namespace PetShop
{
    public partial class Employees : Form
    {

        public Employees()
        {
            InitializeComponent();
            DisplayEmployees();
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Billings BillsObj = new Billings();
            BillsObj.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers CustObj = new Customers();
            CustObj.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products ProObj = new Products();
            ProObj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }        
        SqlConnection Conn = new SqlConnection(@"Data Source=LAB1A\MSSQLSERVER1122;Initial Catalog=PetShopDb;Integrated Security=True");
        private void DisplayEmployees()
        {
            try
            {
                Conn.Open();
                string SelectSqlQuery = "select * from EmployeeTbl";
                SqlDataAdapter Sda = new SqlDataAdapter(SelectSqlQuery, Conn);
                SqlCommandBuilder CmdBuilder = new SqlCommandBuilder(Sda);
                var Ds = new DataSet();
                Sda.Fill(Ds);
                EmployeeDGV.DataSource = Ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            { 
                Conn.Close();
            }                                   

        }
        private void ClearTb()
        {
            EmpNameTb.Text = ""; 
            EmpPhoneTb.Text = "";              
            EmpAddTb.Text = ""; 
            EmpUsernameTb.Text = "";
            EmpPasswordTb.Text = "";
        }
        int key = 0;

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if(EmpNameTb.Text == "" || EmpPhoneTb.Text == "" || EmpDOB.Text == "" || EmpAddTb.Text == "" || EmpUsernameTb.Text == "" || EmpPasswordTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into EmployeeTbl(EmpName,EmpAdd,EmpDOB,EmpPhone,EmpUsername,EmpPass) values(@EN,@EA,@ED,@EP,@EUn,@EPa)", Conn);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    cmd.Parameters.AddWithValue("@ED", EmpDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EUn", EmpUsernameTb.Text);        
                    cmd.Parameters.AddWithValue("@EPa", EmpPasswordTb.Text);        
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Added Successfully");
                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayEmployees();
                    ClearTb();
                }
            }
        }        
        private void EmployeeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EmpNameTb.Text = EmployeeDGV.SelectedRows[0].Cells[1].Value.ToString();
            EmpAddTb.Text = EmployeeDGV.SelectedRows[0].Cells[2].Value.ToString();
            EmpDOB.Text = EmployeeDGV.SelectedRows[0].Cells[3].Value.ToString();
            EmpPhoneTb.Text = EmployeeDGV.SelectedRows[0].Cells[4].Value.ToString();
            EmpUsernameTb.Text = EmployeeDGV.SelectedRows[0].Cells[5].Value.ToString();
            EmpPasswordTb.Text = EmployeeDGV.SelectedRows[0].Cells[6].Value.ToString();
            if(EmpNameTb.Text != "")
            {
                key = Convert.ToInt32(EmployeeDGV.SelectedRows[0].Cells[0].Value.ToString());
                // MessageBox.Show("Employee EmpNum " + key.ToString() + " Selected");
            }
            else
            {
                key = 0;
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpPhoneTb.Text == "" || EmpDOB.Text == "" || EmpAddTb.Text == "" || EmpUsernameTb.Text == "" || EmpPasswordTb.Text == "")
            {
                MessageBox.Show("Missing Information");                
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("update EmployeeTbl set EmpName = @EN,EmpAdd = @EA,EmpDOB = @ED,EmpPhone = @EP,EmpUsername = @EUn,EmpPass = @EPa where EmpNum = @EKey", Conn);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    cmd.Parameters.AddWithValue("@ED", EmpDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EUn", EmpUsernameTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", EmpPasswordTb.Text);
                    cmd.Parameters.AddWithValue("@EKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee updated Successfully where id = " + key.ToString());
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayEmployees();
                    ClearTb();
                }
            }

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select Employee");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from EmployeeTbl where EmpNum = @EKey", Conn);                    
                    cmd.Parameters.AddWithValue("@EKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee id = " + key.ToString() + " has been deleted !");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayEmployees();
                    ClearTb();
                }
            }

        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            Employees EmpObj = new Employees();
            EmpObj.Show();
            this.Hide();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }
    }
}
