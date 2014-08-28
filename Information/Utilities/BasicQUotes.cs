using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Information.Models; 

namespace Information.Utilities
{
    public static class BasicQuotes
    {
        public static List<string> GetQuoteList()
        {
            var app = HttpContext.Current.Application;
            List<string> Quotes = (List<string>)app["AppQuotes"];
            return Quotes;
        }

        public static QuoteRecord BuildQuote(string q)
        {
            string[] data = q.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
            string quote = data[0];
            string author = "";
            if (data.Length > 1)
            {
                author = data[1];
            }

            QuoteRecord qr = new QuoteRecord { Author = author, Quote = quote };
            return qr;
        }


    }
}