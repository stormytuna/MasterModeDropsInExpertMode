using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MasterModeDropsInExpertMode.Common.GlobalNPCs {
    public class TheTwinsGlobalNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Spazmatism || entity.type == NPCID.Retinazer;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (var rule in npcLoot.Get()) {
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
            }
        }
    }
}
