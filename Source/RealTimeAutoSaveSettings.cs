using System;
using UnityEngine;
using Verse;

namespace RealTimeAutoSave
{
    public class RealTimeAutoSaveSettings : ModSettings
    {
        private const int SCALE_FACTOR = 1000 * 60; // Minutes

        private static int _intervalDelay = 10;

        public static int IntervalDelay
        {
            get
            {
                return _intervalDelay * SCALE_FACTOR;
            }
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            string _intervalDelay_buffer = _intervalDelay.ToString();
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("RealTimeAutoSave_SettingsAutosaveInterval".Translate());
            listingStandard.IntEntry(ref _intervalDelay, ref _intervalDelay_buffer);
            _intervalDelay = Math.Max(_intervalDelay, 1);
            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref _intervalDelay, "_intervalDelay");
        }
    }
}
