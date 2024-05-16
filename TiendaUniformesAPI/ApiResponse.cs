namespace TiendaUniformesAPI
{
    public class ApiResponse<T> : BaseResponse where T : class
    {
        public T? Data { get; set; }
    }
}
