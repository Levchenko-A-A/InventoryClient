using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryClient.Model
{
    public class Personrole
    {
        private int userroleid;
        [JsonPropertyName("userroleid")]
        public int Userroleid 
        {
            get => userroleid;
            set
            {
                userroleid = value;
                OnProperyChanged(nameof(userroleid));
            }
        }

        private int userid;
        [JsonPropertyName("userid")]
        public int Userid 
        { 
            get => userid;
            set
            {
                userid = value;
                OnProperyChanged(nameof(userid));
            }
        }

        private int roleid;
        [JsonPropertyName("roleid")]
        public int Roleid 
        {
            get => roleid;
            set
            {
                roleid = value;
                OnProperyChanged(nameof(roleid));
            }
        }

        [JsonIgnore]
        public virtual Role? Role { get; set; }

        [JsonIgnore]
        public virtual Person? User { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnProperyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}

