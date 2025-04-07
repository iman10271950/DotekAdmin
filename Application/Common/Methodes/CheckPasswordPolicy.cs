using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Common.Methodes
{
    public static class CheckPasswordPolicy
    {
        public static int CheckPassworSecurity(string Password)
        {
            int passSecoreLevel = 1;

            if (Password.Length <= 4)
                return 0;

            int MinLengthOfPasswordChar = 8;
            if (Password.Length < MinLengthOfPasswordChar)
                return 0;

            if (Password.Length >= MinLengthOfPasswordChar)
                passSecoreLevel++;

            if (Password.Length >= MinLengthOfPasswordChar + (MinLengthOfPasswordChar / 2))
                passSecoreLevel++;

            var rA = new Regex(@"[A-Z]");
            var ra = new Regex(@"[a-z]");
            var r0 = new Regex(@"[0-9]");
            var r_ = new Regex(@"[~,!,@,#,$,%,^,&,*,(,),_,+,-,=,;,:,',"",{,},\\,|,<,>,.,?,/,\[,\],\ ]");

            if (rA.IsMatch(Password) && ra.IsMatch(Password))
                passSecoreLevel++;

            if (rA.IsMatch(Password) && r0.IsMatch(Password))
                passSecoreLevel++;

            if (r0.IsMatch(Password) && ra.IsMatch(Password))
                passSecoreLevel++;

            if (r_.IsMatch(Password))
                passSecoreLevel++;

            return passSecoreLevel;
        }
    }
}