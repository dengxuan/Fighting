using Dapper;
using Dapper.Contrib.Extensions;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Pomelo.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.Spider.BjPks
{
    public class Urls
    {
        public string Source { get; set; }

        public string Target { get; set; }
    }

    class Program
    {
        private static DbConnection _coreConnection;

        private static DbConnection _tvsConnection;

        private static Urls _urls;

        static Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _coreConnection = new MySqlConnection(builder.GetConnectionString("bb_core"));
            _tvsConnection = new MySqlConnection(builder.GetConnectionString("bb_tvs"));

            _urls = builder.GetValue<Urls>("Urls");
        }

        static async Task Main(string[] args)
        {
            DateTime executeTime = DateTime.Now;
            while (true)
            {
                while (executeTime < DateTime.Now)
                {
                    var issueNumber = _coreConnection.ExecuteScalar<int>("select `issue` from bj_pks_base order by `issue` desc limit 1;");
                    var nextissue = _tvsConnection.QuerySingle("select `issue`, `draw_time` from `bj_pks_issue` where `status` = 1 and `issue` > @issue order by `issue` limit 1;", new { issue = issueNumber });
                    executeTime = nextissue.draw_time;
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage message = await httpClient.GetAsync(string.Format(_urls.Source, nextissue.issue));
                    if (message.IsSuccessStatusCode)
                    {
                        string html = await message.Content.ReadAsStringAsync();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);
                        HtmlNode node = doc.DocumentNode.SelectNodes("//table[@class='tb']/tr").Skip(1).FirstOrDefault();
                        if (node != null && nextissue.issue.ToString() == node.ChildNodes[1].InnerText.Trim())
                        {
                            var drawNumbers = node.ChildNodes[3].InnerText.Split(",");
                            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                            {
                                { "BjPksBase[issue]", nextissue.issue.ToString() },
                                { "BjPksBase[time]", DateTime.Now.ToString() },
                                { "BjPksBase[ball1]", drawNumbers[0] },
                                { "BjPksBase[ball2]", drawNumbers[1] },
                                { "BjPksBase[ball3]", drawNumbers[2] },
                                { "BjPksBase[ball4]", drawNumbers[3] },
                                { "BjPksBase[ball5]", drawNumbers[4] },
                                { "BjPksBase[ball6]", drawNumbers[5] },
                                { "BjPksBase[ball7]", drawNumbers[6] },
                                { "BjPksBase[ball8]", drawNumbers[7] },
                                { "BjPksBase[ball9]", drawNumbers[8] },
                                { "BjPksBase[ball10]", drawNumbers[9] }
                           });
                            await httpClient.PostAsync(_urls.Target, content);
                        }
                        else
                        {
                            Console.WriteLine($"还未开奖：{nextissue.issue} {nextissue.draw_time}");
                        }
                    }
                    Thread.Sleep(10000);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
