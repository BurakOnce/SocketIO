using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientIO
{
    internal class Program
    {
        private const int Port = 1980;
        private const string IpAddress = "127.0.0.1";

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Type 'exit' to close the client.");
                bool done = false;

                while (!done)
                {
                    Console.WriteLine("Your message for server:");
                    string message = Console.ReadLine();

                    if (message.ToLower() == "exit")
                    {
                        done = true;
                        break;
                    }

                    if (!string.IsNullOrEmpty(message))
                    {
                        TcpClient client = new TcpClient(IpAddress, Port);

                        byte[] sendData = Encoding.UTF8.GetBytes(message);

                        using (NetworkStream networkStream = client.GetStream())
                        {
                            await networkStream.WriteAsync(sendData, 0, sendData.Length);
                        }

                        client.Close();
                        Console.WriteLine("Message sent.");
                    }
                    else
                    {
                        Console.WriteLine("Empty message cannot be sent.");
                    }
                }
                Console.WriteLine("Client closed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while connecting to the device: " + ex.Message);
            }
        }
    }
}
