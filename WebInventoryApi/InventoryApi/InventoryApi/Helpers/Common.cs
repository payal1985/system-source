using InventoryApi.DBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InventoryApi.Helpers
{
    public static class Common
    {
        public static InventoryContext _dbContext;
        //public static SSIRequestContext _requestContext;

        public static string GetCartImages(int inventoryitemid, int inventoryid, int conditionid)
        {
            string imagename;
            var entity = _dbContext.InventoryImages.Where(invimg => invimg.InventoryItemID == inventoryitemid && invimg.ConditionID == conditionid).FirstOrDefault();

            if (entity != null)
            {
                imagename = entity.ImageName;
            }
            else
            {
                var inventoryImagesEntity = _dbContext.InventoryImages.Where(invimg => invimg.InventoryID == inventoryid && invimg.ConditionID == conditionid).FirstOrDefault();

                if (inventoryImagesEntity == null)
                {
                    inventoryImagesEntity = _dbContext.InventoryImages.Where(invimg => invimg.InventoryID == inventoryid && invimg.InventoryItemID == null).FirstOrDefault();
                }
                imagename = (inventoryImagesEntity != null ? inventoryImagesEntity.ImageName : "");
            }
            return imagename;
            //return await Task.Run(() => imagename);
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?
                            .GetMember(enumValue.ToString())?
                            .First()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name;
        }

        //public static string GetBuilding(int? buildingid)
        //{

        //    string building = (buildingid == 0 ? "" :_requestContext.ClientLocations.Where(cl => cl.location_id == buildingid).FirstOrDefault().location_name);
        //    return building;
        //}

        //public static string GetFloor(int floorid)
        //{
        //    string floor = (floorid == 0 ? "" : _dbContext.InventoryFloors.Where(f => f.InventoryFloorID == floorid).FirstOrDefault().InventoryFloorName);
        //    return floor;
        //}

        //public static string GetCondition(int conditionid)
        //{
        //    string condition = _dbContext.InventoryItemConditions.Where(c => c.InventoryItemConditionID == conditionid).FirstOrDefault().ConditionName;
        //    return condition;
        //}

    }
}
