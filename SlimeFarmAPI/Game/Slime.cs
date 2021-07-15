using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.Game {
    public enum SlimeId {
        Pink,
        Geo,
        Puddle,
        Volcanic,
        Jungle,
        Fox,
        Vortex,
        Cowboy,
        Cave,
        Neko,
        Oceanic,
        Music,
        Unicorn,
        Void
    }

    public class Slime {
        public string Name { get; set; }
        public double BreedFactor { get; set; }
        public double HungerFactor { get; set; }
        public UpgradeId? Special { get; set; }
    }
}
