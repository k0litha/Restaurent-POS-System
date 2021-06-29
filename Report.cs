using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace pos
{
    public partial class Report : UserControl
    {
        decimal cash, credit;
        int dine, take;
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            Loaddata();
            
        }

        private string DailyTot(string Qry)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string total="";
            SqlCommand cmd = new SqlCommand(Qry, con);
            try
            {
                con.Open();
                total = cmd.ExecuteScalar().ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }
            return total;
        }

       public void Loaddata()
        {
            lblDaily.Text = DailyTot("SELECT isnull(sum(fulltotal),0) FROM Bill WHERE DATEDIFF(day,datetime,GETDATE()) < 1");
            lblWeekly.Text = DailyTot("SELECT  isnull(sum(fulltotal),0) FROM Bill WHERE DATEDIFF(day,datetime,GETDATE()) < 7 ");
            lblMonthly.Text = DailyTot("SELECT  isnull(sum(fulltotal),0) FROM Bill WHERE DATEDIFF(day,datetime,GETDATE()) < 30 ");
            lblYearly.Text = DailyTot("SELECT  isnull(sum(fulltotal),0) FROM Bill WHERE DATEDIFF(day,datetime,GETDATE()) < 365 ");

            CreditCashChart();
           

            DineTakeChart();
        
            
            Top5();
        }

        

        private void CreditCashChart()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {

                    con.Open();
                    
                    string Qry = "SELECT sum(fulltotal) FROM Bill WHERE payment_type='Credit' ";
                    SqlCommand cmd = new SqlCommand(Qry, con);
                     credit = (decimal)cmd.ExecuteScalar() ;
                    
                
            }

            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }

            try
            {

                con.Open();

                string Qry = "SELECT sum(fulltotal) FROM Bill WHERE payment_type='Cash' ";
                SqlCommand cmd = new SqlCommand(Qry, con);
                cash = (decimal)cmd.ExecuteScalar();


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }

            chart1.Series["s1"].Points.Clear();
            chart1.Series["s1"].Label = "#VALX\n#PERCENT{P2}";
            chart1.Series["s1"].Points.AddXY("Credit", credit);
            chart1.Series["s1"].Points.AddXY("Cash", cash);

        }

        private void DineTakeChart()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {

                con.Open();

                string Qry = "SELECT count(order_type) FROM Bill WHERE order_type='Dine in' ";
                SqlCommand cmd = new SqlCommand(Qry, con);
                dine = (int)cmd.ExecuteScalar();


            }

            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }

            try
            {

                con.Open();

                string Qry = "SELECT count(order_type) FROM Bill WHERE order_type='Take out' ";
                SqlCommand cmd = new SqlCommand(Qry, con);
                take = (int)cmd.ExecuteScalar();


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }

            chart2.Series["s1"].Points.Clear();
            chart2.Series["s1"].Label = "#VALX\n#PERCENT{P2}";
            chart2.Series["s1"].Points.AddXY("Dine in", dine);
            chart2.Series["s1"].Points.AddXY("Take Away", take);

        }


        private void Top5()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {

                con.Open();

                string Qry = "select pname,COUNT(*) As ValueFrequency  from BillItems group by pname order by ValueFrequency  DESC OFFSET 0 ROWS FETCH FIRST 10 ROWS ONLY";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();
                
                chart3.Series["s1"].Points.Clear();
                chart3.Series["s1"].Label = "#VALX\n#PERCENT{P2}";
                while (dr.Read())
                {
                    

                    
                    chart3.Series["s1"].Points.AddXY(dr["pname"].ToString(), dr["ValueFrequency"]);
                    

                   

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

    }
}
