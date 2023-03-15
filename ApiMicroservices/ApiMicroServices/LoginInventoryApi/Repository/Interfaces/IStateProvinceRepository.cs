using LoginInventoryApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository.Interfaces
{
    public interface IStateProvinceRepository
    {
        Task<List<StateProvinceModel>> GetStateProvinces(int countryId);
    }
}
