using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugenAITool
{
    public static class StringExtensions
    {
        // Ignore case String.Contains()
        public static bool ContainsIgnoreCase(this String str, String substr)
        {
            if (str == null) return false;
            if (substr == null) throw new ArgumentNullException("substring", "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), StringComparison.CurrentCultureIgnoreCase)) throw new ArgumentException("comp is not a member of StringComparison", "comp");
            return str.IndexOf(substr, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        // Ignore case String.Equals() method 
        public static bool EqualsIgnoreCase(this String str, String substring)
        {
            return str.Equals(substring, StringComparison.CurrentCultureIgnoreCase);
        }

        // Remove comment in Mugen code
        public static String RemoveMugenComment(this String str)
        {
            if (str.Contains(';')) str = str.Substring(0, str.IndexOf(';')).Trim();     
            return str;
        }
    }
}
