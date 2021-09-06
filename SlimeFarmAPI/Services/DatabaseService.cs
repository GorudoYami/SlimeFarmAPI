using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using SlimeFarmAPI.DTOs;
using SlimeFarmAPI.Game;

// TODO
// connection.CloseAsync() at the end of methods

namespace SlimeFarmAPI.Services {
    public class DatabaseService {
        private readonly string connectionString;
        public DatabaseService(Action<MySqlConnectionStringBuilder> configure) {
            var connectionInfo = new MySqlConnectionStringBuilder();
            configure.Invoke(connectionInfo);
            connectionString = connectionInfo.GetConnectionString(true);
        }

        private MySqlConnection GetConnection()
            => new(connectionString);

        public async Task<AccountDTO> GetAccountAsync(ulong? id = null, string nickname = null, string password = null, string email = null) {
            await using MySqlConnection connection = GetConnection();
            await connection.OpenAsync();
            await using MySqlCommand cmd = new("SELECT * FROM accounts", connection);

            // Add conditions if they are specified
            if (id != null || nickname != null || password != null || email != null) {
                cmd.CommandText += " WHERE ";
                bool flag = false;
                if (id != null) {
                    cmd.CommandText += "id = @Id";
                    cmd.Parameters.Add("Id", MySqlDbType.UInt64).Value = id;
                    flag = true;
                }

                if (nickname != null) {
                    if (flag)
                        cmd.CommandText += " AND ";
                    cmd.CommandText += "nickname = @Nickname";
                    cmd.Parameters.Add("Nickname", MySqlDbType.VarChar, 64).Value = nickname;
                    flag = true;
                }

                if (password != null) {
                    if (flag)
                        cmd.CommandText += " AND ";
                    cmd.CommandText += "password = @Password";
                    cmd.Parameters.Add("Password", MySqlDbType.VarChar, 128).Value = password;
                    flag = true;
                }

                if (email != null) {
                    if (flag)
                        cmd.CommandText += " AND ";
                    cmd.CommandText += "email = @Email";
                    cmd.Parameters.Add("Email", MySqlDbType.VarChar, 64).Value = email;
                }
            }
            else
                throw new Exception("At least one argument must be set");

            // Execute query
            await using var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows) {
                await reader.ReadAsync();
                AccountDTO result = new() {
                    Id = (ulong)reader["id"],
                    Email = (string)reader["email"],
                    Nickname = (string)reader["nickname"],
                    Password = (string)reader["password"],
                    Salt = (string)reader["salt"]
                };
                return result;
            }
            else
                return null;
        }

        public async Task<AccountDTO> GetAccountAsync(LoginDTO loginDTO) {
            await using MySqlConnection connection = GetConnection();
            await connection.OpenAsync();

            // Check for player using email
            await using MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM accounts WHERE email = @Email AND password = @Password";
            cmd.Parameters.Add("Email", MySqlDbType.VarChar, 64).Value = loginDTO.Email;
            cmd.Parameters.Add("Password", MySqlDbType.VarChar, 128).Value = loginDTO.Password;

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (reader.HasRows) {
                await reader.ReadAsync();
                AccountDTO result = new() {
                    Id = (ulong)reader["id"],
                    Email = (string)reader["email"],
                    Nickname = (string)reader["nickname"],
                    Password = (string)reader["password"],
                    Salt = (string)reader["salt"]
                };
                return result;
            }
            else
                return null;
        }

        public async Task InsertAccountAsync(AccountDTO accountDTO) {
            await using MySqlConnection connection = GetConnection();
            await connection.OpenAsync();

            using MySqlCommand cmd = new() {
                Connection = connection,
                CommandText = "INSERT INTO accounts VALUES (null, @Email, @Nickname, @Password, @Salt)"
            };

            cmd.Parameters.Add("Email", MySqlDbType.VarChar, 64).Value = accountDTO.Email;
            cmd.Parameters.Add("Nickname", MySqlDbType.VarChar, 64).Value = accountDTO.Nickname;
            cmd.Parameters.Add("Password", MySqlDbType.VarChar, 128).Value = accountDTO.Password;
            cmd.Parameters.Add("Salt", MySqlDbType.VarChar, 24).Value = accountDTO.Salt;

            if (await cmd.ExecuteNonQueryAsync() != 1)
                throw new Exception("This shouldn't ever happen");
        }

        //public async Task InsertDefaultsAsync(ulong accountId) {
        //    List<SlimeFarm> slimeFarms = new();
        //    List<FoodFarm> foodFarms = new();
        //    GameInfo gameInfo = new() {
        //        AccountId = accountId,
        //        Balance = 
        //    };
        //    await using MySqlConnection connection = GetConnection();
        //    await connection.OpenAsync();
        //}

        //public async Task<GameInfo> GetGameInfoAsync(ulong accountId) {
        //    await using MySqlConnection connection = GetConnection();
        //    await connection.OpenAsync();

        //    await using MySqlCommand cmd = new MySqlCommand() {
        //        Connection = connection,
        //        CommandText = "SELECT * FROM gameinfo WHERE account_id = @accountId"
        //    };

        //    await using var reader = await cmd.ExecuteReaderAsync();
        //    if (!reader.HasRows)
        //        throw new Exception("GameInfo not found for accountId: " + accountId);

        //    await reader.ReadAsync();
        //    GameInfo gameInfo = new GameInfo() {
        //        AccountId = accountId,
        //        Farms = JsonSerializer.Deserialize<List<Farm>>((string)reader["farms"]),
        //        Upgrades = JsonSerializer.Deserialize<List<Upgrade>>((string)reader["upgrades"]),
        //        Inventory = JsonSerializer.Deserialize<Inventory>((string)reader["inventory"]),
        //        Expeditions = JsonSerializer.Deserialize<List<Expedition>>((string)reader["expeditions"]),
        //        Balance = (ulong)reader["balance"],
        //        LastUpdate = (DateTime)reader["lastupdate"]
        //    };

        //    return gameInfo;
        //}
    }
}
