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
    public partial class Itemlist : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
        public Itemlist()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Itemadd IA = new Itemadd();
            IA.ShowDialog();
        }


        public void Loaditems()
        {

            try
            {
                
                dataGridView1.Controls.Clear();
                int i = 0;
                con.Open();
                string Qry = "SELECT * FROM items";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
               
                while (dr.Read())
                {
                    i += 1;
                    dataGridView1.RowTemplate.Height = 100;
                    Column6.ImageLayout = DataGridViewImageCellLayout.Stretch;

                    dataGridView1.Rows.Add(i, dr["Id"].ToString(), dr["item_name"].ToString(), dr["item_price"].ToString(), dr["item_category"].ToString(), dr["item_status"].ToString(), dr["item_image"]);
                   
                }  
                dr.Close();

                for (int r=0;r<i;r++)               
                {   
                    if(dataGridView1[5, r].Value.ToString() == "Available")
                    {
                        dataGridView1[5, r].Style.ForeColor = Color.Green;
                        dataGridView1[5, r].Style.SelectionForeColor = Color.Green;
                    }

                    else
                    {
                        dataGridView1[5, r].Style.ForeColor = Color.Red;
                        dataGridView1[5, r].Style.SelectionForeColor = Color.Red;
                    }
                        
                }
           
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var f1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            f1.GetData();
            this.Dispose();
        }

        private void Itemlist_Load(object sender, EventArgs e)
        {
            Loaditems();
        }
    }

    
}