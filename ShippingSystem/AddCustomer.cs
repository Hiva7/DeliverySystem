using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon.SecurityToken.Model;
using MongoDB.Bson;

namespace ShippingSystem
{
    public partial class AddCustomer : Form
    {
        Database db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
        public AddCustomer()
        {
            InitializeComponent();
            TotalCustomer.Text = db.GetLatestID("Customer").ToString();
            Misc.SetPlaceholder(FirstName, "Enter First Name...");
            Misc.SetPlaceholder(LastName, "Enter Last Name...");
            Misc.SetPlaceholder(Contact, "Enter Contact Information...");
        }


        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.WindowState = FormWindowState.Maximized;
            customer.Show();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            Misc.ClearTextBoxes(this);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                var firstName = new BsonString(FirstName.Text);
                var lastName = new BsonString(LastName.Text);
                var contact = new BsonString(Contact.Text);
                db.AddRecord(
                    "Customer",
                    new BsonDocument
                    {
                        { "First_Name", firstName },
                        { "Last_Name", lastName },
                        { "Contact", contact }
                    }
                );
                TotalCustomer.Text = db.GetLatestID("Customer").ToString();
                Misc.ClearTextBoxes(this);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
