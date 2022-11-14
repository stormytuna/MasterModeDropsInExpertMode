using System.Collections.Generic;
using Terraria.ModLoader;

namespace MasterModeDropsInExpertMode.Common.Systems {
    public class ArraySystem : ModSystem {
        public static List<int> Relics { get; private set; } = new();
        public static List<int> Pets { get; private set; } = new();

        public override void Unload() {
            Relics = null;
            Pets = null;
        }

        public static void RegisterNewRelic(int type) {
            if (!Relics.Contains(type)) {
                Relics.Add(type);
            }
        }

        public static void RegisterNewPet(int type) {
            if (!Pets.Contains(type)) {
                Pets.Add(type);
            }
        }
    }
}
