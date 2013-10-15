using DatabaseTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonsFirstThing
{

    enum Rank : byte
    {
        Admin = 1,
        Staff = 0
    }

    [DatabaseEntity]
    class Account
    {
        public static void DefaultAccountCheck()
        {
            int count = Database.SelectAll<Account>().Count;
            if (count == 0)
            {
                Database.Insert(new Account
                {
                    Username = "supervisor", Rank = Rank.Admin
                });
            }
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Capacity(byte.MaxValue)]
        public string Username { get; set; }

        [NotNull]
        public Rank Rank { get; set; }
    }
}
