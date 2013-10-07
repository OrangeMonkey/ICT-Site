﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer;
using DatabaseTools;

namespace JonsFirstThing
{
    [ServletURL("/")]
    class HomePage : Page
    {
        private static RSSFeed _testFeed = new RSSFeed("TechRadar",
            "http://feeds.feedburner.com/techradar/computing-news?format=xml",
            "http://www.techradar.com/");

        private static int IndexOfOrLength(String str, String value)
        {
            int index = str.IndexOf(value);
            if (index == -1) return str.Length;
            return index;
        }

        private static String GetFirstParagraph(String content)
        {
            int end = IndexOfOrLength(content, "\n\n");
            end = Math.Min(end, IndexOfOrLength(content, "\r\n\r\n"));
            return content.Substring(0, end);
        }

        protected override void Content()
        {
            if (_testFeed.LastFetch.AddMinutes(1) < DateTime.Now) {
                _testFeed.Fetch();
            }

            var articles = Database.SelectAll<Article>().OrderByDescending(x => x.Posted).Take(4);
            foreach (var article in articles)
            {
                Write(
                    Tag("h2")(article.Title),
                    Tag("p")(
                        Dyn(() => {
                            var firstParagraph = GetFirstParagraph(article.Content);
                            Write(firstParagraph);

                            if (firstParagraph.Length < article.Content.Length) {
                                Write(Ln, Ln, Tag("a", href => Format("/archive?id={0}", article.ID))("Read&nbsp;More&nbsp;>>"));
                            }
                        })
                    ),
                    Dyn(() => {
                        if (article != articles.Last()) {
                            Write(Tag("div", @class => "hrule"));
                        }
                    })
                );
            }
        }

        protected override void Footer()
        {
            foreach (var item in _testFeed.Recent) {
                Write(Tag("a", href => item.Link, title => item.Title)(item));
                if (item != _testFeed.Recent.Last()) {
                    Write(Ln);
                }
            }
        }
    }
}
