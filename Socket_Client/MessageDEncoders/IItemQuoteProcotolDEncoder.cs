using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProtocolAndUtilities.MessageDEncoders
{
    public interface IItemQuoteProcotolDEncoder
    {
        byte[] Encode(ItemQuoteProtocolFormat item);
    }
    public interface IItemQuoteProcotolDecoder
    {
        ItemQuoteProtocolFormat Decode(Stream source);
        //ItemQuoteProtocolFormat Decode(byte[] packet);
    }
}
