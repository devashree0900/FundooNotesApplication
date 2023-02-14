using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using BusinessLayer.Interfaces;
using System;
using System.Threading.Tasks;
using ModelLayer;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IUserBusiness userBusiness;
        public TicketController(IBus bus, IUserBusiness userBusiness)
        {
            _bus = bus;
            this.userBusiness = userBusiness;
        }

        [HttpPost("ticket")]
        public async Task<IActionResult> CreateTicketForPassword(string email)
        {
            var token = userBusiness.ForgetPassword(email); //to get the token
            if(token!=null)
            {
                var ticket = userBusiness.CreateTicketForPassword(email, token);
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(ticket);
                return Ok(new { Status = true, Message = "Mail Sent Successfully" });

            }
            else
            {
                return BadRequest(new { Status = false, Message = "Unsuccessful" });
            }
        }
    }
}
