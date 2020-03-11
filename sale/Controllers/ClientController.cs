using sale.DTO;
using sale.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sale.Controllers
{
    public class ClientController : Controller
    {
        SALEEntities entity = ModelManage.INSTANCE.entity;
        MailManage mail = new MailManage();
        public ClientController()
        {
            //if (System.Web.HttpContext.Current.Session["Account"] == null)
            //{
            //    ACCOUNT a = new ACCOUNT();
            //    a = entity.ACCOUNTs.Where(acc => acc.UserID == "eee").FirstOrDefault();

            //    System.Web.HttpContext.Current.Session["Account"] = a;
            //}


        }
        // GET: Client
        public class RegisterObject
        {
            public sale.Models.RegisterViewModel register = new Models.RegisterViewModel();
            public ACCOUNT account = new ACCOUNT();
        }


        public ActionResult Register()
        {

            return View();
        }

        public ActionResult Login()
        {
            if (Session["Account"] == null)
                return View();
            return RedirectToAction("Index", "Client");
        }

        [HttpPost]
        public ActionResult DoneLogin(string userid, string pass)
        {
            //ACCOUNT obj = (from s in entity.ACCOUNTs where s.UserID == userid && s.Password == pass select s).FirstOrDefault();
            ACCOUNT obj = (from s in entity.ACCOUNTs where s.UserID == userid select s).FirstOrDefault();
            bool validPassword = BCrypt.Net.BCrypt.Verify(pass, obj.Password);
            if (validPassword == false)
                return RedirectToAction("Login", "Client");

            Session["Account"] = obj;
            return RedirectToAction("Index", "Client");
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["Account"] = null;
            return RedirectToAction("Index", "Client");
        }

        [HttpPost]
        public ActionResult CreateAccount(ACCOUNT ac)
        {
            //    ACCOUNT r = new ACCOUNT();
            //    r.UserID = acc.UserID;
            //    r.Email = acc.Email;
           
            Session["TempAccount"] = ac;
            string OTP = mail.RandomString();
            mail.SendMail(ac.Email, OTP, 2);
            Session["EmailOTP"] = ac.Email + "/" + OTP;
            Session["TimeRegister"] = DateTime.Now;
            Session["TimeSendOTP"] = Session["TimeRegister"];
            return RedirectToAction("ConfirmOTP", "Client");
        }

        public void ResendOTP()
        {
            ACCOUNT ac = (ACCOUNT)Session["TempAccount"];
            string OTP = mail.RandomString();
            mail.SendMail(ac.Email, OTP, 2);
            Session["EmailOTP"] = ac.Email + "/" + OTP;
            Session["TimeSendOTP"] = DateTime.Now;
        }

        public ActionResult ConfirmOTP()
        {
            return View();
        }

        public ActionResult DoneConfirmOTP(string otp)
        {

            if (Session["TempAccount"] == null || Session["EmailOTP"] == null)
                return RedirectToAction("CreateAccount", "Client");

            ACCOUNT a = ((ACCOUNT)Session["TempAccount"]);
            string emailAcc = a.Email;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(a.Password);
            a.Password = hashedPassword;

            string text = Session["EmailOTP"].ToString();
            string[] splitString = text.Split('/');
            DateTime TimeRegister = (DateTime)Session["TimeRegister"];

            if (((DateTime)Session["TimeRegister"]).AddMinutes(10) <= DateTime.Now)
            {
                Session["TempAccount"] = null;
                Session["EmailOTP"] = null;

                return RedirectToAction("CreateAccount", "Client");
            }
            
            if (otp == splitString[1] && emailAcc == splitString[0] && DateTime.Now<=TimeRegister.AddMinutes(3))
            {
                entity.ACCOUNTs.Add(a);
                entity.SaveChanges();
                Session["EmailOTP"] = null;

                return RedirectToAction("Index", "Client");
            }
                     
            return RedirectToAction("ConfirmOTP", "Client");
        }

        public ActionResult ACCOUNTS()
        {
            List<ACCOUNT> listAcc = entity.ACCOUNTs.ToList();
            return View(listAcc);
        }



        public List<PRODUCT> newProducts()
        {
            var newPro = (from s in entity.PRODUCTs orderby s.createDate descending select s).Take(5).ToList();
            return newPro;
        }

        public List<PRODUCT> bigsaleProduct()
        {
            var newPro = (from s in entity.PRODUCTs orderby s.saleCount descending select s).Take(5).ToList();
            return newPro;
        }

        public class HomePageClient
        {
            public List<PRODUCER> producers;
            public List<TYPE> types;
            public List<PRODUCT> newProducts;
            public List<PRODUCT> bigsaleProduct;
            public List<PRODUCT> listProducts;
        }

        public ActionResult Index()
        {
            HomePageClient hp = new HomePageClient();
            hp.producers = entity.PRODUCERs.ToList();
            hp.types = entity.TYPEs.ToList();
            hp.listProducts = entity.PRODUCTs.ToList();
            hp.newProducts = newProducts();
            hp.bigsaleProduct = bigsaleProduct();
            return View(hp);
        }

        public ActionResult DetailProduct(string id)
        {
            PRODUCT p = (from s in entity.PRODUCTs where s.productID == id select s).FirstOrDefault();
            return View(p);

        }

        [MyAuthorization]
        public ActionResult AddToORDER(string id, int soluong)
        {

            var p = (from pro in entity.PRODUCTs where pro.productID == id select pro).FirstOrDefault();

            List<User_ORDER> listuc = (List<User_ORDER>)Session["ORDER"];
            int flag = 0;
            foreach (var item in listuc)
            {

                if (item.pro.productID == id)
                {
                    item.soluong = soluong + item.soluong;
                    flag = 1;
                    break;
                }


            }
            if (flag == 0)
                listuc.Add(new User_ORDER(p, soluong));


            return RedirectToAction("DetailProduct", "Client", new { @id = id });
        }

        [MyAuthorization]
        public ActionResult ORDER()
        {
            return View(Session["ORDER"]);
        }

        [MyAuthorization]
        public ActionResult DeleteORDER(string id)
        {
            List<User_ORDER> listProduct = (List<User_ORDER>)Session["ORDER"];
            foreach (User_ORDER p in listProduct)
            {
                if (p.pro.productID == id)
                {
                    listProduct.Remove(p);
                    break;
                }
            }
            return RedirectToAction("ORDER", "Client");
        }

        [MyAuthorization]
        public ActionResult Order()
        {
            List<User_ORDER> listUC = (List<User_ORDER>)Session["ORDER"];
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); //LOAI BO CAC DAU CHAM

            ORDER c = new ORDER();
            c.voucherID = path;
            c.UserID = ((ACCOUNT)Session["Account"]).UserID;
            //c.UserID = (Session["Account"] as ACCOUNT).UserID;
            c.amount = listUC.Sum(u => (long)(u.soluong * u.pro.price.Value));
            c.orderDate = DateTime.Now;
            entity.ORDERs.Add(c);
            entity.SaveChanges();

            foreach (User_ORDER uc in listUC)
            {
                DetailORDER d = new DetailORDER();
                d.voucherID = c.voucherID;
                d.productID = uc.pro.productID;
                d.number = uc.soluong;
                entity.DetailORDERs.Add(d);
            }
            entity.SaveChanges();

            listUC.Clear();

            return RedirectToAction("ORDER", "Client");
        }

        [HttpGet]
        public ActionResult Search(string searchProduct)
        {
            var p = from pro in entity.PRODUCTs where pro.name.Contains(searchProduct) || pro.productID == searchProduct select pro;
            return View(p);
        }

        [MyAuthorization]
        public ActionResult MyProfile()
        {
            //var receivedOrder = (from c in entity.ORDERs where c.status == 1 && c.UserID == ((ACCOUNT)Session["Account"]).UserID select c).ToList();
            string userid = ((ACCOUNT)Session["Account"]).UserID;
            var receivedOrder = entity.ORDERs.Where(c => c.status == 1 && c.UserID == userid).ToList();

            var waitOrder = entity.ORDERs.Where(c => c.status == 0 && c.UserID == userid).ToList();

            ViewBag.receivedOrder = receivedOrder;
            ViewBag.waitOrder = waitOrder;
            return View();
        }



    }
}