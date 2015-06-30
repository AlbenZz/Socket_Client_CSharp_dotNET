using SocketClient.ProtocolFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApplicationProtocolAndUtilities.Utilities.ByteOrder;
using SocketClient.ProtocolFormats;
namespace SocketClient.MessageDEncoders
{
    //public enum TVLCOMMAND { RESPONSE = 0, REQUEST = 255, NODATA = 128 };

    class TVLForPassengerRecordConst
    {
        public static readonly string DEFAULT_CHAR_ENC = "ascii";

        public static readonly byte FULLREQUEST     = 255;
        public static readonly byte RESPONSE    = 0;
        public static readonly byte NODATA      = 128;

        public static ConsoleColor color = ConsoleColor.Gray;

    }

    public class TVLForPassengerRecordEncoder
    {
        public Encoding encoding { get; set; }
        public TVLForPassengerRecordEncoder()
            : this(TVLForPassengerRecordConst.DEFAULT_CHAR_ENC)
	    {
            
	    }
        public TVLForPassengerRecordEncoder(string encodingDesc)
        {
            // TODO: Complete member initialization
            encoding = Encoding.GetEncoding(encodingDesc);
        }



        public byte[] Encode(TVLForPassengerRecord tvlForPR)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(new BufferedStream(memStream));


            //-------flag for message type
            binWriter.Write(tvlForPR.Type);

            if (tvlForPR.Type == 255)
            {
                // a request type
                // Type == 0: a response type
            }
            else if (tvlForPR.Type == 128)
            {
                // remote server has no new data currently
            }
            else if (tvlForPR.Type == 0)
            {
                if (tvlForPR.NumberOfPassenger <= 0)
                {
                    throw new Exception("Invalid Message: with no passenger record payload!!");
                }
                else
                {
                    binWriter.Write(IPAddress.HostToNetworkOrder(tvlForPR.NumberOfPassenger));
                    foreach (PassengerRecord psgRcd in tvlForPR.PassengerRecords)
                    {
                        // write PassengerNo
                        byte[] pnoInBytes = encoding.GetBytes(psgRcd.PassengerNo);
                        binWriter.Write((byte)psgRcd.PassengerNo.Length);
                        //binWriter.Write(IPAddress.HostToNetworkOrder(psgRcd.PassengerNo.Length));
                        binWriter.Write(pnoInBytes, 0, pnoInBytes.Length);

                        // write PersonalID
                        byte[] pIDInBytes = encoding.GetBytes(psgRcd.PersonalID);
                        binWriter.Write((byte)psgRcd.PersonalID.Length);
                        //binWriter.Write(IPAddress.HostToNetworkOrder(psgRcd.PersonalID.Length));
                        binWriter.Write(pIDInBytes, 0, pIDInBytes.Length);

                        // write OnboardLocation
                        byte[] oblInBytes = encoding.GetBytes(psgRcd.OnboardLocation.ToString());


                        binWriter.Write((byte)psgRcd.OnboardLocation.ToString().Length);
                        //binWriter.Write(IPAddress.HostToNetworkOrder(psgRcd.OnboardLocation.ToString().Length));
                        binWriter.Write(oblInBytes, 0, oblInBytes.Length);

                        // write OffboardLocation
                        byte[] ofblInBytes = encoding.GetBytes(psgRcd.OffboardLocation.ToString());
                        binWriter.Write((byte)psgRcd.OffboardLocation.ToString().Length);
                        //binWriter.Write(IPAddress.HostToNetworkOrder(psgRcd.OffboardLocation.ToString().Length));
                        binWriter.Write(ofblInBytes, 0, ofblInBytes.Length);

                        // write JourneyID
                        byte[] jIDInBytes = encoding.GetBytes(psgRcd.JourneyID);
                        binWriter.Write((byte)psgRcd.JourneyID.Length);
                        //binWriter.Write(IPAddress.HostToNetworkOrder(psgRcd.JourneyID.Length));
                        binWriter.Write(jIDInBytes, 0, jIDInBytes.Length);
                    }
                }
            }
            else
            {
                throw new Exception("Invalid Message Type!");
            }

            binWriter.Flush();
            return memStream.ToArray();
        }
    }




    #region Decoder

    public class TVLForPassengerRecordDecoder
    {
        public Encoding encoding { get; set; }
        public TVLForPassengerRecordDecoder()
            : this(TVLForPassengerRecordConst.DEFAULT_CHAR_ENC)
        {
        }

        public TVLForPassengerRecordDecoder(string encodingDesc)
        {
            encoding = Encoding.GetEncoding(encodingDesc);
        }

        public TVLForPassengerRecord Decode(Stream wire)
        {
            BinaryReader binReader = new BinaryReader(new BufferedStream(wire));

            //-------itemNumber
            byte commandType = binReader.ReadByte();
            if (commandType != 0 && commandType != 255 && commandType != 128)
            {
                throw new Exception("Incoming Unknow Message Type!");
            }
            else if (commandType == 255 || commandType == 128)
            {
                return new TVLForPassengerRecord(commandType);
            }
            else    // 0: the incoming data is a response message, which contains data
            {

                int numberOfPR = IPAddress.NetworkToHostOrder(binReader.ReadInt32());

                List<PassengerRecord> PRList = new List<ProtocolFormats.PassengerRecord>();
                for (int i = 0; i < numberOfPR; i++)
                {
                    // read PassengerNo
                    //int pnl = IPAddress.NetworkToHostOrder(binReader.ReadInt32());
                    byte pnl = binReader.ReadByte();
                    byte[] pnlInBytes = new byte[pnl];
                    binReader.Read(pnlInBytes, 0, pnl);
                    string pasNoStr = encoding.GetString(pnlInBytes);

                    // read PersonalID
                    //int pidl = IPAddress.NetworkToHostOrder(binReader.ReadInt32());
                    byte pidl = binReader.ReadByte();
                    byte[] pidInBytes = new byte[pidl];
                    binReader.Read(pidInBytes, 0, pidl);
                    string perIDStr = encoding.GetString(pidInBytes);

                    // read OnboardLocation
                    //int oblL = IPAddress.NetworkToHostOrder(binReader.ReadInt32());
                    byte oblL = binReader.ReadByte();
                    byte[] oblInBytes = new byte[oblL];
                    binReader.Read(oblInBytes, 0, oblL);
                    string oblStr = encoding.GetString(oblInBytes);
                    float obl = float.Parse(oblStr);
                    // read OffboardLocation

                    //int ofblL = IPAddress.NetworkToHostOrder(binReader.ReadInt32());
                    byte ofblL = binReader.ReadByte();
                    byte[] ofblInBytes = new byte[ofblL];
                    binReader.Read(ofblInBytes, 0, ofblL);
                    string ofblStr = encoding.GetString(ofblInBytes);
                    float ofbl = float.Parse(ofblStr);

                    // read JourneyID
                    //int jouridl = IPAddress.NetworkToHostOrder(binReader.ReadInt32());
                    byte jouridl = binReader.ReadByte();
                    byte[] jourIdInBytes = new byte[jouridl];
                    binReader.Read(jourIdInBytes, 0, jouridl);
                    string jourIDStr = encoding.GetString(jourIdInBytes);

                    PRList.Add(
                        new PassengerRecord(pasNoStr, perIDStr, obl, ofbl, jourIDStr)
                        );
                }
                return new ProtocolFormats.TVLForPassengerRecord(commandType, numberOfPR, PRList);
            }
        }
    }
    #endregion
   
}
