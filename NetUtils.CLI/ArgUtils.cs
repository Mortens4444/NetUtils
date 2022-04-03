using System.Globalization;

namespace NetUtils.CLI
{
    public static class ArgUtils
    {
        public static bool IsParamUsed(string[] args, string paramName)
        {
            return args.Any(arg => NormalizeArg(arg) == paramName);
        }

        public static string GetNextArg(string[] args, string paramName)
        {
            if (args == null || args.Length == 0)
            {
                throw new InvalidOperationException("No argument passed");
            }

            var index = GetParamIndex(args, paramName);
            return args[index + 1];
        }

        private static string NormalizeArg(string arg)
        {
            return arg.TrimStart('-', '/').ToLower(CultureInfo.CurrentCulture);
        }

        private static int GetParamIndex(string[] args, string paramName)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (NormalizeArg(args[i]) == paramName)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}