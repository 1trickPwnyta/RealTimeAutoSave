using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RealTimeAutoSave
{
    public enum AutoSaveMode
    {
        InGame,     // In-game days
        RealTime,   // Real-time minutes
        None
    }

    public class RealTimeAutoSaveSettings : ModSettings
    {
        public static AutoSaveMode AutoSaveMode = AutoSaveMode.RealTime;
        public static float InGameInterval = 1f;
        public static int RealTimeInterval = 10;

        public static void DoSettingsWindowContents(Rect inRect)
        {
            string buffer;

            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);
            listingStandard.Label("RealTimeAutoSave_SettingsAutosaveInterval".Translate());

            // Only difference between 1.2 and 1.3 - this method is called RadioButton_NewTemp 
            // in 1.2 and just RadioButton in 1.3
            if (listingStandard.RadioButton_NewTemp("RealTimeAutoSave_SettingsInGameInterval".Translate(), AutoSaveMode == AutoSaveMode.InGame))
            {
                AutoSaveMode = AutoSaveMode.InGame;
                RealTimeAutosaver.CancelAutosave();
            }
            if (AutoSaveMode == AutoSaveMode.InGame)
            {
                buffer = InGameInterval.ToString();
                string days = "Days".Translate();
                string day = "Day".Translate().ToLower();
                if (listingStandard.ButtonText($"{InGameInterval} {(InGameInterval == 1f ? day : days)}"))
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    float[] debugChoices = new float[] { 0.125f, 0.25f };
                    float[] choices = new float[] { 0.5f, 1f, 3f, 7f, 14f };
                    if (Prefs.DevMode)
                    {
                        foreach (float choice in debugChoices)
                        {
                            list.Add(new FloatMenuOption($"{choice} {days} (debug)", delegate ()
                            {
                                InGameInterval = choice;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null));
                        }
                    }
                    foreach (float choice in choices)
                    {
                        list.Add(new FloatMenuOption($"{choice} {(choice == 1f? day : days)}", delegate ()
                        {
                            InGameInterval = choice;
                        }, MenuOptionPriority.Default, null, null, 0f, null, null));
                    }
                    Find.WindowStack.Add(new FloatMenu(list, ""));
                }
            }

            if (listingStandard.RadioButton_NewTemp("RealTimeAutoSave_SettingsRealTimeInterval".Translate(), AutoSaveMode == AutoSaveMode.RealTime))
            {
                AutoSaveMode = AutoSaveMode.RealTime;
                RealTimeAutosaver.ScheduleAutosave();
            }
            if (AutoSaveMode == AutoSaveMode.RealTime)
            {
                buffer = RealTimeInterval.ToString();
                listingStandard.IntEntry(ref RealTimeInterval, ref buffer);
                RealTimeInterval = Math.Max(RealTimeInterval, 1);
                RealTimeAutosaver.ScheduleAutosave();
            }

            if (listingStandard.RadioButton_NewTemp("RealTimeAutoSave_SettingsNone".Translate(), AutoSaveMode == AutoSaveMode.None))
            {
                AutoSaveMode = AutoSaveMode.None;
                RealTimeAutosaver.CancelAutosave();
            }

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref AutoSaveMode, "AutoSaveMode");
            Scribe_Values.Look(ref InGameInterval, "InGameInterval");
            Scribe_Values.Look(ref RealTimeInterval, "RealTimeInterval");
        }
    }

    public class RealTimeAutoSaveSettingsDialog : Window
    {
        public RealTimeAutoSaveSettingsDialog()
        {
            this.forcePause = true;
            this.doCloseX = true;
            this.doCloseButton = true;
            this.closeOnClickedOutside = true;
            this.absorbInputAroundWindow = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            RealTimeAutoSaveSettings.DoSettingsWindowContents(inRect);
        }

        public override void PostClose()
        {
            base.PostClose();
            RealTimeAutoSaveMod.Settings.Write();
        }
    }
}
