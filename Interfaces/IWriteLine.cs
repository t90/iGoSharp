using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{

    public interface ITest<T>
    {
        
    }

    public interface IWriteLine
    {
        void WriteLine(string format, params object[] args);
    }

    public interface IWriteLineExt : IWriteLine
    {
        void WriteLine(string text);
    }

}
