using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

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
    }
}
