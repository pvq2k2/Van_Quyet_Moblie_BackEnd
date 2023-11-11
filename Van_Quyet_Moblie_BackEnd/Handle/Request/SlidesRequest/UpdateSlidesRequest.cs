namespace Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest
{
    public class UpdateSlidesRequest
    {
        public IFormFile? Image { get; set; }
        public int Status { get; set; }
        public int ProductID { get; set; }
    }
}
