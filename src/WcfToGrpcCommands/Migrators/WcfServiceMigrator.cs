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
    public class WcfServiceMigrator
    {
        public WcfMigrator Parent { get; private set; }

        private ILogger _logger;

        private VsProject _destProject;

        private MigratorOptions _options;


        public GrpcProtoServiceWriter ProtoService { get; } = new GrpcProtoServiceWriter();

        public GrpcCsServiceWriter CsService { get; } = new GrpcCsServiceWriter();

        public List<GrpcConfigDepBlockWriter> DiMappings => Parent.SvcConfig.DiMappings;

        public List<WcfServiceOperationMigrator> WcfServiceOperations { get; } = new List<WcfServiceOperationMigrator>();


        public WcfServiceMigrator(ILogger logger, MigratorOptions options, VsProject destProject, WcfMigrator parent)
        {
            _logger = logger;
            _options = options;

            _destProject = destProject;
            Parent = parent;
        }

        public async Task MigrateService(CsInterface srcContract, CsClass srcImplementation)
        {
            string serviceName = srcImplementation?.Name ?? srcContract.Name.TrimStartWord("I", limit: 1);

            _logger.InfoEnter($"Starting migration of WCF Service '{serviceName}'");

            
            //Migrate Config
            DiMappings.Add(new GrpcConfigDepBlockWriter() { ServiceName = serviceName });


            //Migrate Proto Service
            ProtoService.ServiceName = serviceName;


            //Migrate CsService
            CsService.ServiceName = serviceName;
            CsService.CodeNamespace = $"{_destProject.DefaultNamespace}.Services";
            CsService.ProtoNamespace = Parent.ProtoFile.CodeNamespace;


            //Get ServiceOperations
            var srcMethods = srcContract.Methods
                .Where(m => m.Attributes
                    .Any(a => a.Type.Name.StartsWith("OperationContract")))
                .Select(i => new
                {
                    Contract = i,
                    Implementation = srcImplementation.Methods
                        .FirstOrDefault(m => m.FormatCSharpComparisonHashCode() == i.FormatCSharpComparisonHashCode())
                });

            foreach (var srcMethod in srcMethods)
            {
                await MigrateServiceOperation(srcMethod.Contract, srcMethod.Implementation);
            }


            _logger.InfoExit($"Finished migration of WCF Service '{serviceName}'");
        }

        private async Task MigrateServiceOperation(CsMethod srcMethodContract, CsMethod srcMethodImplementation)
        {
            var wcfSvcOp = new WcfServiceOperationMigrator(_logger, _options, _destProject, this);
            await wcfSvcOp.MigrateServiceOperation(srcMethodContract, srcMethodImplementation);

            WcfServiceOperations.Add(wcfSvcOp);
            ProtoService.Operations.Add(wcfSvcOp.ProtoSvcOperation);
            CsService.Operations.Add(wcfSvcOp.CsSvcOperation);
        }

    }
}
