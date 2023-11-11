using Van_Quyet_Moblie_BackEnd.Handle.Converter;
using Van_Quyet_Moblie_BackEnd.Helpers.DBContext;

namespace Van_Quyet_Moblie_BackEnd.Services.Implement
{
    public class BaseService
    {
        protected readonly AppDbContext _context;
        protected readonly AuthConverter _authConverter;
        protected readonly ProductTypeConverter _productTypeConverter;
        protected readonly ProductConverter _productConverter;
        protected readonly DecentralizationConverter _decentralizationConverter;
        protected readonly ProductImageConverter _productImageConverter;
        protected readonly ProductReviewConverter _productReviewConverter;
        protected readonly CartConverter _cartConverter;
        protected readonly OrderConverter _orderConverter;
        protected readonly VoucherConverter _voucherConverter;
        protected readonly SlidesConverter _slidesConverter;


        public BaseService() {
            _context = new AppDbContext();
            _authConverter = new AuthConverter();
            _productTypeConverter = new ProductTypeConverter();
            _productConverter = new ProductConverter();
            _decentralizationConverter = new DecentralizationConverter();
            _productImageConverter = new ProductImageConverter();
            _productReviewConverter = new ProductReviewConverter();
            _cartConverter = new CartConverter();
            _orderConverter = new OrderConverter();
            _voucherConverter = new VoucherConverter();
            _slidesConverter = new SlidesConverter();
        }
    }
}
