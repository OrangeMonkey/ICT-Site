using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace JonsFirstThing
{
    public abstract class Fetchable
    {
        protected static String BuildURL(String baseURL, params Object[] pairs)
        {
            var sb = new StringBuilder();
            sb.Append(baseURL);
            if (pairs.Length > 0) {
                for (int i = 0; i < pairs.Length; i += 2) {
                    sb.Append(i == 0 ? "?" : "&");
                    sb.AppendFormat("{0}={1}", pairs[i], pairs[i + 1]);
                }
            }
            return sb.ToString();
        }

        public String URL { get; protected set; }
        public bool IsLoaded { get; private set; }

        public Fetchable()
        {
            URL = null;
            IsLoaded = false;
        }

        public Fetchable(String url)
        {
            URL = url;
            IsLoaded = false;
        }

        protected virtual bool OnPreFetch()
        {
            return true;
        }

        protected virtual void OnFetched(XDocument doc)
        {
            return;
        }

        public bool Fetch()
        {
            try {
                if (!OnPreFetch()) return true;
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }

            if (URL == null) {
                throw new InvalidOperationException("Cannot fetch; no URL given");
            }

            WebResponse response = null;

            var request = WebRequest.Create(URL);
            try {
                response = request.GetResponse();
                var stream = response.GetResponseStream();
                var text = new StreamReader(stream).ReadToEnd();
                var document = XDocument.Parse(text);

                OnFetched(document);

                return IsLoaded = true;
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            } finally {
                if (response != null) {
                    response.Close();
                }
            }
        }
    }
}
