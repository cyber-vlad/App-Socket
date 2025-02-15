using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SocketClient
    {
        readonly Socket _socket;
        private bool isConnected = false;

        public SocketClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartConnection(string serverIp, int serverPort)
        {
            IPAddress ipAddress = IPAddress.Parse(serverIp);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, serverPort);
            try
            {
                _socket.Connect(endPoint);
                isConnected = true;
                Console.WriteLine($"Successfully connected to {endPoint.Address}");
                Console.WriteLine("Type exit, if you want to leave");
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

        public void SendMessages()
        {
            while (isConnected)
            {
                try
                {
                    Console.Write("Message: ");
                    string text = Console.ReadLine();

                    if (string.IsNullOrEmpty(text))
                    {
                        Console.WriteLine("Message cannot be empty...");
                        continue;
                    }

                    if (text == "exit")
                    {
                        StopConnection();
                        return;
                    }

                    byte[] bytesData = Encoding.UTF8.GetBytes(text);
                    _socket.Send(bytesData);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Sending-Message Exception: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    StopConnection();
                    return;
                }
            }
        }

        public void ReceiveMessages()
        {
            while (isConnected)
            {
                try
                {
                    byte[] bytesResponse = new byte[2048];
                    int bytesReceived = _socket.Receive(bytesResponse);

                    if (bytesReceived == 0)
                    {
                        Console.WriteLine("Server has disconnected.");
                        StopConnection();
                        return;
                    }

                    if (bytesReceived > 0)
                    {
                        string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesReceived);
                        Console.WriteLine();
                        Console.WriteLine($"From server: {response}");
                        Console.Write("Message: ");
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Receiving-Message Exception: {ex.Message}");
                    StopConnection();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    StopConnection();
                    return;
                }
            }
        }
        public void StopConnection()
        {
            try
            {
                if (isConnected)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                    isConnected = false;
                    Console.WriteLine("\nDisconnected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Disconnection Exception: {ex.Message}");
            }
        }
    }

}
