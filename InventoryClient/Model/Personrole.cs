using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClient.Model
{
    public class Personrole
    {
        public int Userroleid { get; set; }

        public int? Userid { get; set; }

        public int? Roleid { get; set; }

        public virtual Role? Role { get; set; }

        public virtual Person? User { get; set; }
    }
}
