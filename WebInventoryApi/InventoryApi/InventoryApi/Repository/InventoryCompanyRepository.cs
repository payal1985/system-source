using InventoryApi.DBContext;
using InventoryApi.DBModels.SSIDBModels;
using InventoryApi.Models;
using InventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class InventoryCompanyRepository : IInventoryCompanyRepository
    {
        SSIRequestContext _dbContext;

        public InventoryCompanyRepository(SSIRequestContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CompanyModel>> GetCompany()
        {
            try
            {
                //return await _dbContext.Comapnies.Where(usr=> !string.IsNullOrEmpty(usr.company_name)).
                //    Select(usr => new CompanyModel() { CompanyId = (int)usr.company_ID, CompanyName = usr.company_name }).Distinct().ToListAsync();


                //return await _dbContext.Users.Where(usr=> !string.IsNullOrEmpty(usr.company) && usr.company_id != null).
                //                              Select(usr => new CompanyModel()
                //                                      { 
                //                                          CompanyId = (int)usr.company_id, 
                //                                          CompanyName = usr.company 
                //                                      }
                //                              ).Distinct().ToListAsync();


                return await _dbContext.Comapnies.Select(cmp => new CompanyModel()
                                              {
                                                  CompanyId = (int)cmp.company_ID,
                                                  CompanyName = cmp.company_name
                                              }
                                              ).Distinct().OrderBy(ord=>ord.CompanyName).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CompanyUserModel>> GetCompanyUsers(int companyid)
        {
            try
            {
                return await _dbContext.Users.Where(usr => !string.IsNullOrEmpty(usr.company) && usr.company_id.Equals(companyid))
                            .Select(x=>new CompanyUserModel() { UserId=x.user_id,UserName=x.username }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
