using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProtocolAndUtilities
{
    public class ItemQuoteProtocolFormat
    {
        public long itemNumber { get; set; }
        public string itemDesc { get; set; }
        public int quantity { get; set; }
        public double unitPrice { get; set; }
        public Boolean discounted { get; set; }
        public Boolean inStock { get; set; }

        public ItemQuoteProtocolFormat()
        {

        }
        public ItemQuoteProtocolFormat(long n, string d, int q, double p, bool dc, bool ins)
        {
            itemNumber  = n;
            itemDesc    = d;
            quantity    = q;
            unitPrice   = p;
            discounted  = dc;
            inStock     = ins;
        }


        public override string ToString()
        {
            string EOLN = "\n";
            string value = "Item#           : " + itemNumber + EOLN +
                            "Description    : " + itemDesc + EOLN +
                            "Price (each)   : " + unitPrice + EOLN +
                            "Quantity       : " + quantity + EOLN +
                            "Total Price    : " + (quantity * unitPrice);

            if (discounted)
            {
                value += " (discounted)" + EOLN;
            }
            if (inStock)
                value += "In Stock" + EOLN;
            else
                value += "Out of Stock" + EOLN;
            return value;
        }
        
    }
}
