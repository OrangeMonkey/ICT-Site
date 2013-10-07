using DatabaseTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer;

namespace JonsFirstThing
{
    [ServletURL("/restart")]
    class RestartServlet : Servlet 
    {

        protected override void OnService()
        {
            Server.AddScheduledJob("restart", DateTime.Now.AddSeconds(1), TimeSpan.Zero, server =>
            {
                Database.Disconnect();
                Environment.Exit(0);
            });
        }
    }
}
