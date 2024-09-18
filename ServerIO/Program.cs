using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerIO
{
    class Program
    {
        private const int Port = 1980; 

        static async Task Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Any, Port);
            server.Start();
            Console.WriteLine("Server listening started.");

            bool loggedNoRequest = false;
            bool done = false;

            while (!done)
            {
                if (!server.Pending())
                {
                    if (!loggedNoRequest)
                    {
                        Console.WriteLine("There are no pending requests yet.");
                        loggedNoRequest = true;
                    }
                    await Task.Delay(1000); 
                }
                else
                {
                    loggedNoRequest = false;

                    var client = await server.AcceptTcpClientAsync();
                    Console.WriteLine("\n===== Client connection received =====");

                    var stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        var requestMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.ForegroundColor = ConsoleColor.Green; 
                        Console.WriteLine("\n**** Client Message Received ****");
                        Console.WriteLine(requestMessage);
                        Console.WriteLine("================================\n");
                        Console.ResetColor();

                        string data = new string(requestMessage.Where(c => !char.IsControl(c)).ToArray());
                        Console.WriteLine("Processed Message: " + data);
                    }

                    client.Close();
                    Console.WriteLine("Client connection closed.");
                }
            }

            server.Stop();
            Console.WriteLine("Server stoped.");
        }
    }
}
