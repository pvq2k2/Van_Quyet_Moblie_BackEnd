namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class GHNHelper
    {
        //    private readonly IConfiguration _configuration;

        //    public GHNHelper(IConfiguration configuration)
        //    {
        //        _configuration = configuration;
        //    }
        //    public async Task<string> CreateShippingOrderAsync()
        //    {
        //        var apiUrl = _configuration.GetSection("AppSettings:GHNSettings:APICreate").Value;

        //        var httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Add("Token", _configuration.GetSection("AppSettings:GHNSettings:Token").Value);
        //        httpClient.DefaultRequestHeaders.Add("ShopId", _configuration.GetSection("AppSettings:GHNSettings:ShopID").Value);

        //        var requestData = new
        //        {
        //            payment_type_id = 2,
        //            note = "Tintest 123",
        //            required_note = "KHONGCHOXEMHANG",
        //            return_phone = "0332190158",
        //            return_address = "39 NTT",
        //            client_order_code = "",
        //            from_name = "TinTest124",
        //            from_phone = "0987654321",
        //            from_address = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
        //            from_ward_name = "Phường 14",
        //            from_district_name = "Quận 10",
        //            from_province_name = "HCM",
        //            to_name = "TinTest124",
        //            to_phone = "0987654321",
        //            to_address = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
        //            to_ward_name = "Phường 14",
        //            to_district_name = "Quận 10",
        //            to_province_name = "HCM",
        //            cod_amount = 200000,
        //            content = "Theo New York Times",
        //            weight = 200,
        //            length = 1,
        //            width = 19,
        //            height = 10,
        //            cod_failed_amount = 2000,
        //            pick_station_id = 1444,
        //            deliver_station_id = 0,
        //            insurance_value = 10000000,
        //            service_id = 0,
        //            service_type_id = 2,
        //            coupon = 0,
        //            pickup_time = 1692840132,
        //            pick_shift = new List<int> { 2 },
        //            items = new List<Item>
        //        {
        //            new Item
        //            {
        //                name = "Áo Polo",
        //                code = "Polo123",
        //                quantity = 1,
        //                price = 200000,
        //                length = 12,
        //                width = 12,
        //                weight = 1200,
        //                height = 12,
        //                category = new Category
        //                {
        //                    level1 = "Áo"
        //                }
        //            }
        //        }
        //            //    FromName = "Shop",
        //            //    FromPhone = "0987654321",
        //            //    FromAddress = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
        //            //    FromWardName = "Phường 14",
        //            //    FromDistrictName = "Quận 10",
        //            //    FromProvinceName = "HCM",
        //            //    ToName = "User",
        //            //    ToPhone = "0987654321",
        //            //    ToAddress = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
        //            //    ToWardName = "Phường 14",
        //            //    ToDistrictName = "Quận 10",
        //            //    ToProvinceName = "HCM",
        //            //    ReturnPhone = "0332190158",
        //            //    ReturnAddress = "39 NTT",
        //            //    ReturnDistrictName = "Quận 10",
        //            //    ReturnWardName = "Phường 14",
        //            //    ClientOrderCode = "Order_",
        //            //    CodAmount = 200000,
        //            //    Content = "Content test",
        //            //    Weight = 200,
        //            //    Length = 1,
        //            //    Width = 19,
        //            //    Height = 10,
        //            //    PickStationId = 1444,
        //            //    InsuranceValue = 10000000,
        //            //    Coupon = null,
        //            //    ServiceTypeId = 2,
        //            //    ServiceId = 0,
        //            //    PaymentTypeId = 2,
        //            //    Note = "Test",
        //            //    RequiredNote = "KHONGCHOXEMHANG",
        //            //    PickShift = new List<int> { 2 },
        //            //    PickupTime = 1692840132,
        //            //    Items = new List<Item>
        //            //{
        //            //    new Item
        //            //    {
        //            //        Name = "Name Product",
        //            //        Code = "Mã sản phẩm",
        //            //        Quantity = 1,
        //            //        Price = 20000,
        //            //        Length = 1,
        //            //        Width = 1,
        //            //        Weight = 1,
        //            //        Height = 1,
        //            //        CodFailedAmount = 0
        //            //    }
        //            //}
        //        };

        //        var jsonRequestData = JsonSerializer.Serialize(requestData);
        //        var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

        //        var response = await httpClient.PostAsync(apiUrl, content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseContent = await response.Content.ReadAsStringAsync();
        //            return responseContent;
        //        }
        //        else
        //        {
        //            throw new Exception("Failed to create shipping order. HTTP Status Code: " + response.StatusCode);
        //        }
        //    }
        //}

        //public class Item
        //{
        //    public string name { get; set; }
        //    public string code { get; set; }
        //    public int quantity { get; set; }
        //    public int price { get; set; }
        //    public int length { get; set; }
        //    public int width { get; set; }
        //    public int weight { get; set; }
        //    public int height { get; set; }
        //    public Category category { get; set; }
        //}

        //public class Category
        //{
        //    public string level1 { get; set; }
    }
}
