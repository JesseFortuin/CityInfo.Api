using Microsoft.Extensions.Configuration;

namespace CityInfo.Infrastructue.Services
{
    public class LocalMailService : IMailService
    {
        private readonly string _mailTo = string.Empty;

        private readonly string _mailFrom = string.Empty;

        private readonly string accesspoint = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];

            _mailFrom = configuration["mailSettings:mailFromAddress"];

            accesspoint = configuration["DataBaseString"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $"with {nameof(LocalMailService)}.");

            Console.WriteLine($"Subject: {subject}.");

            Console.WriteLine($"Message: {message}.");

            Console.WriteLine($"This is the access point : {accesspoint}.");
        }
    }
}
