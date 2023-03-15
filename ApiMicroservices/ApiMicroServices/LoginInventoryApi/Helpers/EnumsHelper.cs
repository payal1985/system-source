using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Helpers
{
    public class EnumsHelper
    {
        public enum Permission
        {
            ////PERM_CLIENT_NEW_INVENTORY_ITEM = 4194304,
            ////PERM_CLIENT_VIEW_INVENTORY_ITEMS = 8388608,
            ////PERM_CLIENT_EDIT_INVENTORY_ITEMS = 16777216,
            ////PERM_CLIENT_INVENTORY_ADMIN = 2048

            PERM_CLIENT = 1,
            PERM_CLIENT_NEW_REQUEST = 2,
            PERM_CLIENT_VIEW_REQUESTS = 4,
            PERM_CLIENT_EDIT_REQUESTS = 8,
            PERM_CLIENT_CREATE_NOTES = 16,
            PERM_CLIENT_NEW_PROPOSAL = 32,
            PERM_CLIENT_VIEW_PROPOSAL = 64,
            PERM_CLIENT_EDIT_PROPOSAL = 128,
            PERM_CLIENT_EMAIL_REMINDERS = 256,
            PERM_CLIENT_LOCATIONS_ADMIN = 512,
            PERM_CLIENT_CATALOG_ADMIN = 1024,

            PERM_CLIENT_SETTINGS_ADMIN = 4096,
            PERM_CLIENT_SEARCH_REQUESTS = 8192,
            PERM_CLIENT_CATALOG_DISCOUNT_ADMIN = 16384,
            PERM_CLIENT_CATALOG = 32768,
            PERM_CLIENT_CATALOG_SHOP = 65536,
            PERM_CLIENT_CATALOG_PRICE = 131072,
            PERM_CLIENT_CATALOG_ARIBA = 262144,

            PERM_CLIENT_USER_ADMIN = 2097152,

            PERM_CLIENT_INVENTORY_ADMIN = 2048,
            PERM_CLIENT_NEW_INVENTORY_ITEM = 4194304,
            PERM_CLIENT_VIEW_INVENTORY_ITEMS = 8388608,
            PERM_CLIENT_EDIT_INVENTORY_ITEMS = 16777216,
            PERM_INSTALLER_UPDATE_INVITEM_BARCODE = 33554432,

            PERM_HELPDESK_VIEW = 524288,
            PERM_HELPDESK_CREATE = 1048576,
        }

        //int to hex 
        //       int myInt = 18350080;
        //       string myHex = myInt.ToString("x");
        //       Console.WriteLine("Hello World-"+myHex);

        //hex to int
        //string prefixedHex = myHex;
        //       // this works, and returns 1322173
        //       int intValue = Convert.ToInt32(prefixedHex, 16);
        //       Console.WriteLine("int value-"+intValue);

        // res = Math.Pow(2048, 2); power function

        //to get constant value using bitwise operator
        //int a = 2049;
        //int b = 2048;
        //int c = 0;
        //c = a & b;             /* 12 = 0000 1100 */ 
        // Console.WriteLine("Line 1 - Value of c is {0}", c );
    }
}
