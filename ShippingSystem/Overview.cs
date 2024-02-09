﻿using DnsClient;
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

        readonly private Database db;
        readonly String coll = "Order";

        public Overview()
        {
            InitializeComponent();
            db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
            this.WindowState = FormWindowState.Maximized;   
        }

        private void Overview_Load(object sender, EventArgs e)
        {
            RefreshDataGridView(coll);

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

        private void gunaButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Location location = new Location();
            location.WindowState = FormWindowState.Maximized;
            location.Show();
        }

        private void RefreshDataGridView(String collection)
        {
            var data = db.GetCollection(collection); // Assuming this returns List<BsonDocument>

            // Create a new DataTable
            DataTable dataTable = new DataTable();

            AddHeader(ref dataTable);

            // Get the current time
            DateTime currentTime = DateTime.UtcNow;

            // Add the data to the DataTable
            foreach (var document in data)
            {
                // Parse the Start_Time from the document
                DateTime startTime = DateTime.Parse(document["Start_Time"].ToString());

                // Calculate the difference between the current time and the start time
                TimeSpan difference = currentTime - startTime;

                // Only add the row to the DataTable if the difference is less than or equal to 24 hours
                if (difference.TotalHours <= 24)
                {
                    dataTable.Rows.Add(document.Values.ToArray());
                }
            }

            // Assuming Data is the name of your GunaDataGridView control
            Data.DataSource = dataTable;

            // Hide the header row
            Data.ColumnHeadersVisible = false;
        }


        private void AddHeader(ref DataTable dataTable)
        {
            var data = db.GetCollection(coll);

            // Add columns to the DataTable
            foreach (var key in data[0].Names)
            {
                dataTable.Columns.Add(key);
            }

            // Add a new row with the attribute names
            dataTable.Rows.Add(dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray());
        }

        private void gunaDataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
