using System;
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
        protected override void Content()
        {
            var articles = Database.SelectAll<Article>();
            foreach (var article in articles)
            {
                Write(

            }
        }
    }
}
