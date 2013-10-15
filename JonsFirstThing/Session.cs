using DatabaseTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace JonsFirstThing
{
    [DatabaseEntity]
    class Session
    {
        private static Random _sRand = new Random();
        public static Session Create(string username, IPAddress ip)
        {
            var session = Database.SelectFirst<Session>(x => x.Username == username) ?? new Session { Username = username };

            session.LoginTime = DateTime.Now;
            session.IPString = ip.ToString();
            session.Hash = GenerateHash();

            if (session.ID == 0)
            {
                Database.Insert(session);
            }
            else
            {
                Database.Update(session);
            }

            return session;
        }

        private static String GenerateHash()
        {
            var bytes = new byte[16];
            _sRand.NextBytes(bytes);
            return string.Join("", bytes.Select(x => x.ToString("x2")));
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Capacity(byte.MaxValue)]
        public string Username { get; set; }

        [NotNull]
        public DateTime LoginTime { get; set; }

        [Capacity(byte.MaxValue)]
        public string IPString { get; set; }

        [Capacity(32), FixedLength]
        public string Hash { get; set; }
    }
}
