using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfToGrpcCommands.Writers.Templates;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcCsServiceWriter : WriterBase
    {

        public string ServiceName { get; set; }

        public string CodeNamespace { get; set; }

        public string ProtoNamespace { get; set; }

        public List<GrpcCsServiceOperationWriter> Operations { get; } = new List<GrpcCsServiceOperationWriter>();


        public override string WriteCode()
        {
            return GrpcCsServiceTemplate.GenerateSource(this);
        }

    }
}
