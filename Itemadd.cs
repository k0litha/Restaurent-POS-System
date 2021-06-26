using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;



namespace pos
{

    public partial class Itemadd : Form
    {
        public string name;
        public string price;
        public string cat;
        public string st;
        public string id;
        public bool updateON;




        public Itemadd()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "image files (*.jpg)|*.jpg|(*.png)|*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackgroundImage = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        public void SaveEnabled()
        {
            btnSave.Enabled = true;
            btnSave.Focus();
            btnUpdate.Enabled = false;
            btnUpdate.Visible = false;
            updateON = false;
        }
        public void UpdateEnabled()
        {
            btnUpdate.Enabled = true;
            btnUpdate.Focus();
            btnSave.Enabled = false;
            btnSave.Visible = false;
            updateON = true;
        }
        public void LoadCategory()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {
                comboBoxCat.Items.Clear();
                comboBoxCat.Items.Add("-- Select Category --");
                comboBoxCat.SelectedIndex = 0;
                comboBoxSt.SelectedIndex = 0;
                con.Open();
                string Qry = "select * from category";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBoxCat.Items.Add(dr["category"].ToString());
                }
                dr.Close();

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




        private bool CheckItemExists()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            bool exists = false;
            string Qry;

            if (updateON)
                Qry = "select count(*) from items where item_name=@name and not Id='" + id + "'";
            else
                Qry = "select count(*) from items where item_name=@name ";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@name", textBoxName.Text);
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




        private void SaveItem()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string Qry = "insert into items(item_name,item_price,item_category,item_status,item_image)values(@name,@price,@category,@status,@image)";
            SqlCommand cmd = new SqlCommand(Qry, con);
            var r = MessageBox.Show("Are you sure want to save?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            try
            {
                if (r == DialogResult.Yes)
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arrImage = ms.ToArray();

                    con.Open();

                    cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@category", comboBoxCat.Text);
                    cmd.Parameters.AddWithValue("@price", float.Parse(textBoxPrice.Text));
                    cmd.Parameters.AddWithValue("@status", comboBoxSt.Text);
                    cmd.Parameters.AddWithValue("@image", arrImage);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Saved Successfully", "Confirm Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    pictureBox1.BackgroundImage = null;
                    textBoxName.Clear();
                    comboBoxCat.Items.Clear();
                    textBoxPrice.Clear();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            finally
            {
                con.Close();
                if (r == DialogResult.Yes)
                    LoadCategory();
            }
        }


        private void UpdateItem()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string Qry = "UPDATE items SET item_name = @name , item_price = @price , item_category = @category , item_status = @status , item_image = @image WHERE Id = @id";
            SqlCommand cmd = new SqlCommand(Qry, con);
            var r = MessageBox.Show("Are you sure want to Update?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            try
            {
                if (r == DialogResult.Yes)
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arrImage = ms.ToArray();

                    con.Open();

                    cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@category", comboBoxCat.Text);
                    cmd.Parameters.AddWithValue("@price", float.Parse(textBoxPrice.Text));
                    cmd.Parameters.AddWithValue("@status", comboBoxSt.Text);
                    cmd.Parameters.AddWithValue("@image", arrImage);
                    cmd.Parameters.AddWithValue("@id", int.Parse(id));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Updated Successfully", "Confirm Update", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            finally
            {
                con.Close();
                if (r == DialogResult.Yes)
                {
                    var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                    IL.LoadWhenAdd();
                    this.Dispose();
                }

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            IL.LoadWhenAdd();
            this.Dispose();
        }



        private void Itemadd_Load(object sender, EventArgs e)
        {
            LoadCategory();
            comboBoxSt.SelectedIndex = 0;
            PassEditInfo();

        }

        // all the auto validating methods 
        private bool Validation()

        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(textBoxName.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxName, "Product name is required.");
            }
            else
            {
                ep.SetError(textBoxName, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(textBoxPrice.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxPrice, "Product price is required.");
            }
            else
            {
                if (float.TryParse(textBoxPrice.Text, out float n) == false)
                {
                    result = false;
                    ep.SetError(textBoxPrice, "Product price is invalid.");
                }
                else
                {
                    ep.SetError(textBoxPrice, string.Empty);
                }

            }


            if (comboBoxCat.SelectedIndex == 0)
            {
                result = false;
                ep.SetError(comboBoxCat, "Select a product category.");
            }
            else
            {
                ep.SetError(comboBoxCat, string.Empty);
            }
            if (pictureBox1.BackgroundImage == null)
            {
                result = false;
                ep.SetError(button1, "Select a product image");
            }
            else
            {
                ep.SetError(button1, string.Empty);
            }

            return result;
        }



        // ------------------------------------------<
        //  receive values from gridview on itemlistform, when clicked on the edit button

        public string passname
        {
            get { return name; }
            set { name = value; }
        }
        public string passprice
        {
            get { return price; }
            set { price = value; }
        }
        public string passcat
        {
            get { return cat; }
            set { cat = value; }
        }
        public string passst
        {
            get { return st; }
            set { st = value; }
        }
        public string passid
        {
            get { return id; }
            set { id = value; }
        }
        // ------------------------------------------>

        private void PassEditInfo()
        {
            if (name != null)
            {
                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT item_image FROM items Where item_name='" + name + "'", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        long len = dr.GetBytes(0, 0, null, 0, 0);
                        byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                        dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                        MemoryStream ms = new MemoryStream(array);
                        Bitmap bitmap = new Bitmap(ms);
                        pictureBox1.BackgroundImage = bitmap;
                    }
                    dr.Close();
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;

                    textBoxName.Text = name;
                    textBoxPrice.Text = price;
                    comboBoxCat.SelectedItem = cat;
                    comboBoxSt.SelectedItem = st;

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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (CheckItemExists())
                    MessageBox.Show("Entered product name already exists !", "Product Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    UpdateItem();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (CheckItemExists())
                    MessageBox.Show("Entered product name already exists !", "Product Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    SaveItem();
            }

        }
    }
}
