using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI.WinForms;

namespace ShippingSystem
{
    public partial class Tracking : Form
    {
        private Database db;
        public Tracking()
        {
            InitializeComponent();
            db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void Tracking_Load(object sender,  EventArgs e)
        {
            String coll = "Order";
            var data = db.GetCollection(coll); // Assuming this returns List<BsonDocument>

            DataTable dataTable = Misc.ToDataTable(data);

            // Assuming dataGridView1 is the name of your DataGridView control
            gunaDataGridView1.DataSource = dataTable;

            //gunaLabel4.Text = db.GetLatestID(coll).ToString();
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.Show();
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.Show();
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Driver driver = new Driver();
            driver.Show();
        }
    }
}
