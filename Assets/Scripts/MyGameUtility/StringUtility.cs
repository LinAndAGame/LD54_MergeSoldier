using System.Text;
using MyGameExpand;

namespace MyGameUtility {
    public static class StringUtility {
        public static string Connect(string linkString = null, params string[] targets)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < targets.Length; i++)
            {
                result.Append(targets[i]);
                result.Append(linkString);
            }

            if (targets.IsNullOrEmpty() == false && string.IsNullOrEmpty(linkString) == false)
            {
                result.Remove(result.Length - linkString.Length, linkString.Length);
            }

            return result.ToString();
        }

        public static string AddPrefixAndSuffix(string target, string prefix, string suffix)
        {
            StringBuilder sb = new StringBuilder(target);
            sb.Insert(0, prefix);
            sb.Append(suffix);
            return sb.ToString();
        }
    }
}