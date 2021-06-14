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

        public void Loaditems()
        {

            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
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

                //Changing forcolor according to item status 
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

        private void Itemlist_Load(object sender, EventArgs e)
        {
            Loaditems();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var f1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            f1.GetData();
            this.Dispose();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Itemadd IA = new Itemadd();
            IA.SaveEnabled();
            IA.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dataGridView1.Columns[e.ColumnIndex].Name.ToString();
            if (colname == "colEdit")
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                string name = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                string price = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                string cat = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                string st = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                   
                Itemadd IA = new Itemadd
                    {
                        passname = name,
                        passprice = price,
                        passcat = cat,
                        passst = st,
                        passid = id
                    };
                IA.UpdateEnabled();
                IA.ShowDialog();
            }
            
            if (colname == "colDel")
            {
                string Qry = "delete from items where Id=@id and item_name=@name";
                SqlCommand cmd = new SqlCommand(Qry, con);
                var r = MessageBox.Show("You are about to delete product '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' from the database.\n\nAre you sure ? ", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                try
                {
                    if (r == DialogResult.Yes)
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@name", dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        cmd.Parameters.AddWithValue("@id", int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Product Deleted Successfully", "Confirm Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                        Loaditems();
                    }

                }



            }



        }
    }

    
}