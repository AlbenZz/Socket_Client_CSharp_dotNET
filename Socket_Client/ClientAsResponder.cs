using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{


    /** this is essentially a server and just for convience to build a request-and-response tcp channel
     *  based on the echo model 
     */
    

    class ClientAsResponder
    {
        private const int BUFSIZE = 65536;     // Size of recieve buffer
        private const int servPort = 4008;
        private const string hostIP = "127.0.0.1";
        public static void Main(string[] args)
        {
            //if (args.Length > 2)
            //{
            //    throw new ArgumentException("Parameters: [<network>] [<Port>]");
            //}

            //int servPort = (args.Length == 1) ? Int32.Parse(args[0]) : 7;

            
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, servPort);
                listener.Start();
                Console.WriteLine("Echo Server runs on the socket: <{0}>:<{1}>", IPAddress.Any, servPort);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                Environment.Exit(se.ErrorCode);
            }

            byte[] rcvBuffer = new byte[BUFSIZE];    //the one of MSS limitation of tcp
            int bytesRcvd;

            for (; ;)
            {
                TcpClient inComingClient = null;
                NetworkStream netStream = null;

                try
                {
                    /**
                     *  one connection for one request-and-response operation
                     *
                     */
                    inComingClient = listener.AcceptTcpClient();
                    Console.WriteLine("a new coming connection detect, handling Client.... ");
                    netStream = inComingClient.GetStream();
                    
                    
                    
                    // defragment the message, and then response based on the request type
                    

                    int totalBytesEchoed = 0;

                    
                    while ((bytesRcvd = netStream.Read(rcvBuffer, 0, rcvBuffer.Length)) != 0)
                    {
                        netStream.Write(rcvBuffer, 0, bytesRcvd);
                        totalBytesEchoed += bytesRcvd;
                    }



                    if (bytesRcvd == 0)
                    {
                        Console.WriteLine("The connected client has disconnected, and the string is echoed correctly!");
                        Console.WriteLine("Echoed {0} bytes", totalBytesEchoed);
                    }
                    else
                    {
                        Console.WriteLine("the channel has been corrupted by various 'Known'(not by now) reasons");
                    }


                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.ErrorCode + ": " + se.Message);
                }
                finally
                {
                    if (netStream != null)
                    {
                        netStream.Close();
                    }
                    if (inComingClient != null)
                    {
                        inComingClient.Close();
                    }
                }

            }
        }
    }
}
