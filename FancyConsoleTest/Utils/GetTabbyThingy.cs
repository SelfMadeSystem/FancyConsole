using System.Collections.Generic;
using System.Linq;

namespace FancyConsoleTest.Utils
{
    public class GetTabbyThingy
    {
        public static List<string> GetHints(string text)
        {
            var list = new List<string>();
            var m = text.Count(s => s == ' ') + 1;
            for (var i = 0; i < m; i++) list.Add(NameForNumber(i));
            return list;
        }

        private static string[] ones = {"", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

        private static string[] teens =
        {
            "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };

        private static string[] tens =
            {"", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"};

        private static string NameForNumber(long number)
        {
            if (number == 0) return "zero";
            if (number < 10)
                return ones[number];

            if (number < 20)
                return teens[number - 10];

            if (number < 100)
                return tens[number / 10] + ((number % 10 != 0) ? " " + NameForNumber(number % 10) : "");

            if (number < 1000)
                return NameForNumber(number / 100) + " hundred" +
                       ((number % 100 != 0) ? " " + NameForNumber(number % 100) : "");

            if (number < 1000000)
                return NameForNumber(number / 1000) + " thousand" +
                       ((number % 1000 != 0) ? " " + NameForNumber(number % 1000) : "");

            if (number < 1000000000)
                return NameForNumber(number / 1000000) + " million" +
                       ((number % 1000000 != 0) ? " " + NameForNumber(number % 1000000) : "");

            if (number < 1000000000000)
                return NameForNumber(number / 1000000000) + " billion" +
                       ((number % 1000000000 != 0) ? " " + NameForNumber(number % 1000000000) : "");

            return "error";
        }
    }
}