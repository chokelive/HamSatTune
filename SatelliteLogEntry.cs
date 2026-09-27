using System;

namespace HamSatTune
{
    public class SatelliteLogEntry
    {
        public long Id { get; set; }
        public DateTime UtcTime { get; set; }
        public string Callsign { get; set; }
        public string RstSent { get; set; }
        public string RstReceived { get; set; }
        public string Grid { get; set; }
        public string Name { get; set; }
        public string Satellite { get; set; }
        public string Mode { get; set; }
        public string Submode { get; set; }
        public int DownlinkHz { get; set; }
        public int UplinkHz { get; set; }
        public string Comment { get; set; }
    }
}
