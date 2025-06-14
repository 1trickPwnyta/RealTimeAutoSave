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

            if (listingStandard.RadioButton("RealTimeAutoSave_SettingsInGameInterval".Translate(), AutoSaveMode == AutoSaveMode.InGame))
            {
                AutoSaveMode = AutoSaveMode.InGame;
                RealTimeAutosaver.CancelAutosave();
            }
            if (AutoSaveMode == AutoSaveMode.InGame)
            {
                // Show the vanilla-ish autosave interval selection drop down, but with one difference: 
                // we gives ours a (fake) title of "" (empty string), which has no UI effect but allows 
                // us to differentiate this drop down from the actual vanilla drop down when we patch
                // out the vanilla one
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

            if (listingStandard.RadioButton("RealTimeAutoSave_SettingsRealTimeInterval".Translate(), AutoSaveMode == AutoSaveMode.RealTime))
            {
                AutoSaveMode = AutoSaveMode.RealTime;
                RealTimeAutosaver.ScheduleAutosave();
            }
            if (AutoSaveMode == AutoSaveMode.RealTime)
            {
                // Show the real-time interval input box
                buffer = RealTimeInterval.ToString();
                listingStandard.IntEntry(ref RealTimeInterval, ref buffer);
                RealTimeInterval = Math.Max(RealTimeInterval, 1);
                RealTimeAutosaver.ScheduleAutosave();
            }

            if (listingStandard.RadioButton("RealTimeAutoSave_SettingsNone".Translate(), AutoSaveMode == AutoSaveMode.None))
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

    // Window that is shown when clicking the patched-in Configure button in the options 
    // dialog. It just mimics the mod settings dialog.
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
