using System;
using System.Collections.Generic;
using System.Text;

namespace ToplearnDemo.Utility.Generators
{
    public static class NameGenerator
    {
        public static string GenerateUniqueString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
