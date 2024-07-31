using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using UnityModManagerNet;

namespace EnduringRework;

#if DEBUG
[EnableReloading]
#endif
public static class Main
{
    public static UnityModManager.ModEntry ModEntry;
    public static Harmony HarmonyInstance;

#pragma warning disable IDE0051 // Remove unused private members
    static bool Load(UnityModManager.ModEntry modEntry)
#pragma warning restore IDE0051 // Remove unused private members
    {
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        ModEntry = modEntry;
        HarmonyInstance.CreateClassProcessor(typeof(EntryPatch)).Patch();
        return true;
    }

    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class EntryPatch
    {
        static bool Initialized;

        [HarmonyPostfix]
        [HarmonyAfter("TabletopTweaks-Reworks")]
        static void After_BlueprintsCache_Init()
        {
            if (Initialized) return;
            Initialized = true;
            EnduringSpellsRework.UpdateEnduringSpellsMythicAbility();
            HarmonyInstance.CreateClassProcessor(typeof(ItemEntity_AddEnchantment_EnduringSpells_Patch)).Patch();
        }
    }
}
