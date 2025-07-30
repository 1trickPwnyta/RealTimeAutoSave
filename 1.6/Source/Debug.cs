namespace RealTimeAutoSave
{
    public static class Debug
    {
        public static void Log(object message)
        {
#if DEBUG
            Verse.Log.Message($"[{RealTimeAutoSaveMod.PACKAGE_NAME}] {message}");
#endif
        }
    }
}
