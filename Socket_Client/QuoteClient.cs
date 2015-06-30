﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ApplicationProtocolAndUtilities;
using ApplicationProtocolAndUtilities.Utilities;
using ApplicationProtocolAndUtilities.MessageDEncoders;

namespace SocketClient
{




    class QuoteClient
    {
        private bool useDefaultServerSetting = true;
        public bool DefaultServer
        {
            get { return useDefaultServerSetting; }
            set { useDefaultServerSetting = value; }
        }

        
        private bool bufSizeSet = false;
        private int defaultBufferSize = 65536;     // Size of recieve buffer
        private int defaultServPort = 4050;
        private string defaultServer = "127.0.0.1";

        private int servPort;
        public int ServPort
        {
            get { return servPort; }
            set 
            { 
                servPort = value; 

            }
        }

        public bool customServerSet = false;
        private string customServer;
        public string CustomServer
        {
            
            set 
            { 
                customServer = value;
                customServerSet = true;
            }
        }

        private int bufferSize;
        public int BufferSize
        {
            get { return bufferSize; }
            set 
            { 
                bufferSize = value;
                bufSizeSet = true;
            }
        }

        private TcpClient client = null;
        private NetworkStream netStream = null;


        public void Start()
        {

            string server = defaultServer;
            int port = defaultServPort;

            //ItemQuoteProtocolFormat itemQuote = new ItemQuoteProtocolFormat(1234512341234L, "Item 1", 1000, 1234.3, true, false);

            try 
	        {
                // set up connection
                client = new TcpClient(server, port);
                Console.WriteLine("connection establised...");
                // get bidirectional communication channel
                netStream = client.GetStream();
                Console.WriteLine("");

                // prepare command/request or just data
                ItemQuoteProtocolFormat itemQuote = new ItemQuoteProtocolFormat(1234512341234L, "Item 1", 1000, 20.3, true, false);

                // encode the message
                ItemQuoteBinEncoder encoder = new ItemQuoteBinEncoder();
                byte[] encodedMessage = encoder.Encode(itemQuote);

                // send a request/command to the server
                netStream.Write(encodedMessage, 0, encodedMessage.Length);


                // wait to read and decode response from the server
                ItemQuoteBinDecoder decoder = new ItemQuoteBinDecoder();
                ItemQuoteProtocolFormat rcvdItemQuote = decoder.Decode(netStream);
                Console.WriteLine("Received Binary-Encode Quote: ");
                Console.WriteLine(rcvdItemQuote);
                
                // decode the response
                // prepare event arguments
                // raise the event to store and display
                

                // actions: storing, displaying
                netStream.Close();
                client.Close();

	        }
	        catch (SocketException se)
            {
                Console.WriteLine("\n---------");
                Console.WriteLine(System.DateTime.Now.ToString() + ": " + "Error: " + se.ErrorCode +
                                    "\nMessage: " + se.Message);
                Console.Read();

                Environment.Exit(se.ErrorCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n---------");
                Environment.Exit(Environment.ExitCode);
            }
            finally
            {
                if (netStream != null)
                    netStream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }

    class QuoteClientTester
    {

        // ===================================
        public static void Main(string[] args)
        {
            QuoteClient client = new QuoteClient();
            client.Start();
            Console.Read();

        }
	
    }

}
