﻿using SupportUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.Repository
{
    public interface ILeadTimeRepository
    {
        //IEnumerable<LeadTime> GetLeadTime();
        int GetLeadTimeId(string leadTime);
    }
}