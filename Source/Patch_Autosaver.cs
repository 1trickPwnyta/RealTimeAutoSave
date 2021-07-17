using RimWorld;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [HarmonyPatch(typeof(Autosaver))]
    [HarmonyPatch("AutosaverTick")]
    public class Patch_Autosaver
    {
        static bool Prefix()
        {
            return false;
        }
    }
}
