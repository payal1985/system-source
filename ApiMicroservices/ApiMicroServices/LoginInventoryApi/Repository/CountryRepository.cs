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
    public class CountryRepository : ICountryRepository
    {
        LoginContext _dbContext;

        public CountryRepository(LoginContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CountryModel>> GetCountries()
        {
            try
            {
                var countries = await _dbContext.Countries.Select(
                    c => new CountryModel
                    {
                        Id = c.Country_ID,
                        Name = c.country
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
