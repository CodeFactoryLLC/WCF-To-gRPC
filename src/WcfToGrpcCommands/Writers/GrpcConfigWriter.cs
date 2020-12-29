using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfToGrpcCommands.Writers.Templates;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcConfigWriter : WriterBase
    {
        public string CodeNamespace { get; set; }

        public string ClassName { get; set; } = "GrpcConfig";

        public List<GrpcConfigDepBlockWriter> DiMappings { get; } = new List<GrpcConfigDepBlockWriter>();

        public override string WriteCode()
        {
            return GrpcConfigTemplate.GenerateSource(this);
        }
    }
}
