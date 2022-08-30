using Microsoft.AspNetCore.Mvc;
using RSSReader.Models;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;

namespace RSSReader.Controllers
{
	public class ItemsController : Controller
	{
        public IActionResult Index()
		{
            return View();
		}

        [HttpPost]
        public ActionResult Index(string RssUrl)
        {
            HttpClient hclient = new HttpClient();
            string RSSData = hclient.GetStringAsync(RssUrl).Result;

            XDocument xml = XDocument.Parse(RSSData);
                var RSSFeedData = (from i in xml.Descendants("item") select new Item
                                   {
                                       Title = (string)i.Element("title"),
                                       Url = (string)i.Element("link"),
                                       Description = Regex.Replace((string)i.Element("description").Value, 
                                       @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;", string.Empty).Trim(),
                                       PostDate = (DateTime)i.Element("pubDate")
                                   });
            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.URL = RssUrl;
            return View();
        }
    }
}
