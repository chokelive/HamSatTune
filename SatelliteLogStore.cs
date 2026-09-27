using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace HamSatTune
{
    public class SatelliteLogStore
    {
        private const string LogDirectoryName = "logs";
        private const string DatabaseFileName = "satellite_log.db";
        private static Dictionary<string, List<TqslSatelliteName>> tqslSatelliteNames;
        private readonly string databasePath;

        public SatelliteLogStore()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogDirectoryName);
            Directory.CreateDirectory(logDirectory);
            databasePath = Path.Combine(logDirectory, DatabaseFileName);
            EnsureDatabase();
        }

        public void Add(SatelliteLogEntry entry)
        {
            using (SQLiteConnection connection = OpenConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    @"INSERT INTO qsos
                      (utc_time, callsign, rst_sent, rst_received, grid, name, satellite, mode, submode, downlink_hz, uplink_hz, comment)
                      VALUES
                      (@utc_time, @callsign, @rst_sent, @rst_received, @grid, @name, @satellite, @mode, @submode, @downlink_hz, @uplink_hz, @comment)";
                AddParameters(command, entry);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(long id)
        {
            using (SQLiteConnection connection = OpenConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM qsos WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<SatelliteLogEntry> LoadRecent(int limit)
        {
            List<SatelliteLogEntry> entries = new List<SatelliteLogEntry>();
            using (SQLiteConnection connection = OpenConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    @"SELECT id, utc_time, callsign, rst_sent, rst_received, grid, name, satellite, mode, submode, downlink_hz, uplink_hz, comment
                      FROM qsos
                      ORDER BY utc_time DESC, id DESC
                      LIMIT @limit";
                command.Parameters.AddWithValue("@limit", limit);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(ReadEntry(reader));
                    }
                }
            }

            return entries;
        }

        public void ExportAdif(string path)
        {
            List<SatelliteLogEntry> entries = LoadAllOldestFirst();
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("HamSatTune Satellite Log");
                writer.WriteLine("<ADIF_VER:5>3.1.4");
                writer.WriteLine("<PROGRAMID:10>HamSatTune");
                writer.WriteLine("<EOH>");

                foreach (SatelliteLogEntry entry in entries)
                {
                    writer.Write(AdifField("QSO_DATE", entry.UtcTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture)));
                    writer.Write(AdifField("TIME_ON", entry.UtcTime.ToString("HHmmss", CultureInfo.InvariantCulture)));
                    writer.Write(AdifField("CALL", entry.Callsign));
                    writer.Write(AdifField("RST_SENT", entry.RstSent));
                    writer.Write(AdifField("RST_RCVD", entry.RstReceived));
                    writer.Write(AdifField("MY_GRIDSQUARE", entry.Grid));
                    writer.Write(AdifField("NAME", entry.Name));
                    writer.Write(AdifField("SAT_NAME", ResolveLotwSatelliteName(entry.Satellite, entry.UtcTime)));
                    writer.Write(AdifField("PROP_MODE", "SAT"));
                    writer.Write(AdifField("SAT_MODE", GetSatelliteMode(entry.UplinkHz, entry.DownlinkHz)));
                    writer.Write(AdifField("MODE", GetAdifMode(entry)));
                    writer.Write(AdifField("SUBMODE", GetAdifSubmode(entry)));
                    writer.Write(AdifField("FREQ", FormatMhz(entry.UplinkHz)));
                    writer.Write(AdifField("BAND", GetAdifBand(entry.UplinkHz)));
                    writer.Write(AdifField("FREQ_RX", FormatMhz(entry.DownlinkHz)));
                    writer.Write(AdifField("BAND_RX", GetAdifBand(entry.DownlinkHz)));
                    writer.Write(AdifField("COMMENT", entry.Comment));
                    writer.WriteLine("<EOR>");
                }
            }
        }

        private void EnsureDatabase()
        {
            using (SQLiteConnection connection = OpenConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS qsos (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        utc_time TEXT NOT NULL,
                        callsign TEXT NOT NULL,
                        rst_sent TEXT NOT NULL,
                        rst_received TEXT NOT NULL,
                        grid TEXT,
                        name TEXT,
                        satellite TEXT,
                        mode TEXT,
                        submode TEXT,
                        downlink_hz INTEGER NOT NULL DEFAULT 0,
                        uplink_hz INTEGER NOT NULL DEFAULT 0,
                        comment TEXT
                      )";
                command.ExecuteNonQuery();
                EnsureColumn(connection, "qsos", "submode", "TEXT");
            }
        }

        private SQLiteConnection OpenConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + databasePath + ";Version=3;");
            connection.Open();
            return connection;
        }

        private void EnsureColumn(SQLiteConnection connection, string tableName, string columnName, string definition)
        {
            using (SQLiteCommand checkCommand = connection.CreateCommand())
            {
                checkCommand.CommandText = "PRAGMA table_info(" + tableName + ")";
                using (SQLiteDataReader reader = checkCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }
                    }
                }
            }

            using (SQLiteCommand alterCommand = connection.CreateCommand())
            {
                alterCommand.CommandText = "ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + definition;
                alterCommand.ExecuteNonQuery();
            }
        }

        private void AddParameters(SQLiteCommand command, SatelliteLogEntry entry)
        {
            command.Parameters.AddWithValue("@utc_time", entry.UtcTime.ToString("o", CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@callsign", entry.Callsign ?? "");
            command.Parameters.AddWithValue("@rst_sent", entry.RstSent ?? "");
            command.Parameters.AddWithValue("@rst_received", entry.RstReceived ?? "");
            command.Parameters.AddWithValue("@grid", entry.Grid ?? "");
            command.Parameters.AddWithValue("@name", entry.Name ?? "");
            command.Parameters.AddWithValue("@satellite", entry.Satellite ?? "");
            command.Parameters.AddWithValue("@mode", entry.Mode ?? "");
            command.Parameters.AddWithValue("@submode", entry.Submode ?? "");
            command.Parameters.AddWithValue("@downlink_hz", entry.DownlinkHz);
            command.Parameters.AddWithValue("@uplink_hz", entry.UplinkHz);
            command.Parameters.AddWithValue("@comment", entry.Comment ?? "");
        }

        private SatelliteLogEntry ReadEntry(SQLiteDataReader reader)
        {
            DateTime utcTime;
            if (!DateTime.TryParse(reader.GetString(1), null, DateTimeStyles.RoundtripKind, out utcTime))
            {
                utcTime = DateTime.UtcNow;
            }

            return new SatelliteLogEntry
            {
                Id = reader.GetInt64(0),
                UtcTime = utcTime.ToUniversalTime(),
                Callsign = reader.GetString(2),
                RstSent = reader.GetString(3),
                RstReceived = reader.GetString(4),
                Grid = reader.GetString(5),
                Name = reader.GetString(6),
                Satellite = reader.GetString(7),
                Mode = reader.GetString(8),
                Submode = reader.IsDBNull(9) ? "" : reader.GetString(9),
                DownlinkHz = reader.GetInt32(10),
                UplinkHz = reader.GetInt32(11),
                Comment = reader.GetString(12)
            };
        }

        private List<SatelliteLogEntry> LoadAllOldestFirst()
        {
            List<SatelliteLogEntry> entries = new List<SatelliteLogEntry>();
            using (SQLiteConnection connection = OpenConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    @"SELECT id, utc_time, callsign, rst_sent, rst_received, grid, name, satellite, mode, submode, downlink_hz, uplink_hz, comment
                      FROM qsos
                      ORDER BY utc_time ASC, id ASC";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(ReadEntry(reader));
                    }
                }
            }

            return entries;
        }

        private string AdifField(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            return "<" + name + ":" + value.Length.ToString(CultureInfo.InvariantCulture) + ">" + value;
        }

        private string ResolveLotwSatelliteName(string satelliteName, DateTime qsoUtcTime)
        {
            if (string.IsNullOrWhiteSpace(satelliteName))
            {
                return "";
            }

            string configuredName = GetConfiguredSatelliteName(satelliteName);
            if (!string.IsNullOrWhiteSpace(configuredName))
            {
                return configuredName;
            }

            string normalizedName = NormalizeSatelliteName(satelliteName);
            if (normalizedName == "ISS")
            {
                return "ARISS";
            }

            Dictionary<string, List<TqslSatelliteName>> names = GetTqslSatelliteNames();
            List<TqslSatelliteName> matches;
            if (names.TryGetValue(normalizedName, out matches))
            {
                TqslSatelliteName validMatch = matches.Find(item => item.IsValidFor(qsoUtcTime));
                return validMatch == null ? matches[0].LotwName : validMatch.LotwName;
            }

            return satelliteName.Trim();
        }

        private string GetConfiguredSatelliteName(string satelliteName)
        {
            string appSettingKey = "LotwSatelliteName." + satelliteName.Trim();
            string configuredValue = ConfigurationManager.AppSettings[appSettingKey];
            return string.IsNullOrWhiteSpace(configuredValue) ? "" : configuredValue.Trim();
        }

        private static Dictionary<string, List<TqslSatelliteName>> GetTqslSatelliteNames()
        {
            if (tqslSatelliteNames != null)
            {
                return tqslSatelliteNames;
            }

            Dictionary<string, List<TqslSatelliteName>> names = new Dictionary<string, List<TqslSatelliteName>>();
            string configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TrustedQSL",
                "config.xml");

            if (!File.Exists(configPath))
            {
                tqslSatelliteNames = names;
                return tqslSatelliteNames;
            }

            try
            {
                XDocument document = XDocument.Load(configPath);
                foreach (XElement element in document.Descendants("satellite"))
                {
                    string lotwName = GetAttributeValue(element, "name");
                    if (string.IsNullOrWhiteSpace(lotwName))
                    {
                        continue;
                    }

                    TqslSatelliteName satellite = new TqslSatelliteName
                    {
                        LotwName = lotwName.Trim(),
                        Description = (element.Value ?? "").Trim(),
                        StartDate = ParseTqslDate(GetAttributeValue(element, "startDate")),
                        EndDate = ParseTqslDate(GetAttributeValue(element, "endDate"))
                    };

                    AddTqslSatelliteName(names, satellite.LotwName, satellite);
                    AddTqslSatelliteName(names, satellite.Description, satellite);
                }
            }
            catch
            {
                names.Clear();
            }

            tqslSatelliteNames = names;
            return tqslSatelliteNames;
        }

        private static void AddTqslSatelliteName(Dictionary<string, List<TqslSatelliteName>> names, string key, TqslSatelliteName satellite)
        {
            string normalizedKey = NormalizeSatelliteName(key);
            if (string.IsNullOrWhiteSpace(normalizedKey))
            {
                return;
            }

            List<TqslSatelliteName> satellites;
            if (!names.TryGetValue(normalizedKey, out satellites))
            {
                satellites = new List<TqslSatelliteName>();
                names[normalizedKey] = satellites;
            }

            satellites.Add(satellite);
        }

        private static string NormalizeSatelliteName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            StringBuilder normalized = new StringBuilder();
            foreach (char character in value.ToUpperInvariant())
            {
                if (char.IsLetterOrDigit(character))
                {
                    normalized.Append(character);
                }
            }

            return normalized.ToString();
        }

        private static string GetAttributeValue(XElement element, string attributeName)
        {
            XAttribute attribute = element.Attribute(attributeName);
            return attribute == null ? "" : attribute.Value;
        }

        private static DateTime? ParseTqslDate(string value)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out parsedDate))
            {
                return parsedDate.Date;
            }

            return null;
        }

        private string FormatMhz(int hz)
        {
            if (hz <= 0)
            {
                return "";
            }

            return ((double)hz / 1000000.0).ToString("0.00000", CultureInfo.InvariantCulture);
        }

        private string GetAdifBand(int hz)
        {
            if (hz <= 0)
            {
                return "";
            }

            double mhz = (double)hz / 1000000.0;

            if (mhz >= 0.1357 && mhz <= 0.1378) return "2190m";
            if (mhz >= 0.472 && mhz <= 0.479) return "630m";
            if (mhz >= 0.501 && mhz <= 0.504) return "560m";
            if (mhz >= 1.8 && mhz <= 2.0) return "160m";
            if (mhz >= 3.5 && mhz <= 4.0) return "80m";
            if (mhz >= 5.102 && mhz <= 5.4065) return "60m";
            if (mhz >= 7.0 && mhz <= 7.3) return "40m";
            if (mhz >= 10.0 && mhz <= 10.15) return "30m";
            if (mhz >= 14.0 && mhz <= 14.35) return "20m";
            if (mhz >= 18.068 && mhz <= 18.168) return "17m";
            if (mhz >= 21.0 && mhz <= 21.45) return "15m";
            if (mhz >= 24.89 && mhz <= 24.99) return "12m";
            if (mhz >= 28.0 && mhz <= 29.7) return "10m";
            if (mhz >= 50.0 && mhz <= 54.0) return "6m";
            if (mhz >= 70.0 && mhz <= 71.0) return "4m";
            if (mhz >= 144.0 && mhz <= 148.0) return "2m";
            if (mhz >= 222.0 && mhz <= 225.0) return "1.25m";
            if (mhz >= 420.0 && mhz <= 450.0) return "70cm";
            if (mhz >= 902.0 && mhz <= 928.0) return "33cm";
            if (mhz >= 1240.0 && mhz <= 1300.0) return "23cm";
            if (mhz >= 2300.0 && mhz <= 2450.0) return "13cm";
            if (mhz >= 3300.0 && mhz <= 3500.0) return "9cm";
            if (mhz >= 5650.0 && mhz <= 5925.0) return "6cm";
            if (mhz >= 10000.0 && mhz <= 10500.0) return "3cm";
            if (mhz >= 24000.0 && mhz <= 24250.0) return "1.25cm";
            if (mhz >= 47000.0 && mhz <= 47200.0) return "6mm";
            if (mhz >= 75500.0 && mhz <= 81000.0) return "4mm";
            if (mhz >= 119980.0 && mhz <= 120020.0) return "2.5mm";
            if (mhz >= 142000.0 && mhz <= 149000.0) return "2mm";
            if (mhz >= 241000.0 && mhz <= 250000.0) return "1mm";

            return "";
        }

        private string GetSatelliteMode(int uplinkHz, int downlinkHz)
        {
            string uplinkBand = GetSatelliteModeBand(uplinkHz);
            string downlinkBand = GetSatelliteModeBand(downlinkHz);
            if (string.IsNullOrWhiteSpace(uplinkBand) || string.IsNullOrWhiteSpace(downlinkBand))
            {
                return "";
            }

            return uplinkBand + downlinkBand;
        }

        private string GetSatelliteModeBand(int hz)
        {
            if (hz <= 0)
            {
                return "";
            }

            double mhz = (double)hz / 1000000.0;

            if (mhz >= 21.0 && mhz <= 21.45) return "J";
            if (mhz >= 28.0 && mhz <= 29.7) return "H";
            if (mhz >= 50.0 && mhz <= 54.0) return "A";
            if (mhz >= 144.0 && mhz <= 148.0) return "V";
            if (mhz >= 222.0 && mhz <= 225.0) return "P";
            if (mhz >= 420.0 && mhz <= 450.0) return "U";
            if (mhz >= 1240.0 && mhz <= 1300.0) return "L";
            if (mhz >= 2300.0 && mhz <= 2450.0) return "S";
            if (mhz >= 5650.0 && mhz <= 5925.0) return "C";
            if (mhz >= 10000.0 && mhz <= 10500.0) return "X";
            if (mhz >= 24000.0 && mhz <= 24250.0) return "K";

            return "";
        }

        public string GetExportMode(SatelliteLogEntry entry)
        {
            return GetAdifMode(entry);
        }

        public string GetExportSubmode(SatelliteLogEntry entry)
        {
            return GetAdifSubmode(entry);
        }

        public string GetExportSatelliteName(SatelliteLogEntry entry)
        {
            return ResolveLotwSatelliteName(entry.Satellite, entry.UtcTime);
        }

        public string GetExportPropMode()
        {
            return "SAT";
        }

        public string GetExportSatMode(SatelliteLogEntry entry)
        {
            return GetSatelliteMode(entry.UplinkHz, entry.DownlinkHz);
        }

        public string GetExportFreq(SatelliteLogEntry entry)
        {
            return FormatMhz(entry.UplinkHz);
        }

        public string GetExportBand(SatelliteLogEntry entry)
        {
            return GetAdifBand(entry.UplinkHz);
        }

        public string GetExportFreqRx(SatelliteLogEntry entry)
        {
            return FormatMhz(entry.DownlinkHz);
        }

        public string GetExportBandRx(SatelliteLogEntry entry)
        {
            return GetAdifBand(entry.DownlinkHz);
        }

        private string GetAdifMode(SatelliteLogEntry entry)
        {
            if (string.Equals(GetAdifSubmode(entry), "FT4", StringComparison.OrdinalIgnoreCase))
            {
                return "MFSK";
            }

            string mode = entry.Mode;
            if (string.IsNullOrWhiteSpace(mode))
            {
                return "";
            }

            string normalizedMode = mode.Trim().ToUpperInvariant();
            if (normalizedMode == "USB" || normalizedMode == "LSB" || normalizedMode == "USB-D" || normalizedMode == "LSB-D")
            {
                return "SSB";
            }

            if (normalizedMode == "CW" || normalizedMode == "FM" || normalizedMode == "SSB")
            {
                return normalizedMode;
            }

            return normalizedMode.Contains("FM") ? "FM" : normalizedMode;
        }

        private string GetAdifSubmode(SatelliteLogEntry entry)
        {
            string submode = entry.Submode;
            if (string.IsNullOrWhiteSpace(submode))
            {
                return "";
            }

            string normalizedSubmode = submode.Trim().ToUpperInvariant();
            return normalizedSubmode == "FT4" ? "FT4" : "";
        }

        private class TqslSatelliteName
        {
            public string LotwName { get; set; }
            public string Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public bool IsValidFor(DateTime qsoUtcTime)
            {
                DateTime qsoDate = qsoUtcTime.Date;
                return (!StartDate.HasValue || qsoDate >= StartDate.Value)
                    && (!EndDate.HasValue || qsoDate <= EndDate.Value);
            }
        }
    }
}
