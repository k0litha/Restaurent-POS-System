using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace pos
{
    public partial class itemcatgry : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");

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

        private bool CheckCatgryExists()
        {
            string Qry = "select count(*) from Category where category=@category";
            SqlCommand cmd = new SqlCommand(Qry, con);
            bool exists = false;
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@category", texBoxCat.Text);
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var fIA = Application.OpenForms.OfType<Itemadd>().FirstOrDefault();
            fIA.LoadCategory();
            this.Dispose();
        }
    }
}
