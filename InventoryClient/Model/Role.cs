using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryClient.Model
{
    class Role
    {
        [JsonPropertyName("roleid")]
        public int Roleid { get; set; }
        [JsonPropertyName("rolename")]
        public string Rolename { get; set; } = null!;
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<Personrole> Personroles { get; set; } = new List<Personrole>();
    }
}
