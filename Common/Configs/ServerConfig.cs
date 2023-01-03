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
        public bool DropPetsExpert { get; set; }

        [Label("Master Mode relics drop in Expert Mode")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool DropRelicsExpert { get; set; }

        [Label("Master Mode pets drop in Classic Mode")]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool DropPetsClassic { get; set; }

        [Label("Master Mode relics drop in Classic Mode")]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool DropRelicsClassic { get; set; }

        [Label("Empress of Light only drops at daytime")]
        [Tooltip("This will make Empress of Light only drop relics and pets when killed during the day instead of all the time")]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool EmpressDropsDaytimeOnly { get; set; }

        [Label("Replace Master only information")]
        [Tooltip("In Expert, the 'Master' tooltip and rarity are replaced with Expert versions\nIn Classic, the 'Master' tooltip is removed and the rarity is changed to Green or Light Purple")]
        [DefaultValue(true)]
        public bool FixTooltips { get; set; }

        [Label("Pets have 100% drop chance")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool GuaranteedPets { get; set; }

        [Label("Item blacklist")]
        [Tooltip("Adding an item to this list will prevent its Master Mode drop from being converted to an Expert Mode drop")]
        [ReloadRequired]
        public List<ItemDefinition> MasterToExpertBlacklist = new List<ItemDefinition>();
    }
}