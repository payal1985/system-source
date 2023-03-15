using Microsoft.EntityFrameworkCore.Storage;
using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public class ClientInventoryRepository : IClientInventoryRepository
    {
        InventoryContext _dbContext;
        public ClientInventoryRepository(InventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ClientInventoryModel>> getClients()
        {
            try
            {

                //var listClientInventoryModels = (from cust in _dbContext.Customers
                //                                 join cli in _dbContext.Clients on cust.CustID equals cli.teamdesign_cust_no 
                //                                 where cust.CustID  != null
                //                                 where cust.CompanyName != ""
                //                                 where cust.CompanyName != null
                //                                 select new
                //                                 {
                //                                    cli.client_id,
                //                                    cust.CompanyName,
                //                                    cli.teamdesign_cust_no,
                                                 //   cust.HasInventory

                                                 //}).AsQueryable().ToList();



                var listClientInventoryModels = (from cust in _dbContext.Customers
                                                 join cli in _dbContext.Clients on cust.CustID equals cli.teamdesign_cust_no                                                  
                                                 where cust.CustID != null
                                                 where cust.CompanyName != ""
                                                 select new ClientInventoryModel()
                                                 {
                                                     client_id = cli.client_id,
                                                     client_name = cust.CompanyName,
                                                     CustId = cli.teamdesign_cust_no,
                                                     has_inventory = cust.HasInventory

                                                 }).AsQueryable().ToList();

                return await Task.Run(() => listClientInventoryModels.ToList());

                //return await Task.Run(() => new List<ClientInventoryModel>().ToList());

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> updateClientHasInventory(ClientInventoryModel model)
        {
            bool result = false;
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = _dbContext.Clients.Where(c => c.client_id == model.client_id).FirstOrDefault();
                    var custEntity = _dbContext.Customers.Where(cust => cust.CustID == model.CustId).FirstOrDefault();

                    if (entity != null)
                    {
                        entity.has_inventory = model.has_inventory;
                        _dbContext.Update(entity);
                        await _dbContext.SaveChangesAsync();                      
                    }

                    if (custEntity != null)
                    {
                        custEntity.HasInventory = model.has_inventory;
                        _dbContext.Update(custEntity);
                        await _dbContext.SaveChangesAsync();
                    }

                    transaction.Commit();

                    result = true;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;

                }
            }

            //return await Task.Run(() => result);
            return result;

        }

        public async Task<bool> insertClient(ClientInventoryModel clientInventoryModel)
        {
            bool result = false;

            try
            {
                Client client = new Client();
              //  client.ssidb_client_id = clientInventoryModel.ssidb_client_id;
                client.client_name = clientInventoryModel.client_name;
               // client.teamdesign_cust_no = clientInventoryModel.teamdesign_cust_no;
                client.has_inventory = clientInventoryModel.has_inventory;

                //Order orderModel = new Order();

                //orderModel.email = model.requestoremail;
                //orderModel.project = model.request_individual_project;
                //orderModel.location = "";
                //orderModel.room = model.destination_location;
                //// ordeModel.instdate = TimeZoneInfo.ConvertTimeFromUtc(model.requested_inst_date.Date,TimeZoneInfo.Local).Date;                    
                ////orderModel.instdate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(model.requested_inst_date),TimeZoneInfo.Local);                    
                //orderModel.instdate = Convert.ToDateTime(model.requested_inst_date);
                ////orderModel.instdate = System.DateTime.Now;                    
                //orderModel.comments = model.comments;
                //orderModel.destb = model.destination_building;
                //orderModel.destf = model.destination_floor;
                //orderModel.instdater = System.DateTime.Now;

                await _dbContext.AddAsync(client);
                await _dbContext.SaveChangesAsync();

                int client_id = client.client_id;

                result = true;

            }
            catch (Exception ex)
            {
                throw;

            }

            return result;


        }
    }
}
