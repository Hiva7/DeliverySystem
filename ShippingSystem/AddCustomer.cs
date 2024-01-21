using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon.SecurityToken.Model;

namespace ShippingSystem
{
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.WindowState = FormWindowState.Maximized;
            customer.Show();
        }

        private void gunaGradient2Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaGradient2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaGradient2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaGradient2Panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaLabel1_Click(object sender, EventArgs e)
        {

        }

        private void TextToAddDriver_Click(object sender, EventArgs e)
        {

        }

        private void AddCustomer_Load(object sender, EventArgs e)
        {
            
        }
    }
}
