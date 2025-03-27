using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClient.Model
{
    public class RegisterUser
    {
        public static string? UserName { get; set; }
        public static string? access_token { get; set; }
        public string? Role { get; set; }
        public static List<Personrole>? UserAllId { get; set; }
    }
}
