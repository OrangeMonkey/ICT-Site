using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer;
using System.IO;
using System.Collections.Specialized;
using DatabaseTools;

namespace JonsFirstThing
{
    [ServletURL("/publish")]
    class PublishArticlePage : Page
    {
        private static readonly Dictionary<string, string> _sAuthors = new Dictionary<string,string> {
            {"admlkn", "L. Kirkland"},
            {"admnhp", "N. Humphris"},
            {"admsos", "S. O'Sullivan"},
            {"admjml", "J. Morrell"}
        };

        protected override bool OnPreService()
        {
            base.OnPreService();
            if (Session == null)
            {
                Server.CreateNotFoundServlet().Service(Request, Response);
                return false;
            }
            return true;
        }

        protected override void Content()
        {
            Write(
                Tag("h1")("New Article"),
                Tag("form", action => "/publish/submit", method => "post")(
                    Tag("label", @for => "aTitle")("Article Title"), Ln,
                    EmptyTag("input", type => "text", name => "title", id => "aTitle", maxlength => byte.MaxValue), Ln, Ln,
                    Tag("label", @for => "aContent")("Article Content"), Ln,
                    Tag("textarea", name => "content", id => "aContent")(), Ln, Ln,
                    Tag("select", name => "author")(
                        _sAuthors.Select(x => Tag("option", value => x.Key)(x.Value)).ToArray()
                    ),
                    EmptyTag("input", type => "submit", value => "Submit")
                )
            );

        }
    }

    [ServletURL("/publish/submit")]
    class PublishArticleSubmitPage : Page
    {
        protected override bool OnPreService()
        {
            var post = Request.GetParsedPost();
            var article = new Article
            {
                Title = post["title"],
                Content = post["content"], 
                Author = post["author"],
                Posted = DateTime.Now
            };
            Database.Insert(article);
            Response.Redirect("/");
            return base.OnPreService(); 
        }
    }
}
