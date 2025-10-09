using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Cart.Infrastructure.Caches
{
    public class RedisOptions
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 6379;
        public string? User { get; set; }
        public string? Password { get; set; }
        public bool Ssl { get; set; } = false;
        public int DefaultDatabase { get; set; } = 0;
        public int ConnectRetry { get; set; } = 5;
        public int ConnectTimeout { get; set; } = 5000;
        public int SyncTimeout { get; set; } = 5000;

        public override string ToString()
        {
            // Build connection string
            var authPart = string.IsNullOrEmpty(User)
                ? (string.IsNullOrWhiteSpace(Password) ? "" : $",password={Password}")
                : $",user={User},password={Password}";

            return $"{Host}:{Port}{authPart},ssl={Ssl},abortConnect=false,connectRetry={ConnectRetry},connectTimeout={ConnectTimeout},syncTimeout={SyncTimeout},defaultDatabase={DefaultDatabase}";
        }
    }
}
