using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProtocolAndUtilities.Utilities.ByteOrder
{

    public interface IByteOrderConverter
    {

    }

    public class ByteOrderConverterForPrimitiveType
    {

        //public static unsafe double HostToNetworkOrder(double data)
        //{
        //    byte[] netWorkOrder = new byte[sizeof(double)];
        //    if (BitConverter.IsLittleEndian)
        //    {
        //        for (int i = 0; i < sizeof(double); i++)
        //        {
        //            netWorkOrder[i] = (byte)(*((UInt64 *)&data) & 0xFFL);
        //            *((UInt64 *)&data) >>= 8;
        //        }
        //        return *((double *)&netWorkOrder);
        //    }
            
        //}
        //public static unsafe byte[] HostToNetworkOrder(float data)
        //{
        //    byte[] netWorkOrder = new byte[sizeof(float)];
        //    if (BitConverter.IsLittleEndian)
        //    {
        //        for (int i = 0; i < sizeof(float); i++)
        //        {
        //            netWorkOrder[i] = (byte)(*((UInt32 *)&data) & 0xFFL);
        //            *((UInt32 *)&data) >>= 8;
        //        }
        //    }
        //    return netWorkOrder;
        //}

        //public static unsafe double NetworkToHostOrder(byte[] bytes)
        //{
        //    double data;
        //    if (BitConverter.IsLittleEndian)
        //    {

        //    }
            
        //}
    }
}
