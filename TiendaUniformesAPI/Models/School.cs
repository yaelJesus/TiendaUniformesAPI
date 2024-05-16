using System;
using System.Collections.Generic;

namespace TiendaUniformesAPI.Models;

public partial class School : BaseEntity
{
    public int IdSc { get; set; }

    public string Name { get; set; } = null!;
}
