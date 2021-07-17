using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [HarmonyPatch(typeof(Autosaver))]
    [HarmonyPatch("AutosaverTick")]
    public static class Patch_Autosaver
    {
        // Each Autosaver instance manages its own state so we will manage our state per Autosaver instance
        private static Dictionary<Autosaver, DateTime> lastSaved = new Dictionary<Autosaver, DateTime>();

        public static bool Prefix(Autosaver __instance)
        {
            if (!lastSaved.ContainsKey(__instance))
            {
                lastSaved[__instance] = DateTime.Now;
            }

            // Check how long it has been since this Autosaver instance was last used to save and save if necessary
            if ((DateTime.Now - lastSaved[__instance]).TotalSeconds > 15)
            {
                LongEventHandler.QueueLongEvent(new Action(__instance.DoAutosave), "Autosaving", false, null, true);
                lastSaved[__instance] = DateTime.Now;
            }

            // Prevent normal AutosaverTick function
            return false;
        }
    }
}
