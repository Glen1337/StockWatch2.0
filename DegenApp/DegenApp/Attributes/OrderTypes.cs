using System;
using System.Collections.Generic;

namespace DegenApp.Attributes
{
    public class OrderTypes
    {
        public static readonly IReadOnlyList<string> orderTypes = Array.AsReadOnly(new string[]
{
            "buy",

            "sell",

            "selltoopen",

            "buytoopen",

            "buytoclose",

            "selltoclose",

            "sellshort",

            "buytocover"
});
    }
}
