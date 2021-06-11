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
        public itemcatgry()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var fIA = Application.OpenForms.OfType<Itemadd>().FirstOrDefault();
            fIA.LoadCategory();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string Qry = "insert into Category(category)values(@category)";
            SqlCommand cmd = new SqlCommand(Qry, con);

            try
            {
                var r = MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        
    }
}
