using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Http;
using Information.Models;
using Information.Utilities; 

namespace Information.Controllers.ApiControllers
{
    public class QuoteController : ApiController
    {

        public List<QuoteRecord> Get(string id, int maxToReturn )
        {
            if (id == null || id == "" )
            {
                id = " "; 
            }
            List<string> Quotes = BasicQuotes.GetQuoteList();
            List<QuoteRecord> returned = new List<QuoteRecord>();

            if (Quotes == null)
                return ReturnNotFound();

            var results = Quotes.Where(q => q.IndexOf(id, StringComparison.CurrentCultureIgnoreCase) > 0).OrderBy(r => Guid.NewGuid()).Take(maxToReturn); 
            foreach (var q in results)
            {
                QuoteRecord qr = BasicQuotes.BuildQuote(q);
                returned.Add(qr);

            }

            return returned;
        }


        public QuoteRecord Post([FromBody] QuoteRec submittedQuote)
        {
            System.Diagnostics.Debug.WriteLine(submittedQuote.ToString());
            QuoteRecord qr = new QuoteRecord { Author = submittedQuote.Author, Quote = submittedQuote.Quote };
            AddQuote(qr);
            return  qr ; 
        }




        private static List<QuoteRecord> ReturnNotFound()
        {
            List<QuoteRecord> returned = new List<QuoteRecord>(); 
            returned.Add(new QuoteRecord { Author = "", Quote = "No Quote Data Loaded" });
            return returned;
        }


        public List<QuoteRecord> Get()
        {
            List<QuoteRecord> returned = new List<QuoteRecord>();
            List<string> Quotes = BasicQuotes.GetQuoteList();
            if (Quotes == null)
                return ReturnNotFound();

            int Idx = random.Next(0, Quotes.Count);

            QuoteRecord qr = BasicQuotes.BuildQuote(Quotes[Idx]);
            returned.Add(qr);

            return returned;
        }


        private static void AddQuote( QuoteRecord qr)
        {
            string NewQuote = qr.Quote + "--" + qr.Author; 
            
            // WIP lock this access from multiple users 
            var app = HttpContext.Current.Application;
            List<string> Quotes = (List<string>)app["AppQuotes"];
            Quotes.Add(NewQuote);
            app["AppQuotes"] = Quotes; 

            string QuoteFile = HttpContext.Current.Server.MapPath("~/App_Data/sig.dat");
            // WIP save the file 
        
        }

        private static Random random = new Random(); 

    }
}
