using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.TLE;
using SGPdotNET.Util;

namespace HamSatTune.Properties
{
    public partial class frmMap : Form
    {
        private readonly Panel headerPanel = new Panel();
        private readonly WorldMapPanel map = new WorldMapPanel();
        private readonly Label lblAzEl = new Label();
        private readonly Label lblTime = new Label();
        private readonly Label lblSatellite = new Label();
        private readonly Label lblDownlink = new Label();
        private readonly Label lblUplink = new Label();
        private readonly Timer mapTimer = new Timer();

        private Dictionary<int, Tle> tleList = new Dictionary<int, Tle>();
        private GroundStation groundStation;
        private int lastTrackingUpdateNumber = -1;

        public frmMap()
        {
            InitializeComponent();
            InitMap();
            LoadStation();
            LoadTles();
            UpdateMap();

            mapTimer.Interval = 100;
            mapTimer.Tick += MapTimer_Tick;
            mapTimer.Start();
        }

        private void frmMap_Load(object sender, EventArgs e)
        {
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            mapTimer.Stop();
            mapTimer.Dispose();
            base.OnFormClosed(e);
        }

        private void InitMap()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Text = "HamSatTune Satellites Map by E29AHU v." + version;
            ClientSize = new Size(720, 430);
            MinimumSize = new Size(420, 280);
            Resize += frmMap_Resize;

            map.Dock = DockStyle.Fill;
            map.LoadBackgroundImage(Path.Combine("maps", "WorldMap3.jpg"));
            Controls.Add(map);

            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.FromArgb(12, 43, 69);
            Controls.Add(headerPanel);
            headerPanel.BringToFront();
            headerPanel.Resize += HeaderPanel_Resize;

            lblSatellite.ForeColor = Color.White;
            lblSatellite.BackColor = Color.Transparent;
            lblSatellite.Font = new Font("Arial", 12, FontStyle.Bold);
            lblSatellite.AutoSize = true;
            lblSatellite.Location = new Point(14, 7);
            lblSatellite.Parent = headerPanel;

            lblAzEl.ForeColor = Color.Aqua;
            lblAzEl.BackColor = Color.Transparent;
            lblAzEl.Font = new Font("Arial", 9, FontStyle.Bold);
            lblAzEl.AutoSize = true;
            lblAzEl.Location = new Point(14, 30);
            lblAzEl.Parent = headerPanel;

            lblDownlink.ForeColor = Color.FromArgb(110, 255, 110);
            lblDownlink.BackColor = Color.Transparent;
            lblDownlink.Font = new Font("Arial", 13, FontStyle.Bold);
            lblDownlink.AutoSize = true;
            lblDownlink.Location = new Point(ClientSize.Width - 260, 8);
            lblDownlink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDownlink.Parent = headerPanel;

            lblUplink.ForeColor = Color.FromArgb(255, 120, 120);
            lblUplink.BackColor = Color.Transparent;
            lblUplink.Font = new Font("Arial", 13, FontStyle.Bold);
            lblUplink.AutoSize = true;
            lblUplink.Location = new Point(ClientSize.Width - 260, 32);
            lblUplink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUplink.Parent = headerPanel;

            lblTime.ForeColor = Color.White;
            lblTime.BackColor = Color.Transparent;
            lblTime.Font = new Font("Arial", 9, FontStyle.Bold);
            lblTime.AutoSize = true;
            lblTime.Location = new Point(150, 30);
            lblTime.Parent = headerPanel;

            ApplyResponsiveLayout();
        }

        private void frmMap_Resize(object sender, EventArgs e)
        {
            ApplyResponsiveLayout();
        }

        private void HeaderPanel_Resize(object sender, EventArgs e)
        {
            PositionHeaderLabels();
        }

        private void ApplyResponsiveLayout()
        {
            double scale = Math.Min(ClientSize.Width / 720.0, ClientSize.Height / 430.0);
            scale = Math.Max(0.65, Math.Min(1.35, scale));

            headerPanel.Height = Math.Max(46, (int)(58 * scale));

            SetLabelFont(lblSatellite, 12 * scale);
            SetLabelFont(lblAzEl, 9 * scale);
            SetLabelFont(lblTime, 9 * scale);
            SetLabelFont(lblDownlink, 13 * scale);
            SetLabelFont(lblUplink, 13 * scale);

            int left = Math.Max(6, (int)(14 * scale));
            lblSatellite.Location = new Point(left, Math.Max(3, (int)(7 * scale)));
            lblAzEl.Location = new Point(left, Math.Max(18, (int)(30 * scale)));
            PositionHeaderLabels();
        }

        private void SetLabelFont(Label label, double size)
        {
            float fontSize = (float)Math.Max(7.0, size);
            if (Math.Abs(label.Font.Size - fontSize) < 0.1)
            {
                return;
            }

            Font oldFont = label.Font;
            label.Font = new Font("Arial", fontSize, FontStyle.Bold);
            oldFont.Dispose();
        }

        private void LoadStation()
        {
            string qth = ConfigurationManager.AppSettings["QTH"];
            if (string.IsNullOrWhiteSpace(qth))
            {
                qth = "NK93VT";
            }
            string callsign = ConfigurationManager.AppSettings["Callsign"];

            double lat = M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(qth).Lat;
            double lon = M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(qth).Lon;
            GeoPoint station = new GeoPoint(lat, lon);

            groundStation = new GroundStation(new GeodeticCoordinate(Angle.FromDegrees(lat), Angle.FromDegrees(lon), 0));
            map.StationPosition = station;
            map.CenterPosition = station;
            map.StationLabel = string.IsNullOrWhiteSpace(callsign) ? qth : callsign.Trim().ToUpperInvariant();
        }

        private void LoadTles()
        {
            if (!File.Exists("tles.txt"))
            {
                lblSatellite.Text = "Missing tles.txt";
                return;
            }

            LocalTleProvider provider = new LocalTleProvider(true, "tles.txt");
            tleList = provider.GetTles();
        }

        private void MapTimer_Tick(object sender, EventArgs e)
        {
            if (Globals.TrackingUpdateNumber == lastTrackingUpdateNumber)
            {
                return;
            }

            UpdateMap();
        }

        private void UpdateMap()
        {
            Tle tle = GetSelectedTle();
            if (tle == null || groundStation == null)
            {
                lblSatellite.Text = "Select a satellite";
                lblAzEl.Text = "AZ: --   EL: --";
                lblDownlink.Text = "";
                lblUplink.Text = "";
                lblTime.Text = GetTrackingTimeText();
                map.Invalidate();
                return;
            }

            lastTrackingUpdateNumber = Globals.TrackingUpdateNumber;

            Satellite sat = new Satellite(tle);
            DateTime now = Globals.LastTrackingUpdateTime == DateTime.MinValue
                ? DateTime.UtcNow
                : Globals.LastTrackingUpdateTime.ToUniversalTime();
            GeodeticCoordinate satGeo = sat.Predict(now).ToGeodetic();
            TopocentricObservation observation = groundStation.Observe(sat, now);

            map.SatellitePosition = ToGeoPoint(satGeo);
            map.SatelliteLabel = tle.Name;
            map.GroundTrack = BuildGroundTrack(sat, now);
            map.Footprint = satGeo.GetFootprintBoundary(90).Select(ToGeoPoint).ToList();

            lblSatellite.Text = tle.Name;
            lblAzEl.Text = string.Format("Az {0:0.00}°   El {1:0.00}°", observation.Azimuth.Degrees, observation.Elevation.Degrees);
            SetFrequencyText();
            lblTime.Text = GetTrackingTimeText() + " (LOC)";
            PositionHeaderLabels();

            map.Invalidate();
        }

        private Tle GetSelectedTle()
        {
            Sqf selectedSqf;
            if (!TryGetSelectedSqf(out selectedSqf))
            {
                return null;
            }

            return tleList.Values.FirstOrDefault(tle => string.Equals(tle.Name, selectedSqf.sateName, StringComparison.OrdinalIgnoreCase));
        }

        private void SetFrequencyText()
        {
            Sqf selectedSqf;
            if (!TryGetSelectedSqf(out selectedSqf))
            {
                lblDownlink.Text = "";
                lblUplink.Text = "";
                return;
            }

            int downlinkHz = Globals.CalculatedDownlinkHz > 0
                ? Globals.CalculatedDownlinkHz
                : (int)(selectedSqf.downlinkFreq * 1000);
            int uplinkHz = Globals.CalculatedUplinkHz > 0
                ? Globals.CalculatedUplinkHz
                : (int)(selectedSqf.uplinkFreq * 1000);

            lblDownlink.Text = string.Format("DN {0} {1}", FormatFrequency(downlinkHz), selectedSqf.downlinkMode);
            lblUplink.Text = string.Format("UP {0} {1}", FormatFrequency(uplinkHz), selectedSqf.uplinkMode);
            lblSatellite.Text += " " + selectedSqf.comment;
        }

        private string GetTrackingTimeText()
        {
            DateTime time = Globals.LastTrackingUpdateTime == DateTime.MinValue
                ? DateTime.Now
                : Globals.LastTrackingUpdateTime;
            return time.ToString("HH:mm:ss");
        }

        private string FormatFrequency(int hz)
        {
            int mhz = hz / 1000000;
            int khz = Math.Abs((hz / 1000) % 1000);
            int tenHz = Math.Abs((hz / 10) % 100);
            return string.Format("{0}.{1:000}.{2:00}", mhz, khz, tenHz);
        }

        private void PositionFrequencyLabels()
        {
            int rightMargin = Math.Max(8, (int)(18 * (headerPanel.Height / 68.0)));
            int leftLimit = Math.Max(170, lblTime.Right + 12);
            int xDown = headerPanel.ClientSize.Width - lblDownlink.Width - rightMargin;
            int xUp = headerPanel.ClientSize.Width - lblUplink.Width - rightMargin;
            lblDownlink.Location = new Point(Math.Max(leftLimit, xDown), Math.Max(4, (int)(8 * (headerPanel.Height / 68.0))));
            lblUplink.Location = new Point(Math.Max(leftLimit, xUp), Math.Max(22, (int)(32 * (headerPanel.Height / 68.0))));
        }

        private void PositionTimeLabel()
        {
            lblTime.Location = new Point(lblAzEl.Right + 8, lblAzEl.Top);
        }

        private void PositionHeaderLabels()
        {
            PositionTimeLabel();
            PositionFrequencyLabels();
        }

        private bool TryGetSelectedSqf(out Sqf selectedSqf)
        {
            selectedSqf = Globals.CurrentSqf;
            if (!string.IsNullOrWhiteSpace(selectedSqf.sateName))
            {
                return true;
            }

            return TryGetFirstSqf(out selectedSqf);
        }

        private bool TryGetFirstSqf(out Sqf selectedSqf)
        {
            selectedSqf = new Sqf();
            if (!File.Exists("Doppler.sqf"))
            {
                return false;
            }

            string firstLine = File.ReadLines("Doppler.sqf").FirstOrDefault(line => !string.IsNullOrWhiteSpace(line));
            return TryParseSqf(firstLine, out selectedSqf);
        }

        private bool TryParseSqf(string line, out Sqf sqf)
        {
            sqf = new Sqf();
            if (string.IsNullOrWhiteSpace(line))
            {
                return false;
            }

            string[] element = line.Split(',');
            if (element.Length < 9)
            {
                return false;
            }

            double downlink;
            double uplink;
            double downlinkOffset;
            double uplinkOffset;
            if (!double.TryParse(element[1], out downlink) ||
                !double.TryParse(element[2], out uplink) ||
                !double.TryParse(element[6], out downlinkOffset) ||
                !double.TryParse(element[7], out uplinkOffset))
            {
                return false;
            }

            sqf.sateName = element[0];
            sqf.downlinkFreq = downlink;
            sqf.uplinkFreq = uplink;
            sqf.downlinkMode = element[3];
            sqf.uplinkMode = element[4];
            sqf.transponderType = element[5];
            sqf.downlinkOffset = downlinkOffset;
            sqf.uplinkOffset = uplinkOffset;
            sqf.comment = element[8];
            return true;
        }

        private List<GeoPoint> BuildGroundTrack(Satellite sat, DateTime now)
        {
            List<GeoPoint> points = new List<GeoPoint>();
            for (int minutes = -90; minutes <= 90; minutes += 2)
            {
                points.Add(ToGeoPoint(sat.Predict(now.AddMinutes(minutes)).ToGeodetic()));
            }

            return points;
        }

        private GeoPoint ToGeoPoint(GeodeticCoordinate coordinate)
        {
            return new GeoPoint(coordinate.Latitude.Degrees, coordinate.Longitude.Degrees);
        }

        private struct GeoPoint
        {
            public readonly double Lat;
            public readonly double Lon;

            public GeoPoint(double lat, double lon)
            {
                Lat = lat;
                Lon = lon;
            }
        }

        private class WorldMapPanel : Panel
        {
            public GeoPoint StationPosition { get; set; }
            public GeoPoint SatellitePosition { get; set; }
            public GeoPoint CenterPosition { get; set; }
            public string StationLabel { get; set; }
            public string SatelliteLabel { get; set; }
            public List<GeoPoint> GroundTrack { get; set; } = new List<GeoPoint>();
            public List<GeoPoint> Footprint { get; set; } = new List<GeoPoint>();

            private readonly Font labelFont = new Font("Arial", 8, FontStyle.Bold);
            private readonly SolidBrush labelBrush = new SolidBrush(Color.White);
            private Image backgroundImage;

            public WorldMapPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            public void LoadBackgroundImage(string relativePath)
            {
                string[] candidates =
                {
                    Path.Combine(Application.StartupPath, relativePath),
                    Path.Combine(Environment.CurrentDirectory, relativePath),
                    Path.Combine(Environment.CurrentDirectory, "..", "..", relativePath)
                };

                foreach (string candidate in candidates)
                {
                    string path = Path.GetFullPath(candidate);
                    if (!File.Exists(path))
                    {
                        continue;
                    }

                    using (Image sourceImage = Image.FromFile(path))
                    {
                        backgroundImage = new Bitmap(sourceImage);
                    }
                    return;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                DrawOcean(g);
                DrawMapBackground(g);
                DrawGrid(g);
                DrawFootprintShade(g);
                DrawFootprint(g);
                DrawGroundTrack(g);
                DrawMarker(g, StationPosition, Color.DeepSkyBlue, StationLabel, 7);
                DrawMarker(g, SatellitePosition, Color.Lime, SatelliteLabel, 8);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    backgroundImage?.Dispose();
                    labelFont.Dispose();
                    labelBrush.Dispose();
                }

                base.Dispose(disposing);
            }

            private void DrawOcean(Graphics g)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(25, 64, 103), Color.FromArgb(5, 26, 48), LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, ClientRectangle);
                }
            }

            private void DrawMapBackground(Graphics g)
            {
                if (backgroundImage == null)
                {
                    return;
                }

                RectangleF bounds = GetMapBounds();
                float centerX = bounds.Left + (float)((CenterPosition.Lon + 180.0) / 360.0 * bounds.Width);
                float centerY = bounds.Top + (float)((90.0 - CenterPosition.Lat) / 180.0 * bounds.Height);
                float drawX = bounds.Left + bounds.Width / 2.0f - (centerX - bounds.Left);
                float drawY = bounds.Top + bounds.Height / 2.0f - (centerY - bounds.Top);

                for (int i = -1; i <= 1; i++)
                {
                    g.DrawImage(backgroundImage, new RectangleF(drawX + i * bounds.Width, drawY, bounds.Width, bounds.Height));
                }
            }

            private void DrawGrid(Graphics g)
            {
                using (Pen majorPen = new Pen(Color.FromArgb(42, Color.White), 1))
                using (Pen equatorPen = new Pen(Color.FromArgb(68, Color.White), 1))
                {
                    for (int lon = -180; lon <= 180; lon += 60)
                    {
                        g.DrawLine(majorPen, Project(new GeoPoint(-90, lon)), Project(new GeoPoint(90, lon)));
                    }

                    for (int lat = -60; lat <= 60; lat += 30)
                    {
                        g.DrawLine(lat == 0 ? equatorPen : majorPen, Project(new GeoPoint(lat, -180)), Project(new GeoPoint(lat, 180)));
                    }
                }
            }

            private void DrawFootprint(Graphics g)
            {
                if (Footprint == null || Footprint.Count < 3)
                {
                    return;
                }

                using (Pen pen = new Pen(Color.Yellow, 1.4f))
                {
                    DrawWrappedPolyline(g, pen, Footprint, true);
                }
            }

            private void DrawFootprintShade(Graphics g)
            {
                if (Footprint == null || Footprint.Count < 3)
                {
                    return;
                }

                List<PointF> ring = GetUnwrappedProjectedRing(Footprint);
                if (ring.Count < 3)
                {
                    return;
                }

                using (SolidBrush outsideBrush = new SolidBrush(Color.FromArgb(78, Color.Black)))
                {
                    g.FillRectangle(outsideBrush, ClientRectangle);
                }

                using (SolidBrush footprintBrush = new SolidBrush(Color.FromArgb(75, Color.White)))
                {
                    FillWrappedPolygon(g, footprintBrush, ring, -1);
                    FillWrappedPolygon(g, footprintBrush, ring, 0);
                    FillWrappedPolygon(g, footprintBrush, ring, 1);
                }
            }

            private void DrawGroundTrack(Graphics g)
            {
                if (GroundTrack == null || GroundTrack.Count < 2)
                {
                    return;
                }

                using (Pen pen = new Pen(Color.FromArgb(230, Color.White), 1.4f))
                {
                    DrawWrappedPolyline(g, pen, GroundTrack, false);
                }
            }

            private List<PointF> GetUnwrappedProjectedRing(List<GeoPoint> points)
            {
                List<PointF> ring = new List<PointF>();
                RectangleF bounds = GetMapBounds();
                double previousDelta = WrapLongitude(points[0].Lon - CenterPosition.Lon);

                for (int i = 0; i < points.Count; i++)
                {
                    double lonDelta = WrapLongitude(points[i].Lon - CenterPosition.Lon);
                    while (lonDelta - previousDelta > 180)
                    {
                        lonDelta -= 360;
                    }

                    while (lonDelta - previousDelta < -180)
                    {
                        lonDelta += 360;
                    }

                    previousDelta = lonDelta;

                    double lat = Math.Max(-90, Math.Min(90, points[i].Lat));
                    double centerLat = Math.Max(-90, Math.Min(90, CenterPosition.Lat));
                    float x = bounds.Left + bounds.Width / 2.0f + (float)(lonDelta / 360.0 * Math.Max(1, bounds.Width - 1));
                    float y = bounds.Top + bounds.Height / 2.0f - (float)((lat - centerLat) / 180.0 * Math.Max(1, bounds.Height - 1));
                    ring.Add(new PointF(x, y));
                }

                return ring;
            }

            private void FillWrappedPolygon(Graphics g, Brush brush, List<PointF> ring, int xOffset)
            {
                PointF[] polygon = ring.Select(point => new PointF(point.X + xOffset * Width, point.Y)).ToArray();
                RectangleF bounds = GetPolygonBounds(polygon);
                if (bounds.Right < 0 || bounds.Left > Width || bounds.Bottom < 0 || bounds.Top > Height)
                {
                    return;
                }

                g.FillPolygon(brush, polygon);
            }

            private RectangleF GetPolygonBounds(PointF[] polygon)
            {
                float left = polygon.Min(point => point.X);
                float right = polygon.Max(point => point.X);
                float top = polygon.Min(point => point.Y);
                float bottom = polygon.Max(point => point.Y);
                return RectangleF.FromLTRB(left, top, right, bottom);
            }

            private void DrawWrappedPolyline(Graphics g, Pen pen, List<GeoPoint> points, bool closed)
            {
                int count = closed ? points.Count : points.Count - 1;
                for (int i = 0; i < count; i++)
                {
                    PointF a = Project(points[i]);
                    PointF b = Project(points[(i + 1) % points.Count]);

                    if (Math.Abs(b.X - a.X) > Width / 2.0f || Math.Abs(b.Y - a.Y) > Height / 2.0f)
                    {
                        continue;
                    }

                    g.DrawLine(pen, a, b);
                }
            }

            private List<List<PointF>> GetProjectedSegments(List<GeoPoint> points, bool closed)
            {
                List<List<PointF>> segments = new List<List<PointF>>();
                List<PointF> current = new List<PointF>();
                int count = closed ? points.Count + 1 : points.Count;

                for (int i = 0; i < count; i++)
                {
                    PointF p = Project(points[i % points.Count]);
                    if (current.Count > 0)
                    {
                        PointF previous = current[current.Count - 1];
                        if (Math.Abs(p.X - previous.X) > Width / 2.0f || Math.Abs(p.Y - previous.Y) > Height / 2.0f)
                        {
                            if (current.Count >= 3)
                            {
                                segments.Add(current);
                            }

                            current = new List<PointF>();
                        }
                    }

                    current.Add(p);
                }

                if (current.Count >= 3)
                {
                    segments.Add(current);
                }

                return segments;
            }

            private void DrawMarker(Graphics g, GeoPoint point, Color color, string label, int radius)
            {
                PointF p = Project(point);
                RectangleF marker = new RectangleF(p.X - radius, p.Y - radius, radius * 2, radius * 2);

                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    g.FillEllipse(brush, marker);
                    g.DrawEllipse(pen, marker);
                }

                if (!string.IsNullOrWhiteSpace(label))
                {
                    g.DrawString(label, labelFont, labelBrush, p.X + radius + 3, p.Y - radius - 1);
                }
            }

            private PointF Project(GeoPoint point)
            {
                RectangleF bounds = GetMapBounds();
                double lonDelta = WrapLongitude(point.Lon - CenterPosition.Lon);
                double latDelta = Math.Max(-90, Math.Min(90, point.Lat)) - Math.Max(-90, Math.Min(90, CenterPosition.Lat));
                float x = bounds.Left + bounds.Width / 2.0f + (float)(lonDelta / 360.0 * Math.Max(1, bounds.Width - 1));
                float y = bounds.Top + bounds.Height / 2.0f - (float)(latDelta / 180.0 * Math.Max(1, bounds.Height - 1));
                return new PointF(x, y);
            }

            private double WrapLongitude(double lon)
            {
                while (lon < -180)
                {
                    lon += 360;
                }

                while (lon > 180)
                {
                    lon -= 360;
                }

                return lon;
            }

            private RectangleF GetMapBounds()
            {
                return Width <= 0 || Height <= 0
                    ? new RectangleF(0, 0, 1, 1)
                    : new RectangleF(0, 0, Width, Height);
            }
        }
    }
}
