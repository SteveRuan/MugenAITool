using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugenAITool
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this String str, String substring)
        {
            if (substring == null) throw new ArgumentNullException("substring", "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), StringComparison.CurrentCultureIgnoreCase)) throw new ArgumentException("comp is not a member of StringComparison", "comp");

            return str.IndexOf(substring, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public static bool EqualsIgnoreCase(this String str, String substring)
        {
            return str.Equals(substring, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
