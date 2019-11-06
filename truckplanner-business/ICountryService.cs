using System;
using System.Threading.Tasks;

namespace Truckplanner.Business
{
    public interface ICountryService : IDisposable
    {
        public Task<string> GetCountry((float, float) coordinate);
    }
}
