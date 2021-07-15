using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.Game {
    public enum UpgradeId {
        Capacity_1,
        Capacity_2,
        Capacity_3,
        Capacity_4,
        Capacity_5,
        Efficiency_1,
        Efficiency_2,
        Efficiency_3,
        Efficiency_4,
        Efficiency_5,
        Discount_1,
        Discount_2,
        Discount_3,
        Discount_4,
        Discount_5,
        Luck_1,
        Luck_2,
        Luck_3,
        Speed_1,
        Speed_2,
        Speed_3,
        Yield_1,
        Yield_2,
        Yield_3,
        ClimbingGear,
        Flashlight,
        HeatUniform,
        Water,
        Shade,
        Net,
        Music,
        Reinforced
    }

    public class Upgrade {
        public string Name { get; set; }
        public double Factor { get; set; }
        public uint Length { get; set; }
    }
}
