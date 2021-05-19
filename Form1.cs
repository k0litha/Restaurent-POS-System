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

namespace pos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
                    
            saleRegister.Hide();
            Items.Hide();
        }
        
        private void btnHome_Click(object sender, EventArgs e)
        {
            saleRegister.Hide();
            Items.Hide();
        }

        private void btnSaleReg_Click(object sender, EventArgs e)
        {
            Items.Hide();
            saleRegister.Show();
            saleRegister.BringToFront();
        }

        private void saleRegister_Load(object sender, EventArgs e)
        {

        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            saleRegister.Hide();
            Items.Show();
            Items.BringToFront();
        }
    }
}
