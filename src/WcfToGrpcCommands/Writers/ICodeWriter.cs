using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfToGrpcCommands.Writers
{
    public interface ICodeWriter
    {
        string WriteCode();
    }
}
