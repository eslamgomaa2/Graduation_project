using System;
using System.Globalization;

namespace OA.Service.Exceptions
{
    public class AuthenticationResponse : Exception
    {
        public AuthenticationResponse() : base() { }

        public AuthenticationResponse(string message) : base(message) { }

        public AuthenticationResponse(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
