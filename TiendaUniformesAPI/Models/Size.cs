using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaUniformesAPI.Models;

public partial class Size : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdS { get; set; }
    [Required]
    public int Size1 { get; set; }
    [Required]
    public decimal Price { get; set; }
}
