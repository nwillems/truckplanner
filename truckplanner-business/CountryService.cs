using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Truckplanner.Business
{
    public class CountryService
    {
        private readonly Func<HttpMessageHandler> _messageHandlerFactory;

        public CountryService(Func<HttpMessageHandler> messageHandlerFactory)
        {
            _messageHandlerFactory = messageHandlerFactory;
        }

        public async Task<string> GetCountry((float, float) coordinate)
        {
            var serializer = new DataContractJsonSerializer(typeof(CountryServiceResponse));

            using (var client = new HttpClient(_messageHandlerFactory()))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "TruckPlanner v1");

                string url = string.Format("http://country-service/{0}/{1}/", coordinate.Item1, coordinate.Item2);
                var responseTask = client.GetStreamAsync(url);

                var response = serializer.ReadObject(await responseTask) as CountryServiceResponse;

                if (!string.IsNullOrEmpty(response.error_message))
                {
                    throw new Exception(string.Format("Sorry, I failed getting the country. Error: \"{0}\"", response.error_message)); // Maybe retry?
                }

                return response.country;
            }

        }

        public class CountryServiceResponse
        {
            public string error_message;
            public string country;
        }
    }
}
