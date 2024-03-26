using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictacserver
{
    public class User
    {
        public string internalid;

        public User(string internalid)
        {
            this.internalid = internalid;

        }

        public override string ToString()
        {
            return internalid;
        }

    }
}
