using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UniverSmotri.Parser
{
    public class RadioUrlParser
    {
        private static string mainUfmUrl = "http://ufmradio.ru/";
        public static async Task<string> Parse()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(mainUfmUrl);
                string html = await response.Content.ReadAsStringAsync();
                try
                {
                    HtmlDocument hd = new HtmlDocument();
                    hd.LoadHtml(html);

                    string urlOnRadio = hd.GetElementbyId("u253").Element("audio").Attributes["src"].Value.ToString();
                    tcs.SetResult(urlOnRadio);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
            return tcs.Task.Result;
        }
    }
}
