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
            this.WindowState = FormWindowState.Maximized;
            Database db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
            TotalCustomer.Text = db.GetLatestID("Customer").ToString();
            TotalTruck.Text = db.GetLatestID("Truck").ToString();
            TotalOrder.Text = db.GetLatestID("Order").ToString();
        }

        private void gunaButton3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
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
            tracking.WindowState = FormWindowState.Maximized;
            tracking.Show();
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Truck truck = new Truck();
            truck.WindowState = FormWindowState.Maximized;
            truck.Show();
        }

        private void Overview_Load(object sender, EventArgs e)
        {

        }

        private void gunaPictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddOrder addOrder = new AddOrder();
            addOrder.Show();
        }

        private void gunaGradient2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaGradient2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
