using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using ApplicationProtocolAndUtilities.Utilities.ByteOrder;

namespace ApplicationProtocolAndUtilities.MessageDEncoders
{
    public class ItemQuoteBinEncoderConst
    {
        public static readonly string DEFAULT_CHAR_ENC = "ascii";

        public static readonly byte DISTOUNT_FLAG = 1 << 7;
        public static readonly byte IN_STOCK_FLAG = 1 << 0;
        public static readonly int MAX_DESC_LEN = 512;
        public static readonly int MAX_WIRE_LENGTH = 1024;

    }

    #region Encoder
    public class ItemQuoteBinEncoder: IItemQuoteProcotolDEncoder
    {


        public Encoding encoding { get; set; }

        public ItemQuoteBinEncoder (): this(ItemQuoteBinEncoderConst.DEFAULT_CHAR_ENC)
	    {
            
	    }

        public ItemQuoteBinEncoder(string encodingDesc)
        {
            // TODO: Complete member initialization
            encoding = Encoding.GetEncoding(encodingDesc);
        }

        public byte[] Encode(ItemQuoteProtocolFormat item)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(new BufferedStream(memStream));

            //-------itemNumber
            binWriter.Write(IPAddress.HostToNetworkOrder(item.itemNumber));
            
            //-------item quantity
            binWriter.Write(IPAddress.HostToNetworkOrder(item.quantity));
            
            //-------item unit price's string representation length and double string in byte of ASCII encoding
            byte[] encodedUnitPrice = encoding.GetBytes(item.unitPrice.ToString());
            binWriter.Write(IPAddress.HostToNetworkOrder(encodedUnitPrice.Length));
            binWriter.Write(encodedUnitPrice);
            
            //binWriter.Write(ByteOrderConverterForPrimitiveType.HostToNetworkOrder(item.unitPrice));

            //-------item description's length and description in byte of ASCII encoding
            byte[] encodedDesc = encoding.GetBytes(item.itemDesc);
            binWriter.Write(IPAddress.HostToNetworkOrder(encodedDesc.Length));
            binWriter.Write(encodedDesc);

            //-------flag for discounted and stock state
            byte flags = 0;
            if (item.discounted)
                flags |= ItemQuoteBinEncoderConst.DISTOUNT_FLAG;
            if (item.inStock)
                flags |= ItemQuoteBinEncoderConst.IN_STOCK_FLAG;
            binWriter.Write(flags);

            binWriter.Flush();

            return memStream.ToArray();
        }
    }
    #endregion

    #region Decoder

    public class ItemQuoteBinDecoder: IItemQuoteProcotolDecoder
    {
        public Encoding encoding { get; set; }
        public ItemQuoteBinDecoder (): this(ItemQuoteBinEncoderConst.DEFAULT_CHAR_ENC)
    	{
	    }

        public ItemQuoteBinDecoder(string encodingDesc)
        {
            encoding = Encoding.GetEncoding(encodingDesc);
        }

        public ItemQuoteProtocolFormat Decode(Stream inComingByteStream)
        {
            BinaryReader binReader = new BinaryReader(new BufferedStream(inComingByteStream));
            
            //-------itemNumber
            long itemNumuber = IPAddress.NetworkToHostOrder(binReader.ReadInt64());


            //-------item quantity
            int quantity = IPAddress.NetworkToHostOrder(binReader.ReadInt32());


            //-------item unit price's string representation length and double string in byte of ASCII encoding
            int lenForStringPrice = IPAddress.NetworkToHostOrder(binReader.ReadInt32());
            if (lenForStringPrice == -1)
                throw new EndOfStreamException();
            byte[] stringPrice = new byte[lenForStringPrice];
            binReader.Read(stringPrice, 0, lenForStringPrice);
            string itemUnitPriceString = encoding.GetString(stringPrice);
            double itemUnitPrice = Double.Parse(itemUnitPriceString);
            //double itemUnitPrice = binReader.ReadDouble();

            //-------item description's length and description in byte of ASCII encoding
            int descLen = IPAddress.NetworkToHostOrder(binReader.ReadInt32());
            if (descLen == -1)
                throw new EndOfStreamException();
            byte[] descInBytes = new byte[descLen];
            binReader.Read(descInBytes, 0, descLen);
            string itemDesc = encoding.GetString(descInBytes);


            //-------flag for discounted and stock state
            byte flags = binReader.ReadByte();


            return new ItemQuoteProtocolFormat(
                    itemNumuber, itemDesc, quantity, itemUnitPrice,
                    ((flags & ItemQuoteBinEncoderConst.DISTOUNT_FLAG) == ItemQuoteBinEncoderConst.DISTOUNT_FLAG),
                    ((flags & ItemQuoteBinEncoderConst.IN_STOCK_FLAG) == ItemQuoteBinEncoderConst.IN_STOCK_FLAG)
                );
        }

    }
    
    #endregion
}
