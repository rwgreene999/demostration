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
    public class QuoteDBController : ApiController
    {
        public List<QuoteRec> Get(string id, int maxToReturn)
        {
            List<QuoteRec> returned = new List<QuoteRec>(); 
            if (id == null || id == "")
            {
                id = " ";
            }
            id = id.ToLower(); 

            List<QuoteRec> QuoteRecs = QuoteDB.GetQuoteList();

            if (QuoteRecs == null)
                return ReturnNotFound();

            // WIP there has to be a better string ignore case comparison 
            var results = QuoteRecs
                .Where(q =>  q.Quote.ToLower().Contains(id) == true || q.Author.ToLower().Contains(id) )
                .OrderBy( r=>Guid.NewGuid())
                .Take(maxToReturn); 
                ;

            foreach (var qr in results)
            {
                returned.Add(qr);
            }

            return returned;
        }


        public QuoteRec  Post([FromBody] QuoteRec submittedQuote)
        {
            System.Diagnostics.Debug.WriteLine(submittedQuote.ToString());
            QuoteRec qr = submittedQuote; 
            AddQuote(qr);
            return qr;
        }

        public List<QuoteRec> Post(string RowKey, bool VoteUp)
        {
            List<QuoteRec> QuoteRecs = QuoteDB.GetQuoteList();
            QuoteRec qr = QuoteRecs.Find(q => q.RowKey== RowKey); 
            if ( qr == null )
            {
                return ReturnNotFound(); 
            }
            
            if ( VoteUp )
            {
                qr.VoteUp++; 
            } else
            {
                qr.VoteDown++; 
            }
            List<QuoteRec> quoterecs = new List<QuoteRec>() { qr };

            // WIP update Table Store data, and to be really cool, update it in the background after returning to the browser 

            return quoterecs;  
        }



        private static List<QuoteRec> ReturnNotFound()
        {
            List<QuoteRec> returned = new List<QuoteRec>();
            returned.Add(new QuoteRec{ Author = "", Quote = "No Quote Data Loaded" });
            return returned;
        }


        public List<QuoteRec> Get()
        {
            List<QuoteRec> returned = new List<QuoteRec>();

            List<QuoteRec> QuoteRecs = QuoteDB.GetQuoteList();
            QuoteRec qr = QuoteRecs[random.Next(0, QuoteRecs.Count)];
            returned.Add(qr); 
            return returned;
        }


        private static void AddQuote(QuoteRec qr)
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
