using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClient.Model
{
    class Role
    {
        public int Roleid { get; set; }

        public string Rolename { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<Personrole> Personroles { get; set; } = new List<Personrole>();
    }
}
