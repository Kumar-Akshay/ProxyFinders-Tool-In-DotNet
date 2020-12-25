using HtmlAgilityPack;
using Leaf.xNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FindProxyTool
{
    public class ProxyFinder
    {
        // Loading the source links
        List<string> ProxySource = new List<string>()
            {
                "http://free-proxy-list.net/anonymous-proxy.html",
                "http://proxydb.net/",
                "https://www.us-proxy.org/",
                //"http://www.proxylists.net/http_highanon.txt",
                //"https://www.duplichecker.com/free-proxy-list.php",
                //"https://free-proxy-list.net",
                //"https://www.sslproxies.org",
                //"http://www.proxylists.net/http_highanon.txt",
                //"http://rootjazz.com/proxies/proxies.txt",
                //"http://www.proxyserverlist24.top/2020/05/26-05-20-free-proxy-server-list-2864.html",
                //"http://proxyserverlist-24.blogspot.com/feeds/posts/default",
                //"https://smallseotools.com/free-proxy-list/"
            };

        public Regex REGEX = new Regex(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\:[0-9]{1,5}\b");
        List<string> scraped = new List<string>();
        List<string> checkedProxy = new List<string>();
        string CurrentlyScrapingPage;

        public ProxyFinder() { }


        public void FindProxyTool()
        {
            int goodProxy = 0;
            Console.WriteLine("\n\n Please wait for Scraping the Proxies");
            int Totalproxy = ScrapeProxy();
            var proxies = File.ReadLines("../../../ScrapedProxies.txt");

            Console.WriteLine("\nEnter the Proxy Timeout in milliseconds (like 1000, 2000) : ");
            Console.Write("\nTimeout : ");
            var proxyTimeout = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nEnter the Number of Threads : ");
            Console.Write("\nThreads : ");
            var Threads = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nSelect Type of Proxy: ");
            Console.WriteLine("1. HTTP ProxyType");
            Console.WriteLine("2. SOCK4 ProxyType");
            Console.WriteLine("3. SOCK5 ProxyType\n");
            Console.WriteLine("Select your choice: ");
            var choices = Console.ReadLine();
            Console.WriteLine("\n Please wait we are checking the proxies, save into the file \n");
            int choice = Convert.ToInt32(choices);
            ProxyType proxyType;
            switch (choice)
            {
                case 1:
                    proxyType = ProxyType.HTTP;
                    goodProxy = ProxyChecker(proxyType, proxies, proxyTimeout, Threads);
                    Console.WriteLine("\n Total Number of Proxies: " + Totalproxy);
                    Console.WriteLine("\n Total Number of Good Proxy: " + goodProxy);
                    break;
                case 2:
                    proxyType = ProxyType.Socks4;
                    goodProxy = ProxyChecker(proxyType, proxies, proxyTimeout, Threads);
                    Console.WriteLine("\n Total Number of Proxies: " + Totalproxy);
                    Console.WriteLine("\n Total Number of Good Proxy: " + goodProxy);
                    break;
                case 3:
                    proxyType = ProxyType.Socks5;
                    goodProxy = ProxyChecker(proxyType, proxies, proxyTimeout, Threads);
                    Console.WriteLine("\n Total Number of Proxies: " + Totalproxy);
                    Console.WriteLine("\n Total Number of Good Proxy: " + goodProxy);
                    break;

                default:
                    break;
            }
        }

        public int ScrapeProxy()
        {
            File.Delete("../../../ScrapedProxies.txt");
            System.Net.WebClient _WC = new System.Net.WebClient();
            try
            {
                foreach (string PageToScrape in ProxySource)
                {
                    Console.WriteLine("[*] Scraping the " + PageToScrape);
                    string PageSource = _WC.DownloadString(PageToScrape);
                    CurrentlyScrapingPage = PageToScrape;

                    MatchCollection _MC = REGEX.Matches(PageSource);
                    foreach (Match Match in _MC)
                    {
                        scraped.Add(Match.ToString());
                        //Console.WriteLine(Match.ToString());
                        File.AppendAllText("../../../ScrapedProxies.txt", Match.ToString() + Environment.NewLine);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[*]" + "Bad Request  " + CurrentlyScrapingPage);
            }
            SaveFile(scraped, "../../../ScrapedProxies.txt");
            Console.WriteLine("Total Number of Proxies Scraped Found : " + scraped.Count);
            return scraped.Count;
        }


        public int ProxyChecker(ProxyType proxyType, IEnumerable<String> proxies, int proxyTimeout, int threads)
        {
            int correctProxy = 0;
            var requestTimeout = 5 * 1000;
            File.Delete("../../../GoodProxy.txt");
            Parallel.ForEach(proxies, new ParallelOptions()
            {
                MaxDegreeOfParallelism = threads
            }, proxy =>
            {
                try
                {
                    using (var request = new HttpRequest())
                    {
                        request.ConnectTimeout = requestTimeout;
                        request.Proxy = ProxyClient.Parse(proxyType, proxy);
                        request.Proxy.ConnectTimeout = proxyTimeout;
                        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
                        request.Get("http://google.com");
                        Console.WriteLine("[Good Proxy] " + proxy);
                        checkedProxy.Add(proxy);
                        correctProxy = correctProxy + 1;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("[Bad Proxy] " + proxy);
                }
            });
            SaveFile(checkedProxy, "../../../GoodProxy.txt");
            return correctProxy;
        }

        public void SaveFile(List<string> ProxyList, string path)
        {
            foreach (var proxy in ProxyList)
            {
                File.AppendAllText(path, proxy + Environment.NewLine);
            }
        }
    }
}
