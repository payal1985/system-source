using LoginInventoryApi.DBContext;
using LoginInventoryApi.Models;
using LoginInventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository
{
    public class StateProvinceRepository : IStateProvinceRepository
    {
        LoginContext _dbContext;

        public StateProvinceRepository(LoginContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<StateProvinceModel>> GetStateProvinces(int countryId)
        {
            try
            {
                var countries = await _dbContext.StateProvinces
                    .Where(sp => sp.country_ID == countryId)
                    .Select(
                    sp => new StateProvinceModel
                    {
                        Id = sp.State_Province_ID,
                        Name = sp.state_province
                    }).ToListAsync();
                return countries;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
