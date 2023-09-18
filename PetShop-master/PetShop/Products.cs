using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

namespace PetShop
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            DisplayProducts();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employees EmpObj = new Employees();
            EmpObj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers CusObj = new Customers();
            CusObj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products ProObj = new Products();
            ProObj.Show();
            this.Hide();
        }
        SqlConnection Conn = new SqlConnection(@"Data Source=LAB1A\MSSQLSERVER1122;Initial Catalog=PetShopDb;Integrated Security=True");
        private void DisplayProducts()
        {
            try
            {
                Conn.Open();
                string SelectSqlQuery = "select * from ProductTbl";
                SqlDataAdapter Sda = new SqlDataAdapter(SelectSqlQuery, Conn);
                SqlCommandBuilder CmdBuilder = new SqlCommandBuilder(Sda);
                var Ds = new DataSet();
                Sda.Fill(Ds);
                ProductsDGV.DataSource = Ds.Tables[0];
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
            ProNameTb.Text = "";
            ProQtyTb.Text = "";
            ProPriceTb.Text = "";
            ProCategoriesCb.SelectedIndex = 0;
            //ProCategoriesCb.ResetText() ;
        }
        private void ProSaveBtn_Click(object sender, EventArgs e)
        {
            if (ProNameTb.Text == "" || ProQtyTb.Text == "" || ProPriceTb.Text == "" || ProCategoriesCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into ProductTbl(PrName,PrQty,PrPrice,PrCat) values(@PN,@PQ,@PP,@PC)", Conn);
                    cmd.Parameters.AddWithValue("@PN", ProNameTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", ProQtyTb.Text);
                    cmd.Parameters.AddWithValue("@PP", ProPriceTb.Text);
                    cmd.Parameters.AddWithValue("@PC",ProCategoriesCb.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Added Successfully");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayProducts();
                    ClearTb();
                }
            }
        }

        int key = 0;

        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            ProCategoriesCb.Text = ProductsDGV.Rows[0].Cells[2].Value.ToString();
            ProQtyTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString();
            ProPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();

            if (ProNameTb.Text != "")
            {
                key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
                // MessageBox.Show("Employee EmpNum " + key.ToString() + " Selected");
            }
            else
            {
                key = 0;
            }
        }

        private void ProEditBtn_Click(object sender, EventArgs e)
        {
            if (ProNameTb.Text == "" || ProQtyTb.Text == "" || ProPriceTb.Text == "" || ProCategoriesCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("update ProductTbl set PrName = @PN,PrQty = @PQ,PrPrice = @PP,PrCat=@PC where PrId = @CKey", Conn);
                    cmd.Parameters.AddWithValue("@PN", ProNameTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", ProQtyTb.Text);
                    cmd.Parameters.AddWithValue("@PP", ProPriceTb.Text);
                    cmd.Parameters.AddWithValue("@PC", ProCategoriesCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product updated Successfully where id = " + key.ToString());
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayProducts();
                    ClearTb();
                }
            }
        }

        private void ProDelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select Product");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from ProductTbl where PrId = @CKey", Conn);
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product id = " + key.ToString() + " has been deleted !");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Conn.Close();
                    DisplayProducts();
                    ClearTb();
                }
            }
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
