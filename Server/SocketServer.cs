using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class SocketServer
    {
        private readonly Socket _socket;
        private readonly IPEndPoint _endPoint;
        private readonly List<Socket> _clients = new List<Socket>();
        public SocketServer(string ip, int port)
        {
            IPAddress iPAddress = IPAddress.Parse(ip);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _endPoint = new IPEndPoint(iPAddress, port);
        }

        public void StartListening(int limit)
        {
            try
            {
                _socket.Bind(_endPoint);
                _socket.Listen(limit);
                Console.WriteLine($"Server {_endPoint.Address} : {_endPoint.Port}\n");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public void ClientConnection()
        {
            while (true)
            {
                Socket? client;
                client = AwaitClient();

                if (client != null)
                {
                    lock (_clients)
                    {
                        _clients.Add(client);
                    }
                    Thread thread = new Thread(() => ProcessClientMessages(client));
                    thread.Start();
                }
            }
        }

        public Socket? AwaitClient()
        {
            try
            {
                Socket client = _socket.Accept();
                Console.WriteLine($"{client.RemoteEndPoint} connected");
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client-Connection Exception: {ex.Message}");
                return null;
            }
        }

        public void ProcessClientMessages(Socket client)
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesCount = client.Receive(buffer);

                    if (bytesCount == 0)
                    {
                        break;
                    }

                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesCount);
                    Console.WriteLine($"{client.RemoteEndPoint} -> {receivedMessage}");

                    
                    byte[] responseBuffer = Encoding.UTF8.GetBytes(receivedMessage);
                    lock (_clients)
                    {
                        foreach (var c in _clients)
                        {
                            if (c != client)
                            {
                                c.Send(responseBuffer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                DisconnectionClient(client);
            }
        }

        public void DisconnectionClient(Socket client)
        {
            try
            {
                lock (_clients)
                {
                    _clients.Remove(client);
                }
                Console.WriteLine($"{client.RemoteEndPoint} disconnected");
                client.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Closing-Client-Socket Exception: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
