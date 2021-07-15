using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.Game {
    public class SlimeFarm {
        public ulong Capacity { get; set; }
        public SlimeId SlimeType { get; set; }
        public double SlimeAmount { get; set; }
        public double Blobs { get; set; }
        public FoodId FoodType { get; set; }
        public double FoodAmount { get; set; }
    }
}
