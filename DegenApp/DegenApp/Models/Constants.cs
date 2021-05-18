using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DegenApp.Models
{
    public static class Constants
    {
        private static readonly int _short = 4;
        private static readonly int _medium = 8;
        private static readonly int _long = 16;

        public static int Short
        {
            get => _short;
        }

        public static int Medium
        {
            get => _medium;
        }

        public static int Long
        {
            get => _long;
        }

        public const int _Short = 4;

        public const int _Medium = 8;

        public const int _Long = 16;

        public const string _ShortMessage = "Type is limited to 4 characters";

        public const string _MediumMessage = "Type is limited to 8 characters";

        public const string _LongMessage = "Type is limited to 16 characters";

    }
}
