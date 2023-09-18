using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PetShop{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            DisplayCustomer();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employees EmpObj = new Employees();
            EmpObj.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        SqlConnection Conn = new SqlConnection(@"Data Source=LAB1A\MSSQLSERVER1122;Initial Catalog=PetShopDb;Integrated Security=True");
        private void DisplayCustomer()
        {
            try
            {
                Conn.Open();
                string SelectSqlQuery = "select * from CustomerTbl";
                SqlDataAdapter Sda = new SqlDataAdapter(SelectSqlQuery, Conn);
                SqlCommandBuilder CmdBuilder = new SqlCommandBuilder(Sda);
                var Ds = new DataSet();
                Sda.Fill(Ds);
                CustomerDGV.DataSource = Ds.Tables[0];
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
            CusNameTb.Text = "";
            CusPhoneTb.Text = "";
            CusAddTb.Text = "";            
        }
        int key = 0;
        private void CusSaveBtn_Click(object sender, EventArgs e)
        {
            if (CusNameTb.Text == "" || CusPhoneTb.Text == "" || CusAddTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into CustomerTbl(CustName,CustAdd,CustPhone) values(@CN,@CA,@CP)", Conn);
                    cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CusAddTb.Text);                    
                    cmd.Parameters.AddWithValue("@CP", CusPhoneTb.Text);                    
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Added Successfully");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayCustomer();
                    ClearTb();
                }
            }

        }
        private void EmployeeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CusNameTb.Text = CustomerDGV.SelectedRows[0].Cells[1].Value.ToString();
            CusAddTb.Text = CustomerDGV.SelectedRows[0].Cells[2].Value.ToString();
            CusPhoneTb.Text = CustomerDGV.SelectedRows[0].Cells[3].Value.ToString();

            if (CusNameTb.Text != "")
            {
                key = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
                // MessageBox.Show("Employee EmpNum " + key.ToString() + " Selected");
            }
            else
            {
                key = 0;
            }

        }
        private void CusEditBtn_Click(object sender, EventArgs e)
        {
            if (CusNameTb.Text == "" || CusPhoneTb.Text == "" || CusAddTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTbl set CustName = @CN,CustAdd = @CA,CustPhone = @CP where CustId = @CKey", Conn);
                    cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CusAddTb.Text);                    
                    cmd.Parameters.AddWithValue("@CP", CusPhoneTb.Text);                    
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer updated Successfully where id = " + key.ToString());
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayCustomer();
                    ClearTb();
                }
            }
        }
        private void CusDelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select Customer");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTbl where CustId = @CKey", Conn);
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer id = " + key.ToString() + " has been deleted !");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayCustomer();
                    ClearTb();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products ProObj = new Products();
            ProObj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers CustObj = new Customers();
            CustObj.Show();
            this.Hide();
        }

        private void Customers_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Billings BillsObj = new Billings();
            BillsObj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
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