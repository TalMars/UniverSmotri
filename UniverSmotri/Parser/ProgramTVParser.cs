using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniverSmotri.Model;

namespace UniverSmotri.Parser
{
    public class ProgramTVParser
    {

        public ProgramTVParser()
        {

        }

        public static async Task<List<ProgramTVModel>> Parse(string htmlUrl)
        {
            TaskCompletionSource<List<ProgramTVModel>> tcs = new TaskCompletionSource<List<ProgramTVModel>>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(htmlUrl);
                string url = await response.Content.ReadAsStringAsync();

                try
                {
                    List<ProgramTVModel> list = new List<ProgramTVModel>();
                    HtmlDocument hd = new HtmlDocument();
                    hd.LoadHtml(url);

                    string programTV_1 = hd.DocumentNode.InnerText;
                    string[] separator = new string[] { "\n" };
                    string[] els = programTV_1.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in els)
                    {
                        ProgramTVModel pm = new ProgramTVModel();
                        pm.Time = s.Substring(0, 5);
                        pm.Description = s.Substring(6, s.Length - 6).Replace("&quot;", "\"").Replace("&nbsp;", "").Replace("&laquo;", "\"").Replace("&raquo;", "\"").Replace("&ndash;", "–");
                        list.Add(pm);
                    }

                    tcs.SetResult(list);
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
