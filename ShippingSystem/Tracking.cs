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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MongoDB.Bson;
using System.Runtime.InteropServices;

namespace ShippingSystem
{
    public partial class Tracking : Form
    {
        private Database db;
        GMap.NET.WindowsForms.GMapControl gmap;

        public Tracking()
        {
            InitializeComponent();
            db = new Database("mongodb+srv://Hiva:Hiva404@cluster0.4nn0wuf.mongodb.net/", "DeliverySystem");
            try
            {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("www.google.com");
            }
            catch
            {
                MessageBox.Show("No internet connection avaible, going to CacheOnly mode.", "GMap.NET - Demo.WindowsForms", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            gmap = new GMap.NET.WindowsForms.GMapControl();
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            gmap.Dock = DockStyle.Fill;
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap.ShowCenter = true;
            gmap.MinZoom = 7;
            gmap.MaxZoom = 20;
            gmap.DragButton = MouseButtons.Left;
            InitalizeLocation();
            Map.Controls.Add(gmap);

            CoordinateValue.Text = Convert.ToString(gmap.Position.Lat) + ", " + Convert.ToString(gmap.Position.Lng);

            gmap.OnPositionChanged += UpdatePosition;
            gmap.OnMapZoomChanged += UpdatePosition2;

            gmap.IgnoreMarkerOnMouseWheel = true;

        }
        public void Tracking_Load(object sender, EventArgs e)
        {
            String coll = "Order";
            var data = db.GetCollection(coll); // Assuming this returns List<BsonDocument>

            DataTable dataTable = Misc.ToDataTable(data);

            // Assuming dataGridView1 is the name of your DataGridView control
            gunaDataGridView1.DataSource = dataTable;

            String coll2 = "Truck";
            var data2 = db.GetField(coll2, "Location_id");

            String coll3 = "Location";

            GMapOverlay markers = new GMapOverlay("markers");
            GMapOverlay routes = new GMapOverlay("routes");

            int i = 1;

            foreach (BsonArray arr in data2)
            {
                var lat = db.SearchRecord(coll3, "Location_id", Convert.ToInt32(arr[0]));

                BsonValue firstLocationX = 0;
                BsonValue firstLocationY = 0;

                foreach (var document in lat)
                {
                    Console.WriteLine(document.ToString());
                    // Check if the "CordX" field exists in the document before accessing it
                    if (document.Contains("CordX"))
                    {
                        firstLocationX = document["CordX"];
                        // Use cordXValue as needed
                        Console.WriteLine($"CordX Value: {firstLocationX}");
                    }
                    else
                    {
                        // Handle the case where "CordX" field is not present in the document
                        Console.WriteLine("CordX field not found in the document");
                    }
                    // Check if the "CordX" field exists in the document before accessing it
                    if (document.Contains("CordY"))
                    {
                        firstLocationY = document["CordY"];
                        // Use cordXValue as needed
                        Console.WriteLine($"CordY Value: {firstLocationY}");
                    }
                    else
                    {
                        // Handle the case where "CordY" field is not present in the document
                        Console.WriteLine("CordY field not found in the document");
                    }
                }
                var lng = db.SearchRecord(coll3, "Location_id", Convert.ToInt32(arr[1]));

                BsonValue secondLocationX = 0;
                BsonValue secondLocationY = 0;

                foreach (var document in lng)
                {
                    Console.WriteLine(document.ToString());
                    // Check if the "CordX" field exists in the document before accessing it
                    if (document.Contains("CordX"))
                    {
                        secondLocationX = document["CordX"];
                        // Use cordXValue as needed
                        Console.WriteLine($"CordX Value: {secondLocationX}");
                    }
                    else
                    {
                        // Handle the case where "CordX" field is not present in the document
                        Console.WriteLine("CordX field not found in the document");
                    }
                    // Check if the "CordX" field exists in the document before accessing it
                    if (document.Contains("CordY"))
                    {
                        secondLocationY = document["CordY"];
                        // Use cordXValue as needed
                        Console.WriteLine($"CordY Value: {secondLocationY}");
                    }
                    else
                    {
                        // Handle the case where "CordY" field is not present in the document
                        Console.WriteLine("CordY field not found in the document");
                    }
                }

                var truckData = db.SearchRecord(coll2, "Truck_id", i);

                DateTime startTime = DateTime.MinValue;
                double travelTime = 0;

                foreach (var document in truckData)
                {
                    Console.WriteLine(document.ToString());

                    // Check if the "Start_Time" field exists in the document before accessing it
                    if (document.Contains("Start_Time"))
                    {
                        BsonDateTime bsonDateTime = document["Start_Time"].AsBsonDateTime;
                        startTime = bsonDateTime.ToUniversalTime();
                        Console.WriteLine($"Start Time: {startTime}");
                    }
                    else
                    {
                        // Handle the case where "Start_Time" field is not present in the document
                        Console.WriteLine("Start_Time field not found in the document");
                    }


                    // Check if the "Travel_Time" field exists in the document before accessing it
                    if (document.Contains("Travel_Time"))
                    {
                        BsonDecimal128 travelTimeBson = document["Travel_Time"].AsDecimal128;
                        travelTime = (double)travelTimeBson.ToDecimal();
                        Console.WriteLine($"Travel Time: {travelTime}");
                    }
                    else
                    {
                        // Handle the case where "Travel_Time" field is not present in the document
                        Console.WriteLine("Travel_Time field not found in the document");
                    }
                }

                try
                {

                    // Get the current time in UTC
                    DateTime currentTime = DateTime.UtcNow;

                    // Calculate the difference in seconds between the current time and the start time
                    double timeDifference = (currentTime - startTime).TotalSeconds;

                    // Calculate the current position
                    double currentPosition = timeDifference % (travelTime - 2);

                    Console.WriteLine($"Current Position: {currentPosition}");

                    // Calculate the percentage of the trip completed
                    double percentageCompleted = currentPosition / travelTime;

                    // Calculate the position of the marker
                    double markerX, markerY;

                    if (percentageCompleted <= 0.5)
                    {
                        // The marker is on the first half of the trip
                        percentageCompleted *= 2; // Adjust the percentage to be out of 100 for the first half
                        markerX = Convert.ToDouble(firstLocationX) + (Convert.ToDouble(secondLocationX) - Convert.ToDouble(firstLocationX)) * percentageCompleted;
                        markerY = Convert.ToDouble(firstLocationY) + (Convert.ToDouble(secondLocationY) - Convert.ToDouble(firstLocationY)) * percentageCompleted;
                    }
                    else
                    {
                        // The marker is on the second half of the trip
                        percentageCompleted = (percentageCompleted - 0.5) * 2; // Adjust the percentage to be out of 100 for the second half
                        markerX = Convert.ToDouble(secondLocationX) + (Convert.ToDouble(firstLocationX) - Convert.ToDouble(secondLocationX)) * percentageCompleted;
                        markerY = Convert.ToDouble(secondLocationY) + (Convert.ToDouble(firstLocationY) - Convert.ToDouble(secondLocationY)) * percentageCompleted;
                    }

                    //Create a Bitmap
                    Bitmap bmpMarker = new Bitmap(20, 20);

                    using (Graphics g = Graphics.FromImage(bmpMarker))
                    {
                        // Draw a lime circle
                        g.FillEllipse(Brushes.Lime, 0, 0, bmpMarker.Width, bmpMarker.Height);

                        // Draw a border
                        Pen borderPen = new Pen(Color.Black, 2); // Change the color and width as needed
                        g.DrawEllipse(borderPen, 1, 1, bmpMarker.Width - 2, bmpMarker.Height - 2);
                    }

                    // Create a custom marker with an offset
                    GMarkerGoogle markerTruck = new GMarkerGoogle(new PointLatLng(markerX, markerY), bmpMarker)
                    {
                        Offset = new Point(-10, -10) // Adjust these values as needed
                    };

                    GMapMarker marker1 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(firstLocationX), Convert.ToDouble(firstLocationY)), GMarkerGoogleType.red);
                    GMapMarker marker2 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(secondLocationX), Convert.ToDouble(secondLocationY)), GMarkerGoogleType.red);

                    markers.Markers.Add(markerTruck);

                    markers.Markers.Add(marker1);
                    markers.Markers.Add(marker2);

                    // Create a new list of points for the first line
                    List<PointLatLng> points1 = new List<PointLatLng>();

                    // Add the coordinates of your markers
                    points1.Add(new PointLatLng(Convert.ToDouble(firstLocationX), Convert.ToDouble(firstLocationY)));
                    points1.Add(new PointLatLng(markerX, markerY));

                    // Create a new GMapRoute using the points
                    GMapRoute gmapRoute1 = new GMapRoute(points1, "My line 1");

                    // Customize the line (optional)
                    gmapRoute1.Stroke = new Pen(Color.Blue, 3);

                    // Add the line to the overlay
                    routes.Routes.Add(gmapRoute1);

                    // Create a new list of points for the second line
                    List<PointLatLng> points2 = new List<PointLatLng>();

                    // Add the coordinates of your markers
                    points2.Add(new PointLatLng(markerX, markerY));
                    points2.Add(new PointLatLng(Convert.ToDouble(secondLocationX), Convert.ToDouble(secondLocationY)));

                    // Create a new GMapRoute using the points
                    GMapRoute gmapRoute2 = new GMapRoute(points2, "My line 2");

                    // Customize the line (optional)
                    gmapRoute2.Stroke = new Pen(Color.Gray, 2);

                    // Add the line to the overlay
                    routes.Routes.Add(gmapRoute2);

                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                }
            }

            gmap.Overlays.Add(markers);
            gmap.Overlays.Add(routes);
            gmap.Zoom = 10;
            gmap.Zoom = 7;
        }

        private void InitalizeLocation()
        {
            gmap.Zoom = 7;
            gmap.Position = new PointLatLng(12.3, 104.9806);
        }

        private void UpdatePosition(PointLatLng point)
        {
            CoordinateValue.Text = Convert.ToString(gmap.Position.Lat) + ", " + Convert.ToString(gmap.Position.Lng);
        }
        private void UpdatePosition2()
        {
            CoordinateValue.Text = Convert.ToString(gmap.Position.Lat) + ", " + Convert.ToString(gmap.Position.Lng);
        }

        static string RemoveLastCharacter(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                // Remove the last character
                return input.Substring(0, input.Length - 1);
            }

            return input;
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Overview overview = new Overview();
            overview.FormBorderStyle = FormBorderStyle.Sizable;
            overview.WindowState = FormWindowState.Maximized;
            overview.Show();
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.FormBorderStyle = FormBorderStyle.Sizable;
            customer.WindowState = FormWindowState.Maximized;
            customer.Show();
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Truck truck = new Truck();
            truck.FormBorderStyle = FormBorderStyle.Sizable;
            truck.WindowState = FormWindowState.Maximized;
            truck.Show();
        }
    }
}
