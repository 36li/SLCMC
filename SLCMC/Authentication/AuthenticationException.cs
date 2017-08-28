using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLCMC.Authentication
{
    class AuthenticationException : Exception
    {
        public AuthenticationException(string message): base(message)
        {
        }
    }
}
