using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EnduringRework
{
    internal class EnduringSpellsRework
    {
        internal static HashSet<string> ForbidddenPermanentSpells = new()
        {
            "7c33de68880aa444bbb916271b653016", //FirebellyBuff
            "dd91a3c3df275984592edcadb8e90749", //CallLightningBuff
            "65ba3abcdc991004aaf32ca5ad7119ca", //CallLightningStormBuff
            "830804f1bc365e34bb4701b8fd622ad3", //CaveFangsStalactitesBuff
            "f6d1a5549172a0d428b4bbb0ed7a4071"  //CaveFangsStalagmitesBuff
        };

        internal static void UpdateEnduringSpellsMythicAbility()
        {
            var enduringSpells = Utils.GetBlueprint<BlueprintFeature>("2f206e6d292bdfb4d981e99dcf08153f");
            enduringSpells.SetComponents(
                new PrerequisiteFeature()
                {
                    m_Feature = Utils.GetBlueprintReference<BlueprintFeatureReference>("f180e72e4a9cbaa4da8be9bc958132ef")
                },
                new EnduringSpellsRedone()
                {
                    m_Greater = Utils.GetBlueprintReference<BlueprintUnitFactReference>("13f9269b3b48ae94c896f0371ce5e23c")
                }
            );
            enduringSpells.m_Description = Utils.CreateLocalizedString("AlterAsc.EnduringRework.EnduringSpellsDescription"
                , "You've learned a way to prolong the effects of your beneficial extended spells.\r\n" +
                "Benefit: Effects of your spells on your allies cast with extend metamagic applied that should last longer than 1 minute but shorter than 10 minutes last 10 minutes.\r\n" +
                "Effects that should last longer than 10 minutes but shorter than 1 hour last 1 hour.");

            var greaterEnduringSpells = Utils.GetBlueprint<BlueprintFeature>("13f9269b3b48ae94c896f0371ce5e23c");
            greaterEnduringSpells.m_Description = Utils.CreateLocalizedString("AlterAsc.EnduringRework.GreaterEnduringSpellsDescription"
                , "You've mastered a way to prolong your beneficial spells.\r\n" +
                "Benefit: In addition to existing benefits now effects of your spells on your allies cast with extend metamagic that should last longer than an hour are permanent.");
        }
    }

    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("07ab83e0eb884687869c091caa1c292e")]
    public class EnduringSpellsRedone :
          UnitFactComponentDelegate,
          IUnitBuffHandler,
          IGlobalSubscriber,
          ISubscriber
    {
        [SerializeField]
        internal BlueprintUnitFactReference m_Greater;

        public BlueprintUnitFact Greater => this.m_Greater?.Get();

        public void HandleBuffDidAdded(Buff buff)
        {
            AbilityData ability = buff.Context.SourceAbilityContext?.Ability;
            if (ability == null || ability.Spellbook == null || ability.SourceItem != null
                || !(buff.MaybeContext?.MaybeCaster == (UnitDescriptor)this.Owner)
                || !ability.HasMetamagic(Metamagic.Extend)
               )
            {
                return;
            }
            if (buff.TimeLeft > 60.Minutes())
            {
                if (this.Owner.HasFact(Greater) && !EnduringSpellsRework.ForbidddenPermanentSpells.Contains(buff.Blueprint.AssetGuid.ToString()))
                {
                    buff.MakePermanent();
                }
            }
            else if (buff.TimeLeft > 10.Minutes())
            {
                buff.SetEndTime(1.Hours() + buff.AttachTime);
            }
            else if (buff.TimeLeft > 1.Minutes())
            {
                buff.SetEndTime(10.Minutes() + buff.AttachTime);
            }
        }

        public void HandleBuffDidRemoved(Buff buff)
        {
        }
    }

    [HarmonyPatch(typeof(ItemEntity), "AddEnchantment")]
    [HarmonyAfter("TabletopTweaks-Reworks")]
    static class ItemEntity_AddEnchantment_EnduringSpells_Patch
    {
        private static readonly BlueprintFeature EnduringSpells = Utils.GetBlueprint<BlueprintFeature>("2f206e6d292bdfb4d981e99dcf08153f");
        private static readonly BlueprintFeature EnduringSpellsGreater = Utils.GetBlueprint<BlueprintFeature>("13f9269b3b48ae94c896f0371ce5e23c");

        [HarmonyPrefix]
        static bool Prefix(MechanicsContext parentContext, ref Rounds? duration, BlueprintItemEnchantment blueprint)
        {
            if (parentContext != null && parentContext.MaybeOwner != null && duration != null)
            {
                var abilityData = parentContext.SourceAbilityContext?.Ability;
                if (abilityData == null || abilityData.Spellbook == null
                    || abilityData.SourceItem != null || !abilityData.HasMetamagic(Metamagic.Extend))
                {
                    return true;
                }
                var owner = parentContext.MaybeOwner;
                if (owner.Descriptor.HasFact(EnduringSpells))
                {
                    if (owner.Descriptor.HasFact(EnduringSpellsGreater) && duration > DurationRate.Hours.ToRounds())
                    {
                        duration = null;
                    }
                    else if (duration > DurationRate.Minutes.ToRounds() * 10 && duration <= DurationRate.Hours.ToRounds())
                    {
                        duration = DurationRate.Hours.ToRounds();
                    }
                    else if (duration > DurationRate.Minutes.ToRounds() && duration <= DurationRate.Minutes.ToRounds() * 10)
                    {
                        duration = DurationRate.Minutes.ToRounds() * 10;
                    }
                }
            }
            return true;
        }
    }
}
