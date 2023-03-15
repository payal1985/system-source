using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public class DonationInventoryRepository : IDonationInventoryRepository
    {
        InventoryContext _dbContext;
        public DonationInventoryRepository(InventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<DonationInventoryModel> GetInventory(string category)
        {

            try
            {
                List<Inventory> result = new List<Inventory>();

                if (!string.IsNullOrEmpty(category))
                {

                    var inventoryModels = (from i in _dbContext.Inventories
                                           join ii in _dbContext.InventoryItems on i.inventory_id equals ii.inventory_id into invitem
                                           from ii in invitem.DefaultIfEmpty()
                                           join oi in _dbContext.OrderItems on i.inventory_id equals oi.inv_id into ordinvitem
                                           from oi in ordinvitem.DefaultIfEmpty()
                                           join o in _dbContext.Orders on oi.order_id equals o.order_id into ord
                                           from o in ord.DefaultIfEmpty()

                                               //join iii in _dbContext.InventoryItemImages on ii.inv_item_id equals iii.inv_item_id into invitemimg
                                               //from InventoryItemImages in invitemimg.DefaultIfEmpty()
                                           where i.category == category
                                           where o.complete == false
                                           where o.destb.Contains("Donate")
                                           where oi.completed == false
                                           select new DonationInventoryModel()
                                           {
                                               inventory_id = i.inventory_id,
                                               item_code = i.item_code,                                              
                                               description = i.description,
                                               additional_description = i.additionaldescription,
                                               manuf = i.manuf,
                                               fabric = i.fabric,
                                               finish = i.finish,
                                               qty =  i.InventoryItems.Count,                                             

                                               InventoryItemModels = (from ii in _dbContext.InventoryItems
                                                                      where ii.inventory_id == i.inventory_id                                                                     
                                                                      select new InventoryItemModel
                                                                      {
                                                                          inventory_id = ii.inventory_id,
                                                                          inv_item_id = ii.inv_item_id,
                                                                          building = ii.building,
                                                                          floor = ii.floor,
                                                                          mploc = ii.mploc,
                                                                          cond = ii.cond
                                                                      }).ToList(),

                                               image_name = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_name).FirstOrDefault(),
                                               image_url = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_url).ToList()

                                           }).AsQueryable().ToList();

                    if (inventoryModels.Count > 0)
                    {
                        var invModels = inventoryModels.GroupBy(x => x.inventory_id).Select(s => s.First()).Distinct().ToList();

                        foreach (var item in invModels)
                        {
                            var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();

                            var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                            {
                                inventory_id = item.inventory_id,
                                inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
                                inv_image_name = item.image_name,
                                inv_image_url = item.image_url.FirstOrDefault(),
                                building = x.building,
                                floor = x.floor,
                                mploc = x.mploc,
                                cond = x.cond,
                                qty = x.qtyCount,
                                item_code = item.item_code,
                                description = item.description
                            });

                            invModels.Where(r => r.inventory_id == item.inventory_id)
                              .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();

                            StringBuilder stringBuilder = new StringBuilder();
                            int index = 1;
                            foreach (var grp in gropinglocation)
                            {                              

                                if (gropinglocation.Count == index)
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount}");
                                else
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount} \n");

                                index++;
                            }

                            invModels.Where(r => r.inventory_id == item.inventory_id)
                               .Select(r => { r.tootip_inv_item = stringBuilder.ToString(); return r; }).ToList();
                        }

                        return invModels;

                    }
                    else
                        return new List<DonationInventoryModel>();

                }
                else
                    return new List<DonationInventoryModel>();

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<string> GetInventoryCategory()
        {
            try
            {
                //return _dbContext.Inventories.Where(i => !string.IsNullOrEmpty(i.category)).Select(cat => cat.category).Distinct().ToList();

                var donationCategory = (from o in _dbContext.Orders
                                       join oi in _dbContext.OrderItems on o.order_id equals oi.order_id 
                                       join i in _dbContext.Inventories on oi.inv_id equals i.inventory_id                                
                                       where o.complete == false
                                       where o.destb.Contains("Donate")
                                       where oi.completed == false
                                       orderby i.category,i.item_code                                       
                                       select i.category
                                       ).Distinct().ToList();

                if(donationCategory.Count > 0)
                {
                   // donationCategory = donationCategory.GroupBy(x =>x.).Select(s => s.First()).Distinct().ToList();

                }

                return donationCategory;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
