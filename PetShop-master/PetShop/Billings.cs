using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PetShop
{
    public partial class Billings : Form
    {
        public Billings()
        {
            InitializeComponent();
            DisplayProducts();
            DisplayTransactions();
            GetCustomer();
            GetCustName();
        }
        SqlConnection Conn = new SqlConnection(@"Data Source=LAB1A\MSSQLSERVER1122;Initial Catalog=PetShopDb;Integrated Security=True");
        int Key = 0, Stock = 0;
        private void GetCustomer()
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("Select CustId From CustomerTbl", Conn);
                SqlDataReader Rdr;
                Rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("CustId", typeof(int));
                dt.Load(Rdr);
                CusIDCb.ValueMember = "CustId";
                CusIDCb.DataSource = dt;

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
        private void GetCustName()
        {
            try
            {
                string Query = "Select * From CustomerTbl Where CustId ='" + CusIDCb.SelectedValue.ToString() + "'";
                Conn.Open();
                SqlCommand cmd = new SqlCommand(Query, Conn);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    CusNameTb.Text = dr["CustName"].ToString();
                }
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
        private void DisplayTransactions()
        {
            try
            {
                Conn.Open();
                string SelectSqlQuery = "select * from BillTbl";
                SqlDataAdapter Sda = new SqlDataAdapter(SelectSqlQuery, Conn);
                SqlCommandBuilder CmdBuilder = new SqlCommandBuilder(Sda);
                var Ds = new DataSet();
                Sda.Fill(Ds);
                TransactionsDGV.DataSource = Ds.Tables[0];
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
        private void UpdateStock()
        {
            try
            {
                int NewQty = Stock - Convert.ToInt32(QtyTb.Text);
                Conn.Open();
                SqlCommand cmd = new SqlCommand("update ProductTbl set PrQty=@PQ where PrId=@PKey", Conn);
                cmd.Parameters.AddWithValue("@PKey", Key);
                cmd.Parameters.AddWithValue("@PQ", NewQty);
                cmd.ExecuteNonQuery();
                Conn.Close();
                DisplayProducts();
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
        private void Reset()
        {
            PrNameTb.Text = "";
            PrPriceTb.Text = "";
            QtyTb.Text = "";
            Stock = 0;
            Key = 0;
        }
        int n = 0, GrdTotal = 0;
        private void AddToBillBtn_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > Stock)
            {
                MessageBox.Show("No Enough in Stock");
            }
            else if (QtyTb.Text == "" || Key == 0)
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PrPriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = PrNameTb.Text;
                newRow.Cells[2].Value = QtyTb.Text;
                newRow.Cells[3].Value = PrPriceTb.Text;
                newRow.Cells[4].Value = total;

                BillDGV.Rows.Add(newRow);
                n++;
                GrdTotal = GrdTotal + total;
                TotalAmountLbl.Text = "PKR" + GrdTotal;
                UpdateStock();
                Reset();
            }
        }

        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProductsDGV.SelectedRows.Count > 0) // Check if any rows are selected
            {
                PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
                Stock = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[3].Value.ToString());
                QtyTb.Text = Stock.ToString();
                PrPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
                if (PrNameTb.Text == "")
                {
                    Key = 0;
                }
                else
                {
                    Key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
                }
            }
        }
        private void CusIDCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CusIDCb.SelectedItem != null)
            {
                GetCustName();
            }
        }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products ProObj = new Products();
            ProObj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employees EmpObj = new Employees();
            EmpObj.Show();
            this.Hide();
        }

        private void BillDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TotalAmountLbl_Click(object sender, EventArgs e)
        {

        }
        private void InsertBill()
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("insert into BillTbl(BDate,CustID,CustName,EmpName,Amt) values(@BD,@CI,@CN,@EN,@AM)", Conn);
                cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                cmd.Parameters.AddWithValue("@CI", CusIDCb.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CN", CusNameTb.Text);
                cmd.Parameters.AddWithValue("@EN", EmpNameLbl.Text);
                cmd.Parameters.AddWithValue("@AM", GrdTotal);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Bill Created Successfully");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                DisplayTransactions();
                Conn.Close();
            }
        }

        string prodName;
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            InsertBill();
            DisplayTransactions();

            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 458, 600);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        int prodid, prodqty, prodprice, tottal, pos = 60;

        private void EmpNameLbl_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Pet Shop", new Font("Centuary Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(80));
            e.Graphics.DrawString("ID PRODUCT PRICE QUANTITY TOTAL", new Font("Centuary Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["column1"].Value);
                prodName = "" + row.Cells["column2"].Value;
                prodqty = Convert.ToInt32(row.Cells["column3"].Value);
                prodprice = Convert.ToInt32(row.Cells["column4"].Value);
                tottal = Convert.ToInt32(row.Cells["column5"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Centuary Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(26, pos));
                e.Graphics.DrawString("" + prodName, new Font("Centuary Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(45, pos));
                e.Graphics.DrawString("" + prodqty, new Font("Centuary Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(120, pos));
                e.Graphics.DrawString("" + prodprice, new Font("Centuary Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(170, pos));
                e.Graphics.DrawString("" + tottal, new Font("Centuary Gothic", 8, FontStyle.Regular), Brushes.Blue, new Point(235, pos));
                pos = pos + 20;
            }
            e.Graphics.DrawString("Grand Total: PKR " + GrdTotal, new Font("Centuary Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(50, pos + 50));
            e.Graphics.DrawString("*********************PetShop**********************", new Font("Centuary Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(10, pos + 85));
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            n = 0;
            GrdTotal = 0;

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Billings BillsObj = new Billings();
            BillsObj.Show();
            this.Hide();
        }
        private void label5_Click(object sender, EventArgs e)
        {
            Customers CusObj = new Customers();
            CusObj.Show();
            this.Hide();
        }
    }
}