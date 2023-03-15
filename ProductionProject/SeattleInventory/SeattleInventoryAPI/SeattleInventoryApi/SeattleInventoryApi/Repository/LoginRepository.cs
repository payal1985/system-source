using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public class LoginRepository : ILoginRepository
    {
        InventoryContext _dbContext;

        public LoginRepository(InventoryContext dbContext)
        {
            _dbContext = dbContext;           
        }
        public async Task<LoginModel> getUsers(string user, string pwd)
        {

            try
            {
                var result = _dbContext.Logins.Where(usr => usr.UserName.ToLower() == user.ToLower() && usr.Password == pwd.ToLower()).FirstOrDefault();
                
                var login = new LoginModel();
                if(result != null)
                {
                    login.UserId = result.UserId;
                    login.UserName = result.UserName;
                    login.ClientId = result.ClientId;
                    login.Password = result.Password;
                    login.isAdmin = result.isAdmin;
                    login.ClientName = _dbContext.Clients.Where(c => c.client_id == result.ClientId).FirstOrDefault().client_name;
                }

                return await Task.Run(() => login);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
