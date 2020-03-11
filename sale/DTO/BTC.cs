using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sale.DTO
{
    public class Test
    {
        public void testMain()
        {
            BTC btc = BTC.INSTANCE;
            Test a = new Test();
            a.testMain();
            int entity = BTC.entity;
            Test.TestStatic();
        }
        static void TestStatic()
        {

        }
    }
    public class BTC
    {
        public static BTC INSTANCE = new BTC();
        public static int entity;

        private BTC()
        {

        }


    }


    public interface FactoryModel
    {
        Object createModel();
    }

    public class AccountModel : FactoryModel
    {
        public object createModel()
        {
            return new ACCOUNT();
        }
    }

    public class ProductModel : FactoryModel
    {
        public object createModel()
        {
            return new PRODUCT();
        }
    }
    
    public class CuChuoiModel
    {
        //public object createModel(string type)
        //{
        //    if (type == "product")
        //    {
        //        return new PRODUCT();
        //    }
        //    if (type == "USER")
        //    {

        //    }
        //}
    }

    class Test2
    {
        void ham()
        {
            FactoryModel factory = new ProductModel();
            PRODUCT pro = (PRODUCT)factory.createModel();
        }
    }
}