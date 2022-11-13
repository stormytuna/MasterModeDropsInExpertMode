using System.Collections.Generic;
using Terraria.ModLoader;

namespace MasterModeDropsInExpertMode.Common.Systems {
    public class ArraySystem : ModSystem {
        public static List<int> NewExpertDrops { get; private set; }

        public override void Unload() {
            NewExpertDrops = null;
        }

        public void RegisterNewExpertDrop(int type) {
            NewExpertDrops.Add(type);
        }
    }
}
