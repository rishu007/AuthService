using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize (Roles = "Manager,Admin",Policy = "SpecificUsersPolicy", AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //// Get the current ClaimsPrincipal which represnts user
           
            ClaimsPrincipal claimPrincipal = User;
            // Access the identity  ..like and ID card which identity of the user like id card is an identity
            ClaimsIdentity? identity = claimPrincipal?.Identity as ClaimsIdentity;
            //information about the user ..analogy like name and id number on the id card
            IEnumerable<Claim>? claims= identity?.Claims;

            if (claims?.Count()>0  && identity?.IsAuthenticated == true)
            {
                Console.WriteLine("User is Authenticated");
                foreach (var claim in claims)
                {
                    Console.WriteLine(" type {0},value {1}" ,claim.Type,claim.Value.ToString());
                }
            }
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
