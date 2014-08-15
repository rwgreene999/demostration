using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Http;
using Information.Models; 

namespace Information.Controllers.ApiControllers
{
    public class Test1Controller : ApiController
    {
        //
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/s
        public string Get(string id)
        {
            return "value";
        }

        // GET api/values/5
        public string Get(int idint)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            string x = "Post WIP";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            string x = "Put WIP";
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

    }
}
