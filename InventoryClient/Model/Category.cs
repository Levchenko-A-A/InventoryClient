using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace InventoryClient.Model;

public partial class Category
{
    private int categoryid;
    [JsonPropertyName("categoryid")]
    public int Categoryid 
    {
        get => categoryid;
        set
        {
            categoryid = value;
            OnProperyChanged(nameof(categoryid));
        }
    }

    private string name;
    [JsonPropertyName("name")]
    public string Name 
    {
        get => name;
        set
        {
            name = value;
            OnProperyChanged(nameof(name));
        }
    }

    private string description;
    [JsonPropertyName("description")]
    public string Description 
    {
        get => description;
        set
        {
            description = value;
            OnProperyChanged(nameof(description));
        }
    }
    private DateTime createdat;
    [JsonPropertyName("createdat")]
    public DateTime? Createdat
    {
        get => createdat;
        set
        {
            createdat = (DateTime)value;
            OnProperyChanged(nameof(createdat));
        }
    }
    [JsonIgnore]
    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnProperyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
