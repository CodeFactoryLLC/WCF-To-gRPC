using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFactory.DotNet.CSharp;

namespace WcfToGrpcCommands.Writers
{
    public class GrpcTypeConverter
    {
        private CsType _srcType;



        public GrpcTypeConverter(CsType srcType)
        {
            _srcType = srcType;
        }


        public string ToProtoType()
        {
            var result = string.Empty;

            if (_srcType.IsType<Task>())
            {
                _srcType = _srcType.GenericParameters.FirstOrDefault()?.Type;
            }

            if (_srcType == null)
                return null;

            if (_srcType.IsType(typeof(List<>), typeof(IList<>), typeof(IEnumerable<>)))
            {
                _srcType = _srcType.GenericParameters.FirstOrDefault()?.Type;
                result += "repeated ";
            }


            var typeName = _srcType.Name;

            if (_srcType.IsArray)
                result += "repeated ";


            switch (typeName)
            {
                case nameof(Decimal):
                    result += "double";
                    break;

                case nameof(Double):
                    result += "double";
                    break;

                case nameof(Single):
                    result += "float";
                    break;

                case nameof(Int32):
                    result += "int32";
                    break;

                case nameof(Int64):
                    result += "int64";
                    break;

                case nameof(UInt32):
                    result += "uint32";
                    break;

                case nameof(UInt64):
                    result += "uint64";
                    break;

                case nameof(Boolean):
                    result += "bool";
                    break;

                case nameof(String):
                    result += "string";
                    break;

                case nameof(Guid):
                    result += "string";
                    break;

                default:
                    result += _srcType.Name;
                    break;
            }




            return result;
        }



    }
}
