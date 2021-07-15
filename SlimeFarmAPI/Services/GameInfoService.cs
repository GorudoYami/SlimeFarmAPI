using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlimeFarmAPI.Game;

namespace SlimeFarmAPI.Services {
    public class GameInfoService {
        private readonly Dictionary<SlimeId, Slime> slimesInfo;
        private readonly Dictionary<UpgradeId, Upgrade> upgradesInfo;
        private readonly Dictionary<FoodId, Food> foodInfo;

        private readonly DatabaseService database;
        public GameInfoService(DatabaseService database) {
            this.database = database;

            // Read slimes info
            using (StreamReader reader = new("Data/slimes.json")) {
                slimesInfo = JsonConvert.DeserializeObject<Dictionary<SlimeId, Slime>>(reader.ReadToEnd());
                reader.Close();
            }

            // Read upgrades info
            using (StreamReader reader = new("Data/upgrades.json")) {
                upgradesInfo = JsonConvert.DeserializeObject<Dictionary<UpgradeId, Upgrade>>(reader.ReadToEnd());
                reader.Close();
            }

            // Read food info
            using (StreamReader reader = new("Data/food.json")) {
                foodInfo = JsonConvert.DeserializeObject<Dictionary<FoodId, Food>>(reader.ReadToEnd());
                reader.Close();
            }
        }

        private async Task<GameInfo> UpdateGameInfoAsync(GameInfo gameInfo) {
            throw new NotImplementedException();
        }
    }
}
