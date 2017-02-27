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
    public class NewsElementParser
    {
        public NewsElementParser()
        {

        }

        public static async Task<OneNewsModel> Parse(string htmlUrl)
        {
            TaskCompletionSource<OneNewsModel> tcs = new TaskCompletionSource<OneNewsModel>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(htmlUrl);
                string url = await response.Content.ReadAsStringAsync();
                try
                {
                    OneNewsModel news = new OneNewsModel();
                    HtmlDocument hd = new HtmlDocument();
                    hd.LoadHtml(url);
                    HtmlNode itemDecription = hd.DocumentNode.Descendants("div").Where(d =>
                            d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("itemFullText")).ToList()[0];
                    string descr = itemDecription.InnerText.Trim().Replace("<p>", "").Replace("</p>", "").Replace("\r", "");
                    string[] els = descr.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    descr = "";
                    foreach (string s in els)
                    {
                        descr = descr + s.Trim().Replace("&quot;", "\"").Replace("&nbsp;", "")
                            .Replace("&laquo;", "\"").Replace("&raquo;", "\"").Replace("&ndash;", "–").Replace("&mdash;", "—") + "\n";
                    }
                    news.Description = descr;

                    HtmlNode itemVideo = hd.DocumentNode.Descendants("div").Where(d =>
                            d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("avPlayerBlock")).ToList()[0];
                    string videoId = itemVideo.Element("iframe").Attributes["src"].Value.ToString();
                    string temp = videoId.Substring(0, videoId.IndexOf('?'));
                    news.YouTubeID = temp.Substring(29, temp.Length - 29);

                    tcs.SetResult(news);
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
