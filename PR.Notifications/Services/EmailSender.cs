using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PR.Notifications.Services
{
    public class EmailSender
    {
        public void SendNewUserEmail(string email)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("kohnkelabpr@gmail.com", "zaq1@WSX"),
                EnableSsl = true,
            };

            smtpClient.Send("kohnkelabpr@gmail.com", email, "Wiadomosc COVID-19", "Kwarantanna - wiadomosc");
        }
    }
    }
