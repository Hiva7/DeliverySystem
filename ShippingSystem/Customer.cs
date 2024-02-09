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
        readonly private Database db;
        readonly String coll = "Customer";

        public Customer()
        {
            InitializeComponent();
            db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
        }

        private void Overview_Load(object sender, EventArgs e)
        {
            RefreshDataGridView(coll);

            TotalCustomer.Text = db.GetLatestID(coll).ToString();

            Misc.SetPlaceholder(Search, "The Format is [Attribute: Value]");
            Misc.SetPlaceholder(ID, "Enter ID...");
            Misc.SetPlaceholder(FirstName, "Enter First Name...");
            Misc.SetPlaceholder(LastName, "Enter Last Name...");
            Misc.SetPlaceholder(Contact, "Enter Contact Information...");
        }

        private void Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(ID.Text, out int a))
                {
                    BsonDocument updateFields = new BsonDocument();

                    // Check each textbox and add it to the updateFields if it's not empty
                    if (FirstName.Text != "Enter First Name..." && FirstName.Text != "")
                    {
                        updateFields.Add("First_Name", FirstName.Text);
                    }

                    if (LastName.Text != "Enter Last Name..." && LastName.Text != "")
                    {
                        updateFields.Add("Last_Name", LastName.Text);
                    }

                    if (Contact.Text != "Enter Contact Information..." && Contact.Text != "")
                    {
                        updateFields.Add("Contact", Contact.Text);
                    }

                    // Call the EditRecord method with the dynamically created BsonDocument
                    db.EditRecord(coll, a, updateFields);

                    RefreshDataGridView(coll);

                    Misc.ClearTextBoxes(this);
                }
                else
                {
                    // Handle the parsing error, for example, show a message to the user.
                    MessageBox.Show("Invalid ID. Please enter a valid integer.");
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }


        private void gunaButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.WindowState = FormWindowState.Maximized;
            overview.Show();
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton7_Click(object sender, EventArgs e)
        {
            if (int.TryParse(ID.Text, out int userId))
            {
                // Delete the user with the specified ID
                db.DeleteRecord(coll, userId);

                // Refresh the DataGridView after deletion
                RefreshDataGridView(coll);

                Misc.ClearTextBoxes(this);
            }
            else
            {
                MessageBox.Show("Invalid ID. Please enter a valid integer.");
            }
        }

        private void gunaLabel4_Click(object sender, EventArgs e)
        {
            this.Show();
            Tracking tracking = new Tracking();
            tracking.WindowState = FormWindowState.Maximized;
            tracking.Show();
        }

        private void gunaPictureBox6_Click(object sender, EventArgs e)
        {
            if (Search.Text == "" || Search.Text == "The Format is [Attribute: Value]")
            {
                RefreshDataGridView(coll);
                return;
            }
            try
            {

                // Call the method to extract values
                var result = Misc.ExtractValues(Search.Text);

                DataTable dataTable = new DataTable();

                AddHeader(ref dataTable);

                foreach (DataRow row in Misc.ToDataTable(db.SearchRecord(coll, result.Item1, result.Item2)).Rows)
                {
                    dataTable.ImportRow(row);
                }

                // Assuming dataGridView1 is the name of your DataGridView control
                Data.DataSource = dataTable;

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid format: " + ex.Message + "\ne.g. First_Name: Sovannara");
            }
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

        private void TextToAddCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddCustomer addCus = new AddCustomer();
            addCus.Show();
        }

        private void RefreshDataGridView(String collection)
        {
            var data = db.GetCollection(collection); // Assuming this returns List<BsonDocument>

            // Create a new DataTable
            DataTable dataTable = new DataTable();

            AddHeader(ref dataTable);

            // Add the data to the DataTable
            foreach (var document in data)
            {
                dataTable.Rows.Add(document.Values.ToArray());
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

        private void gunaButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Location location = new Location();
            location.WindowState = FormWindowState.Maximized;
            location.Show();
        }
    }
}
