using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using sale.Models;
using Newtonsoft.Json;

namespace sale.Controllers
{
    public class AdminController : Controller
    {
        SALEEntities entity = ModelManage.INSTANCE.entity;
        MailManage mail = new MailManage();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Products()
        {
            List<PRODUCT> listProdcuct = entity.PRODUCTs.ToList();
            return View(listProdcuct);
        }

        public ActionResult Producers()
        {
            List<PRODUCER> listProducer = entity.PRODUCERs.ToList();
            return View(listProducer);
        }

        public ActionResult Types()
        {
            List<TYPE> listType = entity.TYPEs.ToList();
            return View(listType);
        }

        public ActionResult Accounts()
        {
            List<ACCOUNT> listAcc = entity.ACCOUNTs.ToList();
            return View(listAcc);
        }

        public ActionResult CreateProduct()
        {
            List<TYPE> listType = entity.TYPEs.ToList();
            List<PRODUCER> listProducer = entity.PRODUCERs.ToList();
            ViewBag.typeID = new SelectList(listType, "typeID", "name", listType[0].typeID);
            ViewBag.producerID = new SelectList(listProducer, "producerID", "name", listProducer[0].producerID);

            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(PRODUCT p)
        {
            entity.PRODUCTs.Add(p);
            entity.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        public ActionResult CreateProducer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProducer(PRODUCER p)
        {
            entity.PRODUCERs.Add(p);
            entity.SaveChanges();
            return RedirectToAction("Producers", "Admin");
        }

        public ActionResult CreateType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateType(TYPE p)
        {
            entity.TYPEs.Add(p);
            entity.SaveChanges();
            return RedirectToAction("Types", "Admin");
        }

        public ActionResult Orders()
        {
            var receivedOrder = entity.ORDERs.Where(c => c.status == 3).OrderByDescending(c => c.orderDate).ToList();

            var waitOrder = entity.ORDERs.Where(c => c.status != 3).OrderByDescending(c => c.orderDate).ToList();

            ViewBag.receivedOrder = receivedOrder;
            ViewBag.waitOrder = waitOrder;
            return View();
        }
        public class DataResult
        {
            public bool error;
            public string message;
            public object data;

            public DataResult(bool error, string message)
            {
                this.error = error;
                this.message = message;
            }
            public DataResult(bool error, string message, object data)
            {
                this.error = error;
                this.message = message;
                this.data = data;
            }
        }
        [HttpPost]
        public ActionResult UpdateStatus(string id, string value)
        {
            var order = entity.ORDERs.Where(o => o.voucherID == id).First();
            order.status = Convert.ToInt32(value);
            entity.SaveChanges();
            mail.SendMail("tramngochuynh20@gmail.com", "Đơn hàng đã cập nhật", 1);
            JsonResult res = new JsonResult();
            res.Data = new DataResult(false, "update thanh cong");
            return res;
        }
        public ActionResult GetProduct(string id)
        {
            PRODUCT pro = entity.PRODUCTs.First(a => a.productID == id);
            JsonResult res = new JsonResult();
            res.Data = new DataResult(false, "update thanh cong", JsonConvert.SerializeObject(pro));
            return res;
        }

        public ActionResult EditProduct(string id)
        {
            PRODUCT p = entity.PRODUCTs.First(pr => pr.productID == id);
            string price = p.price.ToString();
            price = price.Substring(0, price.IndexOf(','));
            p.price = Convert.ToDecimal(price);
            List<TYPE> listType = entity.TYPEs.ToList();
            List<PRODUCER> listProducer = entity.PRODUCERs.ToList();
            ViewBag.typeID = new SelectList(listType, "typeID", "name", p.typeID);
            ViewBag.producerID = new SelectList(listProducer, "producerID", "name", p.producerID);
            return View(p);
        }

        [HttpGet]
        public ActionResult DoneEditProduct(PRODUCT product)
        {
            entity.Entry(product).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        public ActionResult DeleteProduct(string productID)
        {
            PRODUCT p = entity.PRODUCTs.First(pr => pr.productID == productID);
            return View(p);
        }

        public ActionResult DetailCus(string id)
        {
            ACCOUNT acc = entity.ACCOUNTs.First(a => a.UserID == id);
            return View(acc);
        }

        public ActionResult DetailOrd(string id)
        {
            List<DetailORDER> detO = entity.DetailORDERs.Where(s => s.voucherID == id).ToList();
            return View(detO);
        }

        public ActionResult Login(string user, string pw)
        {
            return View();
        }





        public ActionResult ExportListCustomer()
        {
            List<ACCOUNT> listAcc = entity.ACCOUNTs.Where(a => a.Type == 0).ToList();

            var gv = new GridView();
            gv.DataSource = listAcc;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return View();

            // return View(listAcc);

        }

        public ActionResult ImportListCustomer()
        {
            return View();
        }

    }
}