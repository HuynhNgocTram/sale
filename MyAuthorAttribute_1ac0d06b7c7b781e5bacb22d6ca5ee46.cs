using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNX.DTO
{
    public class MyAuthorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url.LocalPath == "/Home/About")
            {
                filterContext.Result = new RedirectResult("/Home/Contact");
            }
        }   
    }
}