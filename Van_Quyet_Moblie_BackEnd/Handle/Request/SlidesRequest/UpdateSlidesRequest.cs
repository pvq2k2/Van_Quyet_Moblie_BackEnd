namespace Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest
{
    public class UpdateSlidesRequest
    {
        public IFormFile? Image { get; set; }
        public int Status { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int ProductID { get; set; }
    }
}
