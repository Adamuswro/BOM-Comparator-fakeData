using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels.Helpers
{
    public static class UserInputConverter
    {
        public static IEnumerable<String> SplitText(string input)
        {
            if (input == null)
            {
                return null;
            }
            input = input.Replace("\n", String.Empty);
            var results = input.Split(new Char[] { '\t', ',', ';' });
            results = results.Select(i => i.Trim()).ToArray();
            return results;
        }

    }
}
