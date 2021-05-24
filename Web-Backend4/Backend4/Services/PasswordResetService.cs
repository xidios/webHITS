using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Backend4.Services
{
    public sealed class PasswordResetService : IPasswordResetService
    {
        private readonly ILogger logger;
        private readonly Random random = new Random();
        private readonly List<Entry> codes = new List<Entry>();

        public PasswordResetService(ILogger<IPasswordResetService> logger)
        {
            this.logger = logger;
        }

        public void SendResetCode(String email)
        {
            lock (this.codes)
            {
                var code = this.GenerateCode();
                this.codes.Add(new Entry(email, code));
                this.logger.LogInformation($"Sending reset code {code} to {email}");
            }
        }

        public Boolean VerifyResetCode(String email, String resetCode)
        {
            lock (this.codes)
            {
                this.logger.LogInformation($"Validating reset code {resetCode} for {email}");
                return this.codes.Any(x => x.Email == email && x.Code == resetCode) ;
            }
        }

        public Boolean ApplyResetCode(String email, String resetCode, String newPassword)
        {
            lock (this.codes)
            {
                this.logger.LogInformation($"Applying reset code {resetCode} for {email}");
                var entry = this.codes.FirstOrDefault(x => x.Email == email && x.Code == resetCode);
                if (entry != null)
                {
                    this.codes.Remove(entry);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private String GenerateCode()
        {
            // This is not how you create a security code.
            return (this.random.Next() % 100000000).ToString("D8");
        }

        private sealed class Entry
        {
            public Entry(String email, String code)
            {
                this.Email = email;
                this.Code = code;
            }

            public String Email { get; }

            public String Code { get; }
        }
    }
}