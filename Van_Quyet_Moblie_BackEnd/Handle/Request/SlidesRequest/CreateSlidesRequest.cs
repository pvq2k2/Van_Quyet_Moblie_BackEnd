namespace Van_Quyet_Moblie_BackEnd.Handle.Request.SlidesRequest
{
    public class CreateSlidesRequest
    {
        public IFormFile? Image { get; set; }
        public int ProductID { get; set; }
    }
}
