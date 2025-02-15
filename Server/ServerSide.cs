using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServerSide
    {
        static void Main(string[] args)
        {
            var server = new SocketServer("127.0.0.1", 5000);

            server.StartListening(5);
            server.ClientConnection();
        }
    }
}
