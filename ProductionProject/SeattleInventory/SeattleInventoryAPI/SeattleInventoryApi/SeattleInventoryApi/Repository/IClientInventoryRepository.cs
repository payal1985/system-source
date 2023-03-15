using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IClientInventoryRepository
    {
        Task<List<ClientInventoryModel>> getClients();

        Task<bool> updateClientHasInventory(ClientInventoryModel model);

        Task<bool> insertClient(ClientInventoryModel clientInventoryModel);
    }
}
