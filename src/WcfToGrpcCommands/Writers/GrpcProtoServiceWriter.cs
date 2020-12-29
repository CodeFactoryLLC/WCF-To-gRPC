using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfToGrpcCommands.Writers.Templates;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcProtoServiceWriter : WriterBase
    {
        public string ServiceName { get; set; }

        public List<GrpcProtoServiceOperationWriter> Operations { get; } = new List<GrpcProtoServiceOperationWriter>();



        public override string WriteCode()
        {
            return GrpcProtoServiceTemplate.GenerateSource(this);
        }
    }
}
