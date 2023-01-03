using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MasterModeDropsInExpertMode.Common.GlobalNPCs {
    public class EoWGlobalNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.EaterofWorldsHead || entity.type == NPCID.EaterofWorldsBody || entity.type == NPCID.EaterofWorldsTail;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (var rule in npcLoot.Get()) {
                if (rule is LeadingConditionRule eaterOfWorlds && eaterOfWorlds.condition is Conditions.LegacyHack_IsABoss) {
                    foreach (var chain in eaterOfWorlds.ChainedRules) {
                        if (chain.RuleToChain is ItemDropWithConditionRule eaterWithConditionRule && eaterWithConditionRule.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(eaterWithConditionRule.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    if (ServerConfig.Instance.DropRelicsClassic) {
                                        eaterWithConditionRule.condition = new Conditions.NotMasterMode();
                                    }

                                    else if (ServerConfig.Instance.DropRelicsExpert) {
                                        eaterWithConditionRule.condition = new Conditions.IsExpert();
                                    }

                                    ArraySystem.RegisterNewRelic(eaterWithConditionRule.itemId);
                                }
                            }
                        }
                        if (chain.RuleToChain is DropBasedOnMasterMode eaterMasterDrop && eaterMasterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer eaterDrop && eaterDrop.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(eaterDrop.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    // Add and remove since the original rule is a DropBasedOnMasterMode drop
                                    int denom = ServerConfig.Instance.GuaranteedPets ? 1 : eaterDrop.chanceDenominator;
                                    LeadingConditionRule newRule = new LeadingConditionRule(new Conditions.LegacyHack_IsABoss());

                                    if (ServerConfig.Instance.DropPetsClassic) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(eaterDrop.itemId, denom, eaterDrop.amountDroppedMinimum, eaterDrop.amountDroppedMaximum, new Conditions.NotMasterMode()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(eaterDrop);
                                    }

                                    else if (ServerConfig.Instance.DropPetsExpert) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(eaterDrop.itemId, denom, eaterDrop.amountDroppedMinimum, eaterDrop.amountDroppedMaximum, new Conditions.IsExpert()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(eaterDrop);
                                    }

                                    ArraySystem.RegisterNewPet(eaterDrop.itemId);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
