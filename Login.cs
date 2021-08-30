using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace pos
{
    public partial class Login : Form
    {
        bool offline = true;
        string dblocation = "C:\\db\\pos.mdf";
        public Login()
        {
            InitializeComponent();
        }

        private bool Authenticate()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename="+ dblocation + ";Integrated Security = True; Connect Timeout = 30");
            bool sucsess = false;
            string Qry = "select count(*) from Users where username=@username and password=@password";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
                cmd.Parameters.AddWithValue("@password", textBoxPassword.Text);
                sucsess = (int)cmd.ExecuteScalar() > 0;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }
            return sucsess;
        }

        private string Permission()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=" + dblocation + ";Integrated Security = True; Connect Timeout = 30");
            string perm = "";

            try
            {
                con.Open();
                string Qry = "select permission from Users where username='" + textBoxUsername.Text + "' and password='" + textBoxPassword.Text + "'";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    perm = dr["permission"].ToString();
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
            return perm;
        }

        private bool Validation()

        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(textBoxUsername.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxUsername, "Username is required.");
            }
            else
            {
                ep.SetError(textBoxUsername, string.Empty);
            }


            if (string.IsNullOrWhiteSpace(textBoxPassword.Text.Trim()))
            {
                result = false;
                ep.SetError(textBoxPassword, "Password is required.");
            }
            else
            {
                ep.SetError(textBoxPassword, string.Empty);
            }
            return result;
        }

        private void Status()
        {

            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=" + dblocation + ";Integrated Security = True; Connect Timeout = 30");

            try
            {
                string Qry = "UPDATE Users SET status=@status where username=@username";
                SqlCommand cmd = new SqlCommand(Qry, con);
                con.Open();
                cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);


                if (offline)
                {

                    cmd.Parameters.AddWithValue("@status", "Online");
                    cmd.ExecuteNonQuery();
                }

                if (!offline)
                {

                    cmd.Parameters.AddWithValue("@status", "Offline");
                    cmd.ExecuteNonQuery();
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



        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                if (Authenticate())
                {

                    Form1 F1 = new Form1
                    {
                        passuser = textBoxUsername.Text,
                        passperm = Permission()
                    };
                    this.Hide();
                    Status();
                    F1.ShowDialog();
                    offline = false;
                    Status();
                    textBoxUsername.Clear();
                    textBoxPassword.Clear();
                    offline = true;
                    this.Show();


                }
                else
                    MessageBox.Show("Username or Password is incorrect. ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
