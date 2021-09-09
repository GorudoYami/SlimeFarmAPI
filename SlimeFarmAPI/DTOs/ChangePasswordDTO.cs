using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeFarmAPI.DTOs {
    public class ChangePasswordDTO {
        public ulong Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
