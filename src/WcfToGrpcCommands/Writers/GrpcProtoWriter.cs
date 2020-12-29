using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfToGrpcCommands.Writers.Templates;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcProtoWriter : WriterBase
    {
        public string PackageName { get; set; }

        public string CodeNamespace { get; set; }

        public List<GrpcProtoMessageWriter> Messages { get; } = new List<GrpcProtoMessageWriter>();

        public List<GrpcProtoServiceWriter> Services { get; } = new List<GrpcProtoServiceWriter>();

        public override string WriteCode()
        {
            return GrpcProtoTemplate.GenerateSource(this);
        }
    }
}
