using Microsoft.AspNetCore.Mvc;
using System.Text;
using TestTask.API.DTO_s;

namespace TestTask.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var soapBody = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <Login xmlns=""urn:ICUTech.Intf-IICUTech"">
                      <UserName>{request.Username}</UserName>
                      <Password>{request.Password}</Password>
                      <IPs></IPs>
                    </Login>
                  </soap:Body>
                </soap:Envelope>";

            using var client = new HttpClient();
            var content = new StringContent(soapBody, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "urn:ICUTech.Intf-IICUTech#Login");

            try
            {
                var response = await client.PostAsync("http://isapi.mekashron.com/icu-tech/icutech-test.dll/soap/IICUTech", content);
                var responseString = await response.Content.ReadAsStringAsync();
                return Content(responseString, "text/xml");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error calling SOAP service: {ex.Message}");
            }
        }
    }
}
