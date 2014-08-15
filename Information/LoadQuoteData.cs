using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Text; 


namespace Information
{
    public class LoadQuoteData
    {
        public List<Exception> Exceptions { get; private set; }
        public List<string> LoadQuotes(string Filename)
        {
            List<string> results = new List<string>();
            try
            {
                string[] data = File.ReadAllLines(Filename);
                StringBuilder AQuote = new StringBuilder();
                foreach (string s in data)
                {
                    if (s.Trim().Length < 1)
                    {
                        if (AQuote.Length > 0)
                        {
                            results.Add(AQuote.ToString());
                            AQuote = new StringBuilder();
                        }
                    }
                    else
                    {
                        AQuote.Append(s.Trim().Replace((char)0x93, (char)0x27).Replace((char)0x94, (char)0x27));
                        AQuote.Append("<br>");
                    }
                }
            }
            catch (Exception ex)
            {                
                System.Diagnostics.Debug.WriteLine("\r\n\r\nLoadQuotes failure:  \r\n" );
                while (ex != null)
                {
                    Exceptions.Add(ex); 
                    System.Diagnostics.Debug.WriteLine( ex.ToString() + "\r\n============================================================================");
                    ex = ex.InnerException; 
                }
                
                throw;
            }
            return results;
        }
    }
}