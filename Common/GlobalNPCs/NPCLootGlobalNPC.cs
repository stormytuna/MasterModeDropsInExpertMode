using MasterModeDropsInExpertMode.Common.Configs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MasterModeDropsInExpertMode.Common.GlobalNPCs {
    public class NPCLootGlobalNPC : GlobalNPC {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (var rule in npcLoot.Get()) {
                // Should get most relics
                if (rule is ItemDropWithConditionRule withConditionRule && withConditionRule.condition is Conditions.IsMasterMode && ServerConfig.Instance.DropRelics) {
                    if (ItemID.Search.TryGetName(withConditionRule.itemId, out string name)) {
                        if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                            withConditionRule.condition = new Conditions.IsExpert();
                        }
                    }
                }

                // Should get most pets
                if (rule is DropBasedOnMasterMode masterDrop && masterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer dropPerPlayer && ServerConfig.Instance.DropPets) {
                    if (ItemID.Search.TryGetName(dropPerPlayer.itemId, out string name)) {
                        if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                            // Add and remove since the original rule is a DropBasedOnMasterMode drop
                            int denom = ServerConfig.Instance.GuaranteedPets ? 1 : dropPerPlayer.chanceDenominator;
                            npcLoot.Add(new DropPerPlayerOnThePlayer(dropPerPlayer.itemId, denom, dropPerPlayer.amountDroppedMinimum, dropPerPlayer.amountDroppedMaximum, new Conditions.IsExpert()));
                            npcLoot.Remove(dropPerPlayer);
                        }
                    }
                }

                // Should do the twins
                if (rule is LeadingConditionRule missingTwin && missingTwin.condition is Conditions.MissingTwin) {
                    foreach (var chain in missingTwin.ChainedRules) {
                        if (chain.RuleToChain is ItemDropWithConditionRule twinsWithConditionRule && twinsWithConditionRule.condition is Conditions.IsMasterMode && ServerConfig.Instance.DropRelics) {
                            if (ItemID.Search.TryGetName(twinsWithConditionRule.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    twinsWithConditionRule.condition = new Conditions.IsExpert();
                                }
                            }
                        }
                        if (chain.RuleToChain is DropBasedOnMasterMode twinsMasterDrop && twinsMasterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer twinsDrop && twinsDrop.condition is Conditions.IsMasterMode && ServerConfig.Instance.DropPets) {
                            if (ItemID.Search.TryGetName(twinsDrop.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    // Add and remove since the original rule is a DropBasedOnMasterMode drop
                                    int denom = ServerConfig.Instance.GuaranteedPets ? 1 : twinsDrop.chanceDenominator;
                                    npcLoot.Add(new DropPerPlayerOnThePlayer(twinsDrop.itemId, denom, twinsDrop.amountDroppedMinimum, twinsDrop.amountDroppedMaximum, new Conditions.IsExpert()));
                                    npcLoot.Remove(twinsDrop);
                                }
                            }
                        }
                    }
                }

                // Should do the eater of worlds
                if (rule is LeadingConditionRule eaterOfWorlds && eaterOfWorlds.condition is Conditions.LegacyHack_IsABoss) {
                    foreach (var chain in eaterOfWorlds.ChainedRules) {
                        if (chain.RuleToChain is ItemDropWithConditionRule eaterWithConditionRule && eaterWithConditionRule.condition is Conditions.IsMasterMode && ServerConfig.Instance.DropRelics) {
                            if (ItemID.Search.TryGetName(eaterWithConditionRule.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    eaterWithConditionRule.condition = new Conditions.IsExpert();
                                }
                            }
                        }
                        if (chain.RuleToChain is DropBasedOnMasterMode eaterMasterDrop && eaterMasterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer eaterDrop && eaterDrop.condition is Conditions.IsMasterMode && ServerConfig.Instance.DropPets) {
                            if (ItemID.Search.TryGetName(eaterDrop.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    // Add and remove since the original rule is a DropBasedOnMasterMode drop
                                    int denom = ServerConfig.Instance.GuaranteedPets ? 1 : eaterDrop.chanceDenominator;
                                    npcLoot.Add(new DropPerPlayerOnThePlayer(eaterDrop.itemId, denom, eaterDrop.amountDroppedMinimum, eaterDrop.amountDroppedMaximum, new Conditions.IsExpert()));
                                    npcLoot.Remove(eaterDrop);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}