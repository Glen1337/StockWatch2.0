using System.Runtime.Serialization;

namespace DegenApp.Enums
{
    //[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum OrderType
    {
        [EnumMember(Value="Buy")]
        Buy,

        [EnumMember(Value = "Sell")]
        Sell,

        [EnumMember(Value = "SellToOpen")]
        SellToOpen,

        [EnumMember(Value = "BuyToOpen")]
        BuyToOpen,

        [EnumMember(Value = "SellToClose")]
        SellToClose,

        [EnumMember(Value = "BuyToClose")]
        BuyToClose,

        [EnumMember(Value = "SellShort")]
        SellShort,

        [EnumMember(Value = "BuyToCover")]
        BuyToCover
    }
}
