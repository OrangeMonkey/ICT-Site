using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DatabaseTools;

namespace JonsFirstThing
{
    class RSSArticle
    {
        public const int MaxTitleCharacters = 40;

        private RSSFeed _feed;

        public String Title { get; private set; }
        public String Link { get; private set; }
        public String Description { get; private set; }
        public String[] Categories { get; private set; }
        public DateTime PubDate { get; private set; }

        public RSSArticle(RSSFeed feed, XElement elem)
        {
            _feed = feed;
            
            Title = elem.Value<String>("title") ?? "Untitled";
            Link = elem.Value<String>("link") ?? "#";
            Description = elem.Value<String>("description") ?? "";
            Categories = (elem.Value<String>("categories") ?? "")
                .Split(',').Select(x => x.Trim().ToLower()).ToArray();
            PubDate = DateTime.Parse(elem.Value<String>("pubDate"));
        }

        public String TruncatedTitle
        {
            get
            {
                if (Title.Length <= MaxTitleCharacters - 3) {
                    return Title;
                } else {
                    int end = Title.Substring(0, MaxTitleCharacters - 3).LastIndexOf(' ');
                    if (end == -1) end = MaxTitleCharacters - 3;
                    return String.Format("{0}...", Title.Substring(0, end));
                }
            }
        }

        public override string ToString()
        {
            return String.Format("[{1}] {0}", TruncatedTitle, _feed.Name, PubDate.ToShortDateString());
        }
    }

    [DatabaseEntity]
    class RSSFeed : Fetchable
    {
        public const int ArticlesPerFeed = 4;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public DateTime AddDate { get; set; }

        [NotNull]
        public DateTime LastFetch { get; set; }

        [Capacity(byte.MaxValue)]
        public String Name { get; set; }

        [Capacity(byte.MaxValue)]
        public String RSSUrl { get; set; }

        [Capacity(byte.MaxValue)]
        public String WebUrl { get; set; }

        public IEnumerable<RSSArticle> Recent { get; private set; }

        public RSSFeed() { }

        public RSSFeed(String name, String rssUrl, String webUrl)
        {
            AddDate = DateTime.Now;
            LastFetch = DateTime.MinValue;

            Name = name;
            RSSUrl = rssUrl;
            WebUrl = webUrl;
        }

        protected override bool OnPreFetch()
        {
            URL = RSSUrl;
            return true;
        }

        protected override void OnFetched(XDocument doc)
        {
            var channels = doc.Root.Elements("channel").ToArray();
            var items = channels.SelectMany(x => x.Elements("item")).ToArray();
            
            Recent = items
                .Select(x => new RSSArticle(this, x))
                .OrderByDescending(x => x.PubDate)
                .Take(ArticlesPerFeed)
                .ToArray();

            LastFetch = DateTime.Now;
        }
    }
}
