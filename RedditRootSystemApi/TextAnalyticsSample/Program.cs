using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace TextAnalyticsSample
{
    /// <summary>
    /// This is a sample program  that shows how to use the Azure ML Text Analytics app (https://datamarket.azure.com/dataset/amla/text-analytics)
    /// </summary>
    class Program
    {
        private const string ServiceBaseUri = "https://api.datamarket.azure.com/";
        static void Main(string[] args)
        {
            args = new List<string>()
            {
                @"De hecho se trata de dos proyectos de ley, una de Emprendedores (que incluye facilidades de financiamiento) y otra de Sociedades de Beneficio e Interés Colectivo , que dan forma al plan Argentina Emprende.
Las iniciativas, en manos de la Secretaría de Emprendedores y PyMEs , dependiente del Ministerio de Producción, están enfocadas en facilitar y allanar la concreción de micro, pequeños y medianos proyectos .
Manuel Tanoira , director de Políticas Públicas de la Asociación de Emprendedores de la Argentina (ASEA), afirmó a Infobae que el proyecto de Ley de Emprendedores establecerá 'la eliminación de las barreras burocráticas al momento de invertir y un par de incentivos innovadores a nivel de Latinoamérica.
Se incluyen los puntos más demandados, como la simplificación de la creación de nuevas empresas y la ampliación de las fuentes de financiamiento , así como también la promoción de incentivos fiscales para los inversores que apuestan por el capital emprendedor'.
Aunque las fuentes oficiales consultadas por Infobae no brindaron detalles al respecto, en el borrador inicial se había evaluado la posibilidad de otorgar beneficios impositivos en Ganancias, IVA y Bienes personales a las denominadas 'Instituciones de Capital Emprendedor' y la opción de 'crowdfunding público' , es decir, el financiamiento colectivo a través de plataformas , con supervisión de la Comisión Nacional de Valores.
'Si estos beneficios son finalmente implementados, se generará un entorno propicio para agilizar la dinámica de creación y crecimiento de los emprendimientos en el país con lo que esto conlleva: generación de más puestos de trabajo y desarrollo económico ', señaló Bearzi a Infobae .
'Por eso vemos un panorama muy alentador para los emprendedores argentinos: a estas iniciativas se suma el apoyo de los actores que integran el ecosistema emprendedor a través de redes de contacto, capacitaciones y mentorías a emprendedores, dentro del marco de colaboración propio de este sector', acotó.
Fuentes de la Secretaría de Emprendedores y PyMEs, a cargo de Mariano Mayer , indicaron a Infobae que ' esta semana o la próxima se está definiendo la fecha para el tratamiento de la iniciativa en el Congreso '.
Tenemos que aprovechar este contexto para multiplicar posibilidades y generar empleo de calidad ', a la vez que apeló a 'todo el potencial de los emprendedores y de las PyMEs para hacer frente a la generación de millones de puestos de trabajo y a los desafíos de crecimiento y cumplir con el objetivo de Pobreza Cero'.
Argentina Emprende se complementa con los beneficios que fijó la Ley PyME , como la oferta de líneas de crédito más ágiles y a tasa preferencial, el pago trimestral del IVA, la eliminación del impuesto a la ganancia mínima presunta y la compensación del impuesto al cheque.",
                "MARCscESFvRBtRv1S5i35h0SVZyGA3cbRCJrZEryPuQ"
            }.ToArray();

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments. This program requires 2 arguments -");
                Console.WriteLine("\t <input text>: string to be analyzed");
                Console.WriteLine("\t <Account key>: your Account key, from https://datamarket.azure.com/account");
                return;
            }

            string inputText = args[0];
            string accountKey = args[1];

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
                Console.WriteLine("Key phrases: " + string.Join(",", keyPhraseResult.KeyPhrases));

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
                Console.WriteLine("Sentiment score: " + sentimentResult.Score);
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
