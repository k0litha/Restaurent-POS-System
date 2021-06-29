using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;


namespace pos
{
    public partial class Itemlist : UserControl
    {

        public Itemlist()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Loaditems();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            LoadCategory();
        }
        public void Loaditems()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.ClearSelection();
                int i = 0;
                con.Open();
                string Qry = "SELECT * FROM items where item_name LIKE '" + textBox1.Text + "%' order by item_category";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    i += 1;
                    dataGridView1.RowTemplate.Height = 100;
                    Column6.ImageLayout = DataGridViewImageCellLayout.Stretch;

                    dataGridView1.Rows.Add(i, dr["Id"].ToString(), dr["item_image"], dr["item_name"].ToString(), dr["item_price"].ToString(), dr["item_category"].ToString(), dr["item_status"].ToString());

                }
                dr.Close();

                //Changing forcolor according to item status 
                for (int r = 0; r < i; r++)
                {
                    if (dataGridView1[6, r].Value.ToString() == "Available")
                    {
                        dataGridView1[6, r].Style.ForeColor = Color.FromArgb(62, 250, 0);
                        dataGridView1[6, r].Style.SelectionForeColor = Color.FromArgb(62, 250, 0);
                    }
                    else
                    {
                        dataGridView1[6, r].Style.ForeColor = Color.FromArgb(255, 5, 5);
                        dataGridView1[6, r].Style.SelectionForeColor = Color.FromArgb(255, 5, 5);
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
        public void LoadCategory()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                dataGridView2.ClearSelection();
                int i = 0;
                con.Open();
                string Qry = "SELECT * FROM Category where category LIKE '" + textboxcat.Text + "%'";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    i += 1;
                    dataGridView2.RowTemplate.Height = 50;

                    dataGridView2.Rows.Add(i, dr["category"].ToString(), dr["Id"].ToString());

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

        private void Itemlist_Load(object sender, EventArgs e)
        {
            Loaditems();
            LoadCategory();
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string colname = dataGridView1.Columns[e.ColumnIndex].Name.ToString();
            if (e.RowIndex >= 0)
            {
                if (colname == "colEdit")
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string price = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    string cat = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    string st = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

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
                    var r = MessageBox.Show("You are about to delete category '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' from the database.\n\nAre you sure ? ", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    try
                    {
                        if (r == DialogResult.Yes)
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@name", dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string colname = dataGridView2.Columns[e.ColumnIndex].Name.ToString();
            if (e.RowIndex >= 0)
            {
                if (colname == "ColEditCat")
                {
                    string cat = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string id = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();

                    itemcatgry IC = new itemcatgry
                    {
                        passcat = cat,
                        passid = id
                    };
                    IC.UpdateEnabled();
                    IC.ShowDialog();
                }

                if (colname == "ColDelCat")
                {
                    MessageBox.Show("You are about to delete category '" + dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString() + "' from the database. This will results in lost of product items, that assigned to the corresponding category.", "Confirm Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    var r = MessageBox.Show("Are you sure want to delete '" + dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString() + "' from the database? ", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (r == DialogResult.Yes)
                    {

                        try
                        {
                            string Qry = "delete from Category where category=@cat ";
                            SqlCommand cmd = new SqlCommand(Qry, con);
                            con.Open();
                            cmd.Parameters.AddWithValue("@cat", dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
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
                            string Qry = "delete from items where item_category=@cat ";
                            SqlCommand cmd = new SqlCommand(Qry, con);
                            con.Open();
                            cmd.Parameters.AddWithValue("@cat", dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Category Deleted Successfully", "Confirm Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        finally
                        {
                            con.Close();
                            LoadCategory();
                            Loaditems();
                        }

                    }
                }
            }

        }






        private void lblAddCat_Click(object sender, EventArgs e)
        {

            itemcatgry IC = new itemcatgry();
            IC.SaveEnabled();
            IC.ShowDialog();
        }

        private void lblAddItem_Click(object sender, EventArgs e)
        {
            Itemadd IA = new Itemadd();
            IA.SaveEnabled();
            IA.ShowDialog();
        }
    }


}