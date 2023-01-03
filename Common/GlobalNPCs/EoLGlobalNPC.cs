using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MasterModeDropsInExpertMode.Common.GlobalNPCs {
    public class EoLGlobalNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.HallowBoss && ServerConfig.Instance.EmpressDropsDaytimeOnly;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (var rule in npcLoot.Get()) {
                if (rule is ItemDropWithConditionRule withConditionRule && withConditionRule.condition is Conditions.IsMasterMode) {
                    if (ItemID.Search.TryGetName(withConditionRule.itemId, out string name)) {
                        if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                            LeadingConditionRule newRule = new LeadingConditionRule(new Conditions.EmpressOfLightIsGenuinelyEnraged());

                            if (ServerConfig.Instance.DropRelicsClassic) {
                                newRule.OnSuccess(new ItemDropWithConditionRule(withConditionRule.itemId, withConditionRule.chanceDenominator, withConditionRule.amountDroppedMinimum, withConditionRule.amountDroppedMaximum, new Conditions.NotMasterMode()));
                                npcLoot.Add(newRule);
                                npcLoot.Remove(withConditionRule);
                            }

                            else if (ServerConfig.Instance.DropRelicsExpert) {
                                newRule.OnSuccess(new ItemDropWithConditionRule(withConditionRule.itemId, withConditionRule.chanceDenominator, withConditionRule.amountDroppedMinimum, withConditionRule.amountDroppedMaximum, new Conditions.IsExpert()));
                                npcLoot.Add(newRule);
                                npcLoot.Remove(withConditionRule);
                            }

                            ArraySystem.RegisterNewRelic(withConditionRule.itemId);
                        }
                    }
                }
                if (rule is DropBasedOnMasterMode masterDrop && masterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer dropPerPlayer) {
                    if (ItemID.Search.TryGetName(dropPerPlayer.itemId, out string name)) {
                        if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                            // Add and remove since the original rule is a DropBasedOnMasterMode drop
                            int denom = ServerConfig.Instance.GuaranteedPets ? 1 : dropPerPlayer.chanceDenominator;
                            LeadingConditionRule newRule = new LeadingConditionRule(new Conditions.EmpressOfLightIsGenuinelyEnraged());

                            if (ServerConfig.Instance.DropPetsClassic) {
                                newRule.OnSuccess(new DropPerPlayerOnThePlayer(dropPerPlayer.itemId, denom, dropPerPlayer.amountDroppedMinimum, dropPerPlayer.amountDroppedMaximum, new Conditions.NotMasterMode()));
                                npcLoot.Add(newRule);
                                npcLoot.Remove(dropPerPlayer);
                            }

                            else if (ServerConfig.Instance.DropPetsExpert) {
                                newRule.OnSuccess(new DropPerPlayerOnThePlayer(dropPerPlayer.itemId, denom, dropPerPlayer.amountDroppedMinimum, dropPerPlayer.amountDroppedMaximum, new Conditions.IsExpert()));
                                npcLoot.Add(newRule);
                                npcLoot.Remove(dropPerPlayer);
                            }

                            ArraySystem.RegisterNewPet(dropPerPlayer.itemId);
                        }
                    }
                }
            }
        }
    }
}
