using System;
using Xunit;
using Moq;

using Truckplanner.Business;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Protected;
using System.Net;
using System.Threading;

namespace tests
{
    public class CountryServiceTest
    {
        private readonly CountryService _service;
        private Mock<HttpMessageHandler> mockHttpMessageHandler;

        public CountryServiceTest()
        {
            var germany_bounds = ((2.0f, 2.0f), (4.0f, 4.0f));
            mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Default);


            _service = new CountryService(() => mockHttpMessageHandler.Object);
        }

        /*
         * Germany is located in the square that spans ( (2,2), (4,4) )
         *
         * Meaning
         * 5|
         * 4|  D D D
         * 3|  D D D
         * 2|  D D D
         * 1|_ _ _ _ _
         *   1 2 3 4 5
         */

        [Fact]
        public void GetsGermanyWhenCoordinateInGermany()
        {
            // TODO: Verify that the URL is correctly structured according to inputs
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"country\": \"Germany\"}")
                });

            var coordinate = (3.0f, 3.0f);
            var country = _service.GetCountry(coordinate).Result;

            Assert.Equal("Germany", country);
        }
    }
}
