using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using log4net;
using context = System.Web.HttpContext;
namespace DF_WebModule.Classes
{
    public class SymlLogs
    {
        private static String ErrorlineNo, Errormsg, ErrorLocation, extype, exurl, Frommail, ToMail, Sub, HostAdd, EmailHead, EmailSing;
        protected ILog monitoringLogger;
        protected static ILog debugLogger;
        protected static ILog emaillogger;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SymlLogs));
       
        public SymlLogs(string message)
        {
            
            debugLogger = LogManager.GetLogger(message);
            emaillogger = LogManager.GetLogger(message);
            

        }
        public void LogDebugMessage(dynamic message)
        {
            debugLogger.Debug(message);
        }
        public void LogError(string message)
        {
            debugLogger.Error(message);

        }
        
        public void LogInfoMessage(dynamic message)
        {
            debugLogger.Info(message);
        }
        public void LogWarningMessage(string message)
        {
            debugLogger.Warn(message);
        }
        public static string BuildEmailTemplate(Exception exmail)
        {
            var newline = "<br/>";
            ErrorlineNo = exmail.StackTrace.Substring(exmail.StackTrace.Length - 7, 7);
            Errormsg = exmail.GetType().Name.ToString();
            extype = exmail.GetType().ToString();
            exurl = context.Current.Request.Url.ToString();
            ErrorLocation = exmail.Message.ToString();
            EmailHead = "<b>Dear Team,</b>" + "<br/>" + "An exception occurred in a Application Url" + " " + exurl + " " + "With following Details" + "<br/>" + "<br/>";
            EmailSing = newline + "Thanks and Regards" + newline + "    " + "     " + "<b>Application Admin </b>" + "</br>";
            Sub = "Exception occurred" + " " + "in Application" + " " + exurl;

            string errortomail = EmailHead + "<b>Log Written Date: </b>" + " " + DateTime.Now.ToString() + newline + "<b>Error Line No :</b>" + " " + ErrorlineNo + "\t\n" + " " + newline + "<b>Error Message:</b>" + " " + Errormsg + newline + "<b>Exception Type:</b>" + " " + extype + newline + "<b> Error Details :</b>" + " " + ErrorLocation + newline + "<b>Error Page Url:</b>" + " " + exurl + newline + newline + newline + newline + EmailSing;
            string from, to, bcc, cc, subject, body;
            from = "noreply@symlconnect.co.uk";
            to = "tahmad@symlconnect.co.uk";
            bcc = "";
            cc = "";
            subject = "Error Information";
            StringBuilder sb = new StringBuilder();
            sb.Append(errortomail);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            return SendEmail(mail);
        }

    

        private static string SendEmail(MailMessage mail)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = "smtp.symlconnect.co.uk";
                client.Port = 587;
                client.EnableSsl = false;
                client.UseDefaultCredentials = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("noreply@symlconnect.co.uk", "NRPa55!12");
                try
                {
                    client.Send(mail);
                    return "1";
                }
                catch (Exception ex)
                {
                    return "0";
                }
            }

        }

    }
}