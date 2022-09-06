using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sendemail.Models;
using Sendemail.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sendemail.Controllers
{
    [ApiController]
    [Route("api/Mail")]
    public class MailController : ControllerBase
    {
        // GET: /<controller>/
        /*public IActionResult Index()
        {
            return View();
        }*/
        private readonly IMailService mailService;
        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                throw;
            }

        }

    }
}
