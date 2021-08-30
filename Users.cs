using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;


namespace pos
{
    public partial class Users : UserControl
    {

        string dblocation = "C:\\db\\pos.mdf";
        List<string> Searchbox = new List<string>();
        public Users()
        {
            InitializeComponent();
        }

        private void Users_Load(object sender, EventArgs e)
        { 
            comboBoxSearch.Items.Add("Username");
            comboBoxSearch.Items.Add("Full Name");
            comboBoxSearch.Items.Add("Permission");
            comboBoxSearch.Items.Add("Satatus");

            comboBoxSearch.SelectedIndex = 0;

            Searchbox.Add("username");
            Searchbox.Add("fullname");
            Searchbox.Add("permission");
            Searchbox.Add("status");
            GetData();
        }



        public void GetData()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=" + dblocation + ";Integrated Security = True; Connect Timeout = 30");
            PictureBox pic;
            Label name;
            Panel pan;
            Label status;
            Label perm;
            

            try
            {
                flowLayoutPanel1.Controls.Clear();
                con.Open();
                string Qry = "SELECT image,Id,fullname,permission,status,username FROM users where " + Searchbox[comboBoxSearch.SelectedIndex] + " LIKE '" + textBoxsearch.Text + "%' order by permission";

                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    long len = dr.GetBytes(0, 0, null, 0, 0);
                    byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                    dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                    pic = new PictureBox();
                    pic.Width = 170;
                    pic.Height = 190;
                    pic.BackgroundImageLayout = ImageLayout.Stretch;
                    pic.BorderStyle = BorderStyle.FixedSingle;
                    pic.Left = 19;
                    pic.Top = 25;


                    pan = new Panel();
                    pan.Margin = new Padding(20);
                    pan.Width = 210;
                    pan.Height = 280;
                    pan.BorderStyle = BorderStyle.FixedSingle;
                    pan.BackColor = Color.FromArgb(149, 165, 166);
                    pan.Tag = dr["Id"].ToString();
                    pan.Margin = new Padding(20);

                    MemoryStream ms = new MemoryStream(array);
                    Bitmap bitmap = new Bitmap(ms);
                    pic.BackgroundImage = bitmap;
                    pic.Tag = dr["Id"].ToString();

                    status = new Label();
                    status.Text = dr["status"].ToString();
                    status.ForeColor = Color.White;
                    status.Dock = DockStyle.Top;
                    status.TextAlign = ContentAlignment.MiddleCenter;
                    status.Tag = dr["Id"].ToString();
                    status.Width = 210;
                    status.Top = 2;
                    status.Font = new Font("Segoe UI", 10);

                    if (dr["status"].ToString() == "Online")
                    {
                        status.BackColor = Color.FromArgb(39, 174, 96);
                        pan.BackColor = Color.FromArgb(39, 174, 96);
                    }
                    else
                    {
                        status.BackColor = Color.FromArgb(149, 165, 166);
                        pan.BackColor = Color.FromArgb(149, 165, 166);
                    }


                    perm = new Label();
                    perm.Text = ""+ dr["username"].ToString() + " : "+dr["permission"].ToString();
                    perm.BackColor = Color.FromArgb(142, 68, 173);
                    perm.ForeColor = Color.White;
                    perm.TextAlign = ContentAlignment.MiddleCenter;
                    perm.Tag = dr["Id"].ToString();
                    perm.Top = 233;
                    perm.Width = 210;
                    perm.Font = new Font("Segoe UI", 10);

                    name = new Label();
                    name.Text = dr["fullname"].ToString();
                    name.BackColor = Color.FromArgb(142, 68, 173);
                    name.ForeColor = Color.White;
                    name.Dock = DockStyle.Bottom;
                    name.TextAlign = ContentAlignment.MiddleCenter;
                    name.Tag = dr["Id"].ToString();
                    name.Width = 210;
                    name.Font = new Font("Segoe UI", 10);

                    flowLayoutPanel1.Controls.Add(pan);
                    pan.Controls.Add(pic);
                    pan.Controls.Add(perm);
                    pan.Controls.Add(status);
                    pan.Controls.Add(name);

                    perm.Click += new EventHandler(SelectClick);
                    pic.Click += new EventHandler(SelectClick);
                    pan.Click += new EventHandler(SelectClick);
                    name.Click += new EventHandler(SelectClick);
                    status.Click += new EventHandler(SelectClick);

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

        public void SelectClick(Object sender, EventArgs e)
        {
            int id;

            if ((sender as System.Windows.Forms.PictureBox == null) && (sender as System.Windows.Forms.Label != null) && (sender as System.Windows.Forms.Panel == null))
            {

                id = int.Parse((sender as Label).Tag.ToString());
                PassItems(id);
            }
            else if ((sender as System.Windows.Forms.PictureBox != null) && (sender as System.Windows.Forms.Panel == null) && (sender as System.Windows.Forms.Label == null))
            {

                id = int.Parse((sender as PictureBox).Tag.ToString());
                PassItems(id);
            }
            else if ((sender as System.Windows.Forms.PictureBox == null) && (sender as System.Windows.Forms.Panel != null) && (sender as System.Windows.Forms.Label == null))
            {
                id = int.Parse((sender as Panel).Tag.ToString());
                PassItems(id);
            }
        }
        private void PassItems(int x)
        {
            UserAdd UA = new UserAdd
            {
                passid = x.ToString()
            };
            UA.UpdateEnabled();
            UA.PassEditInfo();
            UA.ShowDialog();
        }




        private void lblAddUser_Click(object sender, EventArgs e)
        {
            UserAdd UA = new UserAdd();
            UA.SaveEnabled();
            UA.ShowDialog();
        }

        private void textBoxsearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

    }
}
