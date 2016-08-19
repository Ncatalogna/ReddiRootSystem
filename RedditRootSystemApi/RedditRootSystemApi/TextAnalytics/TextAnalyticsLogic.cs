using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RedditRootSystemApi.TextAnalytics
{
    public class TextAnalyticsLogic
    {
        private const string ServiceBaseUri = "https://api.datamarket.azure.com/";
        public const string accountKey = "MARCscESFvRBtRv1S5i35h0SVZyGA3cbRCJrZEryPuQ";
        public void CallTextAnalitucs(string inputText,out SentimentResult sResult,out KeyPhraseResult kPResult)
        {
            sResult = new SentimentResult();
            kPResult = new KeyPhraseResult();
            //string inputText = args[0];
            //string accountKey = args[1];

            using (var httpClient = new HttpClient())
            {
                string inputTextEncoded = HttpUtility.UrlEncode(inputText);

                httpClient.BaseAddress = new Uri(ServiceBaseUri);
                string creds = "AccountKey:" + accountKey;
                string authorizationHeader = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(creds));
                httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // get key phrases
                string keyPhrasesRequest = "data.ashx/amla/text-analytics/v1/GetKeyPhrases?Text=" + inputTextEncoded;
                Task<HttpResponseMessage> responseTask = httpClient.GetAsync(keyPhrasesRequest);
                responseTask.Wait();
                HttpResponseMessage response = responseTask.Result;
                Task<string> contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();
                string content = contentTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Call to get key phrases failed with HTTP status code: " +
                                        response.StatusCode + " and contents: " + content);
                }

                KeyPhraseResult keyPhraseResult = JsonConvert.DeserializeObject<KeyPhraseResult>(content);
                kPResult = keyPhraseResult;
                //Console.WriteLine("Key phrases: " + string.Join(",", keyPhraseResult.KeyPhrases));

                // get sentiment
                string sentimentRequest = "data.ashx/amla/text-analytics/v1/GetSentiment?Text=" + inputTextEncoded;
                responseTask = httpClient.GetAsync(sentimentRequest);
                responseTask.Wait();
                response = responseTask.Result;
                contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();
                content = contentTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Call to get sentiment failed with HTTP status code: " +
                                        response.StatusCode + " and contents: " + content);
                }

                SentimentResult sentimentResult = JsonConvert.DeserializeObject<SentimentResult>(content);

                sResult = sentimentResult;
                //Console.WriteLine("Sentiment score: " + sentimentResult.Score);
            }
        }        
    }

    /// <summary>
    /// Class to hold result of Key Phrases call
    /// </summary>
    public class KeyPhraseResult
    {
        public List<string> KeyPhrases { get; set; }
    }

    /// <summary>
    /// Class to hold result of Sentiment call
    /// </summary>
    public class SentimentResult
    {
        public double Score { get; set; }
    }
}