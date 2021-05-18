using System;
using System.Collections.Generic;

namespace DegenApp.Attributes
{
    public class SecurityTypes
    {
        public static readonly IReadOnlyList<string> securityTypes = Array.AsReadOnly(new string[]
        {       
           "option",

           "bond",

           "share",

           "call",

           "put",

           "unit",

           "stock",
        });
    }
}
