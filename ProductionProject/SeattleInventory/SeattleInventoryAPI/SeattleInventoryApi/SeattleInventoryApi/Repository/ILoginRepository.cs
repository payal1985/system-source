using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface ILoginRepository
    {
        Task<LoginModel> getUsers(string user, string pwd);
    }
}
