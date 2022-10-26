using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    // Patches the vanilla options dialog autosave section to use a Configure
    // button to open the mod settings instead of allowing selection of an
    // in-game time interval

    [HarmonyPatch(typeof(Listing_Standard))]
    [HarmonyPatch(nameof(Listing_Standard.ButtonTextLabeledPct))]
    public static class Patch_Listing_Standard_ButtonTextLabeledPct
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
