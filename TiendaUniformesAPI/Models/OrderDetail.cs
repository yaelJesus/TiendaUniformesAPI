using System;
using System.Collections.Generic;

namespace TiendaUniformesAPI.Models;

public partial class OrderDetail : BaseEntity
{
    public int IdOd { get; set; }

    public int IdO { get; set; }

    public int IdG { get; set; }

    public int Quantitaty { get; set; }
}
