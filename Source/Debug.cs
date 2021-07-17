namespace RealTimeAutoSave
{
    public static class Debug
    {
        public static void Log(string message)
        {
#if DEBUG
            Verse.Log.Message("[Real Time Auto Save] " + message);
#endif
        }
    }
}
