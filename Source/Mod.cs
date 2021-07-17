using Verse;
using HarmonyLib;
using System.Threading.Tasks;
using System;

namespace RealTimeAutoSave
{
    [StaticConstructorOnStartup]
    public static class Mod
    {
        const string PACKAGE_ID = "realtimeautosave.1trickPonyta";
        const int REATTEMPT_DELAY = 5000;
        const int INTERVAL_DELAY = 15000;

        static Mod()
        {
            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message("[Real Time Auto Save] Loaded.");

            Task.Delay(INTERVAL_DELAY).ContinueWith(t => TryAutosave());
        }

        private static bool TryAutosave()
        {
            Debug.Log("TryAutosave called.");
            int delay = REATTEMPT_DELAY;
            try
            {
                if (Current.Game != null && !Find.WindowStack.WindowsForcePause && !LongEventHandler.ForcePause)
                {
                    LongEventHandler.QueueLongEvent(new Action(Find.Autosaver.DoAutosave), "Autosaving", false, null, true);
                    delay = INTERVAL_DELAY;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return false;
            }
            finally
            {
                Task.Delay(delay).ContinueWith(t => TryAutosave());
            }
        }
    }
}
