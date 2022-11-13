using MasterModeDropsInExpertMode.Common.Configs;
using MasterModeDropsInExpertMode.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace MasterModeDropsInExpertMode.Common.GlobalItems {
    public class MMDIEMGlobalItem : GlobalItem {
        public override void SetDefaults(Item item) {
            if (ArraySystem.NewExpertDrops.Contains(item.type) && ServerConfig.Instance.FixTooltips) {
                item.master = false;
                item.expert = true;
            }
        }
    }
}
