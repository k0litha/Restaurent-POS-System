using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public partial class UserAdd : Form
    {
        string id;
        bool updateON;
        public UserAdd()
        {
            InitializeComponent();
        }
        public void SaveEnabled()
        {
            btnEdit.Enabled = false;
            btnEdit.Visible = false;
            btnCansel.Enabled = false;
            btnCansel.Visible = false;
            button2.Enabled = false;
            button2.Visible = false;
            btnSave.Enabled = true;
            btnSave.Visible = true;
            btnSave.Focus();
            btnUpdate.Enabled = false;
            btnUpdate.Visible = false;
            updateON = false;
        }
        public void UpdateEnabled()
        {
            textBoxEmail.Enabled = false;
            textBoxAdrs.Enabled = false;
            textBoxName.Enabled = false;
            textBoxPhone.Enabled = false;
            textBoxPwd.Enabled = false;
            textBoxUname.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            button1.Enabled = false;


            button2.Enabled = true;
            button2.Visible = true;
            btnUpdate.Enabled = false;
            btnUpdate.Visible = false;
            btnUpdate.Focus();
            btnSave.Enabled = false;
            btnSave.Visible = false;
            btnEdit.Enabled = true;
            btnEdit.Visible = true;
            btnCansel.Enabled = false;
            btnCansel.Visible = false;
            updateON = true;
        }

        public string passid
        {
            get { return id; }
            set { id = value; }
        }
        private void SaveUser()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string Qry = "insert into Users(username,fullname,password,address,phone,email,sex,permission,image)values(@username,@fullname,@password,@address,@phone,@email,@sex,@permission,@image)";
            SqlCommand cmd = new SqlCommand(Qry, con);
            var r = MessageBox.Show("Are you sure want to save?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            try
            {
                if (r == DialogResult.Yes)
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arrImage = ms.ToArray();

                    con.Open();
                    cmd.Parameters.AddWithValue("@username", textBoxUname.Text);
                    cmd.Parameters.AddWithValue("@fullname", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@password", textBoxPwd.Text);
                    cmd.Parameters.AddWithValue("@address", textBoxAdrs.Text);
                    cmd.Parameters.AddWithValue("@phone", textBoxPhone.Text);
                    cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);


                    if (rbMale.Checked)
                        cmd.Parameters.AddWithValue("@sex", rbMale.Text);
                    if (rbFemale.Checked)
                        cmd.Parameters.AddWithValue("@sex", rbFemale.Text);
                    if (rbOther.Checked)
                        cmd.Parameters.AddWithValue("@sex", rbOther.Text);


                    if (rbAdmin.Checked)
                        cmd.Parameters.AddWithValue("@permission", rbAdmin.Text);
                    if (rbStd.Checked)
                        cmd.Parameters.AddWithValue("@permission", rbStd.Text);

                    cmd.Parameters.AddWithValue("@image", arrImage);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Saved Successfully", "Confirm Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    pictureBox1.BackgroundImage = null;
                    textBoxName.Clear();
                    textBoxUname.Clear();
                    textBoxPwd.Clear();
                    textBoxPhone.Clear();
                    textBoxEmail.Clear();
                    textBoxAdrs.Clear();
                    rbAdmin.Checked = true;
                    rbStd.Checked = false;
                    rbMale.Checked = false;
                    rbFemale.Checked = false;

                }
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
        private void UpdateUser()
        {
            if (!CheckAdminCount() && rbAdmin.Checked == false)
            {
                MessageBox.Show("One or more user profiles with 'Admin' permission should be present in the system.", "Request Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {

                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
                string Qry = "UPDATE Users SET username=@username,fullname=@fullname,password=@password,address=@address,phone=@phone,email=@email,sex=@sex,permission=@permission,image=@image where id=@id";
                SqlCommand cmd = new SqlCommand(Qry, con);
                var r = MessageBox.Show("Are you sure want to update?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    try
                    {

                        MemoryStream ms = new MemoryStream();
                        pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] arrImage = ms.ToArray();

                        con.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@username", textBoxUname.Text);
                        cmd.Parameters.AddWithValue("@fullname", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@password", textBoxPwd.Text);
                        cmd.Parameters.AddWithValue("@address", textBoxAdrs.Text);
                        cmd.Parameters.AddWithValue("@phone", textBoxPhone.Text);
                        cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);


                        if (rbMale.Checked)
                            cmd.Parameters.AddWithValue("@sex", rbMale.Text);
                        if (rbFemale.Checked)
                            cmd.Parameters.AddWithValue("@sex", rbFemale.Text);
                        if (rbOther.Checked)
                            cmd.Parameters.AddWithValue("@sex", rbOther.Text);


                        if (rbAdmin.Checked)
                            cmd.Parameters.AddWithValue("@permission", rbAdmin.Text);
                        if (rbStd.Checked)
                            cmd.Parameters.AddWithValue("@permission", rbStd.Text);

                        cmd.Parameters.AddWithValue("@image", arrImage);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User Updated Successfully", "Confirm Update", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    finally
                    {
                        con.Close();
                        var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                        IL.LoadWhenAdd();
                        PassEditInfo();
                    }
                }
            }
        }


        private void DeleteUser()
        {
            if (CheckAdminCount())
            {
                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
                string Qry = "Delete from Users where id=@id";
                SqlCommand cmd = new SqlCommand(Qry, con);
                var r = MessageBox.Show("Are you sure want to delete user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    try
                    {

                        con.Open();
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User Deleted Successfully", "Confirm Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    finally
                    {
                        con.Close();
                        var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                        IL.LoadWhenAdd();
                        this.Dispose();
                    }
                }
            }
            else
                MessageBox.Show("One or more user profiles with 'Admin' permission should be present in the system.", "Request Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        private bool CheckAdminCount()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            bool enough = false;
            string Qry = "select count(*) from Users where permission='Admin' and not Id=@id";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@id", id);
                enough = (int)cmd.ExecuteScalar() >= 1;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }
            return enough;
        }

        public void PassEditInfo()
        {
            pictureBox1.BackgroundImage = null;
            textBoxName.Clear();
            textBoxUname.Clear();
            textBoxPwd.Clear();
            textBoxPhone.Clear();
            textBoxEmail.Clear();
            textBoxAdrs.Clear();
            rbOther.Checked = false;
            rbAdmin.Checked = false;
            rbStd.Checked = false;
            rbMale.Checked = false;
            rbFemale.Checked = false;

            if (id != null)
            {

                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT image,username,fullname,password,address,phone,email,sex,permission FROM Users Where Id='" + id + "'", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        long len = dr.GetBytes(0, 0, null, 0, 0);
                        byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                        dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                        MemoryStream ms = new MemoryStream(array);
                        Bitmap bitmap = new Bitmap(ms);
                        pictureBox1.BackgroundImage = bitmap;

                        textBoxName.Text = dr["fullname"].ToString();
                        textBoxUname.Text = dr["username"].ToString();
                        textBoxEmail.Text = dr["email"].ToString();
                        textBoxPhone.Text = dr["phone"].ToString();
                        textBoxPwd.Text = dr["password"].ToString();
                        textBoxAdrs.Text = dr["address"].ToString();

                        if (dr["sex"].ToString() == "Male")
                            rbMale.Checked = true;
                        if (dr["sex"].ToString() == "Female")
                            rbFemale.Checked = true;
                        if (dr["sex"].ToString() == "Other")
                            rbOther.Checked = true;


                        if (dr["permission"].ToString() == "Admin")
                            rbAdmin.Checked = true;
                        if (dr["permission"].ToString() == "Standard")
                            rbStd.Checked = true;


                    }
                    dr.Close();
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;


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
        }




        private bool CheckUserExists()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            bool exists = false;
            string Qry;

            if (updateON)
                Qry = "select count(*) from Users where username=@name and not Id='" + id + "'";
            else
                Qry = "select count(*) from Users where username=@name ";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@name", textBoxUname.Text);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (CheckUserExists())
                    MessageBox.Show("Entered Username already exists !", "Product Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    UpdateUser();
            }
        }




        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            IL.LoadWhenAdd();
            this.Dispose();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (CheckUserExists())
                    MessageBox.Show("Entered product name already exists !", "Product Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    SaveUser();
                    var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                    IL.LoadWhenAdd();
                    this.Dispose();
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteUser();
            var IL = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            IL.LoadWhenAdd();
            this.Dispose();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            textBoxEmail.Enabled = true;
            textBoxAdrs.Enabled = true;
            textBoxName.Enabled = true;
            textBoxPhone.Enabled = true;
            textBoxPwd.Enabled = true;
            textBoxUname.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            button1.Enabled = true;


            btnCansel.Enabled = true;
            btnCansel.Visible = true;
            btnUpdate.Enabled = true;
            btnUpdate.Visible = true;
            button2.Enabled = false;
            button2.Visible = false;
            btnEdit.Enabled = false;
            btnEdit.Visible = false;

        }

        private void btnCansel_Click(object sender, EventArgs e)
        {
            textBoxEmail.Enabled = false;
            textBoxAdrs.Enabled = false;
            textBoxName.Enabled = false;
            textBoxPhone.Enabled = false;
            textBoxPwd.Enabled = false;
            textBoxUname.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            button1.Enabled = false;

            btnCansel.Enabled = false;
            btnCansel.Visible = false;
            btnUpdate.Enabled = false;
            btnUpdate.Visible = false;
            button2.Enabled = true;
            button2.Visible = true;
            btnEdit.Enabled = true;
            btnEdit.Visible = true;
            PassEditInfo();

        }


        private bool Validation()

        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(textBoxName.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxName, "Full name is required.");
            }
            else
            {
                ep.SetError(textBoxName, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(textBoxUname.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxUname, "Username is required.");
            }
            else
            {
                if (textBoxUname.Text.Length < 5)
                {
                    result = false;
                    ep.SetError(textBoxUname, "Minimum 5 characters required.");
                }
                else
                    ep.SetError(textBoxUname, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(textBoxPwd.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxPwd, "Password is required.");
            }
            else
            {
                if (textBoxPwd.Text.Length < 5)
                {
                    result = false;
                    ep.SetError(textBoxPwd, "Minimum 5 characters required.");
                }
                else
                    ep.SetError(textBoxPwd, string.Empty);

            }

            if (string.IsNullOrWhiteSpace(textBoxPhone.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxPhone, "Phone number is required.");
            }
            else
            {
                if (ulong.TryParse(textBoxPhone.Text, out ulong n) == false)
                {
                    result = false;
                    ep.SetError(textBoxPhone, "Phone number is Invalid.");

                }

                else
                {
                    if (textBoxPhone.Text.Length != 10)
                    {
                        result = false;
                        ep.SetError(textBoxPhone, "Phone number is Invalid..");
                    }
                    else
                        ep.SetError(textBoxPhone, string.Empty);
                }

            }

            if (string.IsNullOrWhiteSpace(textBoxAdrs.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxAdrs, "Home Address is required.");
            }
            else
            {

                ep.SetError(textBoxAdrs, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(textBoxEmail.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxEmail, "Email Address is required.");
            }
            else
            {

                ep.SetError(textBoxEmail, string.Empty);
            }



            if (pictureBox1.BackgroundImage == null)
            {
                result = false;
                ep.SetError(button1, "Select a product image");
            }
            else
            {
                ep.SetError(button1, string.Empty);
            }


            if (!rbMale.Checked && !rbFemale.Checked && !rbOther.Checked)
            {
                result = false;
                ep.SetError(groupBox1, "Select a Gender");
            }
            else
                ep.SetError(groupBox1, string.Empty);

            if (!rbAdmin.Checked && !rbStd.Checked)
            {
                result = false;
                ep.SetError(groupBox2, "Select permission");
            }
            else
                ep.SetError(groupBox2, string.Empty);

            return result;
        }









    }
}
