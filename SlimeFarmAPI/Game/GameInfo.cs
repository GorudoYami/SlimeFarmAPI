using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.Game {
    public class GameInfo {
        public ulong AccountId { get; set; }
        //public List<Farm> Farms { get; set; }
        public List<Upgrade> Upgrades { get; set; }
        //public Inventory Inventory { get; set; }
        //public List<Expedition> Expeditions { get; set; }
        public ulong Balance { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
