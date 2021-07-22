using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [HarmonyPatch(typeof(WindowStack))]
    [HarmonyPatch("Add")]
    public static class Patch_WindowStack_Add
    {
        static AccessTools.FieldRef<FloatMenu, List<FloatMenuOption>> optionsRef =
            AccessTools.FieldRefAccess<FloatMenu, List<FloatMenuOption>>("options");

        static AccessTools.FieldRef<FloatMenu, string> titleRef =
            AccessTools.FieldRefAccess<FloatMenu, string>("title");

        public static bool Prefix(ref Window window)
        {
            FloatMenu menu = window as FloatMenu;
            if (menu != null && titleRef(menu) == null)
            {
                bool isAutosaveInterval = false;

                foreach (FloatMenuOption option in optionsRef(menu))
                {
                    if (option.Label == $"0.5 {"Days".Translate()}")
                    {
                        isAutosaveInterval = true;
                        break;
                    }
                }

                if (isAutosaveInterval)
                {
                    Find.WindowStack.Add(new RealTimeAutoSaveSettingsDialog());
                    return false;
                }
            }

            return true;
        }
    }
}
