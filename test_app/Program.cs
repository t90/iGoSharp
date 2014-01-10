using System.IO;
using Interfaces;

namespace test_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = System.Console.Out.GoIWriteLine();
            Print(i);
        }

        public static void Print(IWriteLine wl)
        {
            wl.WriteLine("some text");
        }

    }

    public static class Extensions
    {

        public static IWriteLine GoIWriteLine(this TextWriter tw)
        {
            return new TextWriterGoFacade(tw);
        }
    }

    public class TextWriterGoFacade : IWriteLine
    {
        private readonly TextWriter _obj;

        public TextWriterGoFacade(TextWriter obj)
        {
            _obj = obj;
        }

        public void WriteLine(string format, params object[] args)
        {
            _obj.WriteLine(format,args);
        }

    }


}
