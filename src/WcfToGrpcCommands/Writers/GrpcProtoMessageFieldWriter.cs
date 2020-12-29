using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFactory.DotNet.CSharp;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcProtoMessageFieldWriter : WriterBase
    {
        public GrpcProtoMessageFieldWriter()
        {

        }


        public string Name { get; set; }

        public GrpcTypeConverter Type { get; set; }

        public int Position { get; set; }

        public override string WriteCode()
        {
            return $"{Type.ToProtoType()} {Name} = {Position};";
        }
    }
}
