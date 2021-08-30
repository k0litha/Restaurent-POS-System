using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public partial class itemcatgry : Form
    {
        string dblocation = "C:\\db\\pos.mdf";
        bool updateON;
        string cat;
        string id;
        public itemcatgry()
        {
            InitializeComponent();
        }



        private void button2_Click(object sender, EventArgs e)
        {

            if (Validation())
            {
                if (CheckCatgryExists())
                    MessageBox.Show("Entered product category already exists !", "Category Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    SaveCatgry();
            }
        }

        private void SaveCatgry()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=" + dblocation + ";Integrated Security = True; Connect Timeout = 30");
            string Qry = "insert into Category(category)values(@category)";
            SqlCommand cmd = new SqlCommand(Qry, con);

            try
            {
                var r = MessageBox.Show("Are you sure want to save?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@category", texBoxCat.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("New category added successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
                texBoxCat.Clear();
                texBoxCat.Focus();
            }
        }

        private void UpdateCategory()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=" + dblocation + ";Integrated Security = True; Connect Timeout = 30");
            var r = MessageBox.Show("Are you sure want to Update?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                try
                {
                    string Qry = "UPDATE Category SET category = @newcat WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(Qry, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@newcat", texBoxCat.Text);
                    cmd.Parameters.AddWithValue("@id", id);
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
                    string Qry = "UPDATE Items SET item_category = @newcat WHERE  item_category= @cat";
                    SqlCommand cmd = new SqlCommand(Qry, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@newcat", texBoxCat.Text);
                    cmd.Parameters.AddWithValue("@cat", cat);
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
                MessageBox.Show("Product Updated Successfully", "Confirm Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                IL.LoadWhenAdd();
                this.Dispose();
            }

        }
        private bool CheckCatgryExists()
        {
            string Qry;
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=" + dblocation + ";Integrated Security = True; Connect Timeout = 30");
            if (updateON)
                Qry = "select count(*) from Category where category=@cat and not Id='" + id + "'";
            else
                Qry = "select count(*) from Category where category=@cat ";
            SqlCommand cmd = new SqlCommand(Qry, con);
            bool exists = false;
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@cat", texBoxCat.Text);
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

        private bool Validation()

        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(texBoxCat.Text.Trim()))
            {
                result = false;
                ep.SetError(texBoxCat, "Product category is required.");
            }
            else
            {
                ep.SetError(texBoxCat, string.Empty);
            }



            return result;
        }

        public string passcat
        {
            get { return cat; }
            set { cat = value; }
        }
        public string passid
        {
            get { return id; }
            set { id = value; }
        }

        public void SaveEnabled()
        {
            button2.Enabled = true;
            button2.Focus();
            btnUpdate.Enabled = false;
            btnUpdate.Visible = false;
            updateON = false;
        }
        public void UpdateEnabled()
        {
            btnUpdate.Enabled = true;
            btnUpdate.Focus();
            button2.Enabled = false;
            button2.Visible = false;
            updateON = true;
            texBoxCat.Text = cat;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            IL.LoadWhenAdd();
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (CheckCatgryExists())
                    MessageBox.Show("Entered product name already exists !", "Product Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    UpdateCategory();
            }
        }
    }
}
