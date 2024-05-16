using System;
using System.Collections.Generic;

namespace TiendaUniformesAPI.Models;

public partial class User
{
    public int IdU { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public bool IsActive { get; set; }
}
