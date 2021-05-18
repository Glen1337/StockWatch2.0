using System.Runtime.Serialization;

namespace DegenApp.Enums
{
    public enum SecurityType
    {
        [EnumMember(Value = "Bond")]
        Bond,

        [EnumMember(Value = "Unit")]
        Unit,

        [EnumMember(Value = "Share")]
        Share,

        [EnumMember(Value = "Call")]
        Call,

        [EnumMember(Value = "Put")]
        Put
    }
}


