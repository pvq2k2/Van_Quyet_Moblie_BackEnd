using Requests;

namespace Responses
{
    public class PageResult<T>
    {
        public Pagination Pagination { get; set; }
        public List<T> Data { get; set; }

        public PageResult(Pagination pagination, List<T> data)
        {
            Pagination = pagination;
            Data = data;
        }
    }
}
