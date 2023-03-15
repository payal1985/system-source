using DataEntryUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Repository
{
    public interface IProductImagesRepository
    {
        Task<string> InsertProductImages(ProductImages productImages);
    }
}
