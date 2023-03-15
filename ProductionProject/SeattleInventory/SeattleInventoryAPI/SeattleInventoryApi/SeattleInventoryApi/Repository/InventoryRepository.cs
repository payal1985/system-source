using Microsoft.EntityFrameworkCore;
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
    public class InventoryRepository : IInventoryRepository
    {
        InventoryContext _dbContext;
        public InventoryRepository(InventoryContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<InventoryModel>> GetInventory(string category,int client_id, string bulding, string floor,string room,string cond)
        {

            try
            {
                List<Inventory> result = new List<Inventory>();

                if (!string.IsNullOrEmpty(category))
                {
                    #region Subquery method using linq query
                    //var v = (from i in _dbContext.Inventories
                    //        where i.category == category
                    //        select new InventoryModel()
                    //        {

                    //            inventory_id = i.inventory_id,
                    //            item_code = i.item_code,
                    //            //description = i.description.ToString(),
                    //            description = i.description.ToString() + "-" + i.additionaldescription.ToString(),
                    //            manuf = i.manuf,
                    //            height = i.height.ToString(),
                    //            width = i.width.ToString(),
                    //            depth = i.depth.ToString(),
                    //            hwd_str = (i.height.ToString() == "0.000" ? "" : i.height.ToString() + "X") + (i.width.ToString() == "0.000" ? "" : i.width.ToString() + "X") + (i.depth.ToString() == "0.000" ? "" : i.depth.ToString()),
                    //            fabric = i.fabric,
                    //            finish = i.finish,
                    //            qty = i.InventoryItems.Count,
                    //            InventoryItemModels = (from ii in _dbContext.InventoryItems
                    //                                   where ii.inventory_id == i.inventory_id
                    //                                   where ii.building.Contains((!string.IsNullOrEmpty(bulding) ? bulding : ""))
                    //                                   where ii.floor.Contains((!string.IsNullOrEmpty(floor) ? floor : ""))
                    //                                   select new InventoryItemModel
                    //                                   {
                    //                                       inventory_id = ii.inventory_id,
                    //                                       inv_item_id = ii.inv_item_id,
                    //                                       building = ii.building,
                    //                                       floor = ii.floor,
                    //                                       mploc = ii.mploc,
                    //                                       cond = ii.cond
                    //                                   }).ToList(),

                    //            image_name = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_name).FirstOrDefault()

                    //        }).ToList();

                    #endregion 

                    var inventoryModels = (from i in _dbContext.Inventories
                                           join ii in _dbContext.InventoryItems on i.inventory_id equals ii.inventory_id into invitem
                                           from ii in invitem.DefaultIfEmpty()
                                               //join iii in _dbContext.InventoryItemImages on ii.inv_item_id equals iii.inv_item_id into invitemimg
                                               //from InventoryItemImages in invitemimg.DefaultIfEmpty()
                                           where i.category == category
                                           where i.client_id == client_id
                                           where ii.building.Contains((!string.IsNullOrEmpty(bulding) ? bulding : ""))
                                           where ii.floor.Contains((!string.IsNullOrEmpty(floor) ? floor : ""))
                                           where ii.mploc.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                           where ii.cond.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
                                           where ii.building != "Donate"
                                           select new InventoryModel()
                                           {
                                               inventory_id = i.inventory_id,
                                               item_code = i.item_code,
                                               //description = i.description.ToString(),
                                               description = (!string.IsNullOrEmpty(i.additionaldescription) ? i.description + "-" + i.additionaldescription : i.description),
                                               manuf = i.manuf,
                                               height = i.height.ToString(),
                                               width = i.width.ToString(),
                                               depth = i.depth.ToString(),
                                               //hwd_str = (i.height.ToString() == "0.000" ? "" : i.height.ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : i.width.ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : i.depth.ToString() + "d"),
                                               //hwd_str = (i.height.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.height), 1).ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.width),1).ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : Math.Round(Convert.ToDecimal(i.depth),1).ToString() + "d"),
                                               hwd_str = (i.height.ToString() == "0.000" ? "" : Decimal.Round(i.height, 1).ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : Decimal.Round(i.width,1).ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : Decimal.Round(i.depth,1).ToString() + "d"),
                                               fabric = i.fabric,
                                               finish = i.finish,
                                               client_id=i.client_id,
                                               category = i.category,
                                               qty = (!string.IsNullOrEmpty(bulding) && !string.IsNullOrEmpty(floor) && !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x=>x.building == bulding && x.floor == floor && x.mploc == room && x.cond == cond).Count()
                                                                                                                     : (!string.IsNullOrEmpty(bulding) ? i.InventoryItems.Where(x =>x.building == bulding).Count()
                                                                                                                                                       : !string.IsNullOrEmpty(floor) ? i.InventoryItems.Where(x =>x.floor == floor).Count()
                                                                                                                                                       : !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.mploc == room).Count()
                                                                                                                                                       : !string.IsNullOrEmpty(cond) ? i.InventoryItems.Where(x => x.cond == cond).Count()
                                                                                                                                                                                      : i.InventoryItems.Count)),
                                                                                                                 
                                               //qty = (!string.IsNullOrEmpty(bulding) && !string.IsNullOrEmpty(floor) ? i.InventoryItems.GroupBy(x => new { x.building, x.floor }).Count()
                                               //       : (!string.IsNullOrEmpty(bulding) ? i.InventoryItems.GroupBy(x => new { x.building }).Count() 
                                               //                                         : !string.IsNullOrEmpty(floor) ? i.InventoryItems.GroupBy(x => new { x.floor }).Count() 
                                               //                                                                         : i.InventoryItems.Count)) ,
                                               //qty = i.InventoryItems.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Count(),
                                                                                //.Select(g=> new {
                                                                                //   building = g.Key.building,
                                                                                //   floor = g.Key.floor,
                                                                                //   mploc = g.Key.mploc,
                                                                                //   cond = g.Key.cond }).Count(),

                   
                                                                   //inventoryResult.GroupBy(x => x.inventory_id,
                                                                   //                                 (key, values) => new
                                                                   //                                 {
                                                                   //                                     inventory_id = key,
                                                                   //                                     qty = values.Count()
                                                                   //                                 });

                                                InventoryItemModels = (from ii in _dbContext.InventoryItems
                                                where ii.inventory_id == i.inventory_id
                                                where ii.building.Contains((!string.IsNullOrEmpty(bulding) ? bulding : ""))
                                                where ii.floor.Contains((!string.IsNullOrEmpty(floor) ? floor : ""))
                                                where ii.mploc.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                                where ii.cond.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
                                                where ii.building != "Donate"
                                                select new InventoryItemModel
                                                {
                                                    inventory_id = ii.inventory_id,
                                                    inv_item_id = ii.inv_item_id,
                                                    building = ii.building,
                                                    floor = ii.floor,
                                                    mploc = ii.mploc,
                                                    cond = ii.cond,
                                                    client_id = ii.client_id
                                                }).ToList(),

                                               image_name = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_name).FirstOrDefault(),
                                               image_url = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_url).FirstOrDefault()
                                             //  qty = (!string.IsNullOrEmpty(floor) ? i.InventoryItems.GroupBy(x => new { x.floor }).Count() : i.InventoryItems.Count),

                                           }).AsQueryable().ToList();

                    if(inventoryModels.Count > 0)
                    {
                        
                        var invModels = inventoryModels.GroupBy(x => x.inventory_id).Select(s => s.First()).Distinct().ToList();

                        invModels.RemoveAll(i => i.qty == 0);

                        foreach (var item in invModels)
                        {
                            var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();

                            var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                            {
                                inventory_id = item.inventory_id,
                                //inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
                                inv_item_id = item.InventoryItemModels.Where(i=>i.building == x.building && i.floor == x.floor && i.mploc == x.mploc && i.cond == x.cond).FirstOrDefault().inv_item_id,
                                inv_image_name = item.image_name,
                                inv_image_url = item.image_url,
                                building = x.building,
                                floor = x.floor,
                                mploc = x.mploc,
                                cond = x.cond,
                                qty = x.qtyCount,
                                item_code = item.item_code,
                                description = item.description,
                                client_id = item.client_id
                            });

                            invModels.Where(r => r.inventory_id == item.inventory_id)
                              .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();

                            StringBuilder stringBuilder = new StringBuilder();
                            int index = 1;
                            foreach (var grp in gropinglocation)
                            {
                                int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.building == grp.building && i.floor == grp.floor && i.mploc == grp.mploc && i.cond == grp.cond).FirstOrDefault().inv_item_id;

                                //inventoryResult.Where(r => r.inventory_id == item.inventory_id).Select(x => new InventoryItemModel() {
                                //    inventory_id = item.inventory_id,
                                //    inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
                                //    inv_image_name = item.image_name,
                                //    building = grp.building,
                                //    floor = grp.floor,
                                //    mploc = grp.mploc,
                                //    cond = grp.cond,
                                //    qty = grp.qtyCount
                                //}).ToList();

                                //< a[routerLink] = ""(click) = "onGoToPage2()" > Go to page</ a >
                                //if (gropinglocation.Count == index)
                                //    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount}");
                                //else
                                //    stringBuilder.Append($"<a href='' (click)='openDialogNew({tmp_inv_item_id})'>{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount}</a> \n");


                                if (gropinglocation.Count == index)
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount}");
                                else
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount} \n");

                                index++;
                            }

                            invModels.Where(r => r.inventory_id == item.inventory_id)
                               .Select(r => { r.tootip_inv_item = stringBuilder.ToString(); return r; }).ToList();
                        }

                        //await Task.Run(() => manager.GetList());

                        //return invModels;
                        return await Task.Run(() => invModels.ToList());

                    }
                    else
                        return new List<InventoryModel>();

                    #region Oldcode

                    //var inventoryResult = result.Select(x => new InventoryModel
                    //{
                    //    inventory_id = x.inventory_id
                    //    ,
                    //    description = x.description.ToString()
                    //    ,
                    //    manuf = x.manuf
                    //    ,
                    //    height = x.height.ToString()
                    //    ,
                    //    width = x.width.ToString()
                    //    ,
                    //    depth = x.depth.ToString()
                    //    //,
                    //    //building = x.building
                    //    //,
                    //    //floor = x.floor
                    //    //,
                    //    //mploc = x.mploc
                    //    //,
                    //    //cond = x.cond
                    //    //,
                    //    //image_name = x.image_name
                    //    //,
                    //    //image_url = x.image_url
                    //    //,
                    //    //inv_item_id = x.inv_item_id
                    //    ,
                    //    fabric = x.fabric
                    //    ,
                    //    finish = x.finish
                    //}).ToList();

                    //var qtyresult = inventoryResult.GroupBy(x => x.inventory_id,
                    //                                 (key, values) => new
                    //                                 {
                    //                                     inventory_id = key,
                    //                                     qty = values.Count()
                    //                                 });

                    //var newinventoryResult = inventoryResult.GroupBy(dist => dist.inventory_id).FirstOrDefault().ToList();


                    //var invitemresult = (inventoryResult.GroupBy(x => x.inventory_id, x => new { x.building, x.floor, x.mploc, x.cond })
                    //                    .Select(g => new { inventory_id = g.Key, Values = g.ToList() })).ToList();

                    ////Select(x => { 
                    ////bulding = x.by                    });
                    //List<InventoryItemModel> inventoryItemModels = new List<InventoryItemModel>();

                    //foreach (var item in invitemresult)
                    //{
                    //    inventoryItemModels.Add(new InventoryItemModel()
                    //    {
                    //        inventory_id = item.inventory_id,
                    //        invItemDetailsModels = item.Values.Select(x => new InvItemDetailsModel()
                    //        {
                    //            building = x.building,
                    //            floor = x.floor,
                    //            mploc = x.mploc,
                    //            cond = x.cond
                    //        }).ToList()

                    //    });
                    //}



                    //// var newinventoryResult = inventoryResult.GroupBy(dist => dist.inventory_id).FirstOrDefault().ToList();
                    //var newinventoryResult = inventoryResult.GroupBy(dist => dist.inventory_id).Select(r => r.FirstOrDefault()).ToList();

                    //foreach (var qty in qtyresult)
                    //{
                    //    newinventoryResult.Where(r => r.inventory_id == qty.inventory_id).Select(r => { r.qty = qty.qty; return r; }).ToList();
                    //}



                    //foreach (var iim in inventoryItemModels)
                    //{
                    //    //newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //    //    .Select(r => { r.InventoryItemModelsList = iim; return r; }).ToList();


                    //    var gropinglocation = iim.invItemDetailsModels.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() });

                    //    var invDetailsModel = gropinglocation.Select(x => new InvItemDetailsModel { building = x.building, floor = x.floor, mploc = x.mploc, cond = x.cond, qty = x.qtyCount });

                    //    newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //      .Select(r => { r.invItemDetailsModel = invDetailsModel.ToList(); return r; }).ToList();

                    //    StringBuilder stringBuilder = new StringBuilder();
                    //    int index = 1;
                    //    foreach (var grp in invDetailsModel)
                    //    {
                    //        if (invDetailsModel.Count() == index)
                    //            stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qty}");
                    //        else
                    //            stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qty} \n");

                    //        index++;
                    //        //  inv.building + ': ' + inv.floor + ': Loc ' + inv.mploc + ': Cond ' + inv.cond
                    //        // newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //        //.Select(r => { r.invItemDetailsModel = invDetailsModel.ToList(); return r; }).ToList();
                    //    }
                    //    newinventoryResult.Where(r => r.inventory_id == iim.inventory_id)
                    //       .Select(r => { r.tootip_inv_item = stringBuilder.ToString(); return r; }).ToList();



                    //}


                    //return newinventoryResult;
                    #endregion

                }
                //return _dbContext.Inventories.Where(inv => inv.category.ToLower().Equals(category.ToLower())).ToList();
                else
                    return new List<InventoryModel>();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<InventoryModel> GetInventory1(string category, string bulding, string floor)
        {

            try
            {
                List<Inventory> result = new List<Inventory>();

                if (!string.IsNullOrEmpty(category))
                {
                    if ((!string.IsNullOrEmpty(bulding) && bulding != "undefined") && (!string.IsNullOrEmpty(floor) && floor != "undefined"))
                    {
                        result = _dbContext.Inventories.Where(i => i.category == category).Include(ii => ii.InventoryItems.Where(x => x.building == bulding && x.floor == floor)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();
                    }
                    else if (!string.IsNullOrEmpty(bulding) && bulding != "undefined")
                    {
                        result = _dbContext.Inventories.Where(i => i.category == category).Include(ii => ii.InventoryItems.Where(x => x.building == bulding)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();
                    }
                    else if (!string.IsNullOrEmpty(floor) && floor != "undefined")
                    {
                        result = _dbContext.Inventories.Where(i => i.category == category).Include(ii => ii.InventoryItems.Where(x => x.floor == floor)).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();
                    }
                    else
                    {
                        result = _dbContext.Inventories.Where(i => i.category == category).Include(ii => ii.InventoryItems).ThenInclude(iii => iii.InventoryItemImages.Take(1)).ToList();

                    }
                    // var result = _dbContext.Inventories.Where(i => i.category == category).Include(ii => ii.InventoryItems).ThenInclude(iii => iii.InventoryItemImages.Take(1));

                    result = result.Where(x => x.InventoryItems.Count > 0).Select(s => { return s; }).ToList();

                    if(result.Count > 0)
                    {
                        var inventoryResult = result.Select(x => new InventoryModel
                        {
                            inventory_id = x.inventory_id,
                            item_code = x.item_code,
                            description = x.description.ToString(),
                            manuf = x.manuf,
                            height = x.height.ToString(),
                            width = x.width.ToString(),
                            depth = x.depth.ToString(),
                            hwd_str = (x.height.ToString() == "0.000" ? "" : x.height.ToString() + "X") + (x.width.ToString() == "0.000" ? "" : x.width.ToString() + "X") + (x.depth.ToString() == "0.000" ? "" : x.depth.ToString()),
                            //building = x.building
                            //    ,
                            //floor = x.floor
                            //    ,
                            //mploc = x.mploc
                            //    ,
                            //cond = x.cond
                            //    ,
                            //image_name = x.image_name
                            //    ,
                            //image_url = x.image_url
                            //    ,
                            //inv_item_id = x.inv_item_id
                            // ,
                            fabric = x.fabric,
                            finish = x.finish,
                            qty = x.InventoryItems.Count,
                            //InventoryItemModels  = x.InventoryItems.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();
                            InventoryItemModels = x.InventoryItems.Select(y => new InventoryItemModel
                            {
                                inventory_id = y.inventory_id,
                                inv_item_id = y.inv_item_id,
                                building = y.building,
                                floor = y.floor,
                                mploc = y.mploc
                                ,
                                cond = y.cond
                            }).ToList(),
                            image_name = x.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_name).FirstOrDefault()

                        }).ToList();

                        foreach (var item in inventoryResult)
                        {
                            var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();

                            var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                            {
                                inventory_id = item.inventory_id
                                ,
                                inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id
                                ,
                                inv_image_name = item.image_name
                                ,
                                building = x.building
                                ,
                                floor = x.floor
                                ,
                                mploc = x.mploc
                                ,
                                cond = x.cond
                                ,
                                qty = x.qtyCount
                                ,
                                item_code = item.item_code
                                ,
                                description = item.description
                            });

                            inventoryResult.Where(r => r.inventory_id == item.inventory_id)
                              .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();

                            StringBuilder stringBuilder = new StringBuilder();
                            int index = 1;
                            foreach (var grp in gropinglocation)
                            {
                                //inventoryResult.Where(r => r.inventory_id == item.inventory_id).Select(x => new InventoryItemModel() {
                                //    inventory_id = item.inventory_id,
                                //    inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
                                //    inv_image_name = item.image_name,
                                //    building = grp.building,
                                //    floor = grp.floor,
                                //    mploc = grp.mploc,
                                //    cond = grp.cond,
                                //    qty = grp.qtyCount
                                //}).ToList();



                                if (gropinglocation.Count == index)
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount}");
                                else
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount} \n");

                                index++;
                            }

                            inventoryResult.Where(r => r.inventory_id == item.inventory_id)
                               .Select(r => { r.tootip_inv_item = stringBuilder.ToString(); return r; }).ToList();
                        }

                        return inventoryResult;
                    }
                    else
                    {
                        return new List<InventoryModel>();
                    }



                }
                else
                    return new List<InventoryModel>();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<string>> GetInventoryCategory(int client_id)
        {
            try
            {
                return await _dbContext.Inventories.Where(i=>!string.IsNullOrEmpty(i.category) && i.client_id == client_id).Select(cat=>cat.category).Distinct().ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InventoryModel>> SearchInventory(string strSearch,int client_id, string bulding, string floor, string room, string cond)
        {

            try
            {
                List<Inventory> result = new List<Inventory>();

                if (!string.IsNullOrEmpty(strSearch))
                {

                    var inventoryModels = (from i in _dbContext.Inventories
                                           join ii in _dbContext.InventoryItems on i.inventory_id equals ii.inventory_id into invitem
                                           from ii in invitem.DefaultIfEmpty()
                                           where i.client_id == client_id
                                           where ii.building.Contains((!string.IsNullOrEmpty(bulding) ? bulding : ""))
                                           where ii.floor.Contains((!string.IsNullOrEmpty(floor) ? floor : ""))
                                           where ii.mploc.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                           where ii.cond.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
                                           where ii.building != "Donate"
                                           where (i.item_code.Contains(strSearch) || i.description.Contains(strSearch) || i.additionaldescription.Contains(strSearch))
                                           select new InventoryModel()
                                           {
                                               inventory_id = i.inventory_id,
                                               item_code = i.item_code,
                                               description = (!string.IsNullOrEmpty(i.additionaldescription) ? i.description + "-" + i.additionaldescription : i.description),
                                               manuf = i.manuf,
                                               height = i.height.ToString(),
                                               width = i.width.ToString(),
                                               depth = i.depth.ToString(),
                                               hwd_str = (i.height.ToString() == "0.000" ? "" : Decimal.Round(i.height, 1).ToString() + "h X ") + (i.width.ToString() == "0.000" ? "" : Decimal.Round(i.width, 1).ToString() + "w X ") + (i.depth.ToString() == "0.000" ? "" : Decimal.Round(i.depth, 1).ToString() + "d"),
                                               fabric = i.fabric,
                                               finish = i.finish,
                                               client_id = i.client_id,
                                               category = i.category,
                                               qty = (!string.IsNullOrEmpty(bulding) && !string.IsNullOrEmpty(floor) && !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.building == bulding && x.floor == floor && x.mploc == room && x.cond == cond).Count()
                                                                                                                     : (!string.IsNullOrEmpty(bulding) ? i.InventoryItems.Where(x => x.building == bulding).Count()
                                                                                                                                                       : !string.IsNullOrEmpty(floor) ? i.InventoryItems.Where(x => x.floor == floor).Count()
                                                                                                                                                       : !string.IsNullOrEmpty(room) ? i.InventoryItems.Where(x => x.mploc == room).Count()
                                                                                                                                                       : !string.IsNullOrEmpty(cond) ? i.InventoryItems.Where(x => x.cond == cond).Count()
                                                                                                                                                                                      : i.InventoryItems.Count)),

                                               InventoryItemModels = (from ii in _dbContext.InventoryItems
                                                                      where ii.inventory_id == i.inventory_id
                                                                      where ii.building.Contains((!string.IsNullOrEmpty(bulding) ? bulding : ""))
                                                                      where ii.floor.Contains((!string.IsNullOrEmpty(floor) ? floor : ""))
                                                                      where ii.mploc.Contains((!string.IsNullOrEmpty(room) ? room : ""))
                                                                      where ii.cond.Contains((!string.IsNullOrEmpty(cond) ? cond : ""))
                                                                      where ii.building != "Donate"
                                                                      select new InventoryItemModel
                                                                      {
                                                                          inventory_id = ii.inventory_id,
                                                                          inv_item_id = ii.inv_item_id,
                                                                          building = ii.building,
                                                                          floor = ii.floor,
                                                                          mploc = ii.mploc,
                                                                          cond = ii.cond,
                                                                          client_id = ii.client_id
                                                                      }).ToList(),

                                               image_name = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_name).FirstOrDefault(),
                                               image_url = i.InventoryItems.Where(i => i.InventoryItemImages.Count > 0).FirstOrDefault().InventoryItemImages.Select(r => r.image_url).FirstOrDefault()
                                               //  qty = (!string.IsNullOrEmpty(floor) ? i.InventoryItems.GroupBy(x => new { x.floor }).Count() : i.InventoryItems.Count),

                                           }).AsQueryable().ToList();

                    if (inventoryModels.Count > 0)
                    {

                        var invModels = inventoryModels.GroupBy(x => x.inventory_id).Select(s => s.First()).Distinct().ToList();

                        invModels.RemoveAll(i => i.qty == 0);

                        foreach (var item in invModels)
                        {
                            var gropinglocation = item.InventoryItemModels.GroupBy(x => new { x.building, x.floor, x.mploc, x.cond }).Select(g => new { g.Key.building, g.Key.floor, g.Key.mploc, g.Key.cond, qtyCount = g.Count() }).ToList();

                            var invItemModelDisplay = gropinglocation.Select(x => new InventoryItemModel
                            {
                                inventory_id = item.inventory_id,
                                //inv_item_id = item.InventoryItemModels.FirstOrDefault().inv_item_id,
                                inv_item_id = item.InventoryItemModels.Where(i => i.building == x.building && i.floor == x.floor && i.mploc == x.mploc && i.cond == x.cond).FirstOrDefault().inv_item_id,
                                inv_image_name = item.image_name,
                                inv_image_url = item.image_url,
                                building = x.building,
                                floor = x.floor,
                                mploc = x.mploc,
                                cond = x.cond,
                                qty = x.qtyCount,
                                item_code = item.item_code,
                                description = item.description,
                                client_id = item.client_id
                            });

                            invModels.Where(r => r.inventory_id == item.inventory_id)
                              .Select(r => { r.inventoryItemModelsDisplay = invItemModelDisplay.ToList(); return r; }).ToList();

                            StringBuilder stringBuilder = new StringBuilder();
                            int index = 1;
                            foreach (var grp in gropinglocation)
                            {
                                int tmp_inv_item_id = item.InventoryItemModels.Where(i => i.building == grp.building && i.floor == grp.floor && i.mploc == grp.mploc && i.cond == grp.cond).FirstOrDefault().inv_item_id;

                                if (gropinglocation.Count == index)
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount}");
                                else
                                    stringBuilder.Append($"{grp.building} : {grp.floor} : Loc {grp.mploc} : Cond {grp.cond} : Qty {grp.qtyCount} \n");

                                index++;
                            }

                            invModels.Where(r => r.inventory_id == item.inventory_id)
                               .Select(r => { r.tootip_inv_item = stringBuilder.ToString(); return r; }).ToList();
                        }

                        return await Task.Run(() => invModels.ToList());

                    }
                    else
                        return new List<InventoryModel>();



                }
                else
                    return new List<InventoryModel>();

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
