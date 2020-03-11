using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sale.DTO
{
    public class User_ORDER
    {
        public PRODUCT pro { get; set; }
        public int soluong { get; set; }

        public User_ORDER()
        {

        }

        public long tongtien ()
        {
            return (long) pro.price.Value * soluong;
        }
        public User_ORDER(PRODUCT p, int sl)
        {
            pro = p;
            soluong = sl;
        }

    }
}