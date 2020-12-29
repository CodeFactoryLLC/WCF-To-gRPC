using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeFactory.DotNet.CSharp;
using CodeFactory.Logging;
using CodeFactory.VisualStudio;
using WcfToGrpcCommands.Writers;

namespace WcfToGrpcCommands.Migrators
{
    public class WcfMigrator
    {
        private ILogger _logger;

        private VsProject _destProject;

        private MigratorOptions _options;



        public GrpcConfigWriter SvcConfig { get; } = new GrpcConfigWriter();

        public List<GrpcCsServiceWriter> CsServices { get; } = new List<GrpcCsServiceWriter>();

        public GrpcProtoWriter ProtoFile { get; } = new GrpcProtoWriter();

        public List<WcfServiceMigrator> WcfServices { get; } = new List<WcfServiceMigrator>();



        public WcfMigrator(ILogger logger, MigratorOptions options, VsProject destProject)
        {
            _logger = logger;
            _options = options;

            _destProject = destProject;
        }


        /// <summary>
        /// Migrates all WCF Services in the solution
        /// </summary>
        /// <returns></returns>
        public async Task MigrateAsync(VsSolution srcSln)
        {
            //Initialize the writer models
            SvcConfig.CodeNamespace = $"{_destProject.DefaultNamespace}.Services";
            SvcConfig.ClassName = "GrpcConfig";
            
            ProtoFile.PackageName = _destProject.DefaultNamespace.Replace(".", "");
            ProtoFile.CodeNamespace = $"{_destProject.DefaultNamespace}.Protos";
            

            //Search all project files to migrate WCF
            var srcProjects = await srcSln.GetProjectsAsync(true);
            foreach (var srcProject in srcProjects)
            {
                if (srcProject.Name == _destProject.Name)
                    continue;
                await MigrateProjectAsync(srcProject);
            }


            //Write Files out to visual studio
            var grpcConfigContent = SvcConfig.WriteCode();
            await _destProject.AddDocumentTo($"/Services/{SvcConfig.ClassName}.cs", grpcConfigContent);

            var grpcProtoContent = ProtoFile.WriteCode();
            await _destProject.AddDocumentTo($"/Protos/{ProtoFile.PackageName}.proto", grpcProtoContent);

            foreach (var grpcCsService in CsServices)
            {
                var grpcCsConent = grpcCsService.WriteCode();
                await _destProject.AddDocumentTo($"/Services/{grpcCsService.ServiceName}.cs", grpcCsConent);
            }
        }

        /// <summary>
        /// Migrates all WCF Services in the project
        /// </summary>
        /// <returns></returns>
        private async Task MigrateProjectAsync(VsProject srcProject)
        {
            if (!srcProject.HasChildren)
            {
                _logger.Information($"Project '{srcProject.Name}' has no code files to migrate");
                return;
            }

            //Ask if there is a better way to do this.  Some kind of code search
            var models = await srcProject.GetChildrenAsync(true, true);



            //Migrate DataContracts
            var dataContracts = models
                .Where(m => m.IsLoaded)
                .OfType<VsCSharpSource>()
                .SelectMany(src => src.SourceCode.Classes)
                .Where(i => i.Attributes.Any(attr => attr.Type.Name.StartsWith("DataContract")))
                .ToList();

            _logger.Information($"Project '{srcProject.Name}' found {dataContracts.Count} WCF DataContracts to migrate");

            foreach (var item in dataContracts)
            {
                await MigrateDataContract(item);
            }


            //Migrate Service Contracts
            var wcfServices = models
                .Where(m => m.IsLoaded)
                .OfType<VsCSharpSource>()
                .SelectMany(src => src.SourceCode.Interfaces)
                .Where(i => i.Attributes.Any(attr => attr.Type.Name.StartsWith("ServiceContract")))
                .Select(sc => new
                {
                    Contract = sc,
                    Implementation = models
                        .Where(m => m.IsLoaded)
                        .OfType<VsCSharpSource>()
                        .SelectMany(src => src.SourceCode.Classes)
                        .FirstOrDefault(c =>
                            c.InheritedInterfaces.Any(i => i.Name == sc.Name && i.Namespace == sc.Namespace))
                })
                .ToList();

            _logger.Information($"Project '{srcProject.Name}' found {wcfServices.Count} WCF ServiceContracts to migrate");

            foreach (var item in wcfServices)
            {
                await MigrateService(item.Contract, item.Implementation);
            }
        }

        private async Task MigrateDataContract(CsClass srcModel)
        {
            string modelName = srcModel.Name;

            _logger.InfoEnter($"Starting migration of WCF DataContract '{modelName}'");

            var srcProps = srcModel.Properties
                .Where(p => p.Attributes
                    .Any(a => a.Type.Name.StartsWith("DataMember")))
                .ToList();

            #region Migrate Proto Message
            var message = new GrpcProtoMessageWriter()
            {
                MessageName = modelName,
            };

            int posCount = 1;
            foreach (var srcProp in srcProps)
            {
                message.Fields.Add(new GrpcProtoMessageFieldWriter()
                {
                    Name = srcProp.Name.ToCamelCase(),
                    Type = new GrpcTypeConverter(srcProp.PropertyType) ,
                    Position = posCount++
                });
            }
            ProtoFile.Messages.Add(message);
            #endregion


            _logger.InfoExit($"Finished migration of WCF DataContract '{modelName}'");
        }



        public async Task MigrateService(CsInterface srcContract, CsClass srcImplementation)
        {
            var wcfService = new WcfServiceMigrator(_logger, _options, _destProject, this);
            await wcfService.MigrateService(srcContract, srcImplementation);

            WcfServices.Add(wcfService);
            CsServices.Add(wcfService.CsService);
            ProtoFile.Services.Add(wcfService.ProtoService);

        }


    }
}
