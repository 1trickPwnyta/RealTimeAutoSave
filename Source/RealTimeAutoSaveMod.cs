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

        public static RealTimeAutoSaveSettings Settings;

        public RealTimeAutoSaveMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<RealTimeAutoSaveSettings>();

            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message($"[{PACKAGE_NAME}] Loaded.");

            if (RealTimeAutoSaveSettings.AutoSaveMode == AutoSaveMode.RealTime)
            {
                RealTimeAutosaver.ScheduleAutosave();
            }
        }

        public override string SettingsCategory() => PACKAGE_NAME;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            RealTimeAutoSaveSettings.DoSettingsWindowContents(inRect);
        }
    }
}
