using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IWriteLine
    {
        void WriteLine(string format, params object[] args);
    }
}
