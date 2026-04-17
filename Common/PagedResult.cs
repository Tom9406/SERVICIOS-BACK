namespace Logistica.API.Common
{
    public class PagedResult<T>
    {
        public int TotalRows { get; set; }
        public List<T> Data { get; set; } = new();
    }
}
