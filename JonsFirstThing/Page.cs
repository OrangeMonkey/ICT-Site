using DatabaseTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer;

namespace JonsFirstThing
{
    abstract class Page : HTMLServlet
    {
        protected Session Session { get; private set; }

        protected string NavButton(string text, string link = "/404")
        {
            if (Request.Url.LocalPath == link)
            {
                return Tag("div", @class => "current")(
                    text
                );
            }
            else
            {
                return Tag("div", @class => "button")(
                    Tag("a", href => link)(text)
                );
            }
        }

        protected override bool OnPreService()
        {
            var username = Request.Cookies["username"];
            var hash = Request.Cookies["session"];

            if (username != null && hash != null)
            {
                Session = Database.SelectFirst<Session>(x => x.Username == username.Value && x.Hash == hash.Value);
            }
            else
            {
                Session = null;
            }

            return base.OnPreService();
        }

        protected override void OnService()
        {
            //Write("Oogally Boogaly Moogaly");
            Write(
                DocType("html"),
                Tag("html", lang => "en")(
                    Tag("head")(
                        Tag("title")("ICT Support News"),
                        EmptyTag("link", href => "/res/styles.css", rel => "stylesheet")
                    ),
                    Tag("body")(
                        Tag("div", id => "main")(
                            Tag("div", id => "lshadow"),
                            Tag("div", id => "rshadow"),
                            Tag("div", id => "page")(
                                Tag("div", id => "headerrow")(
                                    Tag("div", id => "header")(
                                        Tag("div", id => "title"),
                                        Tag("div", id => "contactinfo")
                                    ),
                                    Tag("div", id => "navbar")(
                                        NavButton("Home", "/"),
                                        NavButton("Guides", "/guides"),
                                        NavButton("Statistics"),
                                        NavButton("Meet the Team"),
                                        NavButton("Newsletter"),
                                        NavButton("Resources"),
                                        NavButton("ICT News"),
                                        NavButton("Archive", "/archive")
                                    )
                                ),
                                Tag("div", id => "innerpage")(Tag("div", @class => "content")(Dyn(Content))),
                                Tag("div", id => "footer")(Tag("div", @class => "content")(Dyn(Footer)))
                            )
                        )
                    )
                )
            );
        }

        protected virtual void Content()
        {
            return;
        }

        protected virtual void Footer()
        {
            return;
        }
    }
}
