using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pos
{
    public partial class Form1 : Form
    {

        public string SessionUser="";
        public string SessionPerm ="";

        List<string> pnamelist = new List<string>();
        List<string> qtylist = new List<string>();
        List<string> requestlist = new List<string>();
        List<string> totallist = new List<string>();
        string transno = "";
        string datetime = "";
        string payment_type = "";
        string payment = "";
        float fulltotal = 0;
        string change = "";
        float discount = 0;
        string user = "";
        string order_type = "";
        private TextBox focusedTextbox = null;
        private string Pname;
        private string Pprice;
        private float Pqty = 2;
        public bool Neworder = false;
        private PictureBox pic;
        private Label price;
        private Label name;
        private Button btnCategory, btnAll;
        private String Filter = "all";
        System.Windows.Forms.Timer t = null;


      

        public Form1()
        {
            StartTimer();
            InitializeComponent();
        }

        private void StartTimer()
        {
            t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }

        void t_Tick(object sender, EventArgs e)
        {
            time_lbl.Text = DateTime.Now.ToString();
        }

        public string passuser
        {
            get { return SessionUser; }
            set { SessionUser = value; }
        }
        public string passperm
        {
            get { return SessionPerm; }
            set { SessionPerm = value; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Cashier";
            lbluser.Text = ""+SessionPerm+" : "+SessionUser;
            lblServer.Text = SessionUser;
            itemlist1.Hide();
            GetData();
            ClearCart();
            LoadCategory();
            Loaditems();

            btnNewOrder.Enabled = true;
            btnCanselOrder.Enabled = false;
            btnNewOrder.Visible = true;
            btnCanselOrder.Visible = false;

            dataGridView1.Enabled = false;
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btn0.Enabled = false;
            btnDot.Enabled = false;
            btnBackspace.Enabled = false;
            btnClear.Enabled = false;
            btn20.Enabled = false;
            btn50.Enabled = false;
            btn100.Enabled = false;
            btn500.Enabled = false;
            btn1000.Enabled = false;
            btn5000.Enabled = false;
            btnSave.Enabled = false;

            textBoxCash.Enabled = false;
            textBoxCash.Text = "0";
            textBoxCash.MaxLength = 18;
            textBoxChange.Enabled = false;
            textBoxChange.Text = "0";
            textBoxDiscount.Enabled = false;
            textBoxDiscount.Text = "0";
            textBoxDiscount.MaxLength = 2;

            textBoxDue.Enabled = false;
            textBoxDue.Text = "0";
            textBoxTotal.Enabled = false;
            comboBoxPayment.Enabled = false;
            comboBoxToutDin.Enabled = false;

            comboBoxPayment.Items.Add("Cash");
            comboBoxPayment.Items.Add("Credit");
            comboBoxToutDin.Items.Add("-- Order --");
            comboBoxToutDin.Items.Add("Dine in");
            comboBoxToutDin.Items.Add("Take out");
            comboBoxToutDin.SelectedIndex = 0;
            comboBoxPayment.SelectedIndex = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        public void GetData()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {
                flowLayoutPanel1.Controls.Clear();
                con.Open();
                string Qry;
                if (Filter == "all")
                    Qry = "SELECT item_image,Id, item_name, item_price FROM items Where item_status='Available' and item_name LIKE '" + textBox1.Text + "%' order by item_category";
                else
                    Qry = "SELECT item_image,Id, item_name, item_price FROM items Where item_status='Available' and item_category='" + Filter + "' and item_name LIKE '" + textBox1.Text + "%' order by item_name";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    long len = dr.GetBytes(0, 0, null, 0, 0);
                    byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                    dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                    pic = new PictureBox();
                    pic.Width = 150;
                    pic.Height = 150;
                    pic.BackgroundImageLayout = ImageLayout.Stretch;
                    pic.BorderStyle = BorderStyle.FixedSingle;
                    pic.Margin = new Padding(10);

                    MemoryStream ms = new MemoryStream(array);
                    Bitmap bitmap = new Bitmap(ms);
                    pic.BackgroundImage = bitmap;
                    pic.Tag = dr["Id"].ToString();

                    //add price label 
                    price = new Label();
                    price.Text = dr["item_price"].ToString();
                    price.BackColor = Color.FromArgb(68, 189, 50);
                    price.ForeColor = Color.White;
                    price.Tag = dr["Id"].ToString();
                    price.Width = 60;
                    price.Height = 18;
                    price.TextAlign = ContentAlignment.MiddleCenter;
                    price.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                    //add name label  
                    name = new Label();
                    name.Text = dr["item_name"].ToString();
                    name.BackColor = Color.FromArgb(68, 189, 50);
                    name.ForeColor = Color.White;
                    name.Dock = DockStyle.Bottom;
                    price.Height = 20;
                    name.TextAlign = ContentAlignment.MiddleCenter;
                    name.Tag = dr["Id"].ToString();
                    name.Font= new Font("Segoe UI", 10, FontStyle.Bold);

                    flowLayoutPanel1.Controls.Add(pic);
                    pic.Controls.Add(name);
                    pic.Controls.Add(price);
                    pic.Click += new EventHandler(SelectClick);
                    price.Click += new EventHandler(SelectClick);
                    name.Click += new EventHandler(SelectClick);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                con.Close();
            }

        }


        public void SelectClick(Object sender, EventArgs e)
        {
            int id;
            if (Neworder)
            {
                if ((sender as System.Windows.Forms.PictureBox == null) && (sender as System.Windows.Forms.Label != null))
                {
                    dataGridView1.EndEdit();
                    id = int.Parse((sender as Label).Tag.ToString());
                    PassItems(id);
                }
                else if ((sender as System.Windows.Forms.PictureBox != null) && (sender as System.Windows.Forms.Label == null))
                {
                    dataGridView1.EndEdit();
                    id = int.Parse((sender as PictureBox).Tag.ToString());
                    PassItems(id);
                }
            }
            else
                MessageBox.Show("Please, Place an order before adding items.", "Place Order", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }




        public void Loaditems()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                int i = 0;
                con.Open();
                string Qry = "SELECT * FROM Cart";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    dataGridView1.RowTemplate.Height = 23;
                    i += 1;
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[1].Value = dr["Id"].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = dr["pname"].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = dr["price"].ToString();
                    dataGridView1.Rows[n].Cells[5].Value = dr["qty"].ToString();
                    dataGridView1.Rows[n].Cells[7].Value = dr["total"].ToString();
                    dataGridView1.Rows[n].Cells[8].Value = dr["request"].ToString().Replace(',', '\n');

                }
                dr.Close();
                dataGridView1.ClearSelection();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Loaditems()\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (CheckCartEmpty())
                {
                    textBoxTotal.Text = Total();
                    ChangeDue_Count();
                }

                else
                {
                    textBoxTotal.Text = "0";
                }

                con.Close();
            }
        }


        private bool CheckItemExists()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            bool exists = true;
            string Qry = "select count(*) from Cart where pname=@name ";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@name", Pname);
                exists = (int)cmd.ExecuteScalar() > 0;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }
            return exists;
        }

        private bool CheckCartEmpty()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            bool CartCleared = false;
            string Qry = "select count(*) from Cart";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                CartCleared = (int)cmd.ExecuteScalar() > 0;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }
            return CartCleared;
        }

        private void GetQty()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");

            try
            {
                con.Open();
                string Qry = "select qty from Cart where pname='" + Pname + "'";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Pqty = float.Parse(dr["qty"].ToString());
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {

                con.Close();
            }
        }


        private void PassItems(int x)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {
                con.Open();
                string Qry = "SELECT item_name,item_price FROM Items where Id=" + x + "";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Pname = dr["item_name"].ToString();
                    Pprice = dr["item_price"].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                con.Close();
            }

            if (CheckItemExists())
            {
                GetQty();

                string Qry = "UPDATE Cart SET qty = @qty ,total=@total WHERE pname = @name";
                SqlCommand cmd = new SqlCommand(Qry, con);
                try
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@name", Pname);
                    cmd.Parameters.AddWithValue("@total", (float.Parse(Pprice)) * (Pqty + 1));
                    cmd.Parameters.AddWithValue("@qty", Pqty + 1);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    con.Close();
                    Loaditems();
                }
            }
            else
            {
                string Qry = "insert into Cart(transno,pname,price,total,qty,request)values(@transno,@pname,@price,@total,@qty,@request)";
                SqlCommand cmd = new SqlCommand(Qry, con);
                try
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@transno", lblTransNo.Text);
                    cmd.Parameters.AddWithValue("@pname", Pname);
                    cmd.Parameters.AddWithValue("@price", Pprice);
                    cmd.Parameters.AddWithValue("@total", float.Parse(Pprice));
                    cmd.Parameters.AddWithValue("@qty", 1);
                    cmd.Parameters.AddWithValue("@request", "");
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    con.Close();
                    Loaditems();
                }
            }

        }

        private string Total()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");

            string Total = "0";
            string Qry = "select sum(total) from Cart";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                textBoxSubTotal.Text = cmd.ExecuteScalar().ToString();
                decimal Subtotal = decimal.Parse(cmd.ExecuteScalar().ToString());

                Total = ((Subtotal / 100) * (100 - decimal.Parse(textBoxDiscount.Text))).ToString();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Total()\n" + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }
            return Total;
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.EndEdit();
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string colname = dataGridView1.Columns[e.ColumnIndex].Name.ToString();

            if (colname == "Colmin")
            {

                Pname = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                GetQty();
                if (Pqty > 1)
                {
                    string Qry = "UPDATE Cart SET qty = @qty ,total=@total WHERE pname = @name";
                    SqlCommand cmd = new SqlCommand(Qry, con);
                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@name", dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        cmd.Parameters.AddWithValue("@total", (float.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString())) * (Pqty - 1));
                        cmd.Parameters.AddWithValue("@qty", Pqty - 1);
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    finally
                    {
                        con.Close();
                        Loaditems();
                        ChangeDue_Count();
                    }
                }

            }

            if (colname == "Colmax")
            {
                Pname = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                GetQty();
                string Qry = "UPDATE Cart SET qty = @qty ,total=@total WHERE pname = @name";
                SqlCommand cmd = new SqlCommand(Qry, con);
                try
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@name", dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    cmd.Parameters.AddWithValue("@total", (float.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString())) * (Pqty + 1));
                    cmd.Parameters.AddWithValue("@qty", Pqty + 1);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    con.Close();
                    Loaditems();
                    ChangeDue_Count();
                }
            }

            if (colname == "ColDel")
            {
                string Qry = "delete from Cart where Id=@Id";
                SqlCommand cmd = new SqlCommand(Qry, con);
                try
                {

                    con.Open();
                    cmd.Parameters.AddWithValue("@Id", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    con.Close();
                    Loaditems();

                }
            }




        }


        public void GetTransNo()
        {
            if (Neworder)
                lblTransNo.Text = DateTime.Now.ToString("yHHddMMmmss");
        }

        private void ClearCart()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string Qry = "delete from Cart";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            finally
            {
                con.Close();
                Loaditems();


            }
        }
        private void LoadCategory()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {   //all items button
                flowLayoutPanel2.Controls.Clear();
                btnAll = new Button();
                btnAll.Width = 90;
                btnAll.Height = 45;
                btnAll.Text = "All";
                btnAll.FlatStyle = FlatStyle.Popup;
                btnAll.BackColor = Color.FromArgb(192, 57, 43);
                btnAll.ForeColor = Color.White;
                btnAll.TextAlign = ContentAlignment.MiddleCenter;
                flowLayoutPanel2.Controls.Add(btnAll);
                btnAll.Click += new EventHandler(Filter_Click);
                btnAll.Margin = new Padding(8);
                btnAll.Font = new Font("Segoe UI", 10);

                con.Open();
                string Qry = "SELECT * FROM category";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //category filter buttons
                    btnCategory = new Button();
                    btnCategory.Width = 90;
                    btnCategory.Height = 45;
                    btnCategory.Text = dr["category"].ToString();
                    btnCategory.FlatStyle = FlatStyle.Popup;
                    btnCategory.BackColor = Color.FromArgb(104, 33, 122);
                    btnCategory.ForeColor = Color.White;
                    btnCategory.TextAlign = ContentAlignment.MiddleCenter;
                    btnCategory.Margin = new Padding(8);
                    btnCategory.Font = new Font("Segoe UI", 10);

                    flowLayoutPanel2.Controls.Add(btnCategory);

                    btnCategory.Click += new EventHandler(Filter_Click);


                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                con.Close();
            }
        }



        public void Filter_Click(Object sender, EventArgs e)
        {

            if ((sender as Button).Text.ToString() == "All")
            {
                Filter = "all";
                GetData();
            }
            else
            {
                Filter = (sender as Button).Text.ToString();
                GetData();
            }
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string colname = dataGridView1.Columns[e.ColumnIndex].Name.ToString();

            if (colname == "ColRequests")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[8].Value != null)
                {
                    if (!string.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString()))
                    {
                        string sentence = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();

                        string Qry = "UPDATE Cart SET request = @request WHERE Id=@Id";
                        SqlCommand cmd = new SqlCommand(Qry, con);
                        try
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@Id", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                            cmd.Parameters.AddWithValue("@request", sentence);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        finally
                        {
                            con.Close();
                            Loaditems();

                        }
                    }
                }


            }
        }

        private void Cleartransaction()
        {

            ClearCart();
            Neworder = false;
            lblTransNo.Text = "[ Place an order ]";
            btnNewOrder.Enabled = true;
            btnCanselOrder.Enabled = false;
            btnNewOrder.Visible = true;
            btnCanselOrder.Visible = false;
            dataGridView1.Enabled = false;
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btn0.Enabled = false;
            btnDot.Enabled = false;
            btnBackspace.Enabled = false;
            btnClear.Enabled = false;
            btn20.Enabled = false;
            btn50.Enabled = false;
            btn100.Enabled = false;
            btn500.Enabled = false;
            btn1000.Enabled = false;
            btn5000.Enabled = false;
            btnSave.Enabled = false;
            textBoxCash.Enabled = false;
            textBoxChange.Enabled = false;
            textBoxDiscount.Enabled = false;
            textBoxDue.Enabled = false;
            textBoxTotal.Enabled = false;
            comboBoxPayment.Enabled = false;
            comboBoxToutDin.Enabled = false;
            textBoxDue.Text = "0";
            textBoxCash.Text = "0";
            textBoxChange.Text = "0";
            textBoxDiscount.Text = "0";

        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            if (!Neworder)
            {
                Neworder = true;
                GetTransNo();
                btnNewOrder.Enabled = false;
                btnCanselOrder.Enabled = true;
                btnNewOrder.Visible = false;
                btnCanselOrder.Visible = true;
                dataGridView1.Enabled = true;
                btn1.Enabled = true;
                btn2.Enabled = true;
                btn3.Enabled = true;
                btn4.Enabled = true;
                btn5.Enabled = true;
                btn6.Enabled = true;
                btn7.Enabled = true;
                btn8.Enabled = true;
                btn9.Enabled = true;
                btn0.Enabled = true;
                btnDot.Enabled = true;
                btnBackspace.Enabled = true;
                btnClear.Enabled = true;
                btn20.Enabled = true;
                btn50.Enabled = true;
                btn100.Enabled = true;
                btn500.Enabled = true;
                btn1000.Enabled = true;
                btn5000.Enabled = true;
                btnSave.Enabled = true;
                textBoxCash.Enabled = true;
                textBoxChange.Enabled = true;
                textBoxDiscount.Enabled = true;
                textBoxDue.Enabled = true;
                textBoxTotal.Enabled = true;
                comboBoxPayment.SelectedIndex = 0;
                comboBoxToutDin.SelectedIndex = 0;
                comboBoxPayment.Enabled = true;
                comboBoxToutDin.Enabled = true;
                textBoxDue.Text = "0";
                textBoxCash.Text = "0";
                textBoxChange.Text = "0";
                textBoxDiscount.Text = "0";
            }

        }
        private void btnCanselOrder_Click(object sender, EventArgs e)
        {
            if (Neworder)
            {
                var r = MessageBox.Show("You are about to cancel an order. Are you sure ? ", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (r == DialogResult.Yes)
                {
                    Cleartransaction();
                    MessageBox.Show("Order has been Canceled", "Cancel Order", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }

        // Numeric keypad functions
        //----------------------------------------------------------------------------------------------------
        private void textBoxCash_Enter(object sender, EventArgs e)
        {

            focusedTextbox = (TextBox)sender;
        }
        private void textBoxDiscount_Click(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("1");
            }

        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("2");
            }

        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("3");
            }

        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("4");
            }

        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("5");
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("6");
            }

        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("7");
            }

        }

        private void btn8_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("8");
            }

        }

        private void btn9_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("9");
            }
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send(".");
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("0");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                focusedTextbox.Clear();
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                SendKeys.Send("{BS}");
            }
        }
        private void btn20_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                if ((focusedTextbox.Text.Length < 1) || (focusedTextbox.Text == "0"))
                {
                    focusedTextbox.Text = "20";
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();

                }
                else
                {
                    focusedTextbox.Text = (float.Parse(focusedTextbox.Text) + 20).ToString();
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();
                }
            }
        }
        private void btn50_Click(object sender, EventArgs e)
        {
            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                if ((focusedTextbox.Text.Length < 1) || (focusedTextbox.Text == "0"))
                {
                    focusedTextbox.Text = "50";
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();

                }
                else
                {
                    focusedTextbox.Text = (float.Parse(focusedTextbox.Text) + 50).ToString();
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();
                }
            }
        }

        private void btn100_Click(object sender, EventArgs e)
        {

            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                if ((focusedTextbox.Text.Length < 1) || (focusedTextbox.Text == "0"))
                {
                    focusedTextbox.Text = "100";
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();

                }
                else
                {
                    focusedTextbox.Text = (float.Parse(focusedTextbox.Text) + 100).ToString();
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();
                }
            }
        }

        private void btn500_Click(object sender, EventArgs e)
        {

            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                if ((focusedTextbox.Text.Length < 1) || (focusedTextbox.Text == "0"))
                {
                    focusedTextbox.Text = "500";
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();

                }
                else
                {
                    focusedTextbox.Text = (float.Parse(focusedTextbox.Text) + 500).ToString();
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();
                }
            }
        }

        private void btn1000_Click(object sender, EventArgs e)
        {

            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                if ((focusedTextbox.Text.Length < 1) || (focusedTextbox.Text == "0"))
                {
                    focusedTextbox.Text = "1000";
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();

                }
                else
                {
                    focusedTextbox.Text = (float.Parse(focusedTextbox.Text) + 1000).ToString();
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();
                }
            }
        }

        private void btn5000_Click(object sender, EventArgs e)
        {

            if (!(focusedTextbox == null))
            {
                focusedTextbox.Focus();
                if ((focusedTextbox.Text.Length < 1) || (focusedTextbox.Text == "0"))
                {
                    focusedTextbox.Text = "5000";
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();

                }
                else
                {
                    focusedTextbox.Text = (float.Parse(focusedTextbox.Text) + 5000).ToString();
                    focusedTextbox.Select(focusedTextbox.Text.Length, 0);
                    focusedTextbox.ScrollToCaret();
                }
            }
        }


        private void textBoxCash_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCash.Text.Trim()))
            {
                textBoxCash.Text = "0";

            }
            else
            {
                ChangeDue_Count();
            }

        }

        private void textBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDiscount.Text.Length > 2)
                textBoxDiscount.Text = "0";

            if (string.IsNullOrWhiteSpace(textBoxDiscount.Text.Trim()))
            {
                textBoxDiscount.Text = "0";
            }
            else
            {
                if (CheckCartEmpty())
                    textBoxTotal.Text = Total();
                else
                    textBoxTotal.Text = "0";
            }
            ChangeDue_Count();
        }

        private void textBoxDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }


        }

        private void textBoxCash_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        //----------------------------------------------------------------------------------------------------








        private void ChangeDue_Count()
        {
            float dif = float.Parse(textBoxCash.Text) - float.Parse(textBoxTotal.Text);

            if (dif < 0)
            {
                textBoxDue.Text = Math.Abs(dif).ToString("0.00");
                textBoxChange.Text = "0";
            }

            else
            {
                textBoxDue.Text = "0";
                textBoxChange.Text = dif.ToString("0.00");
            }

        }

        private void Savebill()
        {

            if (CheckCartEmpty())
            {
                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");

                try
                {
                    string Qry = "insert into BillItems (transno,pname,qty,request,total) select transno,pname,qty,request,total from Cart";
                    SqlCommand cmd = new SqlCommand(Qry, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    con.Close();
                }



                try
                {
                    string Qry = "insert into Bill (transno,datetime,payment,payment_type,fulltotal,change,discount,[user],order_type) values(@transno,@datetime,@payment,@payment_type,@fulltotal,@change,@discount,@user,@order_type)";
                    SqlCommand cmd = new SqlCommand(Qry, con);
                    con.Open();

                    cmd.Parameters.AddWithValue("@transno", lblTransNo.Text);
                    cmd.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    cmd.Parameters.AddWithValue("@fulltotal", decimal.Parse(textBoxTotal.Text));
                    cmd.Parameters.AddWithValue("@payment_type", comboBoxPayment.Text);
                    cmd.Parameters.AddWithValue("@discount", decimal.Parse(textBoxDiscount.Text));
                    cmd.Parameters.AddWithValue("@order_type", comboBoxToutDin.Text);
                    cmd.Parameters.AddWithValue("@user", SessionUser);

                    if (comboBoxPayment.Text == "Cash")
                    {
                        cmd.Parameters.AddWithValue("@payment", decimal.Parse(textBoxCash.Text));
                        cmd.Parameters.AddWithValue("@change", decimal.Parse(textBoxChange.Text));

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@payment", decimal.Parse(textBoxTotal.Text));
                        cmd.Parameters.AddWithValue("@change", 0);
                    }
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    con.Close();
                }

                MessageBox.Show("Transaction successfull.", "Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Cart is empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (comboBoxToutDin.SelectedIndex == 0)
            {
                MessageBox.Show("Transaction could not be proceed.\n(Order type not set)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (comboBoxPayment.Text == "Cash" && ((string.IsNullOrWhiteSpace(textBoxCash.Text.Trim()) || textBoxCash.Text == "0") || float.Parse(textBoxDue.Text) > 0))
                {
                    MessageBox.Show("Transaction could not be proceed.\n(Insufficent cash amount)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Savebill();
                    printbill();
                    Cleartransaction();
                }

            }
        }


        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {


            Graphics graphics = e.Graphics;
            Font font9 = new Font("Courier New", 9);
            Font font10 = new Font("Courier New", 10);
            Font font11 = new Font("Courier New", 11);
            Font font14 = new Font("Courier New", 14);
            Font font11b = new Font("Courier New", 11, FontStyle.Bold);
            Font font14b = new Font("Courier New", 14, FontStyle.Bold);

            float leading = 3;
            float lineheight9 = font9.GetHeight() + leading;
            float lineheight10 = font10.GetHeight() + leading;
            float lineheight11 = font11.GetHeight() + leading;
            float lineheight14 = font14.GetHeight() + leading;

            float startX = 16;
            float startY = leading;
            float Offset = 0;

            StringFormat formatLeft = new StringFormat(StringFormatFlags.NoClip);
            StringFormat formatCenter = new StringFormat(formatLeft);
            StringFormat formatRight = new StringFormat(formatLeft);

            formatCenter.Alignment = StringAlignment.Center;
            formatRight.Alignment = StringAlignment.Far;
            formatLeft.Alignment = StringAlignment.Near;

            SizeF layoutSize = new SizeF(400 - Offset * 2, lineheight14);
            SizeF layoutSizeLine = new SizeF(386 - Offset * 2, lineheight14);
            SizeF layoutSizeString = new SizeF(370 - Offset * 2, lineheight14);
            SizeF layoutSizeCalcu = new SizeF(320 - Offset * 2, lineheight14);
            RectangleF layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);

            Brush brush = Brushes.Black;
            layout = new RectangleF(new PointF(startX, startY + 1), layoutSizeString);
            graphics.DrawString("Dine Fine", font14b, brush, layout, formatCenter);
            Offset = Offset + lineheight14 + 6;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("No 12, High-Level Road,", font10, brush, layout, formatCenter);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Homagama, Colombo, Sri Lanka. ", font10, brush, layout, formatCenter);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Tel: 011 7575888 ", font10, brush, layout, formatCenter);
            Offset = Offset + lineheight10 - 2;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Date: " + datetime, font10, brush, layout, formatLeft);
            graphics.DrawString("*PAID*", font14b, brush, layout, formatRight);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Bill No: " + transno, font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Payment: " + payment_type, font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Server: " + user, font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10 - 2;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight10 - 4;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("~" + order_type + "~", font11b, brush, layout, formatCenter);
            Offset = Offset + lineheight10 - 4;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            int i = 0;
            foreach (string a in pnamelist)
            {
                graphics.DrawString("" + qtylist[i] + " x " + a, font11, brush, layout, formatLeft);
                graphics.DrawString("" + totallist[i] + "   ", font11, brush, layout, formatRight);
                if (!string.IsNullOrWhiteSpace(requestlist[i]))
                {
                    string[] req = requestlist[i].Split(',');
                    foreach (string r in req)
                    {
                        Offset = Offset + lineheight9 - 2;
                        layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
                        graphics.DrawString("         -" + r, font10, brush, layout, formatLeft);
                    }
                }

                Offset = Offset + lineheight11 + 2;
                layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
                i += 1;
            }
            Offset = Offset + lineheight10 - 10;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '-'), font11, brush, layout, formatCenter);

            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Sub Total", font11, brush, layout, formatLeft);
            graphics.DrawString(((fulltotal / (100 - discount)) * 100).ToString("0.00"), font11, brush, layout, formatRight);
            Offset = Offset + lineheight11;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Discount: " + discount + "%", font11, brush, layout, formatLeft);
            graphics.DrawString("-" + ((fulltotal / (100 - discount)) * discount).ToString("0.00"), font11, brush, layout, formatRight);

            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Total", font11b, brush, layout, formatLeft);
            graphics.DrawString(fulltotal.ToString("0.00"), font11b, brush, layout, formatRight);
            Offset = Offset + lineheight10 - 4;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(25, '-'), font11, brush, layout, formatRight);
            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Paid Amount", font11, brush, layout, formatLeft);
            graphics.DrawString(payment, font11, brush, layout, formatRight);
            Offset = Offset + lineheight11;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Change", font11, brush, layout, formatLeft);
            graphics.DrawString(change, font11, brush, layout, formatRight);
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            Offset = Offset + lineheight10 + 10;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight10 + 10;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("Thank You! Come Again.", font11b, brush, layout, formatCenter);
            
            font10.Dispose(); font11.Dispose(); font14.Dispose();


            pnamelist.Clear();
            qtylist.Clear();
            requestlist.Clear();
            totallist.Clear();
        }



        private void printbill()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");

            try
            {
                con.Open();
                string Qry = "SELECT B.transno AS transno,B.datetime AS datetime,B.payment_type AS payment_type,B.payment AS payment,B.fulltotal AS fulltotal,B.change AS change,B.discount AS discount,B.[user] AS [user],B.order_type AS order_type,BI.pname AS pname,BI.qty,BI.request AS request,BI.total AS total FROM Bill B INNER JOIN BillItems BI On B.transno=BI.transno where B.transno='" + lblTransNo.Text + "'";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    pnamelist.Add(dr["pname"].ToString());
                    qtylist.Add(dr["qty"].ToString());
                    requestlist.Add(dr["request"].ToString());
                    totallist.Add(dr["total"].ToString());
                    transno = dr["transno"].ToString();
                    datetime = dr["datetime"].ToString();
                    payment_type = dr["payment_type"].ToString();
                    payment = dr["payment"].ToString();
                    fulltotal = float.Parse(dr["fulltotal"].ToString());
                    change = dr["change"].ToString();
                    discount = float.Parse(dr["discount"].ToString());
                    user = dr["user"].ToString();
                    order_type = dr["order_type"].ToString();



                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
            int i = 0;
            int x = 480;

            foreach (string a in pnamelist)
            {
                x += 20;
                if (!string.IsNullOrWhiteSpace(requestlist[i]))
                {
                    string[] req = requestlist[i].Split(',');
                    foreach (string r in req)
                    {
                        x += 17;
                    }
                }

            }

            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("bill", 400, x);
            printPreviewDialog1.PrintPreviewControl.AutoZoom = false;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            printPreviewDialog1.PrintPreviewControl.Width = 450;
            printPreviewDialog1.PrintPreviewControl.Height = 600;
            printPreviewDialog1.ShowDialog();


        }

 


      

        private void lblSaleReg_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Cashier";
            invoices1.Hide();
            report1.Hide();
            itemlist1.Hide();
            users1.Hide();
            GetData();
            LoadCategory();
        }

        private void lblItems_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Products";
            itemlist1.Loaditems();
            itemlist1.BringToFront();
            itemlist1.Show();
            invoices1.Hide();
            users1.Hide();
            report1.Hide();
        }

        private void lblInvoices_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Invoices";
            invoices1.LoadInvoices();
            invoices1.BringToFront();
            invoices1.Show();
            itemlist1.Hide();
            users1.Hide();
            report1.Hide();
        }

        private void lblSales_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Reports";
            report1.Loaddata();
            report1.BringToFront();
            report1.Show();
            invoices1.Hide();
            users1.Hide();
            itemlist1.Hide();
        }

        private void lblUsers_Click(object sender, EventArgs e)
        {
            if (SessionPerm == "Admin")
            {
                lblTitle.Text = "Users";
                users1.GetData();
                users1.BringToFront();
                users1.Show();
                invoices1.Hide();
                itemlist1.Hide();
                report1.Hide();
            }
            else
            {
                MessageBox.Show("You dont have administrator permission to enter.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lblLogout_Click(object sender, EventArgs e)
        {
          var r=  MessageBox.Show("Are you sure want logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
          if (r == DialogResult.Yes)
                this.Dispose();
        }


        //user control's  methods calling function
        public void LoadWhenAdd()
        {
            GetData();
            itemlist1.LoadCategory();
            itemlist1.Loaditems();
            users1.GetData();
            invoices1.LoadInvoices();
        }

    }

}




