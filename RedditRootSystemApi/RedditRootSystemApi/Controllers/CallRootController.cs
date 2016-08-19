using RedditRootSystemApi.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RedditRootSystemApi.Controllers
{
    public class CallRootController : ApiController
    {
        [HttpGet]
        [ActionName("TextAnalyticsOnDeman")]
        public IEnumerable<string> TextAnalyticsOnDeman(string txt)
        {          
            SentimentResult sResult = new SentimentResult();
            KeyPhraseResult kPResult = new KeyPhraseResult();

            new TextAnalytics.TextAnalyticsLogic().CallTextAnalitucs(txt, out sResult, out kPResult);

            //return Request.CreateResponse(HttpStatusCode.OK, new object[] { sResult, kPResult });
            return new string[] { sResult.Score.ToString(), string.Join(" , ", kPResult.KeyPhrases) };
        }

        
    }
}