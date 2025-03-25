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

    private bool isAdmin;
    public bool IsAdmin 
    {
        get => isAdmin;
        set
        {
            if (value)
            {
                isManager = false;
                isUser = false;
                isGuest = false;
                OnPropertyChanged(nameof(isManager));
                OnPropertyChanged(nameof(isUser));
                OnPropertyChanged(nameof(isGuest));
            }
            isAdmin = value;
            OnPropertyChanged(nameof(isAdmin));
        }
    }
    private bool isManager;
    public bool IsManager
    {
        get => isManager;
        set
        {
            if (value)
            {
                isAdmin = false;
                isUser = false;
                isGuest = false;
                OnPropertyChanged(nameof(isAdmin));
                OnPropertyChanged(nameof(isUser));
                OnPropertyChanged(nameof(isGuest));
            }
            isManager = value;
            OnPropertyChanged(nameof(isManager));
        }
    }
    private bool isUser;
    public bool IsUser
    {
        get => isUser;
        set
        {
            if (value)
            {
                isAdmin = false;
                isManager = false;
                isGuest = false;
                OnPropertyChanged(nameof(isAdmin));
                OnPropertyChanged(nameof(isManager));
                OnPropertyChanged(nameof(isGuest));
            }
            isUser = value;
            OnPropertyChanged(nameof(isUser));
        }
    }
    private bool isGuest;
    public bool IsGuest
    {
        get => isGuest;
        set
        {
            if (value)
            {
                isAdmin = false;
                isManager = false;
                isUser = false;
                OnPropertyChanged(nameof(isAdmin));
                OnPropertyChanged(nameof(isManager));
                OnPropertyChanged(nameof(isUser));
            }
            isGuest = value;
            OnPropertyChanged(nameof(isGuest));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
