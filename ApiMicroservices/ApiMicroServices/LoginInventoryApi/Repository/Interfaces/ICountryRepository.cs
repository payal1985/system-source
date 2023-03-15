using LoginInventoryApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<CountryModel>> GetCountries();
    }
}
