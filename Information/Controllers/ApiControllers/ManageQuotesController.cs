using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Information.Models;

using System.Web.Http;
using System.Web;
using System.Net;
using System.Net.Http;
using Information.Utilities;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Table.Protocol;


namespace Information.Controllers.ApiControllers
{
    public class ManageQuotesController : ApiController
    {
        //
        // GET: /ManageQuotes/

        public HttpResponseMessage Index()
        {
            var rsp = "WIP: Not Implemented Yet";
            return Request.CreateResponse(HttpStatusCode.InternalServerError, rsp ); 
        }

        public HttpResponseMessage Get(string filter)
        {
            if (String.IsNullOrEmpty(filter))
            {
                filter = "";
            }
            var rsp = new QuoteRec { Author = "WIP", Quote = "WIP" };
            return Request.CreateResponse(HttpStatusCode.OK, rsp);
        }

        public HttpResponseMessage Get()
        {
            var rsp = new QuoteRec { Author = "WIP", Quote = "WIP" };
            return Request.CreateResponse(HttpStatusCode.OK, rsp);
        }

        public HttpResponseMessage Delete()
        {
            var code = HttpStatusCode.InternalServerError;
            var rsp = ""; 
            try
            {
                CloudTable tableReference = QuoteDB.GetQuotesTableReference();
                tableReference.DeleteIfExists();
                code = HttpStatusCode.OK; 
            }
            catch (Exception ex)
            {
                new LogException(ex);
                rsp = ex.ToString(); 
            }
            finally
            {
            }
            return Request.CreateResponse(code, rsp);  
        }

        public HttpResponseMessage Post(QuoteRec submittedQuote)
        {
            var rsp = "WIP: Not Implemented Yet";
            return Request.CreateResponse(HttpStatusCode.InternalServerError, rsp);
        }

        public HttpResponseMessage Put(QuoteRec submittedQuote)
        {
            var rsp = "Failed";
            var code = HttpStatusCode.InternalServerError;
            try
            {
                RecreateTableStorage();
                rsp = "Worked";
                code = HttpStatusCode.OK; 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                rsp = ex.ToString(); 
            }

            return Request.CreateResponse(code, rsp); 
        }

        private bool RecreateTableStorage()
        {
            CloudTable tableReference = QuoteDB.GetQuotesTableReference();
            DateTime dtTimeout = DateTime.Now.AddSeconds(20); 
            
            try
            {
                tableReference.Create();
            }
            catch ( StorageException ex)
            {
                if (  (ex.RequestInformation.HttpStatusCode == 409) 
                   && (ex.RequestInformation.ExtendedErrorInformation.ErrorCode.Equals(TableErrorCodeStrings.TableBeingDeleted))
                    && (DateTime.Now < dtTimeout )
                   )
                {
                    Thread.Sleep(2000);// The table is currently being deleted. Try again until it works.
                } else{
                    new LogException(ex); 
                    return false; 
                }
            }
            catch (Exception ex)
            {
                new LogException(ex); 
                return false; 
            }

            return AddQuotesToNewTable();
        }

        private bool AddQuotesToNewTable()
        {
            bool Worked = false; 
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); 
            sw.Start();
            try
            {

                CloudTable tableReference = QuoteDB.GetQuotesTableReference();

                // 
                // this process of adding batchs of 90 came from an error 
                // when I created all 220 entries in one batch 
                // error received: "Unexpected response code for operation : 99"
                // solution option: http://stackoverflow.com/questions/18170920/azure-table-storage-error-unexpected-response-code-for-operation-99
                // 

                var quoteList = BasicQuotes.GetQuoteList();
                int CntQuote = 0; 
                foreach (var chunk in quoteList.Chunk(90))
                {
                    TableBatchOperation batchOperation = new TableBatchOperation();

                    foreach (var quote in chunk)
                    {
                        var thisQuote = BasicQuotes.BuildQuote(quote);
                        QuoteRec q = new QuoteRec { Author = thisQuote.Author, Quote = thisQuote.Quote, ModeratorApproved = true, QuoteSubmitter = Guid.Empty };
                        QuoteTableEntity qte = new QuoteTableEntity { PartitionKey = "quoter", RowKey = Guid.NewGuid().ToString("N"), idxQuoteAsLoaded=++CntQuote };
                        qte.StoreQuoteRecord(q); 
                        batchOperation.Insert(qte);
                    }
                    tableReference.ExecuteBatch(batchOperation);
                }
                Worked = true; 
            }
            catch (Exception ex)
            {
                new LogException(ex); 
            }
            finally
            {
                sw.Stop();
                System.Diagnostics.Debug.WriteLine(">>>> Table Load times:" + sw.Elapsed.ToString());
            }
            return Worked; 
        }


        public HttpResponseMessage Patch(string filter)
        {
            var rsp = "WIP: Not Implemented Yet";
            return Request.CreateResponse(HttpStatusCode.InternalServerError, rsp);
        }

    }

}
