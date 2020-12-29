using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFactory.DotNet.CSharp;
using CodeFactory.Logging;
using CodeFactory.VisualStudio;
using WcfToGrpcCommands.Writers;

namespace WcfToGrpcCommands.Migrators
{
    public class WcfServiceOperationMigrator
    {
        public WcfServiceMigrator Parent { get; private set; }

        private ILogger _logger;

        private VsProject _destProject;

        private MigratorOptions _options;


        public GrpcProtoServiceOperationWriter ProtoSvcOperation { get; } = new GrpcProtoServiceOperationWriter();

        public GrpcCsServiceOperationWriter CsSvcOperation { get; } = new GrpcCsServiceOperationWriter();


        public WcfServiceOperationMigrator(ILogger logger, MigratorOptions options, VsProject destProject, WcfServiceMigrator parent)
        {
            _logger = logger;
            _options = options;

            _destProject = destProject;
            Parent = parent;
        }



        public async Task MigrateServiceOperation(CsMethod srcMethodContract, CsMethod srcMethodImplementation)
        {
            await MigrateProtoServiceOperation(srcMethodContract);
            await MigrateCsServiceOperation(srcMethodContract, srcMethodImplementation);
        }


        private async Task MigrateCsServiceOperation(CsMethod srcMethodContract, CsMethod srcMethodImplementation)
        {
            var operationName = srcMethodContract.Name;

            _logger.InfoEnter($"Starting migration of WCF->Cs Service Operation'{operationName}'");


            CsSvcOperation.MethodName = srcMethodContract.Name;
            CsSvcOperation.OldContent = await srcMethodImplementation.GetBodySyntaxAsync();


            if (srcMethodContract.HasParameters)
            {
                CsSvcOperation.RequestMessageName = $"{CsSvcOperation.MethodName}Request";


                foreach (var srcParam in srcMethodContract.Parameters)
                {

                }
            }

            if (!srcMethodContract.IsVoid)
            {
                CsSvcOperation.ResponseMessageName = $"{srcMethodContract.Name}Response";
            }


            _logger.InfoExit($"Finished migration of WCF->Cs Service '{operationName}'");
        }



        private async Task MigrateProtoServiceOperation(CsMethod srcMethod)
        {
            var operationName = srcMethod.Name;

            _logger.InfoEnter($"Starting migration of WCF->Proto Service Operation'{operationName}'");


            ProtoSvcOperation.MethodName = srcMethod.Name;


            if (srcMethod.HasParameters)
            {
                ProtoSvcOperation.RequestMessage = new GrpcProtoMessageWriter()
                {
                    MessageName = $"{srcMethod.Name}Request",
                };

                int posCount = 1;
                foreach (var srcParam in srcMethod.Parameters)
                {
                    ProtoSvcOperation.RequestMessage.Fields.Add(new GrpcProtoMessageFieldWriter()
                    {
                        Name = srcParam.Name.ToCamelCase(),
                        Type = new GrpcTypeConverter(srcParam.ParameterType),
                        Position = posCount++
                    });
                }
            }

            if (!srcMethod.IsVoid)
            {
                ProtoSvcOperation.ResponseMessage = new GrpcProtoMessageWriter()
                {
                    MessageName = $"{srcMethod.Name}Response",
                    Fields =
                        {
                            new GrpcProtoMessageFieldWriter()
                            {
                                Name = "result",
                                Type = new GrpcTypeConverter(srcMethod.ReturnType),
                                Position = 1
                            }
                        }
                };
            }


            _logger.InfoExit($"Finished migration of WCF->Proto Service '{operationName}'");
        }

    }
}
