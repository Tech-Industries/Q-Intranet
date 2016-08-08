using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dashboard.Models;
using Dashboard.ViewModels;
using Dashboard.APIControllers;
using System.Net.Mail;
using System.Text;

namespace Dashboard.Helpers
{
    public class Helpers
    {
        DashboardEntities db = new DashboardEntities();

        public void NewLogEntry(int UserID, string Reference, int ReferenceID, string Action, string Description)
        {
            Log l = new Log();
            l.UserID = UserID;
            l.Reference = Reference;
            l.ReferenceID = ReferenceID;
            l.Action = Action;
            l.Description = Description;
            l.DateCreated = DateTime.Now;
            LogController lc = new LogController();
            lc.Post(l);
        }

        public static void sendEmail(string to, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "172.31.0.11";
            client.Port = 366;
            client.EnableSsl = false;
            client.Timeout = 20000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;

            MailMessage mm = new MailMessage("QAutoMailer@ti-kc.com", to);


            mm.Subject = subject;
            mm.Body = body;




            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mm.IsBodyHtml = true;
            client.Send(mm);
            mm.Dispose();
            client.Dispose();
            

        }
    }
}