using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfToGrpcCommands.Writers.Templates;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcProtoServiceOperationWriter : WriterBase
    {

        public string MethodName { get; set; }

        public GrpcProtoMessageWriter RequestMessage { get; set; }

        public GrpcProtoMessageWriter ResponseMessage { get; set; }



        public string RequestMessageName => RequestMessage?.MessageName;

        public string ResponseMessageName => ResponseMessage?.MessageName;



        public override string WriteCode()
        {
            return $"rpc {MethodName}({RequestMessageName}) returns ({ResponseMessageName});";
        }
    }
}
