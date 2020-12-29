using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CodeFactory.DotNet.CSharp;
using CodeFactory.VisualStudio;
using NLog.Filters;
using WcfToGrpcCommands.Writers;

namespace WcfToGrpcCommands
{
    public static class CodeFactoryMixins
    {
        public static async Task<VsDocument> AddDocumentTo(this VsProject source, string vsPath, string content)
        {
            if (string.IsNullOrEmpty(vsPath))
            {
                throw new Exception($"Parameter {nameof(vsPath)} cannot be null or empty");
            }

            var fileParts = vsPath.Trim('/', '\\').Split('/', '\\');
            if (fileParts.Length == 1)
            {
                var children = await source.GetChildrenAsync(false, false);
                
                var file = children.OfType<VsDocument>().FirstOrDefault(f => string.Equals(f.Name, fileParts[0], StringComparison.CurrentCultureIgnoreCase));

                if (file == null)
                {
                    return await source.AddDocumentAsync(fileParts[0], content);
                }

                await file.ReplaceContentAsync(content);
                return file;
            }
            else
            {
                var children = await source.GetChildrenAsync(false, false);

                var folder = children.OfType<VsProjectFolder>().FirstOrDefault(f => string.Equals(f.Name, fileParts[0], StringComparison.CurrentCultureIgnoreCase));

                if (folder == null)
                {
                    folder = await source.AddProjectFolderAsync(fileParts[0]);
                }

                return await folder.AddDocumentTo(fileParts.Skip(1).JoinString("\\"), content);
            }
        }

        public static async Task<VsDocument> AddDocumentTo(this VsProjectFolder source, string vsPath, string content)
        {
            if (string.IsNullOrEmpty(vsPath))
            {
                throw new Exception($"Parameter {nameof(vsPath)} cannot be null or empty");
            }

            var fileParts = vsPath.Trim('/', '\\').Split('/', '\\');
            if (fileParts.Length == 1)
            {
                var children = await source.GetChildrenAsync(false, false);

                var file = children.OfType<VsDocument>().FirstOrDefault(f => string.Equals(f.Name, fileParts[0], StringComparison.CurrentCultureIgnoreCase));

                if (file == null)
                {
                    return await source.AddDocumentAsync(fileParts[0], content);
                }

                await file.ReplaceContentAsync(content);
                return file;
            }
            else
            {
                var children = await source.GetChildrenAsync(false, false);

                var folder = children.OfType<VsProjectFolder>().FirstOrDefault(f => string.Equals(f.Name, fileParts[0], StringComparison.CurrentCultureIgnoreCase));

                if (folder == null)
                {
                    folder = await source.AddProjectFolderAsync(fileParts[0]);
                }

                return await folder.AddDocumentTo(fileParts.Skip(1).JoinString("\\"), content);

            }
        }

        public static bool IsType<T>(this CsType source)
        {
            return source.IsType(typeof(T));
        }
        /// <summary>
        /// Returns true if the CsType matches any of the passed in types
        /// </summary>
        public static bool IsType(this CsType source, params Type[] types)
        {
            return types.Any(type => source.Name == type.Name.TrimEndWord("`1") && source.Namespace == type.Namespace);
        }
    }
}
