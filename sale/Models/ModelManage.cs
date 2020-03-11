using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace sale.Models
{
    public class ModelManage
    {
        public SALEEntities entity;

        public static ModelManage INSTANCE = new ModelManage();
        private ModelManage()
        {
            entity = new SALEEntities();
        }

        

    }
}