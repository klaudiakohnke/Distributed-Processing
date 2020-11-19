using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DP.Patients.KK.Model;
using DP.Patients.KK.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP.Patients.KK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_context.Patients.ToList());
        }
        [HttpPost]
       
        public async Task <IActionResult> Add(Patient p)
        {
            _context.Patients.Add(p);
            _context.SaveChanges();

           await _sender.SendMessage(new MessagePayload() { EventName = "NewUserRegistered", UserEmail = "klaudiakohnke@gmail.com" });
            return Created("api/patients/", p);
        }
    }
   
}