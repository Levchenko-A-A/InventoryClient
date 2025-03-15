using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryClient.Model
{
    public class Personrole
    {
        [JsonPropertyName("userroleid")]
        public int Userroleid { get; set; }
        [JsonPropertyName("userid")]
        public int? Userid { get; set; }
        [JsonPropertyName("roleid")]
        public int? Roleid { get; set; }
        [JsonPropertyName("role")]
        public virtual Role? Role { get; set; }
        [JsonIgnore]
        public virtual Person? User { get; set; }
    }
}

