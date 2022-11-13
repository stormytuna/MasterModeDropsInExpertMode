using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MasterModeDropsInExpertMode.Common.Configs {
    public class ServerConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static ServerConfig Instance; // tMod sets up singletons for us

        [Label("Master Mode pets drop in Expert Mode")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool DropPets { get; set; }

        [Label("Master Mode relics drop in Expert Mode")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool DropRelics { get; set; }

        [Label("Master tooltip replaced with Expert tooltip")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool FixTooltips { get; set; }

        [Label("Pets have 100% drop chance")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool GuaranteedPets { get; set; }

        [Label("Master Mode to Expert Mode item blacklist")]
        [Tooltip("Adding an item to this list will prevent its Master Mode drop from being converted to an Expert Mode drop")]
        [ReloadRequired]
        public List<ItemDefinition> MasterToExpertBlacklist = new List<ItemDefinition>();
    }
}