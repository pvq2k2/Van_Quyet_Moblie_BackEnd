using VQM.Models.TokenAuth;
using VQM.Web.Controllers;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace VQM.Web.Tests.Controllers;

public class HomeController_Tests : VQMWebTestBase
{
    [Fact]
    public async Task Index_Test()
    {
        await AuthenticateAsync(null, new AuthenticateModel
        {
            UserNameOrEmailAddress = "admin",
            Password = "123qwe"
        });

        //Act
        var response = await GetResponseAsStringAsync(
            GetUrl<HomeController>(nameof(HomeController.Index))
        );

        //Assert
        response.ShouldNotBeNullOrEmpty();
    }
}