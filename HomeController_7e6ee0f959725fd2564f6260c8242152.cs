using QLNX.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNX.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
            System.Web.HttpContext.Current.Session["text"] = 1;
        }


        [MyAuthor]
        public ActionResult Index()
        {
            return View();
        }

       
        [AdminAuthor(role = "admin")]
        public ActionResult Admin()
        {
            string a = "admin,user,khachhang";
            string[] roles = a.Split(',');
            return View();
        }


        [MyAuthor]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}