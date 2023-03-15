using LoginInventoryApi.DBContext;
using LoginInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Repository
{
    public class LoginRepository : ILoginRepository
    {
        LoginContext _dbContext;
        public LoginRepository(LoginContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<ClientModel>> getClients(string user, string pwd)
        {
            try 
            {
                var result = (from usr in _dbContext.Users
                              join up in _dbContext.UserPermissions on usr.user_id equals up.user_id
                              join c in _dbContext.Clients on up.client_id equals c.client_id
                              where usr.username == user
                              where usr.password == pwd
                              where c.active == true
                              where up.permission > 0
                              where c.client_id > 1

                              select new ClientModel()
                              {
                                  ClientId = c.client_id,
                                  ClientName = c.name
                              }).AsQueryable().OrderBy(r => r.ClientName).ToList();

                return Task.Run(() => result);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public Task<UserModel> getUsers(string user, string pwd)
        {
            try
            {
                var result = (from usr in _dbContext.Users
                              join ut in _dbContext.UserTypes on usr.user_type_id equals ut.user_type_id
                              join up in _dbContext.UserPermissions on usr.user_id equals up.user_id
                              //join c in _dbContext.Clients on up.client_id equals c.client_id
                              where usr.username == user
                              where usr.password == pwd
                              //where c.active == true
                              where up.permission > 0
                              //where c.client_id > 1
                              //where c.client_id == 75
                              select new UserModel()
                              {
                                  UserId = usr.user_id,
                                  UserName = usr.username,    
                                  Password = usr.password,
                                  FirstName = usr.first_name,
                                  LastName = usr.last_name,
                                  Email = usr.email,
                                  //ClientId = c.client_id,
                                  //ClientName = c.name,
                                  UserTypeId = ut.user_type_id,
                                  UserType = ut.name,
                                  isAdmin = (ut.name == "Admin" ? true : false),
                                  role = (ut.name == "Admin" ? "Admin" : "User")
                                 //Enum.GetNames(typeof(Role.Admin))
                              //}).AsQueryable().FirstOrDefault();
                                }).AsQueryable().Distinct().FirstOrDefault();

                return Task.Run(() => result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
