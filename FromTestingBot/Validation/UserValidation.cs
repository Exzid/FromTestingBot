using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Order.Validation
{
    static class UserValidation
    {
        public static string PhoneRegex(string str)
        {
            Regex regex = new Regex(@"^((\+7|7|8)+([0-9]){10})$");
            str = Regex.Replace(str, "[ ()//-]", new string(""));

            if (regex.Matches(str).Count() == 1)
            {
                return str;
            }
            return null;
        }

        public static string EmailRegex(string str)
        {          
            Regex regex = new Regex(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$");
            if (str.Length < 30 && regex.Matches(str.ToLower()).Count() == 1)
            {
                return str;
            }
            Console.WriteLine(regex.Matches(str).Count());
            return null;
        }
    }
}
