using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseTools;
using WebServer;

namespace JonsFirstThing
{
    [ServletURL("/archive")]
    class ArticlePage : Page
    {
        private Article _article;

        protected override bool OnPreService()
        {
            base.OnPreService();

            _article = null;

            int id;
            if (!int.TryParse(Request.QueryString["id"] ?? "", out id)) {
                Response.Redirect("/");
                return true;
            }

            _article = Database.SelectFirst<Article>(x => x.ID == id);
            if (_article == null) {
                Server.CreateNotFoundServlet().Service(Request, Response);
                return false;
            }

            return true;
        }

        protected override void Content()
        {
            if (_article == null) return;

            Write(
                Tag("h1")(_article.Title),
                Tag("p")(_article.Content),
                Tag("span", @class => "postinfo")(Format("Posted {0} by {1}", _article.Posted, _article.Author))
            );
        }
    }
}
