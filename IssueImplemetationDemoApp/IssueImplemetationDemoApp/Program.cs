using System;
using System.Globalization;

namespace IssueImplemetationDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var datestring = DateTime.Now;

            DateTime final = new DateTime();
            var test = DateTime.TryParseExact("2016-03-14 11:22:21.352", new string[] { "yyyy-MM-dd HH:mm:ss.fff" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out final);
            Console.WriteLine(final);

            Console.WriteLine("Hello World!");
        }
    }
}
