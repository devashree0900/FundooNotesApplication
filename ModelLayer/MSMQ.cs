using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ModelLayer
{
    public class MSMQ
    {
        MessageQueue messageQueue= new MessageQueue();
        private string receiverEmailAddr;
        private string receiverName;

        //Asynchronous process
        public void SendMessage(string token,string EmailId,string name)
        {
            receiverEmailAddr = EmailId;
            receiverName= name;
            messageQueue.Path = @".\Private$\Token";
            try
            {
                if(!MessageQueue.Exists(messageQueue.Path))
                {
                    MessageQueue.Create(messageQueue.Path);
                }
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted; //message is sent after its execution
                messageQueue.Send(token);
                messageQueue.BeginReceive(); 
                messageQueue.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Receive the messages sent by the frontend
        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQueue.EndReceive(e.AsyncResult); //messages received in queues
                string token = msg.Body.ToString();
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("devashreegajendragadkar54598@gmail.com", "hhxksgcujwtdoayc"),
                };
                mailMessage.From = new MailAddress("devashreegajendragadkar54598@gmail.com");
                mailMessage.To.Add(new MailAddress(receiverEmailAddr));
                string mailBody = $"<!DOCTYPE html>" +
                                  $"<html>" +
                                  $"<style>" +
                                  $".blink" +
                                  $"</style>" +
                                  $"<body style = \"background-color:#DBFF73;text-align:center;padding:5px;\">" +
                                  $"<h1 style = \"color:#6A8D02;border-bottom:3px solid #84AF08;margin-top:5px;\"> Dear <b>{receiverName}</b></h1>\n" +
                                  $"<h3 style = \"color:#8AB411;\"> For resetting pasword the below link is issued</h3>" +
                                  $"<h3 style = \"color:#8AB411;\"> Please click the link below to reset your password</h3>" +
                                  $"<a style = \"color:#00802b;text-decoration:none;font-size:20px;\" href =''>Click Me</a>\n" +
                                  $"<h3 style = \"color:#8AB411;margin-bottom:5px;\"><blink>This Token Will be valid for next 6 hours</blink></h3>" +
                                  $"</body>" +
                                  $"</html>";
                mailMessage.Body = mailBody;
                mailMessage.IsBodyHtml= true;
                mailMessage.Subject = "Fundoo Notes Password Reset link";
                smtpClient.Send(mailMessage);


            }
            catch (Exception ex) { throw ex; }
        }


    }
}
