using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryClient.Model
{
    public class Role
    {
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

        private string rolename;
        [JsonPropertyName("rolename")]
        public string Rolename 
        {
            get => rolename; 
            set
            {
                rolename = value;
                OnProperyChanged(nameof(rolename));
            }
        }

        private string description;
        [JsonPropertyName("description")]
        public string? Description 
        {
            get => description; 
            set
            {
                description = value;
                OnProperyChanged(nameof(description));
            }
        }

        [JsonIgnore]
        public virtual ICollection<Personrole> Personroles { get; set; } = new List<Personrole>();

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
