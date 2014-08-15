using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Http;
using Information.Models; 

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
            List<string> Quotes = GetQuoteList();
            List<QuoteRecord> returned = new List<QuoteRecord>();

            if (Quotes == null)
                return ReturnNotFound();

            var results = Quotes.Where(q => q.IndexOf(id, StringComparison.CurrentCultureIgnoreCase) > 0).OrderBy(r => Guid.NewGuid()).Take(maxToReturn); 
            foreach (var q in results)
            {
                QuoteRecord qr = BuildQuote(q);
                returned.Add(qr);

            }

            return returned;
        }


        public QuoteRecord Post([FromBody] SubmittedQuote submittedQuote)
        {
            System.Diagnostics.Debug.WriteLine(submittedQuote.ToString());
            QuoteRecord qr = new QuoteRecord { Author = submittedQuote.Author, Quote = submittedQuote.Quote };
            AddQuote(qr);
            return  qr ; 
        }


        public class SubmittedQuote
        {
            public string Author { get; set; }
            public string Quote{ get; set; }
            public string Reference { get; set; }
            public string Email { get; set; }
            public int VoteUp { get; set; }
            public int VoteDown { get; set; }
            public bool ModeratorApproved { get; set; }
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
            List<string> Quotes = GetQuoteList();
            if (Quotes == null)
                return ReturnNotFound();

            int Idx = random.Next(0, Quotes.Count);

            QuoteRecord qr = BuildQuote(Quotes[Idx]);
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

        private static List<string> GetQuoteList()
        {
            var app = HttpContext.Current.Application;
            List<string> Quotes = (List<string>)app["AppQuotes"];
            return Quotes;
        }


        private static QuoteRecord BuildQuote(string q)
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

        private static Random random = new Random(); 

    }
}
