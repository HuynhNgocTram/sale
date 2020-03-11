using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNX.DTO
{
    public class AdminAuthorAttribute : ActionFilterAttribute
    {
        public string username;
        public string role;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (role != null && role != "")
            {
                if (System.Web.HttpContext.Current.Session["Account"] == null)
                {
                    filterContext.Result = new RedirectResult("/Home/Login");
                }
                else{
                    Account ac = (Account)System.Web.HttpContext.Current.Session["Account"];
                    if(ac.role!= role)
                    {
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }

        }
    }
}