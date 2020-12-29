using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfToGrpcCommands.Writers.Templates;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcCsServiceOperationWriter : WriterBase
    {
        public string MethodName { get; set; }

        public string RequestMessageName { get; set; }

        public string ResponseMessageName { get; set; }

        public string OldContent { get; set; }

        public override string WriteCode()
        {
            return GrpcCsServiceOperationTemplate.GenerateSource(this);
        }
    }
}
