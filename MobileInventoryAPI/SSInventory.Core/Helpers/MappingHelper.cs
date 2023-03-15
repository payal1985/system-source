using AutoMapper;
using SSInventory.Core.Models;
using SSInventory.Share.Constants;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryBuildings;
using SSInventory.Share.Models.Dto.InventoryFloors;
using SSInventory.Share.Models.Dto.InventoryImage;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.InventoryItemConditions;
using SSInventory.Share.Models.Dto.ItemTypeAttributes;
using SSInventory.Share.Models.Dto.ItemTypeOption;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Dto.ItemTypeSupportFiles;
using SSInventory.Share.Models.Dto.Manufactory;
using SSInventory.Share.Models.Dto.Status;
using SSInventory.Share.Models.Dto.Submission;
using SSInventory.Share.Ultilities;
using System.Collections.Generic;

namespace SSInventory.Core.Helpers
{
    public class MappingHelper : Profile
    {
        public MappingHelper()
        {
            CreateMap<ItemTypes, ItemTypeModel>();
            CreateMap<CreateOrUpdateItemTypeModel, ItemTypes>().ReverseMap();
            CreateMap<ItemTypeOptions, ItemTypeOptionModel>().ReverseMap();
            CreateMap<ItemTypeOptions, CreateOrUpdateItemTypeOptionModel>().ReverseMap();
            CreateMap<List<ItemTypeOptions>, List<ItemTypeOptionModel>>();
            CreateMap<ItemTypeOptionLines, ItemTypeOptionLineModel>();

            CreateMap<ItemTypeApiModel, SubmissionModel>();
            CreateMap<SubmissionModel, Submissions>().ReverseMap();
            CreateMap<InventoryModel, Inventory>()
                .ForMember(dest => dest.InventoryItem, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Createdate, map => map.MapFrom(x => x.CreateDateTime));
            CreateMap<InventoryItemModel, InventoryItem>().ReverseMap();
            CreateMap<InventoryImageModel, InventoryImages>().ReverseMap();
            CreateMap<CreateOrUpdateItemTypeOptionLineModel, ItemTypeOptionLines>().ReverseMap();
            CreateMap<ItemTypeOptionLinesModel, ItemTypeOptionLines>().ReverseMap();
            CreateMap<InventoryBuildingModel, InventoryBuildings>().ReverseMap();
            CreateMap<InventoryFloorModel, InventoryFloors>().ReverseMap();
            CreateMap<CreateOrUpdateInventoryBuildingModel, InventoryBuildings>().ReverseMap();
            CreateMap<ItemTypeAttributeModel, ItemTypeAttribute>().ReverseMap();
            CreateMap<ItemTypeSupportFileModel, ItemTypeSupportFiles>().ReverseMap();
            CreateMap<CreateOrEditManufacturerModel, Manufacturers>().ReverseMap();
            CreateMap<ManufacturerModel, Manufacturers>().ReverseMap();
            CreateMap<ItemTypeModel, SubmissionResponseModel>().ReverseMap();
            CreateMap<StatusModel, Status>().ReverseMap();
            CreateMap<StatusModel, Status>().ReverseMap();
            CreateMap<InventoryItem, InventoryItemHistoryViewModel>().ReverseMap();
            CreateMap<Inventory, InventoryHistoryViewModel>().ReverseMap();

            // Export Excel mapping
            CreateMap<SubmissionModel, ExportSubmissionModel>();
            CreateMap<Inventory, ExportInventoryModel>();
            CreateMap<InventoryItem, ExportInventoryItemModel>();
            CreateMap<InventoryImages, ExportInventoryItemImageModel>();
            CreateMap<InventoryItemCondition, InventoryItemConditionModel>();
            CreateMap<Order, OrderModel>();
            CreateMap<OrderItem, OrderItemModel>();

            #region History
            CreateMap<InventoryItem, InventoryItemHistoryViewModel>()
                .ForMember(dest => dest.PoOrderDate, map => map.MapFrom(x => x.PoOrderDate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.NonSsipurchaseDate, map => map.MapFrom(x => x.NonSsipurchaseDate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.CreateDateTime, map => map.MapFrom(x => x.CreateDateTime.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.UpdateDateTime, map => map.MapFrom(x => x.UpdateDateTime.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.SubmissionDate, map => map.MapFrom(x => x.SubmissionDate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)));

            CreateMap<Inventory, InventoryHistoryViewModel>()
                .ForMember(dest => dest.CreateDateTime, map => map.MapFrom(x => x.CreateDateTime.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.UpdateDateTime, map => map.MapFrom(x => x.UpdateDateTime.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.DeviceDate, map => map.MapFrom(x => x.DeviceDate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.SubmissionDate, map => map.MapFrom(x => x.SubmissionDate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)));

            CreateMap<InventoryModel, InventoryHistoryViewModel>()
                .ForMember(dest => dest.CreateDateTime, map => map.MapFrom(x => x.Createdate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.UpdateDateTime, map => map.MapFrom(x => x.UpdateDateTime.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.DeviceDate, map => map.MapFrom(x => x.DeviceDate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)))
                .ForMember(dest => dest.SubmissionDate, map => map.MapFrom(x => x.SubmissionDate.ToDatetimeString(ConstantConfig.DateTimeFormat.YYYYMMDDHHMMSS)));

            #endregion
        }
    }
}
