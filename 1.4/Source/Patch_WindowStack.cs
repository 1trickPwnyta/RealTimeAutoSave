using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace RealTimeAutoSave
{
    // Patches the vanilla autosave interval selection dialog to instead show the 
    // mod settings

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

            // Check title here because our autosave interval selection has a (fake) title
            // This way we can tell the difference and not accidentally patch out our own 
            // drop down menu
            if (menu != null && titleRef(menu) == null)
            {
                bool isAutosaveInterval = false;

                // If we find an option in the drop down labeled "0.5 Days" we assume this 
                // is the autosave interval menu and we patch it
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
                    // Use mod settings dialog and prevent calling original method
                    Find.WindowStack.Add(new RealTimeAutoSaveSettingsDialog());
                    return false;
                }
            }

            // It wasn't the autosave interval drop down so call the original method
            return true;
        }
    }
}
