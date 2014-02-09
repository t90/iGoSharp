namespace iGo{
public static class Extensions
{
    public class TextWriterProxy : Interfaces.IWriteLine
    {
        private readonly System.IO.TextWriter _inObject;

        internal TextWriterProxy(System.IO.TextWriter inObject)
        {
            _inObject = inObject;
        }

        public void WriteLine(string format, params object[] args)
        {
            _inObject.WriteLine(format,args);
        }
    }

    public static Interfaces.IWriteLine xInterfacesIWriteLine(this System.IO.TextWriter inObject)
    {
        return new TextWriterProxy(inObject);
    }
}
}