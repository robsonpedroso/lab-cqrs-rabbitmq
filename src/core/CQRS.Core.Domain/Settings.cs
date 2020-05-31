using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace CQRS.Core.Domain
{
    public class Settings
    {
        public RabbitConfig RabbitConfig { get; set; }

        public Settings()
        {
            RabbitConfig = new RabbitConfig();
        }
    }

    public class RabbitConfig
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public RabbitConfig()
        {
            this.HostName = "192.168.33.10";
            this.UserName = "admin";
            this.Password = "123";
        }
    }
}
