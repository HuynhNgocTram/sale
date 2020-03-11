using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace sale.Models
{
    public class MailManage
    {
        public string RandomString()
        {
            //int iRanDom;
            string strRanDom;
            Random rd = new Random();
            //iRanDom sẽ nhận có giá trị ngẫu nhiên trong khoảng 1 đến 100:
            //iRanDom = ;
            //Chuyển giá trị ramdon về kiểu string:
            strRanDom = rd.Next(100000, 999999).ToString();
            return strRanDom;
        }
        public void SendMail(string address, string message, int type)
        {

            string email = "tramngochuynh20@gmail.com";
            string password = "1997muathulove";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            if(type==1)
            {
                msg.Subject = "Xác nhận đơn hàng thành công";
            }
            else if(type==2)
            {
                msg.Subject = "Xác nhận mã OTP";
            }
            
            msg.Body = message;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            //smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);

        }
    }
}