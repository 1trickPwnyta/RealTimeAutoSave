using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [StaticConstructorOnStartup]
    public static class Mod
    {
        const string PACKAGE_ID = "realtimeautosave.1trickPonyta";
        static Mod()
        {
            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message("[Real Time Auto Save] Loaded.");
        }
    }
}
