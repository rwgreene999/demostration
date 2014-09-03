using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using Information.Models; 


namespace Information.Utilities
{
    public class QuoteDB
    {
        public static CloudTable GetQuotesTableReference()
        {
            var conn = ConfigurationManager.AppSettings["StorageConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conn);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable tableReference = tableClient.GetTableReference("quotes");
            return tableReference;
        }



        public static List<QuoteRec> GetQuoteList()
        {
            var app = HttpContext.Current.Application;
            List<QuoteRec> Quotes = (List<QuoteRec>)app["QuoteDBDataCache"];
            // WIP: if null, the load the data instead of expecting Global.asax to always have this loaded 
            return Quotes;
        }


    }
}