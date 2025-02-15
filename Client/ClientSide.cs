using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientSide
    {       
        static void Main(string[] args)
        {
            var client = new SocketClient();

            client.StartConnection("127.0.0.1", 5000);

            Thread sendThread = new Thread(client.SendMessages);
            sendThread.Start();

            Thread receiveThread = new Thread(client.ReceiveMessages);
            receiveThread.Start();
        }
    }
}
