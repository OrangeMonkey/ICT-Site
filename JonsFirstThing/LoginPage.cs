using DatabaseTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer;
using System.DirectoryServices.AccountManagement;
using System.Net;

namespace JonsFirstThing
{
    [ServletURL("/adminlogin")]
    class LoginPage : Page
    {
        protected override void Content()
        {
            Write(
                Tag("h1")("Admin Login"),
                Tag("form", action => "/adminlogin/submit", method => "post")(
                    Tag("label", @for => "uName")("Username"), Ln,
                    EmptyTag("input", type => "text", name => "username", id => "uName", maxlength => byte.MaxValue), Ln, Ln,
                    Tag("label", @for => "uPass")("Password"), Ln,
                    EmptyTag("input", type => "password", name => "password", id => "uPass", maxlength => byte.MaxValue), Ln, Ln,
                    EmptyTag("input", type => "submit", value => "Submit")
                )
            );
        }
    }

    [ServletURL("/adminlogin/submit")]
    class AdminLoginSubmitPage : Page
    {
        protected override bool OnPreService()
        {
            var post = Request.GetParsedPost();
            var username = post["username"] ?? "";
            var password = post["password"] ?? "";

            using (var pc = new PrincipalContext(ContextType.Domain, "plume.local"))
            {
                var isvalid = pc.ValidateCredentials(username, password);

                if (isvalid)
                {
                    var user = UserPrincipal.FindByIdentity(pc, username);
                    user.GetAuthorizationGroups();
                    var session = Session.Create(username, Request.RemoteEndPoint.Address);
                    Response.SetCookie(new Cookie("username", username));
                    Response.SetCookie(new Cookie("session", session.Hash));
                    Response.Redirect("/publish");
                    return false;
                }

                Response.Redirect("/adminlogin");
                return false;
            }
        }
    }
}
