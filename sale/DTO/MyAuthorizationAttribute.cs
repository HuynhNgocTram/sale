using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sale.DTO
{
    public class MyAuthorizationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (System.Web.HttpContext.Current.Session["Account"] == null)
            {
                filterContext.Result = new RedirectResult("/Client/Login");
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["ORDER"] == null)
                {
                    List<User_ORDER> listuORDER = new List<User_ORDER>();
                    System.Web.HttpContext.Current.Session["ORDER"] = listuORDER;
                }
            }
        }
    }
}