using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MasterModeDropsInExpertMode.Common.GlobalNPCs {
    public class FrostMoonGlobalNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => (entity.type == NPCID.IceQueen || entity.type == NPCID.SantaNK1 || entity.type == NPCID.Everscream) && ServerConfig.Instance.EmpressDropsDaytimeOnly;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (var rule in npcLoot.Get()) {
                if (rule is LeadingConditionRule frostMoonDropGatingChance && frostMoonDropGatingChance.condition is Conditions.FrostMoonDropGatingChance) {
                    foreach (var chain in frostMoonDropGatingChance.ChainedRules) {
                        if (chain.RuleToChain is ItemDropWithConditionRule frostMoonConditionRule && frostMoonConditionRule.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(frostMoonConditionRule.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    if (ServerConfig.Instance.DropRelicsClassic) {
                                        frostMoonConditionRule.condition = new Conditions.NotMasterMode();
                                    }

                                    else if (ServerConfig.Instance.DropRelicsExpert) {
                                        frostMoonConditionRule.condition = new Conditions.IsExpert();
                                    }

                                    ArraySystem.RegisterNewRelic(frostMoonConditionRule.itemId);
                                }
                            }
                        }
                        if (chain.RuleToChain is DropBasedOnMasterMode frostMoonMasterDrop && frostMoonMasterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer frostMoonDrop && frostMoonDrop.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(frostMoonDrop.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    // Add and remove since the original rule is a DropBasedOnMasterMode drop
                                    int denom = ServerConfig.Instance.GuaranteedPets ? 1 : frostMoonDrop.chanceDenominator;
                                    LeadingConditionRule newRule = new LeadingConditionRule(new Conditions.FrostMoonDropGatingChance());

                                    if (ServerConfig.Instance.DropPetsClassic) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(frostMoonDrop.itemId, denom, frostMoonDrop.amountDroppedMinimum, frostMoonDrop.amountDroppedMaximum, new Conditions.NotMasterMode()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(frostMoonDrop);
                                    }

                                    else if (ServerConfig.Instance.DropPetsExpert) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(frostMoonDrop.itemId, denom, frostMoonDrop.amountDroppedMinimum, frostMoonDrop.amountDroppedMaximum, new Conditions.IsExpert()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(frostMoonDrop);
                                    }

                                    ArraySystem.RegisterNewPet(frostMoonDrop.itemId);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
