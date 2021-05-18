using System.Runtime.Serialization;

namespace DegenApp.Enums
{
    public enum Outlook
    {
        [EnumMember(Value = "Positive")]
        Positive,

        [EnumMember(Value = "Negative")]
        Negative
    }
}