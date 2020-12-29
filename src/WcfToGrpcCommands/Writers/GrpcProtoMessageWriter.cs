using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfToGrpcCommands.Writers.Templates;

namespace WcfToGrpcCommands.Writers
{

    public class GrpcProtoMessageWriter : WriterBase
    {
        
        public string MessageName { get; set; }

        public List<GrpcProtoMessageFieldWriter> Fields { get; } = new List<GrpcProtoMessageFieldWriter>();

        public override string WriteCode()
        {
            return GrpcProtoMessageTemplate.GenerateSource(this);
        }
    }
}
