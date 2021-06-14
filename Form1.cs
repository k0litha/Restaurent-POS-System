using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.IO;

namespace pos
{
    public partial class Form1 : Form
    {
        private PictureBox pic;
        private Label price;
        private Label name;

        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");



        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            
            GetData();

        }

        public void GetData()
        {

            try
            {
                flowLayoutPanel1.Controls.Clear();
                con.Open();
                string Qry = "SELECT item_image, item_name, item_price FROM items Where item_status='Available'";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    long len = dr.GetBytes(0, 0, null, 0, 0);
                    byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                    dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                    pic = new PictureBox();
                    pic.Width = 140;
                    pic.Height = 140;
                    pic.BackgroundImageLayout = ImageLayout.Stretch;
                    pic.BorderStyle = BorderStyle.FixedSingle;

                    MemoryStream ms = new MemoryStream(array);
                    Bitmap bitmap = new Bitmap(ms);
                    pic.BackgroundImage = bitmap;

                    //add price label 
                    price = new Label();
                    price.Text = dr["item_price"].ToString();
                    price.BackColor = Color.FromArgb(28, 28, 28);
                    price.ForeColor = Color.White;

                    price.Width = 60;
                    price.Height = 18;
                    price.TextAlign = ContentAlignment.MiddleCenter;

                    //add name label  
                    name = new Label();
                    name.Text = dr["item_name"].ToString();
                    name.BackColor = Color.FromArgb(28, 28, 28);
                    name.ForeColor = Color.White;
                    name.Dock = DockStyle.Bottom;
                    name.TextAlign = ContentAlignment.MiddleCenter;


                    flowLayoutPanel1.Controls.Add(pic);
                    pic.Controls.Add(name);
                    pic.Controls.Add(price);
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

       

        private void Itemslbl_Click(object sender, EventArgs e)
        {
            Itemadd f2 = new Itemadd();
            f2.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Itemlist IL = new Itemlist();
            IL.ShowDialog();
        }

        private void CLOSE_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }

}
