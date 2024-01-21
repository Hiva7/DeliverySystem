using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Windows.Forms;
using Guna.UI.WinForms;

namespace ShippingSystem
{
    static internal class Misc
    {
        public static DataTable ToDataTable(List<BsonDocument> bsonDocuments)
        {
            DataTable dataTable = new DataTable();

            // Assuming all documents in the list have the same structure
            if (bsonDocuments.Count > 0)
            {
                foreach (BsonElement element in bsonDocuments[0].Elements)
                {
                    // Add columns to DataTable
                    dataTable.Columns.Add(element.Name, typeof(string));
                }

                foreach (var bsonDocument in bsonDocuments)
                {
                    // Add data rows
                    var row = dataTable.NewRow();
                    foreach (BsonElement element in bsonDocument.Elements)
                    {
                        row[element.Name] = element.Value.ToString();
                    }
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }
        public static Tuple<string, string> ExtractValues(string input)
        {
            // Split the input string by the colon and space
            string[] parts = input.Split(new[] { ": " }, StringSplitOptions.None);

            // Check if there are exactly two parts
            if (parts.Length == 2)
            {
                // Return a Tuple with the values
                return Tuple.Create(parts[0], parts[1]);
            }
            else
            {
                // Throw a FormatException if the format is incorrect
                throw new FormatException("Invalid format. Expected format: 'Key: Value'");
            }
        }
        public static void SetPlaceholder(GunaTextBox textBox, string placeholderText)
        {
            textBox.Text = placeholderText;
            textBox.ForeColor = Color.Black;

            textBox.GotFocus += RemoveText(placeholderText);
            textBox.LostFocus += AddText(placeholderText);
        }

        public static EventHandler RemoveText(string placeholderText)
        {
            return (sender, e) =>
            {
                GunaTextBox textBox = sender as GunaTextBox;
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };
        }

        public static EventHandler AddText(string placeholderText)
        {
            return (sender, e) =>
            {
                GunaTextBox textBox = sender as GunaTextBox;
                if (string.IsNullOrWhiteSpace(textBox.Text))
                    textBox.Text = placeholderText;
            };
        }
        public static void ClearTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is GunaTextBox)
                {
                    ((GunaTextBox)c).Text = "";
                    c.Focus();
                    control.Focus();
                }
                else
                {
                    ClearTextBoxes(c);
                }
            }
        }
    }
}
