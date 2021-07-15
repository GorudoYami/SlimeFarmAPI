using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.Game {
    public enum FoodId {
        Pineapple,
        Mango,
        Carrot,
        Dandelion,
        CrystalShard,
        VolcanicAsh,
        Meat,
        Hay,
        Mushroom,
        Seaweed,
        EnchantedApple
    }

    public class Food {
        public string Name { get; set; }
        public double Saturation { get; set; }
    }
}
