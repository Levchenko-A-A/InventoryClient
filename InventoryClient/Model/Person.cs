using InventoryClient.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace InventoryClient.Model;

public partial class Person: BaseViewModel
{
    private int personid;
    [JsonPropertyName("personid")]
    public int Personid 
    { 
        get => personid; 
        set
        {
            personid = value;
            OnPropertyChanged(nameof(personid));
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
            OnPropertyChanged(nameof(personname));
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
            OnPropertyChanged(nameof(passwordhash));
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
            OnPropertyChanged(nameof(salt));
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
            OnPropertyChanged(nameof(createdat));
        }
    }
}
