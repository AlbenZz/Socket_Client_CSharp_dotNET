using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProtocolAndUtilities.Utilities
{
    public class StatePrinter
    {
        public static void PrintProcedureState(string stateMessage)
        {
            Console.WriteLine(System.DateTime.Now.ToString() + ":   " + stateMessage);
        }

    }
}
