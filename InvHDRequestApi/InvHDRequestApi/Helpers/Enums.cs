using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Helpers
{
    public class Enums
    {
        public enum Status
        {
            Active = 5,
            Inactive = 6,
            Reserved = 7,
            MovingLocation = 8,
            QtyUpdate = 9,
            ConditionUpdate = 10,
            OrdOpen = 11,
            OrdApprovalPending = 12,
            OrdWorkinProgress = 13,
            OrdClosed = 14,
            OrdCancelled = 15,
            WarrantyService = 16,
            Maintenance = 17
        }
        public enum Condition
        {
            New = 1,
            Good = 2,
            Fair = 3,
            Poor = 4,
            Damaged = 5,
            MissingParts = 6,
        }
        public enum OrderType
        {
            Relocate = 1,
            Warranty = 2,
            Clean = 3,
            NonWarrantyRepair=4
        }

        public enum RequestType
        {
            Warranty,
            Maintenance,
            Cleaning
        }
    }
}
