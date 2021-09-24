using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace ITSoftStore.Modules
{
    public static class Commerce
    {
        private static RestAPI rest = new RestAPI("https://www.it-soft.store/wp-json/wc/v3/", "ck_87b9ca2829e1389136bbb8ffc64a7e82025e2c4e", "cs_cc23d0b3243036321cece28ec963add1766803de");
        public static WCObject wc = new WCObject(rest);
    }
}
