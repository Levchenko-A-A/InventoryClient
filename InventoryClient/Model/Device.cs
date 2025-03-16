using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace InventoryClient.Model;

public partial class Device
{
    private int deviceid;
    [JsonPropertyName("deviceid")]
    public int Deviceid 
    {
        get => deviceid; 
        set
        {
            deviceid = value;
            OnProperyChanged(nameof(deviceid));
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

    private int categoryid;
    [JsonPropertyName("categoriid")]
    public int Categoryid 
    {
        get => categoryid;
        set
        {
            categoryid = value;
            OnProperyChanged(nameof(categoryid));
        }
    }

    private int manufacturerid;
    [JsonPropertyName("manufacturerid")]
    public int Manufacturerid 
    {
        get => manufacturerid;
        set
        {
            manufacturerid = value;
            OnProperyChanged(nameof(manufacturerid));
        }
    }

    private int locationid;
    [JsonPropertyName("locationid")]
    public int Locationid 
    {
        get => locationid;
        set
        {
            locationid = value;
            OnProperyChanged(nameof(locationid));
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
    public virtual ICollection<Barcode> Barcodes { get; set; } = new List<Barcode>();
    [JsonIgnore]
    public virtual Category? Category { get; set; }
    [JsonIgnore]
    public virtual ICollection<Inventorynumber> Inventorynumbers { get; set; } = new List<Inventorynumber>();
    [JsonIgnore]
    public virtual Location? Location { get; set; }
    [JsonIgnore]
    public virtual Manufacturer? Manufacturer { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnProperyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
