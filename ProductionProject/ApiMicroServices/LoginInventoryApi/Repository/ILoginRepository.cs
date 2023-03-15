using LoginInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository
{
    public interface ILoginRepository 
    {
        Task<List<ClientModel>> getClients(string user, string pwd);
        Task<UserModel> getUsers(string user, string pwd);

    }
}
