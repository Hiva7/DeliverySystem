using MongoDB.Bson;
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
        readonly private Database db;
        readonly String coll = "Truck";
        public Truck()
        {
            InitializeComponent();
            db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
        }

        private void Truck_Load(object sender, EventArgs e)
        {
            RefreshDataGridView(coll);

            TotalTruck.Text = db.GetLatestID("Truck").ToString();

            Misc.SetPlaceholder(Search, "The Format is [Attribute: Value]");
            Misc.SetPlaceholder(ID, "Enter ID...");
            Misc.SetPlaceholder(LocationOne, "Enter Starting Point ID...");
            Misc.SetPlaceholder(LocationTwo, "Enter Destination ID...");
            Misc.SetPlaceholder(TravelTime, "Enter Time Required for a Round-trip...");
        }

        private void Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(ID.Text, out int a))
                {
                    BsonDocument updateFields = new BsonDocument();
                    BsonDocument updateRelationship = new BsonDocument();

                    List<BsonDocument> data = db.SearchRecord(coll, "Truck_id", a);

                    foreach (BsonDocument document in data)
                    {
                        if (document.Contains("Location_id"))
                        {
                            BsonArray locationIdArray = document["Location_id"].AsBsonArray;

                            if (locationIdArray.Count == 2)
                            {
                                // Update the first element if LocationOne.Text meets conditions
                                if (LocationOne.Text != "Enter Starting Point ID..." && LocationOne.Text != "")
                                {
                                    // Assuming LocationOne.Text can be parsed to an integer
                                    if (int.TryParse(LocationOne.Text, out int locationOneValue))
                                    {
                                        locationIdArray[0] = locationOneValue;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Use ID when inputting location.");
                                    }
                                }

                                // Update the second element if LocationTwo.Text meets conditions
                                if (LocationTwo.Text != "Enter Destination ID..." && LocationTwo.Text != "")
                                {
                                    // Assuming LocationTwo.Text can be parsed to an integer
                                    if (int.TryParse(LocationTwo.Text, out int locationTwoValue))
                                    {
                                        locationIdArray[1] = locationTwoValue;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Use ID when inputting location.");
                                    }
                                }

                                // Add the updated "Location_id" array to the updateRelationship document
                                updateRelationship.Add("Location_id", locationIdArray);
                            }

                            break;
                        }
                    }

                    if (TravelTime.Text != "Enter Time Required for a Round-trip..." && TravelTime.Text != "")
                    {
                        // Convert the string to a decimal type
                        decimal travelTimeDecimal;
                        if (decimal.TryParse(TravelTime.Text, out travelTimeDecimal))
                        {
                            // Create a BsonDecimal128 from the decimal value
                            var travelTimeValue = BsonDecimal128.Create(travelTimeDecimal);
                            updateFields.Add("Travel_Time", travelTimeValue);

                            db.EditRecord(coll, a, updateFields);
                        }
                        else
                        {
                            MessageBox.Show("Travel Time needs to be an integer or a decimal.");
                        }

                    }

                    db.EditRelationship(coll, a, updateRelationship);

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

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.WindowState = FormWindowState.Maximized;
            customer.Show();
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tracking tracking = new Tracking();
            tracking.WindowState = FormWindowState.Maximized;
            tracking.Show();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.WindowState = FormWindowState.Maximized;
            overview.Show();
        }

        private void TextToAddDriver_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddTruck addTruck = new AddTruck();
            addTruck.Show();
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
