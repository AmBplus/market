using System;

namespace Framework.Settings.AppSettings
{
    public class SmsProviderSettings
    {
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string DefaultSenderNumber { get; set; } = String.Empty;
    }
}
