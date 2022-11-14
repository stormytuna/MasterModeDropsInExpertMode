using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
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
                if (rule is ItemDropWithConditionRule withConditionRule && withConditionRule.condition is Conditions.IsMasterMode) {
                    if (ItemID.Search.TryGetName(withConditionRule.itemId, out string name)) {
                        if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                            if (ServerConfig.Instance.DropRelicsClassic) {
                                withConditionRule.condition = new Conditions.NotMasterMode();
                            }

                            else if (ServerConfig.Instance.DropRelicsExpert) {
                                withConditionRule.condition = new Conditions.IsExpert();
                            }

                            ArraySystem.RegisterNewRelic(withConditionRule.itemId);
                        }
                    }
                }

                // Should get most pets
                if (rule is DropBasedOnMasterMode masterDrop && masterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer dropPerPlayer) {
                    if (ItemID.Search.TryGetName(dropPerPlayer.itemId, out string name)) {
                        if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                            // Add and remove since the original rule is a DropBasedOnMasterMode drop
                            int denom = ServerConfig.Instance.GuaranteedPets ? 1 : dropPerPlayer.chanceDenominator;

                            if (ServerConfig.Instance.DropPetsClassic) {
                                npcLoot.Add(new DropPerPlayerOnThePlayer(dropPerPlayer.itemId, denom, dropPerPlayer.amountDroppedMinimum, dropPerPlayer.amountDroppedMaximum, new Conditions.NotMasterMode()));
                                npcLoot.Remove(dropPerPlayer);
                            }

                            else if (ServerConfig.Instance.DropPetsExpert) {
                                npcLoot.Add(new DropPerPlayerOnThePlayer(dropPerPlayer.itemId, denom, dropPerPlayer.amountDroppedMinimum, dropPerPlayer.amountDroppedMaximum, new Conditions.IsExpert()));
                                npcLoot.Remove(dropPerPlayer);
                            }

                            ArraySystem.RegisterNewPet(dropPerPlayer.itemId);
                        }
                    }
                }

                // Should do the twins
                if (rule is LeadingConditionRule missingTwin && missingTwin.condition is Conditions.MissingTwin) {
                    foreach (var chain in missingTwin.ChainedRules) {
                        if (chain.RuleToChain is ItemDropWithConditionRule twinsWithConditionRule && twinsWithConditionRule.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(twinsWithConditionRule.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    if (ServerConfig.Instance.DropRelicsClassic) {
                                        twinsWithConditionRule.condition = new Conditions.NotMasterMode();
                                    }

                                    else if (ServerConfig.Instance.DropRelicsExpert) {
                                        twinsWithConditionRule.condition = new Conditions.IsExpert();
                                    }

                                    ArraySystem.RegisterNewRelic(twinsWithConditionRule.itemId);
                                }
                            }
                        }
                        if (chain.RuleToChain is DropBasedOnMasterMode twinsMasterDrop && twinsMasterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer twinsDrop && twinsDrop.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(twinsDrop.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    // Add and remove since the original rule is a DropBasedOnMasterMode drop
                                    int denom = ServerConfig.Instance.GuaranteedPets ? 1 : twinsDrop.chanceDenominator;
                                    LeadingConditionRule newRule = new LeadingConditionRule(new Conditions.MissingTwin());

                                    if (ServerConfig.Instance.DropPetsClassic) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(twinsDrop.itemId, denom, twinsDrop.amountDroppedMinimum, twinsDrop.amountDroppedMaximum, new Conditions.NotMasterMode()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(twinsDrop);
                                    }

                                    else if (ServerConfig.Instance.DropPetsExpert) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(twinsDrop.itemId, denom, twinsDrop.amountDroppedMinimum, twinsDrop.amountDroppedMaximum, new Conditions.IsExpert()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(twinsDrop);
                                    }

                                    ArraySystem.RegisterNewPet(twinsDrop.itemId);
                                }
                            }
                        }
                    }
                }

                // Should do the eater of worlds
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
                                    LeadingConditionRule newRule = new LeadingConditionRule(new Conditions.MissingTwin());

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