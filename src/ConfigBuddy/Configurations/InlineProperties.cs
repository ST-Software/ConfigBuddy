using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigBuddy.Core.Configurations
{
    public static class InlineProperties
    {
        public static Dictionary<string, string> Parse(string propertiesText)
        {
            if (String.IsNullOrEmpty(propertiesText))
                return new Dictionary<string, string>();

            return propertiesText.Split(';')
                .Select(i => i.Split('='))
                .Where(i => i.Length == 2)
                .ToDictionary(i => i[0], i => i[1]);
        }
    }
}