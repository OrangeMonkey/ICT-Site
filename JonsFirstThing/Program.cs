using DatabaseTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebServer;

namespace JonsFirstThing
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.AddPrefix("http://+:8080/");
            server.BindServletsInAssembly(Assembly.GetExecutingAssembly());
            DefaultResourceServlet.EnableCaching = true;
#if DEBUG
            DefaultResourceServlet.ResourceDirectory = "../../res";
#else 
            DefaultResourceServlet.ResourceDirectory = "res";
#endif
            Database.ConnectLocal();
            server.Run();
            Database.Disconnect();
        }
    }
}
