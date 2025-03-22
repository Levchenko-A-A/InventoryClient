using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace InventoryClient.Model;

public partial class Person
{
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

    private string personname;
    [JsonPropertyName("personname")]
    public string Personname 
    {
        get => personname;
        set
        {
            personname = value;
            OnProperyChanged(nameof(personname));
        }
    }

    private string passwordhash;
    [JsonPropertyName("passwordhash")]
    public string Passwordhash 
    {
        get => passwordhash;
        set
        {
            passwordhash = value;
            OnProperyChanged(nameof(passwordhash));
        }
    }

    private string salt;
    [JsonPropertyName("salt")]
    public string Salt 
    {
        get => salt; 
        set
        {
            salt = value;
            OnProperyChanged(nameof(salt));
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
    public bool IsAdmin { get; set; } // новое для  Checkbox
    public bool IsManager { get; set; } // новое для  Checkbox
    public bool IsUser { get; set; }    //новое для  Checkbox
    public bool IsGuest { get; set; } //новое для  Checkbox

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnProperyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
