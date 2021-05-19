using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace pos
{
    public partial class SaleRegister : UserControl
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        private PictureBox pic;
        private Label price;
        private Label name;

        public SaleRegister()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = "server=localhost; user=root; password=; database=pos";


        }

        private void SaleRegister_Load(object sender, EventArgs e)
        {
            
        }

        //run clear command and then getdata and display on flowlayout, everytime when openning the saleregister. so database changes will be update instantly 
        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                flowLayoutPanel1.Controls.Clear();
                GetData();
            }
        }


        private void GetData()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT item_image, item_name, item_price FROM items", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                //store retrived  pictures from db to an array, to display on flow layout
                long len = dr.GetBytes(0, 0, null, 0, 0 );
                byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                pic = new PictureBox();
                pic.Width = 140;
                pic.Height = 140;
                pic.BackgroundImageLayout = ImageLayout.Stretch;
                pic.BorderStyle = BorderStyle.Fixed3D;

                MemoryStream ms = new MemoryStream(array);
                Bitmap bitmap = new Bitmap(ms);
                pic.BackgroundImage = bitmap;

                //add price label 
               price = new Label();
               price.Text = dr["item_price"].ToString();          
               price.BackColor = Color.FromArgb(28, 28, 28);
               price.ForeColor = Color.White;
             
               price.Width = 50;
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
            cn.Close();
        }
    }
}
