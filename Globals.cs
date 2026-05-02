namespace HamSatTune
{
    public static class Globals
    {
        // Shared global state. Use with caution (thread-safety and tight coupling).
        public static Sqf CurrentSqf;
        public static int CalculatedDownlinkHz;
        public static int CalculatedUplinkHz;
        public static System.DateTime LastTrackingUpdateTime;
        public static int TrackingUpdateNumber;
    }
}
