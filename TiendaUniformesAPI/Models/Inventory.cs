using System;
using System.Collections.Generic;

namespace TiendaUniformesAPI.Models;

public partial class Inventory : BaseEntity
{
    public int IdI { get; set; }

    public int IdSc { get; set; }

    public int IdG { get; set; }

    public int Quantitaty { get; set; }
}
