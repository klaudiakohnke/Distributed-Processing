﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DP.Patients.KK.Model;
using DP.Patients.KK.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP.Patients.KK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DpDataContext _context;
        private readonly ServiceBusSender _sender;
        public PatientsController(DpDataContext context, ServiceBusSender sender)
        {
            _context = context;
            _sender = sender;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Patients.ToList());
        }
        [HttpPost]
        public async Task <IActionResult> Add(Patient p)
        {
            _context.Patients.Add(p);
            _context.SaveChanges();
            /*
            HttpClient client = new HttpClient();

            string emailJson = JsonSerializer.Serialize(new EmailMessageRequest()
                {
                    EmailAddress = "klaudiakohnke@gmail.com",
                    Subject = "Zarejestrowano pacjenta",
                    Body = "Informacje o kwarantannie"
                });

            HttpResponseMessage message = await client.PostAsync("https://localhost:5002/api/email",
                new StringContent(emailJson, Encoding.UTF8, "application/json"));
                */

           await _sender.SendMessage(new MessagePayload() { EventName = "NewUserRegistered", UserEmail = "klaudiakohnke@gmail.com" });
            return Created("api/patients/", p);
        }
    }
    /*
    public class EmailMessageRequest
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    } */
}