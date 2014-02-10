using System.IO;
using iGo;
using Interfaces;

namespace test_app
{

    class Program
    {
        static void Main(string[] args)
        {
//            IWriteLine i = (IGo<TextWriter>)System.Console.Out;
//            Print(i);
        }

        public static void Print(IWriteLine wl)
        {
            wl.WriteLine("some text");
        }

    }




}
