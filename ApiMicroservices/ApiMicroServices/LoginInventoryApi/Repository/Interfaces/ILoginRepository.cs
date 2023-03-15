using LoginInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository.Interfaces
{
    public interface ILoginRepository
    {
       // Task<List<ClientModel>> getClients(string user, string pwd);
        Task<List<ClientModel>> getClients(int userId);
        Task<UserModel> getUsers(string user, string pwd);
        Task<UserModel> getUser(int userId);
        Task<int> UpdateAcceptRules(int userId, string rules);
        Task<string> ForgotPassword(string email);
        Task<bool> InsertUser(UserModel usermodel);
    }
}
