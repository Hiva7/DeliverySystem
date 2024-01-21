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
    public partial class AddTruck : Form
    {
        public AddTruck()
        {
            InitializeComponent();
        }

        private void AddDriver_Load(object sender, EventArgs e)
        {

        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Truck truck = new Truck();
            truck.FormBorderStyle = FormBorderStyle.Sizable;
            truck.WindowState = FormWindowState.Maximized;
            truck.Show();
        }

        private void gunaLabel5_Click(object sender, EventArgs e)
        {

        }
    }
}
