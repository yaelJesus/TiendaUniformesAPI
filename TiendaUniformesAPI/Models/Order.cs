using System;
using System.Collections.Generic;

namespace TiendaUniformesAPI.Models;

public partial class Order : BaseEntity
{
    public int IdO { get; set; }

    public DateOnly DateOrder { get; set; }

    public DateOnly DeadLine { get; set; }

    public int IdC { get; set; }

    public decimal TotalPrice { get; set; }
}
