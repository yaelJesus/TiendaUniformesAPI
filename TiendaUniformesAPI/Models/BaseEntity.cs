namespace TiendaUniformesAPI.Models
{
    public class BaseEntity
    {
        public bool IsActive { get; set; }
        public int CreateUser { get; set; }
        public DateOnly CreateDate { get; set; }
        public int? ModifyUser { get; set; }
        public DateOnly? ModifyDate { get; set; }
    }
}
