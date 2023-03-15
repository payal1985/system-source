using InventoryApi.DBModels.SSIDBModels;
using InventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IInventoryCompanyRepository
    {
        Task<List<CompanyModel>> GetCompany();
        Task<List<CompanyUserModel>> GetCompanyUsers(int companyid);
    }
}
