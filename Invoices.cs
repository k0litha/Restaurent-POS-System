using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace pos
{
    public partial class Invoices : UserControl
    {
        List<string> Searchbox = new List<string>();
        List<string> requestlist = new List<string>();
        List<string> pnamelist = new List<string>();
        List<string> qtylist = new List<string>();
        List<string> totallist = new List<string>();
        string transno = "";
        string datetime = "";
        string payment_type = "";
        string payment = "";
        float fulltotal = 0;
        string change = "";
        float discount = 0;
        string user = "";
        string order_type = "";
        public Invoices()
        {
            InitializeComponent();
        }
        public void LoadInvoices()
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.ClearSelection();
                int i = 0;
                con.Open();
                string Qry = "SELECT * FROM Bill WHERE " + Searchbox[comboBoxSearch.SelectedIndex] + " LIKE '" + textBox1.Text + "%' ORDER BY Id DESC";
                SqlCommand cmd = new SqlCommand(Qry, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    i += 1;
                    dataGridView1.RowTemplate.Height = 40;
                    dataGridView1.Rows.Add(i, dr["Id"].ToString(), dr["transno"].ToString(), dr["datetime"].ToString(), dr["payment_type"].ToString(), dr["payment"].ToString(), ""+dr["discount"].ToString()+"%", dr["fulltotal"].ToString(), dr["change"].ToString(), dr["order_type"].ToString(), dr["user"].ToString());
                    
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

       


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\C#pos\db\pos.mdf;Integrated Security = True; Connect Timeout = 30");
            string colname = dataGridView1.Columns[e.ColumnIndex].Name.ToString();
            if (e.RowIndex >= 0)
            {
                if (colname == "colBill")
                {
                    try
                    {
                        con.Open();
                        string Qry = "SELECT B.transno AS transno,B.datetime AS datetime,B.payment_type AS payment_type,B.payment AS payment,B.fulltotal AS fulltotal,B.change AS change,B.discount AS discount,B.[user] AS [user],B.order_type AS order_type,BI.pname AS pname,BI.qty,BI.request AS request,BI.total AS total FROM Bill B INNER JOIN BillItems BI On B.transno=BI.transno where B.transno='" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'";
                        SqlCommand cmd = new SqlCommand(Qry, con);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {

                            pnamelist.Add(dr["pname"].ToString());
                            qtylist.Add(dr["qty"].ToString());
                            requestlist.Add(dr["request"].ToString());
                            totallist.Add(dr["total"].ToString());
                            transno = dr["transno"].ToString();
                            datetime = dr["datetime"].ToString();
                            payment_type = dr["payment_type"].ToString();
                            payment = dr["payment"].ToString();
                            fulltotal = float.Parse(dr["fulltotal"].ToString());
                            change = dr["change"].ToString();
                            discount = float.Parse(dr["discount"].ToString());
                            user = dr["user"].ToString();
                            order_type = dr["order_type"].ToString();



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
                        printbill();
                    }

                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            Graphics graphics = e.Graphics;
            Font font9 = new Font("Courier New", 9);
            Font font10 = new Font("Courier New", 10);
            Font font11 = new Font("Courier New", 11);
            Font font14 = new Font("Courier New", 14);
            Font font11b = new Font("Courier New", 11, FontStyle.Bold);
            Font font14b = new Font("Courier New", 14, FontStyle.Bold);

            float leading = 3;
            float lineheight9 = font9.GetHeight() + leading;
            float lineheight10 = font10.GetHeight() + leading;
            float lineheight11 = font11.GetHeight() + leading;
            float lineheight14 = font14.GetHeight() + leading;

            float startX = 16;
            float startY = leading;
            float Offset = 0;

            StringFormat formatLeft = new StringFormat(StringFormatFlags.NoClip);
            StringFormat formatCenter = new StringFormat(formatLeft);
            StringFormat formatRight = new StringFormat(formatLeft);

            formatCenter.Alignment = StringAlignment.Center;
            formatRight.Alignment = StringAlignment.Far;
            formatLeft.Alignment = StringAlignment.Near;

            SizeF layoutSize = new SizeF(400 - Offset * 2, lineheight14);
            SizeF layoutSizeLine = new SizeF(386 - Offset * 2, lineheight14);
            SizeF layoutSizeString = new SizeF(370 - Offset * 2, lineheight14);
            SizeF layoutSizeCalcu = new SizeF(320 - Offset * 2, lineheight14);
            RectangleF layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);

            Brush brush = Brushes.Black;
            layout = new RectangleF(new PointF(startX, startY + 1), layoutSizeString);
            graphics.DrawString("Dine Fine", font14b, brush, layout, formatCenter);
            Offset = Offset + lineheight14 + 6;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("No 12, High-Level Road,", font10, brush, layout, formatCenter);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Homagama, Colombo, Sri Lanka. ", font10, brush, layout, formatCenter);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Tel: 011 7575888 ", font10, brush, layout, formatCenter);
            Offset = Offset + lineheight10 - 2;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Date: " + datetime, font10, brush, layout, formatLeft);
            graphics.DrawString("*PAID*", font14b, brush, layout, formatRight);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Bill No: " + transno, font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Payment: " + payment_type, font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("Server: " + user, font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10 - 2;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight10 - 4;
            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSizeString);
            graphics.DrawString("~" + order_type + "~", font11b, brush, layout, formatCenter);
            Offset = Offset + lineheight10 - 4;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            int i = 0;
            foreach (string a in pnamelist)
            {
                graphics.DrawString("" + qtylist[i] + " x " + a, font11, brush, layout, formatLeft);
                graphics.DrawString("" + totallist[i] + "   ", font11, brush, layout, formatRight);
                if (!string.IsNullOrWhiteSpace(requestlist[i]))
                {
                    string[] req = requestlist[i].Split(',');
                    foreach (string r in req)
                    {
                        Offset = Offset + lineheight9 - 2;
                        layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
                        graphics.DrawString("         -" + r, font10, brush, layout, formatLeft);
                    }
                }

                Offset = Offset + lineheight11 + 2;
                layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
                i += 1;
            }
            Offset = Offset + lineheight10 - 10;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '-'), font11, brush, layout, formatCenter);

            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Sub Total", font11, brush, layout, formatLeft);
            graphics.DrawString(((fulltotal / (100 - discount)) * 100).ToString("0.00"), font11, brush, layout, formatRight);
            Offset = Offset + lineheight11;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Discount: " + discount + "%", font11, brush, layout, formatLeft);
            graphics.DrawString("-" + ((fulltotal / (100 - discount)) * discount).ToString("0.00"), font11, brush, layout, formatRight);

            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Total", font11b, brush, layout, formatLeft);
            graphics.DrawString(fulltotal.ToString("0.00"), font11b, brush, layout, formatRight);
            Offset = Offset + lineheight10 - 4;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(25, '-'), font11, brush, layout, formatRight);
            Offset = Offset + lineheight11 + 10;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Paid Amount", font11, brush, layout, formatLeft);
            graphics.DrawString(payment, font11, brush, layout, formatRight);
            Offset = Offset + lineheight11;
            layout = new RectangleF(new PointF(startX + 25, startY + Offset), layoutSizeCalcu);
            graphics.DrawString("            Change", font11, brush, layout, formatLeft);
            graphics.DrawString(change, font11, brush, layout, formatRight);
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            Offset = Offset + lineheight10 + 10;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("".PadRight(40, '='), font11, brush, layout, formatCenter);
            Offset = Offset + lineheight10 + 10;
            layout = new RectangleF(new PointF(startX - 8, startY + Offset), layoutSizeLine);
            graphics.DrawString("Thank You! Come Again.", font11b, brush, layout, formatCenter);
            font10.Dispose(); font11.Dispose(); font14.Dispose();


            pnamelist.Clear();
            qtylist.Clear();
            requestlist.Clear();
            totallist.Clear();


        }



        private void printbill()
        {
            int i = 0;
            int x = 480;

            foreach (string a in pnamelist)
            {
                x += 20;
                if (!string.IsNullOrWhiteSpace(requestlist[i]))
                {
                    string[] req = requestlist[i].Split(',');
                    foreach (string r in req)
                    {
                        x += 17;
                    }
                }

            }


            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("bill", 400, x);
            printPreviewDialog1.PrintPreviewControl.AutoZoom = false;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            printPreviewDialog1.PrintPreviewControl.Width = 450;
            printPreviewDialog1.PrintPreviewControl.Height = 600;
            printPreviewDialog1.ShowDialog();


        }



        private void Invoices_Load(object sender, EventArgs e)
        {
            
            comboBoxSearch.Items.Add("Invoice No");
            comboBoxSearch.Items.Add("Payment");
            comboBoxSearch.Items.Add("Discount");
            comboBoxSearch.Items.Add("Total");
            comboBoxSearch.Items.Add("Change");
            comboBoxSearch.Items.Add("Server");
            comboBoxSearch.SelectedIndex = 0;
            Searchbox.Add("transno");
            Searchbox.Add("payment");
            Searchbox.Add("discount");
            Searchbox.Add("fulltotal");
            Searchbox.Add("change");
            Searchbox.Add("[user]");
            LoadInvoices();
        }

       

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadInvoices();
        }
    }
}
