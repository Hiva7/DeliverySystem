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
    public partial class AddOrder : Form
    {
        public AddOrder()
        {
            InitializeComponent();
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.FormBorderStyle = FormBorderStyle.Sizable;
            overview.WindowState = FormWindowState.Maximized;
            overview.Show();
        }
    }
}
