using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Socket_Client
{
    class EchoClient
    {

        private Socket client_Socket;

        EchoClient() {
            

        }

        public Int32 IPv4Addr { get; set; }
        public Int16 TCP_Port { get; set; }


        
        public void SetupSocket() { }

        public void InitiateConnection()
        {

        }

        public void SendEchoString(string echo) 
        { 
        }

    }
}
