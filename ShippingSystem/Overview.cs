using DnsClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShippingSystem
{
    public partial class Overview : Form
    {
        public Overview()
        {
            InitializeComponent();
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {

        }

        private void gunaButton3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.FormBorderStyle = FormBorderStyle.Sizable;
            customer.WindowState = FormWindowState.Maximized;
            customer.Show();
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gunaButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tracking tracking = new Tracking();
            tracking.FormBorderStyle = FormBorderStyle.Sizable;
            tracking.WindowState = FormWindowState.Maximized;
            tracking.Show();
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Driver driver = new Driver();
            driver.FormBorderStyle = FormBorderStyle.Sizable;
            driver.WindowState = FormWindowState.Maximized;
            driver.Show();
        }

        private void Overview_Load(object sender, EventArgs e)
        {

        }

        private void gunaGradientCircleButton1_Click(object sender, EventArgs e)
        {

        }

        private void gunaPictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddOrder addOrder = new AddOrder();
            addOrder.FormBorderStyle = FormBorderStyle.Sizable;
            addOrder.Show();
        }
    }
}
