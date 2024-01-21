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
    public partial class Truck : Form
    {
        public Truck()
        {
            InitializeComponent();
        }

        private void Driver_Load(object sender, EventArgs e)
        {

        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {

        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.FormBorderStyle = FormBorderStyle.Sizable;
            customer.WindowState = FormWindowState.Maximized;
            customer.Show();
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tracking tracking = new Tracking();
            tracking.FormBorderStyle = FormBorderStyle.Sizable;
            tracking.WindowState = FormWindowState.Maximized;
            tracking.Show();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.FormBorderStyle = FormBorderStyle.Sizable;
            overview.WindowState = FormWindowState.Maximized;
            overview.Show();
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TextToAddDriver_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddTruck addTruck = new AddTruck();
            addTruck.FormBorderStyle = FormBorderStyle.Sizable;
            addTruck.Show();
        }
    }
}
