using System;
using System.Collections.Generic;

namespace TiendaUniformesAPI.Models;

public partial class Garment : BaseEntity
{
    public int IdG { get; set; }

    public string Type { get; set; } = null!;

    public string? Desctiption { get; set; }

    public int IdS { get; set; }

    public int IdSc { get; set; }
}
