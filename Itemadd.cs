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
   
    public partial class Itemadd : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
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
        }
        public void UpdateEnabled()
        {

        }
        public void LoadCategory()
        {
            
            try
            {
                
                comboBoxCat.Items.Clear();
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


        private void button2_Click(object sender, EventArgs e)
        {
            string Qry = "insert into items(item_name,item_price,item_category,item_status,item_image)values(@name,@price,@category,@status,@image)";
            SqlCommand cmd = new SqlCommand(Qry, con);

            try
            {
                var r = MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arrImage = ms.ToArray();

                    con.Open();

                    cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@price", int.Parse(textBoxPrice.Text));
                    cmd.Parameters.AddWithValue("@category", comboBoxCat.Text);
                    cmd.Parameters.AddWithValue("@status", comboBoxSt.Text);
                    cmd.Parameters.AddWithValue("@image", arrImage);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Saved Successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            finally
            {
                pictureBox1.BackgroundImage = null;
                textBoxName.Clear();
                comboBoxCat.Items.Clear();
                textBoxPrice.Clear();
                con.Close();
                LoadCategory();
            }
        }


        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            var IL = Application.OpenForms.OfType<Itemlist>().FirstOrDefault();
            IL.Loaditems();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = null;
            itemcatgry IC = new itemcatgry();
            IC.ShowDialog();

        }

        private void Itemadd_Load(object sender, EventArgs e)
        {
            comboBoxSt.SelectedIndex = 0;
            LoadCategory();
        }
    }
}
