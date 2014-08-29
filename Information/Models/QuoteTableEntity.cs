using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json; 

namespace Information.Models
{
    internal class QuoteTableEntity : TableEntity
    {
        public QuoteTableEntity(string partition, string guid)
        {
            this.PartitionKey = partition;
            this.RowKey = guid;
        }
        public QuoteTableEntity()
        {

        }
        public int idxQuoteAsLoaded { get; set; }
        public string ActualQuoteRec { get; set; }
        public QuoteRec ExtractQuoteRecord()
        {
            var qr = JsonConvert.DeserializeObject<QuoteRec>(ActualQuoteRec);
            return qr; 
        }
        public void StoreQuoteRecord(QuoteRec q)
        {
            this.ActualQuoteRec = JsonConvert.SerializeObject(q); 
        }

    }







}