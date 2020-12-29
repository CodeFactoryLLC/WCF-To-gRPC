using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcConfigDepBlockWriter : WriterBase
    {
        public string ServiceName { get; set; }

        public override string WriteCode()
        {

            return $"endpoints.MapGrpcService<{ServiceName}>();";

        }
    }
}
