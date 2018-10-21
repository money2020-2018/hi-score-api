using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Vdp;
using static Vdp.VisaAPIClient;

namespace WebApplication1.Controllers
{
    public class VisaController : Controller
    {
        public static Stack<decimal> balance = new Stack<decimal>();

        private IConfiguration _configuration;

        public VisaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("/balance")]
        public IActionResult Balance()
        {
            balance.TryPop(out var k);

            return Json(new { Balance = k});
        }

        public class Transfer
        {
            public decimal Amount { get; set; }
        }

        [HttpPost("/")]
        public IActionResult TransferFunds([FromBody] Transfer transfer)
        {
            balance.Push(transfer.Amount);

            Configuration configuration = new Configuration
            {
                CertificatePassword=_configuration["CertificatePassword"],
                CertificatePath = _configuration["CertificatePath"],
                Password = _configuration["Password"],
                VisaUrl = _configuration["VisaUrl"],
                UserId = _configuration["UserId"]
            };

            var visaAPIClient = new VisaAPIClient();

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string baseUri = "visadirect/";


            var requestFile = @"c:\Visa\PullRequest.json";
            string request = "";

            if (System.IO.File.Exists(requestFile))
            {
                request = System.IO.File.ReadAllText(requestFile);
            }

            
            string resourcePath = "fundstransfer/v1/pullfundstransactions/";
            string status = visaAPIClient.DoMutualAuthCall(baseUri + resourcePath, "POST", "Pull Funds Transaction Test", request, configuration);
            
            var requestFile2 = @"c:\Visa\PushRequest.json";

            string request2 = "";
            if (System.IO.File.Exists(requestFile))
            {
                request2 = System.IO.File.ReadAllText(requestFile2);
            }
            string resourcePath2 = "fundstransfer/v1/pushfundstransactions/";
            string status2 = visaAPIClient.DoMutualAuthCall(baseUri + resourcePath2, "POST", "Push Funds Transaction Test", request2, configuration);            

            return Ok();
        } 
    }
}
