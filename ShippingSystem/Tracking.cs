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

            LatitudeValue.Text = Convert.ToString(gmap.Position.Lat);
            LongitudeValue.Text = Convert.ToString(gmap.Position.Lng);

            gmap.OnPositionChanged += updatePosition;

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

                try
                {
                    GMapMarker marker1 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(firstLocationX), Convert.ToDouble(firstLocationY)), GMarkerGoogleType.red);
                    GMapMarker marker2 = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(secondLocationX), Convert.ToDouble(secondLocationY)), GMarkerGoogleType.red);

                    marker1.IsVisible = true;
                    marker2.IsVisible = true;

                    markers.Markers.Add(marker1);
                    markers.Markers.Add(marker2);

                    // Create the route between the two points
                    MapRoute route = GMap.NET.MapProviders.OpenStreetMapProvider.Instance.GetRoute(
                        new PointLatLng(Convert.ToDouble(firstLocationX), Convert.ToDouble(firstLocationY)),
                        new PointLatLng(Convert.ToDouble(secondLocationX), Convert.ToDouble(secondLocationY)),
                        false, false, 15);

                    // Create a GMapRoute from the points in the MapRoute
                    GMapRoute gmapRoute = new GMapRoute(route.Points, "My route");

                    // Customize the route (optional)
                    gmapRoute.Stroke = new Pen(Color.Blue, 3);

                    // Add the route to the routes overlay
                    routes.Routes.Add(gmapRoute);

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
        public void UpdateTruckPosition(double lat1, double lng1, double lat2, double lng2, double travelTime, GMapMarker truckMarker, GMapRoute truckRoute)
        {
            // Assuming 'start' and 'end' are the positions of your two locations
            PointLatLng start = new PointLatLng(lat1, lng1);
            PointLatLng end = new PointLatLng(lat2, lng2);

            // Get the current time in UTC
            DateTime currentTime = DateTime.UtcNow;

            // Parse the Start_Time from ISO 8601 format
            DateTime Start_Time = DateTime.Parse("2024-01-10T02:55:34.829Z").ToUniversalTime();

            // Calculate the elapsed time since Start_Time in minutes
            double elapsedTime = (currentTime - Start_Time).TotalMinutes;
            // Calculate the progress of the current trip
            double progress = elapsedTime % travelTime;

            // Determine the current direction of the truck
            PointLatLng currentDirection;

            double oneWayTime = travelTime / 2;
            if (progress < oneWayTime)
            {
                // The truck is traveling from location 1 to location 2
                currentDirection = end;
            }
            else
            {
                // The truck is traveling from location 2 to location 1
                currentDirection = start;
                progress = progress - 30; // Reset progress for the second half of the trip
            }

            // Calculate the current position of the truck
            double lat = start.Lat + (currentDirection.Lat - start.Lat) * (progress / oneWayTime);
            double lng = start.Lng + (currentDirection.Lng - start.Lng) * (progress / oneWayTime);
            PointLatLng currentPosition = new PointLatLng(lat, lng);

            // Update the position of the truck's marker
            truckMarker.Position = currentPosition;

            // Update the width of the route
            truckRoute.Stroke.Width = (float)(3 - (3 * (progress / oneWayTime)));

            // Refresh the map
            gmap.Refresh();
        }

        private void InitalizeLocation()
        {
            gmap.Zoom = 7;
            gmap.Position = new PointLatLng(12.3, 104.9806);
        }
        private void GoTo_click(object sender, EventArgs e)
        {
            if(Double.TryParse(latitude.Text, out double result1) && Double.TryParse(longitude.Text, out double result2))
            {
                gmap.Position = new PointLatLng(result1, result2);
                gmap.Update();
                gmap.Refresh();

                latitude.Text = "";
                longitude.Text = "";
            }
            else
            {
                MessageBox.Show("Invalid Value.");
            }
        }



        private void updatePosition(PointLatLng point)
        {
            LatitudeValue.Text = Convert.ToString(point.Lat);
            LongitudeValue.Text = Convert.ToString(point.Lng);
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
            overview.Show();
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer();
            customer.Show();
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Driver driver = new Driver();
            driver.Show();
        }

        
    }
}
