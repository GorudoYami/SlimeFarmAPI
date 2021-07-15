using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.Game {
    public class FoodFarm {
        public ulong Capacity { get; set; }
        public FoodId FoodType { get; set; }
        public double FoodAmount { get; set; }
        public double Yield { get; set; }
    }
}
