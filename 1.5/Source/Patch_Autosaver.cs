using System;
using RimWorld;
using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    // Patches the vanilla autosaver to not perform regular autosaves if the mod 
    // is configured to use a real-time interval, and to use the mod's in-game 
    // time interval setting otherwise

    [HarmonyPatch(typeof(Autosaver))]
    [HarmonyPatch("AutosaverTick")]
    public static class Patch_Autosaver_AutosaverTick
    {
        private static int ticksSinceSave;

        public static bool Prefix(Autosaver __instance)
        {
            if (RealTimeAutoSaveSettings.AutoSaveMode == AutoSaveMode.InGame)
            {
                ticksSinceSave++;
                if (ticksSinceSave >= RealTimeAutoSaveSettings.InGameInterval * 60000)
                {
                    if (RealTimeAutosaver.TryAutosave())
                    {
                        ticksSinceSave = 0;
                    }
                }
            }
            return false;
        }
    }
}
