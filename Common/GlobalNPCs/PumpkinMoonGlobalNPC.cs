using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MasterModeDropsInExpertMode.Common.GlobalNPCs {
    public class PumpkinMoonGlobalNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => (entity.type == NPCID.Pumpking || entity.type == NPCID.MourningWood) && ServerConfig.Instance.EmpressDropsDaytimeOnly;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (var rule in npcLoot.Get()) {
                if (rule is LeadingConditionRule pumpkinMoonDropGatingChance && pumpkinMoonDropGatingChance.condition is Conditions.PumpkinMoonDropGatingChance) {
                    foreach (var chain in pumpkinMoonDropGatingChance.ChainedRules) {
                        if (chain.RuleToChain is ItemDropWithConditionRule pumpkinMoonConditionRule && pumpkinMoonConditionRule.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(pumpkinMoonConditionRule.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    if (ServerConfig.Instance.DropRelicsClassic) {
                                        pumpkinMoonConditionRule.condition = new Conditions.NotMasterMode();
                                    }

                                    else if (ServerConfig.Instance.DropRelicsExpert) {
                                        pumpkinMoonConditionRule.condition = new Conditions.IsExpert();
                                    }

                                    ArraySystem.RegisterNewRelic(pumpkinMoonConditionRule.itemId);
                                }
                            }
                        }
                        if (chain.RuleToChain is DropBasedOnMasterMode pumpkinMoonMasterDrop && pumpkinMoonMasterDrop.ruleForMasterMode is DropPerPlayerOnThePlayer pumpkinMoonDrop && pumpkinMoonDrop.condition is Conditions.IsMasterMode) {
                            if (ItemID.Search.TryGetName(pumpkinMoonDrop.itemId, out string name)) {
                                if (!ServerConfig.Instance.MasterToExpertBlacklist.Contains(new ItemDefinition(name))) {
                                    // Add and remove since the original rule is a DropBasedOnMasterMode drop
                                    int denom = ServerConfig.Instance.GuaranteedPets ? 1 : pumpkinMoonDrop.chanceDenominator;
                                    LeadingConditionRule newRule = new LeadingConditionRule(new Conditions.PumpkinMoonDropGatingChance());

                                    if (ServerConfig.Instance.DropPetsClassic) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(pumpkinMoonDrop.itemId, denom, pumpkinMoonDrop.amountDroppedMinimum, pumpkinMoonDrop.amountDroppedMaximum, new Conditions.NotMasterMode()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(pumpkinMoonDrop);
                                    }

                                    else if (ServerConfig.Instance.DropPetsExpert) {
                                        newRule.OnSuccess(new DropPerPlayerOnThePlayer(pumpkinMoonDrop.itemId, denom, pumpkinMoonDrop.amountDroppedMinimum, pumpkinMoonDrop.amountDroppedMaximum, new Conditions.IsExpert()));
                                        npcLoot.Add(newRule);
                                        npcLoot.Remove(pumpkinMoonDrop);
                                    }

                                    ArraySystem.RegisterNewPet(pumpkinMoonDrop.itemId);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
