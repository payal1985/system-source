﻿using DataEntryUI.DBContext;
using DataEntryUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Repository
{
    public class LeadTimeRepository : ILeadTimeRepository
    {
        ProductContext _dbContext;
        public LeadTimeRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public IEnumerable<LeadTime> GetLeadTime()
        //{
        //    try
        //    {
        //        return _dbContext.LeadTimes.ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public int GetLeadTimeId(string leadTime)
        {
            try
            {
                return _dbContext.LeadTimes.Where(l => l.lead_time_description.ToLower() == leadTime.ToLower().Trim()).SingleOrDefault().lead_time_ID;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
