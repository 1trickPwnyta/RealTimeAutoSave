using System.Collections.Generic;
using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [HarmonyPatch(typeof(Listing_Standard))]
    [HarmonyPatch("ButtonTextLabeled")]
    public static class Patch_Listing_Standard_ButtonTextLabeled
    {
        public static bool Prefix(ref string label, ref string buttonLabel)
        {
            if (label == "AutosaveInterval".Translate())
            {
                buttonLabel = "RealTimeAutoSave_Configure".Translate();
            }
            return true;
        }
    }
}
