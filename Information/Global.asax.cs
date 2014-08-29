using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Information.Utilities;
using Information.Models; 

namespace Information
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801


    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            LoadQuoteFile();
            LoadFullQuotesFromDatabase(); 
        }

        private void LoadQuoteFile()
        {
            string QuoteFile = HttpContext.Current.Server.MapPath("~/App_Data/sig.dat");
            List<string> Quotes = new LoadQuoteData().LoadQuotes(QuoteFile);
            Application["AppQuotes"] = Quotes;
        }

        private void LoadFullQuotesFromDatabase()
        {
            try
            {
                List<QuoteRec> quoteRecs = new List<QuoteRec> { new QuoteRec { Author = "DB", Quote = "loading data " } };
                Application["FullAppQuotes"] = quoteRecs;

                var qtr = QuoteDB.GetQuotesTableReference();

                // Construct the query operation for all entities 
                TableQuery<QuoteTableEntity> query = new TableQuery<QuoteTableEntity>();
                // note: WIP: might need to do this , like this: TableQuery<QuoteTableEntity> query = new TableQuery<QuoteTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

                var results = qtr.ExecuteQuery(query);                
                List<QuoteTableEntity> quoteTableEntitiess = results.ToList();
                quoteRecs = new List<QuoteRec>(); 
                foreach (QuoteTableEntity qte in quoteTableEntitiess)
                {
                    QuoteRec qr = qte.ExtractQuoteRecord();
                    quoteRecs.Add(qr); 
                }
                Application["FullAppQuotes"] = quoteRecs;

            }
            catch( Microsoft.WindowsAzure.Storage.StorageException ex )
            {
                new LogException(ex);
                List<QuoteRec> quoteRecs = new List<QuoteRec> { new QuoteRec { Author = "DB", Quote = "Database error loading data" } };
                Application["FullAppQuotes"] = quoteRecs;
            }
            catch (Exception ex)
            {
                new LogException(ex, true); 
            }
            finally
            {
            }

        }
    }
}