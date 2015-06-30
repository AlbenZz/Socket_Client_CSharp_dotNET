using SocketClient.MessageDEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient.ProtocolFormats
{


    public class PassengerRecord
    {
        public string PassengerNo { get; set; }
        public string PersonalID { get; set; }
        public float OnboardLocation { get; set; }
        public float OffboardLocation { get; set; }
        public string JourneyID { get; set; }

        public PassengerRecord()
        {

        }
        public PassengerRecord(string pn, string pid, float ol, float offl, string jid)
        {
            PassengerNo = pn;
            PersonalID = pid;
            OnboardLocation = ol;
            OffboardLocation = offl;
            JourneyID = jid;
        }

        public override string ToString()
        {
            string EOL = "\n";
            return "Passenger NO.  : " + PassengerNo + EOL +
                    "\tPersonal ID          : " + PersonalID + EOL +
                    "\tOnboard Location     : " + OnboardLocation + EOL +
                    "\tOffboardLocation     : " + OffboardLocation + EOL +
                    "\tJourney ID           : " + JourneyID + EOL;
        }
    }

    public class TVLForPassengerRecord
    {
        public TVLForPassengerRecord(byte type)
        {
            Type = type;
        }
        public TVLForPassengerRecord(byte type, int length, List<PassengerRecord> pasgRcds)
        {
            Type = type;
            NumberOfPassenger = length;
            PassengerRecords = pasgRcds;
        }
        public byte Type { get; set; }
        public int NumberOfPassenger { get; set; }
        public List<PassengerRecord> PassengerRecords { get; set; }

        public override string ToString()
        {
            string EOLN = "\n";
            string value;
            if (Type == TVLForPassengerRecordConst.FULLREQUEST)
                value = "The message is a request...";
            else if (Type == TVLForPassengerRecordConst.NODATA)
                value = "The message is a null response for a request...";
            else // a data response
            {
                value = "This is a data response for pulling data from the remote server..." + EOLN;
                value += "The response message contains " + NumberOfPassenger + " passenger records: " + EOLN;
                foreach (PassengerRecord pr in PassengerRecords)
                {
                    value += pr.ToString();
                }
            }

            return value ;
        }
    }
    


}
