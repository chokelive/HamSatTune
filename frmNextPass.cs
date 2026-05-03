using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.TLE;
using SGPdotNET.Util;

namespace HamSatTune
{
    public class frmNextPass : Form
    {
        private readonly DataGridView grid = new DataGridView();
        private readonly Timer refreshTimer = new Timer();

        private GroundStation groundStation;
        private Dictionary<int, Tle> tleList = new Dictionary<int, Tle>();

        public frmNextPass()
        {
            InitializePassForm();
            LoadData();
            RefreshPasses();

            refreshTimer.Interval = 30000;
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            refreshTimer.Stop();
            refreshTimer.Dispose();
            base.OnFormClosed(e);
        }

        private void InitializePassForm()
        {
            Text = "Next Pass";
            ClientSize = new Size(650, 345);
            MinimumSize = new Size(520, 260);
            StartPosition = FormStartPosition.CenterParent;

            grid.Dock = DockStyle.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.MultiSelect = false;
            grid.BackgroundColor = SystemColors.Window;
            grid.BorderStyle = BorderStyle.Fixed3D;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            grid.GridColor = SystemColors.ControlDark;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 22;
            grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle.Font = grid.Font;
            grid.DefaultCellStyle.BackColor = SystemColors.Window;
            grid.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            grid.DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            grid.DefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
            grid.DefaultCellStyle.Font = grid.Font;
            grid.RowTemplate.Height = 22;
            grid.SelectionChanged += Grid_SelectionChanged;

            AddColumn("Satellite", 180, DataGridViewContentAlignment.MiddleLeft);
            AddColumn("Status", 58, DataGridViewContentAlignment.MiddleCenter);
            AddColumn("Until", 86, DataGridViewContentAlignment.MiddleRight);
            AddColumn("Max El", 56, DataGridViewContentAlignment.MiddleRight);
            AddColumn("AOS", 78, DataGridViewContentAlignment.MiddleRight);
            AddColumn("LOS", 78, DataGridViewContentAlignment.MiddleRight);
            AddColumn("Duration", 70, DataGridViewContentAlignment.MiddleRight);
            AddColumn("AOS Az", 58, DataGridViewContentAlignment.MiddleRight);
            AddColumn("LOS Az", 58, DataGridViewContentAlignment.MiddleRight);

            Controls.Add(grid);
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            grid.ClearSelection();
        }

        private void AddColumn(string header, int width, DataGridViewContentAlignment alignment)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = header;
            column.Width = width;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            column.DefaultCellStyle.Alignment = alignment;
            grid.Columns.Add(column);
        }

        private void LoadData()
        {
            LoadStation();
            LoadTles();
        }

        private void LoadStation()
        {
            string qth = ConfigurationManager.AppSettings["QTH"];
            if (string.IsNullOrWhiteSpace(qth))
            {
                qth = "NK93VT";
            }

            double lat = M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(qth).Lat;
            double lon = M0JIV.MaidenheadLocator.MaidenheadLocatorEngine.GetLatLon(qth).Lon;
            groundStation = new GroundStation(new GeodeticCoordinate(Angle.FromDegrees(lat), Angle.FromDegrees(lon), 0));
        }

        private void LoadTles()
        {
            if (!File.Exists("tles.txt"))
            {
                return;
            }

            LocalTleProvider provider = new LocalTleProvider(true, "tles.txt");
            tleList = provider.GetTles();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshPasses();
        }

        private void RefreshPasses()
        {
            grid.Rows.Clear();

            if (groundStation == null || tleList.Count == 0 || !File.Exists("Doppler.sqf"))
            {
                return;
            }

            DateTime nowUtc = DateTime.UtcNow;
            DateTime endUtc = nowUtc.AddHours(24);
            List<NextPassRow> rows = new List<NextPassRow>();
            HashSet<string> addedSatellites = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Sqf sqf in LoadSqfList())
            {
                if (string.IsNullOrWhiteSpace(sqf.sateName) || !addedSatellites.Add(sqf.sateName))
                {
                    continue;
                }

                Tle tle = tleList.Values.FirstOrDefault(item => string.Equals(item.Name, sqf.sateName, StringComparison.OrdinalIgnoreCase));
                if (tle == null)
                {
                    continue;
                }

                NextPassRow row;
                if (TryBuildNextPassRow(sqf, tle, nowUtc, endUtc, out row))
                {
                    rows.Add(row);
                }
            }

            foreach (NextPassRow row in rows.OrderBy(row => row.Pass.Start))
            {
                int rowIndex = grid.Rows.Add(
                    row.Name,
                    GetPassStatus(row.Pass, nowUtc),
                    FormatUntil(row.Pass, nowUtc),
                    FormatDegrees(row.Pass.MaxElevation.Degrees),
                    FormatPassTime(row.Pass.Start),
                    FormatPassTime(row.Pass.End),
                    FormatDuration(row.Pass.End - row.Pass.Start),
                    FormatDegrees(row.AosAz),
                    FormatDegrees(row.LosAz));

                if (IsPassActive(row.Pass, nowUtc))
                {
                    grid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220);
                    grid.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 255, 220);
                    grid.Rows[rowIndex].DefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
                }
            }

            FitWindowToTable();
        }

        private void FitWindowToTable()
        {
            int visibleColumnWidth = grid.Columns
                .Cast<DataGridViewColumn>()
                .Where(column => column.Visible)
                .Sum(column => column.Width);

            int rowCount = Math.Max(1, grid.Rows.Count);
            int tableWidth = visibleColumnWidth + 6;
            int tableHeight = grid.ColumnHeadersHeight + (rowCount * grid.RowTemplate.Height) + 6;

            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int maxClientHeight = Math.Max(220, Math.Min(workingArea.Height - 90, 720));
            int maxClientWidth = Math.Max(520, workingArea.Width - 90);
            bool needsVerticalScroll = tableHeight > maxClientHeight;

            if (needsVerticalScroll)
            {
                tableWidth += SystemInformation.VerticalScrollBarWidth;
            }

            ClientSize = new Size(
                Math.Max(520, Math.Min(tableWidth, maxClientWidth)),
                Math.Max(120, Math.Min(tableHeight, maxClientHeight)));
        }

        private bool TryBuildNextPassRow(Sqf sqf, Tle tle, DateTime nowUtc, DateTime endUtc, out NextPassRow row)
        {
            row = null;

            try
            {
                Satellite satellite = new Satellite(tle);
                List<SatelliteVisibilityPeriod> passes = groundStation.Observe(
                    satellite,
                    nowUtc,
                    endUtc,
                    TimeSpan.FromSeconds(30),
                    Angle.Zero,
                    true,
                    false,
                    0);

                SatelliteVisibilityPeriod pass = passes.FirstOrDefault(item => item.MaxElevation.Degrees > 0);
                if (pass == null)
                {
                    return false;
                }

                TopocentricObservation aos = groundStation.Observe(satellite, pass.Start);
                TopocentricObservation los = groundStation.Observe(satellite, pass.End);

                row = new NextPassRow
                {
                    Name = sqf.sateName,
                    Pass = pass,
                    AosAz = aos.Azimuth.Degrees,
                    LosAz = los.Azimuth.Degrees
                };
                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<Sqf> LoadSqfList()
        {
            List<Sqf> list = new List<Sqf>();
            foreach (string line in File.ReadLines("Doppler.sqf"))
            {
                Sqf sqf;
                if (TryParseSqf(line, out sqf))
                {
                    list.Add(sqf);
                }
            }

            return list;
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

            sqf.sateName = element[0].Trim();
            sqf.downlinkFreq = downlink;
            sqf.uplinkFreq = uplink;
            sqf.downlinkMode = element[3].Trim();
            sqf.uplinkMode = element[4].Trim();
            sqf.transponderType = element[5].Trim();
            sqf.downlinkOffset = downlinkOffset;
            sqf.uplinkOffset = uplinkOffset;
            sqf.comment = element[8].Trim();
            return true;
        }

        private string FormatUntil(SatelliteVisibilityPeriod pass, DateTime nowUtc)
        {
            TimeSpan until = pass.Start <= nowUtc ? pass.End - nowUtc : pass.Start - nowUtc;
            if (until < TimeSpan.Zero)
            {
                until = TimeSpan.Zero;
            }

            return "- " + FormatDuration(until);
        }

        private string GetPassStatus(SatelliteVisibilityPeriod pass, DateTime nowUtc)
        {
            return IsPassActive(pass, nowUtc) ? "On Air" : "Wait";
        }

        private bool IsPassActive(SatelliteVisibilityPeriod pass, DateTime nowUtc)
        {
            return pass.Start <= nowUtc && pass.End > nowUtc;
        }

        private string FormatDuration(TimeSpan duration)
        {
            int totalHours = (int)Math.Floor(duration.TotalHours);
            return string.Format("{0:00}:{1:00}:{2:00}", totalHours, duration.Minutes, duration.Seconds);
        }

        private string FormatDegrees(double value)
        {
            return string.Format("{0:0}", value) + "°";
        }

        private string FormatPassTime(DateTime utc)
        {
            DateTime local = utc.ToLocalTime();
            string prefix = local.Date == DateTime.Now.Date ? "" : "T ";
            return prefix + local.ToString("HH:mm:ss");
        }

        private class NextPassRow
        {
            public string Name { get; set; }
            public SatelliteVisibilityPeriod Pass { get; set; }
            public double AosAz { get; set; }
            public double LosAz { get; set; }
        }
    }
}
