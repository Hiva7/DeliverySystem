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
    public partial class AddOrder : Form
    {
        Database db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
        public AddOrder()
        {
            InitializeComponent();
            TotalOrder.Text = db.GetLatestID("Order").ToString();
            Misc.SetPlaceholder(LocationOne, "Enter Starting Point...");
            Misc.SetPlaceholder(LocationTwo, "Enter Destination...");
            Misc.SetPlaceholder(Price, "Enter Cost($)...");
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.WindowState = FormWindowState.Maximized;
            overview.Show();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            Misc.ClearTextBoxes(this);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                decimal priceDecimal;
                if (decimal.TryParse(Price.Text, out priceDecimal))
                {
                    var price = new BsonDecimal128(priceDecimal);
                    db.AddRecord(
                    "Order",
                    new BsonDocument
                    {
                        { "Status", "Pending" },
                        { "Price",  price }
                    },
                    new BsonDocument { { "Location_id", new BsonArray(new[] { int.Parse(LocationOne.Text), int.Parse(LocationTwo.Text) }) } }
                    );
                }
                else
                {
                    MessageBox.Show("Invalid cost");
                }
                TotalOrder.Text = db.GetLatestID("Order").ToString();
                Misc.ClearTextBoxes(this);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
