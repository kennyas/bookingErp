using Booking.Core.Context;
using Booking.Core.Services;
using Booking.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Tornado.Shared.EF.Repository;
using Xunit;
using Tornado.Shared.Test.BaseClassTest;
using Xunit.Abstractions;
using Booking.Tests.Setup;
using Booking.Core.Models;
using Booking.Core.ViewModels;
using Tornado.Shared.AspNetCore;
using Moq;
using System.Threading.Tasks;
using Tornado.Shared.ViewModels;
using Booking.Api.Controllers;
using Tornado.Shared.EF;

namespace Booking.Tests.ServiceTests
{
    public class CountryServiceTest : BaseClassTest, IClassFixture<ServiceSetup>
    {
        private readonly Mock<ICountryService> _countryServiceMock;
        private readonly Mock<IRepository<Country>> _countryMockRepository;
        private readonly Mock<IHttpUserService> _currentUserServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public CountryServiceTest(ITestOutputHelper output, ServiceSetup serviceSetup) : base(output)
        {
            _countryServiceMock = new  Mock<ICountryService>();
            _countryMockRepository = new Mock<IRepository<Country>>();
            _currentUserServiceMock = new Mock<IHttpUserService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        //[Fact]
        //public async Task Ensure_Country_Was_Created()
        //{
        //    //Arrange

        //    var getFakeCountryModel = FakeCountryModel();
        //    var context = new Mock<DbContext>();

        //    var countryModelResponse = FakeCountryResponseModel(getFakeCountryModel);

        //    _countryServiceMock.Setup(p => p.CreateCountryAsync(It.IsAny<CountryCreateModel>()))
        //        .Returns(Task.FromResult(countryModelResponse));
            
        //    var countryController = new CountryApiController(_countryServiceMock.Object);

        //    var response = await countryController.CreateCountry(getFakeCountryModel);

        //    Assert.Equal(1, (int)response.Code);
        //    Assert.Equal(response.Payload.Code, getFakeCountryModel.Code);
        //}

        [Fact]
        public void Ensure_Country_Was_Updated()
        {
            
        }

        [Fact]
        public void Can_Get_Country_Detail()
        {
        }

        [Fact]
        public void Endure_Country_Deleted()
        {
          
        }
        [Fact]
        public void Can_Get_Paginated_Country_Details()
        {

          
        }


        private CountryCreateModel FakeCountryModel()
        {
            return new CountryCreateModel
            {
                Code = "NGA",
                Description = "Most populous black nation",
                Name = "Nigeria",
            };
        }

        private ApiResponse<CountryCreateModel> FakeCountryResponseModel(CountryCreateModel model)
        {
            return new ApiResponse<CountryCreateModel>
            {
                Payload = model
            };
        }
        private  Guid GetFakeCustomerId()
        {
            return Guid.NewGuid();
        }
    }
}
