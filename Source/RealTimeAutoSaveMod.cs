using System;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    public class RealTimeAutoSaveMod : Mod
    {
        public const string PACKAGE_ID = "realtimeautosave.1trickPonyta";
        public const string PACKAGE_NAME = "Real Time Auto Save";
        private const int REATTEMPT_DELAY = 5000;

        public RealTimeAutoSaveMod(ModContentPack content) : base(content)
        {
            GetSettings<RealTimeAutoSaveSettings>();

            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message($"[{PACKAGE_NAME}] Loaded.");

            Task.Delay(RealTimeAutoSaveSettings.IntervalDelay).ContinueWith(t => TryAutosave());
        }

        public override string SettingsCategory() => PACKAGE_NAME;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            RealTimeAutoSaveSettings.DoSettingsWindowContents(inRect);
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
                    delay = RealTimeAutoSaveSettings.IntervalDelay;
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
