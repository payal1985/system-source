 using LoginInventoryApi.DBContext;
using LoginInventoryApi.DBModels;
using LoginInventoryApi.Helpers;
using LoginInventoryApi.Models;
using LoginInventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            
        //public async Task<List<ClientModel>> getClients(string user, string pwd)
        //{
        //    try
        //    {
        //        //var userModel = await _dbContext.Users.Where(usr => (usr.username.ToLower() == user.ToLower()
        //        //                                                || usr.email == user)
        //        //                                        && usr.password == pwd).FirstOrDefaultAsync();


        //        var result = (from usr in _dbContext.Users
        //                      join up in _dbContext.UserPermissions on usr.user_id equals up.user_id
        //                      join c in _dbContext.Clients on up.client_id equals c.client_id
        //                      where usr.username.ToLower() == user.ToLower()
        //                      where usr.password == pwd
        //                      where c.active == true
        //                      where up.permission > 0
        //                      where c.client_id > 1
        //                      select new ClientModel()
        //                      {
        //                          ClientId = c.client_id,
        //                          ClientName = c.name,
        //                          Inventory_Client_Group_ID = c.inventory_client_group_id,
        //                          Path = c.path
        //                          //}).AsQueryable().OrderBy(r => r.ClientName).ToList();
        //                      }).AsQueryable();

        //        return await result.OrderBy(r => r.ClientName).ToListAsync(); 
        //        ////return Task.Run(() => result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public Task<List<ClientModel>> getClients(int userId)
        {
            try
            {
                //var result = (from usr in _dbContext.Users
                //              join up in _dbContext.UserPermissions on usr.user_id equals up.user_id
                //              join c in _dbContext.Clients on up.client_id equals c.client_id
                //              where usr.user_id == userId
                //              where c.active
                //              where up.permission > 0
                //              where c.client_id > 1
                //              select new ClientModel()
                //              {
                //                  ClientId = c.client_id,
                //                  ClientName = c.name,
                //                  Inventory_Client_Group_ID = c.inventory_client_group_id
                //              }).AsQueryable().OrderBy(r => r.ClientName).ToList();

                var result = (from up in _dbContext.UserPermissions //on usr.user_id equals up.user_id
                              join c in _dbContext.Clients on up.client_id equals c.client_id
                              where up.user_id == userId
                              where c.active
                              where up.permission > 0
                              where c.client_id > 1
                              select new ClientModel()
                              {
                                  ClientId = c.client_id,
                                  ClientName = c.name,
                                  ClientInventoryDisplaycolumn = c.client_inventory_display_column,
                                  Path = c.path
                              }).AsQueryable().OrderBy(r => r.ClientName).ToList();

                return Task.Run(() => result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserModel> getUsers(string user, string pwd)
        {
            try
            {
                var userentity = await _dbContext.Users.Where(usr => (usr.username.ToLower() == user.ToLower() || usr.email == user)
                                                        && usr.password == pwd).FirstOrDefaultAsync();
                
               // string perm_level = await PermissionLevel(userentity.user_id);

                var result = (from ut in _dbContext.UserTypes
                              join up in _dbContext.UserPermissions on userentity.user_id equals up.user_id
                              where ut.user_type_id == userentity.user_type_id
                              where up.permission > 0
                              select new UserModel()
                              {
                                  UserId = userentity.user_id,
                                  UserName = userentity.username,
                                  Password = "",
                                  FirstName = userentity.first_name,
                                  LastName = userentity.last_name,
                                  Email = userentity.email,
                                  Inventory_App = true,
                                  UserTypeId = ut.user_type_id,
                                  UserType = ut.name,
                                  //isAdmin = (ut.name == "Admin" ? true : false),
                                  //Role = (ut.name == "Admin" ? "Admin" : "User"),
                                  isAdmin = ((ut.name == "Admin" || ut.name == "Client") ? true : false),
                                  Role = ((ut.name == "Admin" || ut.name == "Client") ? "Admin" : "User"),
                                  inventory_user_accept_rules_reqd = userentity.inventory_user_accept_rules_reqd,
                                  Title= userentity.user_title,
                                  Company = userentity.company,
                                  CompanyId = userentity.company_id,
                                  StreetAddress = userentity.addr1,
                                  Country=userentity.country_ID,
                                  State = userentity.state_province_ID,
                                  City = userentity.city,
                                  Zip = userentity.zip,
                                  WorkPhone = userentity.work_phone,
                                  MobilePhone = userentity.mobile
                                  //}).AsQueryable().Distinct().FirstOrDefault();
                              }).AsQueryable();

                var usrresult = await result.Distinct().FirstOrDefaultAsync();

                //usrresult.PermissionLevel = perm_level;

                usrresult.Clients = await ClientsList(userentity.user_id);

                //return await result.Distinct().FirstOrDefaultAsync();
                return usrresult;

                //var result = (from usr in _dbContext.Users
                //              join ut in _dbContext.UserTypes on usr.user_type_id equals ut.user_type_id
                //              join up in _dbContext.UserPermissions on usr.user_id equals up.user_id
                //              where (usr.username.ToLower() == user.ToLower() || usr.email == user)
                //              where usr.password == pwd
                //              where up.permission > 0
                //              select new UserModel()
                //              {
                //                  UserId = usr.user_id,
                //                  UserName = usr.username,
                //                  Password = usr.password,
                //                  FirstName = usr.first_name,
                //                  LastName = usr.last_name,
                //                  Email = usr.email,
                //                  Inventory_App = true,
                //                  UserTypeId = ut.user_type_id,
                //                  UserType = ut.name,
                //                  isAdmin = (ut.name == "Admin" ? true : false),
                //                  Role = (ut.name == "Admin" ? "Admin" : "User"),
                //                  inventory_user_accept_rules_reqd = usr.inventory_user_accept_rules_reqd
                //              //}).AsQueryable().Distinct().FirstOrDefault();
                //              }).AsQueryable();

                //return await result.Distinct().FirstOrDefaultAsync();

                ////return Task.Run(() => result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<UserModel> getUser(int userId)
        {
            try
            {
                var result = (from usr in _dbContext.Users
                              join ut in _dbContext.UserTypes on usr.user_type_id equals ut.user_type_id
                              join up in _dbContext.UserPermissions on usr.user_id equals up.user_id
                              where usr.user_id == userId
                              where up.permission > 0
                              select new UserModel()
                              {
                                  UserId = usr.user_id,
                                  UserName = usr.username,
                                  Password = usr.password,
                                  FirstName = usr.first_name,
                                  LastName = usr.last_name,
                                  Email = usr.email,
                                  UserTypeId = ut.user_type_id,
                                  UserType = ut.name,
                                  isAdmin = ((ut.name == "Admin" || ut.name == "Client") ? true : false),
                                  Role = ((ut.name == "Admin" || ut.name == "Client") ? "Admin" : "User")
                              }).AsQueryable().Distinct().FirstOrDefault();

                return Task.Run(() => result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> UpdateAcceptRules(int userId, string rules)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.user_id == userId);
                if (user != null && !string.IsNullOrWhiteSpace(user.inventory_user_accept_rules_reqd))
                {
                    // Remove rules
                    var currentRules = user.inventory_user_accept_rules_reqd.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var incomeRules = rules.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var newRules = currentRules.Except(incomeRules);
                    user.inventory_user_accept_rules_reqd = $",{string.Join(",", newRules)},";
                    return await _dbContext.SaveChangesAsync();
                }

                return -1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> ForgotPassword(string email)
        {
            string result = "";
            try
            {
                var entity = await _dbContext.Users.Where(usr => usr.email == email).FirstOrDefaultAsync();
                if(entity != null)
                    result = entity.password;                
            }
            catch(Exception ex)
            {
                throw;
            }

            return result;
        }

        public async Task<bool> InsertUser(UserModel model)
        {
            bool result = false;
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Users user = new Users();

                    user.first_name = model.FirstName;
                    user.last_name = model.LastName;
                    user.job_title = model.Title;
                    user.company = model.Company;
                    user.addr1 = model.StreetAddress;
                    user.country_ID = model.Country;
                    user.state_province_ID = model.State;
                    user.city = model.City;
                    user.zip = model.Zip;
                    user.work_phone = model.WorkPhone;
                    user.mobile = model.MobilePhone;
                    user.email = model.Email;
                    user.user_type_id = model.UserTypeId;
                    user.createdate = System.DateTime.Now;
                    user.updatedate = System.DateTime.Now;
                    user.inventory_user_accept_rules_reqd = ",1,2";
                    user.password = model.Password;
                    //user.password = $"{model.FirstName.FirstOrDefault().ToString().ToUpperInvariant()}{model.LastName}$${System.DateTime.Now.ToString("yy")}";
                    user.username = model.FirstName.FirstOrDefault().ToString() + model.LastName;
                    //user.username = "testmp1";
                    user.active = true;

                    await _dbContext.AddAsync(user);
                    await _dbContext.SaveChangesAsync();

                    //int user_id = user.user_id;

                    await transaction.CommitAsync();

                    result = true;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        private async Task<List<ClientModel>> ClientsList(int userid)
        {
            try
            {
                var result = (from up in _dbContext.UserPermissions //on usr.user_id equals up.user_id
                              join c in _dbContext.Clients on up.client_id equals c.client_id
                              where up.user_id == userid
                              where c.active
                              where up.permission > 0
                              where c.client_id > 1
                              select new ClientModel()
                              {
                                  ClientId = c.client_id,
                                  ClientName = c.name,
                                  ClientInventoryDisplaycolumn = c.client_inventory_display_column,
                                  Path = c.path,
                                  InventoryApp = c.inventory_app
                              //}).AsQueryable().OrderBy(r => r.ClientName).ToList();
                              }).AsQueryable();

                var clientresult = await result.OrderBy(r => r.ClientName).ToListAsync();

                foreach(var client in clientresult)
                {
                    client.Permission = await PermissionLevel(userid,client.ClientId);
                }

                return clientresult;
            }
            catch (Exception ex)
            {
                throw;
            }
           

        }
        private async Task<string> PermissionLevel(int userid,int clientid)
        {
            var perm = await _dbContext.UserPermissions.Where(p => p.user_id == userid && p.client_id == clientid && p.permission > 0).Select(r=>r.permission).ToListAsync();
            List<string> permlevel = new List<string>();

            foreach(var p in perm)
            {                

                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_EDIT_INVENTORY_ITEMS) == (int)EnumsHelper.Permission.PERM_CLIENT_EDIT_INVENTORY_ITEMS)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_EDIT_INVENTORY_ITEMS.ToString());
                }
                if((p & (int)EnumsHelper.Permission.PERM_CLIENT_INVENTORY_ADMIN) == (int)EnumsHelper.Permission.PERM_CLIENT_INVENTORY_ADMIN)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_INVENTORY_ADMIN.ToString());
                }
                if((p & (int)EnumsHelper.Permission.PERM_CLIENT_NEW_INVENTORY_ITEM) == (int)EnumsHelper.Permission.PERM_CLIENT_NEW_INVENTORY_ITEM)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_NEW_INVENTORY_ITEM.ToString());
                }
                if((p & (int)EnumsHelper.Permission.PERM_CLIENT_VIEW_INVENTORY_ITEMS) == (int)EnumsHelper.Permission.PERM_CLIENT_VIEW_INVENTORY_ITEMS)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_VIEW_INVENTORY_ITEMS.ToString());
                }                
                if ((p & (int)EnumsHelper.Permission.PERM_INSTALLER_UPDATE_INVITEM_BARCODE) == (int)EnumsHelper.Permission.PERM_INSTALLER_UPDATE_INVITEM_BARCODE)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_INSTALLER_UPDATE_INVITEM_BARCODE.ToString());
                }

                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_USER_ADMIN) == (int)EnumsHelper.Permission.PERM_CLIENT_USER_ADMIN)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_USER_ADMIN.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT) == (int)EnumsHelper.Permission.PERM_CLIENT)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_NEW_REQUEST) == (int)EnumsHelper.Permission.PERM_CLIENT_NEW_REQUEST)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_NEW_REQUEST.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_VIEW_REQUESTS) == (int)EnumsHelper.Permission.PERM_CLIENT_VIEW_REQUESTS)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_VIEW_REQUESTS.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_EDIT_REQUESTS) == (int)EnumsHelper.Permission.PERM_CLIENT_EDIT_REQUESTS)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_EDIT_REQUESTS.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_CREATE_NOTES) == (int)EnumsHelper.Permission.PERM_CLIENT_CREATE_NOTES)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_CREATE_NOTES.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_NEW_PROPOSAL) == (int)EnumsHelper.Permission.PERM_CLIENT_NEW_PROPOSAL)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_NEW_PROPOSAL.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_VIEW_PROPOSAL) == (int)EnumsHelper.Permission.PERM_CLIENT_VIEW_PROPOSAL)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_VIEW_PROPOSAL.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_EDIT_PROPOSAL) == (int)EnumsHelper.Permission.PERM_CLIENT_EDIT_PROPOSAL)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_EDIT_PROPOSAL.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_EMAIL_REMINDERS) == (int)EnumsHelper.Permission.PERM_CLIENT_EMAIL_REMINDERS)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_EMAIL_REMINDERS.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_LOCATIONS_ADMIN) == (int)EnumsHelper.Permission.PERM_CLIENT_LOCATIONS_ADMIN)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_LOCATIONS_ADMIN.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_ADMIN) == (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_ADMIN)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_CATALOG_ADMIN.ToString());
                }

                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_SETTINGS_ADMIN) == (int)EnumsHelper.Permission.PERM_CLIENT_SETTINGS_ADMIN)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_SETTINGS_ADMIN.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_SEARCH_REQUESTS) == (int)EnumsHelper.Permission.PERM_CLIENT_SEARCH_REQUESTS)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_SEARCH_REQUESTS.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_DISCOUNT_ADMIN) == (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_DISCOUNT_ADMIN)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_CATALOG_DISCOUNT_ADMIN.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG) == (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_CATALOG.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_SHOP) == (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_SHOP)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_CATALOG_SHOP.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_PRICE) == (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_PRICE)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_CATALOG_PRICE.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_ARIBA) == (int)EnumsHelper.Permission.PERM_CLIENT_CATALOG_ARIBA)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_CATALOG_ARIBA.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_HELPDESK_VIEW) == (int)EnumsHelper.Permission.PERM_HELPDESK_VIEW)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_HELPDESK_VIEW.ToString());
                }
                if ((p & (int)EnumsHelper.Permission.PERM_HELPDESK_CREATE) == (int)EnumsHelper.Permission.PERM_HELPDESK_CREATE)
                {
                    permlevel.Add(EnumsHelper.Permission.PERM_HELPDESK_CREATE.ToString());
                }
            }  
            if(permlevel.Count <= 0)
            {
                permlevel.Add(EnumsHelper.Permission.PERM_CLIENT_VIEW_INVENTORY_ITEMS.ToString());
            }


            return string.Join(",", permlevel);            
        }

    }
}
