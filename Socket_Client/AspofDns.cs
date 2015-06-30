using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class AspofDns
    {
    }


    class IPAddressExample
    {

        // ===================================
        public static void PrintHostInfo(string host)
        {
            try
            {

                IPHostEntry hostInfo;
                Console.WriteLine("resolving " + host + "...");
                hostInfo = Dns.GetHostEntry(host);
                Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);

                Console.WriteLine("Family\t\tIP addresses: ");
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.WriteLine(ipaddr.AddressFamily + "\t" + ipaddr.ToString());
                }
                Console.WriteLine();

                Console.WriteLine("Aliases:   ");
                foreach (String alias in hostInfo.Aliases)
                {
                    Console.WriteLine(alias + ", ");
                }

                Console.WriteLine("\nend of aliases list" );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void PrintHostInfo(IPAddress ip)
        {
            try
            {
                IPHostEntry hostInfo;

                Console.WriteLine("resolving " + ip.ToString() + "...");
                hostInfo = Dns.GetHostEntry(ip);

                Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);

                Console.WriteLine("Family\t\tIP addresses: ");
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.WriteLine(ipaddr.AddressFamily + "\t" + ipaddr.ToString());
                }
                Console.WriteLine();

                Console.WriteLine("Aliases:   ");
                foreach (String alias in hostInfo.Aliases)
                {
                    Console.WriteLine(alias + ", ");
                }
                Console.WriteLine("\n\nend of aliases list");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class TestIPAddressExample
    {

        // ===================================
        public static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("Local Host: ");
                String lHN = Dns.GetHostName();
                Console.WriteLine("\tHost Name:     " + lHN);

                Console.WriteLine("------------------------------------");
                Console.WriteLine("------------------------------------");

                IPAddressExample.PrintHostInfo(lHN);


                Console.WriteLine("------------------------------------");
                Console.WriteLine("------------------------------------");
                IPAddressExample.PrintHostInfo(IPAddress.Parse("172.27.35.18"));
                
                // phone doesn't have a DNS service? no!
                //IPAddressExample.PrintHostInfo(IPAddress.Parse("172.27.35.4"));

                Console.WriteLine("------------------------------------");
                Console.WriteLine("------------------------------------");
                IPAddressExample.PrintHostInfo(IPAddress.Parse("169.1.1.2"));


                Console.WriteLine("------------------------------------");
                Console.WriteLine("------------------------------------");
                IPAddressExample.PrintHostInfo("www.en.wikipedia.org");

                Console.WriteLine("------------------------------------");
                Console.WriteLine("------------------------------------");
                IPAddressExample.PrintHostInfo("www.cnbeta.com");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }

            Console.Read();
        }
	
    }
}
