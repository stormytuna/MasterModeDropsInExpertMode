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
            if (npc.ModNPC?.Mod.Name == "CalamityMod") {
                return;
            }

            if (npc.type == NPCID.HallowBoss && ServerConfig.Instance.EmpressDropsDaytimeOnly) {
                return;
            }

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
            }
        }
    }
}