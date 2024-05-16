using System;
using System.Collections.Generic;

namespace TiendaUniformesAPI.Models;

public partial class Customer : BaseEntity
{
    public int IdC { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;
}
