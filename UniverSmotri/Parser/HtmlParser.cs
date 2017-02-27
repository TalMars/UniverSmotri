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
    public class HtmlParser
    {

        public HtmlParser()
        {

        }

        public static async Task<List<NewsModel>> Parse(string htmlUrl)
        {
            TaskCompletionSource<List<NewsModel>> tcs = new TaskCompletionSource<List<NewsModel>>();

            List<NewsModel> listNews = new List<NewsModel>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(htmlUrl);
                string html = await response.Content.ReadAsStringAsync();
                try
                {
                    HtmlDocument hd = new HtmlDocument();
                    hd.LoadHtml(html);

                    var nodeCollection = hd.DocumentNode.Descendants("div").Where(d =>
                        d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("catItemView"));

                    foreach (HtmlNode node in nodeCollection)
                    {
                        NewsModel newsElement = new NewsModel();

                        HtmlNode itemHeader = node.Descendants("div").Where(d =>
                            d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("catItemHeader")).ToList()[0];
                        newsElement.NewsHeader = itemHeader.Element("h3").Element("a").InnerText.Trim().Replace("&quot;", "\"").Replace("&nbsp;", "").Replace("&laquo;", "\"").Replace("&raquo;", "\"").Replace("&ndash;", "–");

                        string dateNews = "";
                        if(itemHeader.Element("span") != null)
                            dateNews = itemHeader.Element("span").InnerText.Trim().Substring(dateNews.IndexOf(' ') + 1);
                        newsElement.DateNews = dateNews;

                        newsElement.Href = "http://tv.kpfu.ru" + itemHeader.Element("h3").Element("a").Attributes["href"].Value.ToString();

                        HtmlNode itemImage = node.Descendants("span").Where(d =>
                            d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("catItemImage")).ToList()[0];
                        newsElement.Image = "http://tv.kpfu.ru" + itemImage.Element("a").Element("img").Attributes["src"].Value.ToString();

                        listNews.Add(newsElement);
                    }
                    tcs.SetResult(listNews);
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
