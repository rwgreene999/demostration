using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Information.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Break a IEnumerable<> into chunks
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunksize">number of entries in this chunk of items </param>
        /// <returns></returns>
        /// <example>
        /// List<MyStuff> allTheStuff = GetMyStuff(); 
        /// foreach( var tenStuffs in allTheStuff.Chunk(10) )
        /// {
        ///    Console.Writeline( "Processing another group of 10 stuffs " ); 
        ///    foreach( var stuff in tenStuffs )
        ///    {
        ///        DoSomething( stuff ); 
        ///    }
        /// }
        /// </example>
        /// <note>taken from: http://stackoverflow.com/questions/419019/split-list-into-sublists-with-linq </note>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        public static int TryParse( this string input, int defaultResponse  )
        {
            if (String.IsNullOrEmpty(input)) return defaultResponse;
            int response = defaultResponse;
            if (Int32.TryParse(input, out response) != true) return defaultResponse;
            return response; 
        }

    }
}