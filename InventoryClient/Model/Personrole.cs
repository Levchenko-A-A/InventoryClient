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
        private int personrolid;
        [JsonPropertyName("personroleid")]
        public int Personroleid 
        {
            get => personrolid;
            set
            {
                personrolid = value;
                OnProperyChanged(nameof(personrolid));
            }
        }

        private int personid;
        [JsonPropertyName("personid")]
        public int Personid 
        { 
            get => personid;
            set
            {
                personid = value;
                OnProperyChanged(nameof(personid));
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

