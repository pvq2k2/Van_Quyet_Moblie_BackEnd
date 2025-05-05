namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { 
            get
            {
                if (TotalCount == 0) return 0;
                //var total = PageSize / PageNumber;
                //if (PageSize % PageNumber != 0) total++;
                var total = (int)Math.Ceiling((double)TotalCount / PageSize);
                return total;
            }
        }

        public Pagination()
        {
            PageSize = -1;
            PageNumber = 1;
        }
    }
}
