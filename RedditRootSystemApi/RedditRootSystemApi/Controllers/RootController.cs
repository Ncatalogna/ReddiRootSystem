using RedditRootSystemApi.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RedditRootSystemApi.Controllers
{
    public class RootController1 : ApiController
    {

        public HttpResponseMessage TextAnalyticsOnDeman(string txt)
        {
            SentimentResult sResult = new SentimentResult();
            KeyPhraseResult kPResult = new KeyPhraseResult();

            new TextAnalytics.TextAnalyticsLogic().CallTextAnalitucs(txt, out sResult, out kPResult);

            return Request.CreateResponse(HttpStatusCode.OK, new object[] { sResult, kPResult });
            //return new object[] { sResult, kPResult };
        }

        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}