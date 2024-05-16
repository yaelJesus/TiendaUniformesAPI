namespace TiendaUniformesAPI
{
    public class BaseResponse
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public IList<string>? Errors { get; set; }
    }
}
