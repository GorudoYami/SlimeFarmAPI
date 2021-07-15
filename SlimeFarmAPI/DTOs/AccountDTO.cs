using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.DTOs {
    public class AccountDTO {
        public ulong Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
