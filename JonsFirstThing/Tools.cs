using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JonsFirstThing
{
    static class Tools
    {
        public static T Value<T>(this XElement element, String name)
        {
            try {
                return (T) Convert.ChangeType(element.Element(name).Value, typeof(T));
            } catch {
                return default(T);
            }
        }

        public static T Value<T>(this XElement element, String name, IFormatProvider provider)
        {
            try {
                return (T) Convert.ChangeType(element.Element(name).Value, typeof(T), provider);
            } catch {
                return default(T);
            }
        }
    }
}
