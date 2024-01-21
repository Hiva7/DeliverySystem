using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;


namespace ShippingSystem
{

    public partial class Customer : Form
    {
        private Database db;

        public Customer()
        {
            InitializeComponent();
            db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
        }

        private void Overview_Load(object sender, EventArgs e)
        {
            String coll = "Customer";
            var data = db.GetCollection(coll); // Assuming this returns List<BsonDocument>

            // Convert List<BsonDocument> to DataTable
            DataTable dataTable = Misc.ToDataTable(data);

            // Assuming dataGridView1 is the name of your DataGridView control
            gunaDataGridView1.DataSource = dataTable;

            gunaLabel4.Text = db.GetLatestID(coll).ToString();
        }

        // Function to convert List<BsonDocument> to DataTable
        

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gunaTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Update_Click(object sender, EventArgs e)
        {
            int a = int.Parse(TextToId.Text);
            db.EditRecord("Customer", a, new BsonDocument
            {
                {gunaTextBox2.Text ,gunaTextBox3.Text}
            }
                );
            var data = db.GetCollection("Customer"); // Assuming this returns List<BsonDocument>

            // Convert List<BsonDocument> to DataTable
            DataTable dataTable = Misc.ToDataTable(data);

            // Assuming dataGridView1 is the name of your DataGridView control
            gunaDataGridView1.DataSource = dataTable;
        }

        private void gunaLabel1_Click(object sender, EventArgs e)
        {

        }

        private void gunaTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.FormBorderStyle = FormBorderStyle.Sizable;
            overview.WindowState = FormWindowState.Maximized;
            overview.Show();
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton7_Click(object sender, EventArgs e)
        {
            if (int.TryParse(TextToId.Text, out int userId))
            {
                // Delete the user with the specified ID
                db.DeleteRecord("Customer", userId);

                // Refresh the DataGridView after deletion
                RefreshDataGridView();
            }
            else
            {
                MessageBox.Show("Please enter a valid user ID for deletion.");
            }
        }
        private void RefreshDataGridView()
        {
            var data = db.GetCollection("Customer"); // Assuming this returns List<BsonDocument>

            // Convert List<BsonDocument> to DataTable
            DataTable dataTable = Misc.ToDataTable(data);

            // Assuming dataGridView1 is the name of your DataGridView control
            gunaDataGridView1.DataSource = dataTable;
        }

        //private void gunaLabel4_Load(object sender, EventArgs e)
        //{
        //    var data = db.GetCollection("Customer"); // Assuming this returns List<BsonDocument>

        //    // Convert List<BsonDocument> to DataTable
        //    DataTable dataTable = ToDataTable(data);

        //    // Assuming dataGridView1 is the name of your DataGridView control
        //    gunaDataGridView1.DataSource = dataTable;

        //    // Display the total number of customers in the label
        //    var datas = db.GetLatestID("Customer");
        //}

        private void gunaLabel4_Click(object sender, EventArgs e)
        {
            this.Show();
            Tracking tracking = new Tracking();
            tracking.FormBorderStyle = FormBorderStyle.Sizable;
            tracking.WindowState = FormWindowState.Maximized;
            tracking.Show();
        }

        private void gunaTextBox1_TextChanged_1(object sender, EventArgs e)
        {
           

        }

        private void gunaGradient2Panel9_Paint(object sender, PaintEventArgs e)
            
        {

        }

        private void gunaPictureBox6_Click(object sender, EventArgs e)
        {
            if (gunaTextBox1.Text == "")
            {
                String coll = "Customer";
                var data = db.GetCollection(coll); // Assuming this returns List<BsonDocument>

                // Convert List<BsonDocument> to DataTable
                DataTable dataTable = Misc.ToDataTable(data);

                // Assuming dataGridView1 is the name of your DataGridView control
                gunaDataGridView1.DataSource = dataTable;
                return;
            }
            try
            {

                // Call the method to extract values
                var result = Misc.ExtractValues(gunaTextBox1.Text);

                DataTable dataTable = Misc.ToDataTable(db.SearchRecord("Customer", result.Item1,result.Item2));

                // Assuming dataGridView1 is the name of your DataGridView control
                gunaDataGridView1.DataSource = dataTable;

            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid format: " + ex.Message);
            }
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tracking tracking = new Tracking();
            tracking.FormBorderStyle = FormBorderStyle.Sizable;
            tracking.WindowState = FormWindowState.Maximized;
            tracking.Show();
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {

        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Truck truck = new Truck();
            truck.FormBorderStyle = FormBorderStyle.Sizable;
            truck.WindowState = FormWindowState.Maximized;
            truck.Show();
        }

        private void TextToAddCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddCustomer addCus = new AddCustomer();
            addCus.FormBorderStyle = FormBorderStyle.Sizable;
            addCus.Show();
        }
    }
}
