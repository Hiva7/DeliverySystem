using Guna.UI.WinForms;
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
using MongoDB.Driver;

namespace ShippingSystem
{
    public partial class AddTruck : Form
    {
        Database db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
        public AddTruck()
        {
            InitializeComponent();
            TotalTruck.Text = db.GetLatestID("Truck").ToString();
            Misc.SetPlaceholder(LocationOne, "Enter Starting Point ID...");
            Misc.SetPlaceholder(LocationTwo, "Enter Destination ID...");
            Misc.SetPlaceholder(TravelTime, "Enter Time Needed For A Round-Trip(s)...");
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Truck truck = new Truck();
            truck.WindowState = FormWindowState.Maximized;
            truck.Show();
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            Misc.ClearTextBoxes(this);
        }
        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                decimal travelTimeDecimal;
                if (decimal.TryParse(TravelTime.Text, out travelTimeDecimal))
                {
                    var travelTime = new BsonDecimal128(travelTimeDecimal);
                    db.AddRecord(
                    "Truck",
                    new BsonDocument
                    {
                        { "Start_Time", DateTime.Now },
                        { "Travel_Time", travelTime }
                    },
                    new BsonDocument { { "Location_id", new BsonArray(new[] { int.Parse(LocationOne.Text), int.Parse(LocationTwo.Text) }) } }
                    );
                }
                else
                {
                    MessageBox.Show("Invalid travel time");
                }
                TotalTruck.Text = db.GetLatestID("Truck").ToString();
                Misc.ClearTextBoxes(this);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
