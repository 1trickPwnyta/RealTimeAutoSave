using RimWorld;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [HarmonyPatch(typeof(Autosaver))]
    [HarmonyPatch("AutosaverTick")]
    public static class Patch_Autosaver
    {
        public static bool Prefix(Autosaver __instance)
        {
            // Prevent normal AutosaverTick function
            return false;
        }
    }
}
