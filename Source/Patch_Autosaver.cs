using System;
using System.Timers;
using RimWorld;
using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [HarmonyPatch(typeof(Autosaver))]
    [HarmonyPatch("AutosaverTick")]
    public static class Patch_Autosaver
    {
        private static Timer timer = new Timer(15000);
        private static Autosaver autosaver;

        static Patch_Autosaver()
        {
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (autosaver != null)
            {
                LongEventHandler.QueueLongEvent(new Action(autosaver.DoAutosave), "Autosaving", false, null, true);
            }
        }

        public static bool Prefix(Autosaver __instance)
        {
            // Check if we have a new instance of Autosaver
            if (__instance != autosaver)
            {
                timer.Stop();
                timer.Start();
                autosaver = __instance;
            }

            // Prevent normal AutosaverTick function
            return false;
        }
    }
}
