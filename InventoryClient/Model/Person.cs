using System;
using System.Collections.Generic;

namespace InventoryClient.Model;

public partial class Person
{
    public int Personid { get; set; }

    public string Personname { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime? Createdat { get; set; }
}
