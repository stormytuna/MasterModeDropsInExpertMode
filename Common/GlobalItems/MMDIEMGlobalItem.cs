using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasterModeDropsInExpertMode.Common.GlobalItems {
    public class MMDIEMGlobalItem : GlobalItem {
        public override void SetDefaults(Item item) {
            if ((ArraySystem.Relics.Contains(item.type) || ArraySystem.Pets.Contains(item.type)) && ServerConfig.Instance.FixTooltips) {
                item.master = false;

                if (ServerConfig.Instance.DropRelicsClassic) {
                    item.rare = ArraySystem.Relics.Contains(item.type) ? ItemRarityID.Green : ItemRarityID.LightPurple;
                }

                else if (ServerConfig.Instance.DropRelicsExpert) {
                    item.expert = true;
                    item.rare = ItemRarityID.Expert;
                }
            }
        }
    }
}
