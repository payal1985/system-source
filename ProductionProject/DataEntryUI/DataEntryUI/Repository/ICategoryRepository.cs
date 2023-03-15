using DataEntryUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Repository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategory();
    }
}
