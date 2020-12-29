using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WcfToGrpcCommands.Writers
{
    public static class WriterMixins
    {


        private const string IndentText = "    ";
        private const string IndentLineRegEx = @"\n[^\S\r\n]*";

        /// <summary>
        /// Joins an array of strings into a single string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static string JoinString(this IEnumerable<string> source, string seperator)
        {
            if (source == null)
                return string.Empty;

            return string.Join(seperator, source);
        }

        /// <summary>
        /// Joins the string output of an array of ICodeWrite objects.  
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string WriteAllCode(this IEnumerable<ICodeWriter> source)
        {
            if (source == null)
                return string.Empty;

            return source.Select(s => s.WriteCode()).JoinString(Environment.NewLine);
        }


        /// <summary>
        /// Will format a multi-line statement.  If the multi-line statement can be fit into a single line, then it will do so.
        /// Otherwise it will indent each new line besides the first line based on the <para name="identNum" /> parameter.  
        /// </summary>
        /// <param name="source">The string to format</param>
        /// <param name="indentNum">The number of tabs to indent the statement</param>
        /// <param name="singleLineCount">The max number of chars used format the statement as a single line</param>
        /// <returns>The formatted statement</returns>
        public static string FormatMultiLineStatement(this string source, int indentNum, int singleLineCount = 180)
        {
            const string removeNewLineRegEx = @"\s*\n\s*";
            const string extraSpacingRegEx = @"\s*([()<>])\s*";

            var charCount = source.Count(c => !char.IsWhiteSpace(c));
            if (charCount <= singleLineCount)
            {
                var result = source;
                result = Regex.Replace(result, removeNewLineRegEx, " ");
                result = Regex.Replace(result, extraSpacingRegEx, "$1");
                return result;
            }
            else
            {
                var indentReplacement = Enumerable.Repeat(IndentText, indentNum).JoinString("");
                var result = Regex.Replace(source, IndentLineRegEx, indentReplacement);
                return result;
            }
        }


        public static string CommentOut(this string source)
        {
            return "//" + Regex.Replace(source.Trim(), IndentLineRegEx,  "\n//");
        }

        /// <summary>
        /// Will indent each new line in the statement based on the <para name="identNum" /> parameter.  First line indent does not change
        /// </summary>
        /// <param name="source">The string to indent</param>
        /// <param name="indentNum">The number of tabs to indent the statement</param>
        /// <returns>The indented statement</returns>
        public static string IndentSub(this string source, int indentNum = 1)
        {
            return source.Indent(indentNum, false);
        }

        /// <summary>
        /// Will indent the statement and each new line in the statement based on the <para name="identNum" /> parameter.
        /// </summary>
        /// <param name="source">The string to indent</param>
        /// <param name="indentNum">The number of tabs to indent the statement</param>
        /// <returns>The indented statement</returns>
        public static string Indent(this string source, int indentNum = 1, bool includeFirstLine = true)
        {
            var indentReplacement = Enumerable.Repeat(IndentText, indentNum).JoinString("");
            var result = Regex.Replace(source.Trim(), IndentLineRegEx, "\n" + indentReplacement);
            if (includeFirstLine)
            {
                result = indentReplacement + result;
            }
            return result;
        }

        /// <summary>
        /// Converts the string to camelCase
        /// </summary>
        public static string ToCamelCase(this string source)
        {
            if (String.IsNullOrEmpty(source))
                return source;

            var s1 = source.Substring(0, 1).ToLower();
            if (source.Length == 1)
                return s1;
            return s1 + source.Substring(1);
        }

        /// <summary>
        /// Converts the string to PascalCase
        /// </summary>
        public static string ToPascalCase(this string source)
        {
            if (String.IsNullOrEmpty(source))
                return source;

            var s1 = source.Substring(0, 1).ToUpper();
            if (source.Length == 1)
                return s1;
            return s1 + source.Substring(1);
        }

        /// <summary>
        /// Trims the start of a string using an exact word match, ie... remove 'Grpc' in 'GrpcConfigWriter'
        /// </summary>
        public static string TrimStartWord(this string source, string value, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase, int limit = 0)
        {
            if (string.IsNullOrEmpty(value))
                return source;

            var count = 0;
            while (!string.IsNullOrEmpty(source) && source.StartsWith(value, comparisonType) && (limit == 0 || count <= limit))
            {
                source = source.Substring(value.Length - 1);
                count++;
            }

            return source;
        }

        /// <summary>
        /// Trims the end of a string using an exact word match, ie... remove 'Writer' in 'GrpcConfigWriter'
        /// </summary>
        public static string TrimEndWord(this string source, string value, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase, int limit = 0)
        {
            if (string.IsNullOrEmpty(value))
                return source;

            var count = 0;
            while (!string.IsNullOrEmpty(source) && source.EndsWith(value, comparisonType) && (limit == 0 || count <= limit))
            {
                source = source.Substring(0, (source.Length - value.Length));
                count++;
            }

            return source;
        }

        /// <summary>
        /// Replaces the end of a string using an exact word match, ie... replace 'Writer' in 'GrpcConfigWriter' with 'Model => 'GrpcConfigModel';
        /// </summary>
        public static string ReplaceSuffix(this string source, string match, string replacement)
        {
            return source.TrimEndWord(match) + replacement;
        }

        /// <summary>
        /// Evaluates string using wildcard (*) syntax
        /// </summary>
        public static bool IsWildcardMatch(this string source, string pattern)
        {
            var regExPattern = "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            return Regex.IsMatch(source, regExPattern);
        }


    }
}
