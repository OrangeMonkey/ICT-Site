using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseTools;

namespace JonsFirstThing
{
    [DatabaseEntity]
    class Article 
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Capacity(byte.MaxValue), NotNull]
        public string Title { get; set; }

        [Capacity(short.MaxValue), NotNull]
        public string Content { get; set; }

        [NotNull]
        public DateTime Posted { get; set; }

        [Capacity(byte.MaxValue), NotNull]
        public string Author { get; set; }
    }
}
