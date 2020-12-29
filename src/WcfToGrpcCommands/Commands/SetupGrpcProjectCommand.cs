using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CodeFactory.Logging;
using CodeFactory.VisualStudio;
using CodeFactory.VisualStudio.SolutionExplorer;
using WcfToGrpcCommands.Migrators;

namespace WcfToGrpcCommands.Commands
{
    /// <summary>
    /// Code factory command for automation of a project when selected from solution explorer.
    /// </summary>
    public class SetupGrpcProjectCommand : ProjectCommandBase
    {
        private static readonly string commandTitle = "Migrate WCF Services";
        private static readonly string commandDescription = "Migrates any WCF Services in this sln to this project as gRPC";

#pragma warning disable CS1998

        /// <inheritdoc />
        public SetupGrpcProjectCommand(ILogger logger, IVsActions vsActions) : base(logger, vsActions, commandTitle, commandDescription)
        {
            //Intentionally blank
        }
#pragma warning disable CS1998
        #region Overrides of VsCommandBase<VsProject>

        /// <summary>
        /// Validation logic that will determine if this command should be enabled for execution.
        /// </summary>
        /// <param name="result">The target model data that will be used to determine if this command should be enabled.</param>
        /// <returns>Boolean flag that will tell code factory to enable this command or disable it.</returns>
        public override async Task<bool> EnableCommandAsync(VsProject result)
        {
            //Result that determines if the the command is enabled and visible in the context menu for execution.
            bool isEnabled = false;

            try
            {
                //TODO: Add logic to determine if this command is enabled.

                var refs = await result.GetProjectReferencesAsync();

                isEnabled  = refs.Any(a => a.Name == "Grpc.AspNetCore.Server");

                Debug.WriteLine($"Enabling Command {commandTitle}.  ");


                
            }
            catch (Exception unhandledError)
            {
                _logger.Error($"The following unhandled error occured while checking if the solution explorer project command {commandTitle} is enabled. ",
                    unhandledError);
                isEnabled = false;
            }

            return isEnabled;
        }

        /// <summary>
        /// Code factory framework calls this method when the command has been executed. 
        /// </summary>
        /// <param name="result">The code factory model that has generated and provided to the command to process.</param>
        public override async Task ExecuteCommandAsync(VsProject result)
        {
            try
            {
                //For now lets hard code the project name.  
                //TODO: Move this to user control


                var sln = await this.VisualStudioActions.GetSolutionAsync();

                var migrator = new WcfMigrator(_logger, new MigratorOptions(), result);

                await migrator.MigrateAsync(sln);
            }
            catch (Exception unhandledError)
            {
                _logger.Error($"The following unhandled error occured while executing the solution explorer project command {commandTitle}. ",
                    unhandledError);

            }
        }

        #endregion
    }
}
