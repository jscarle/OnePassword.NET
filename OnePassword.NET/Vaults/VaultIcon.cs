using OnePassword.Common;

namespace OnePassword.Vaults;

[JsonConverter(typeof(JsonStringEnumConverterEx<VaultIcon>))]
public enum VaultIcon
{
    [EnumMember(Value = "airplane")]
    Airplane,

    [EnumMember(Value = "application")]
    Application,

    [EnumMember(Value = "art-supplies")]
    ArtSupplies,

    [EnumMember(Value = "bankers-box")]
    BankersBox,

    [EnumMember(Value = "brown-briefcase")]
    BrownBriefcase,

    [EnumMember(Value = "brown-gate")]
    BrownGate,

    [EnumMember(Value = "buildings")]
    Buildings,

    [EnumMember(Value = "cabin")]
    Cabin,

    [EnumMember(Value = "castle")]
    Castle,

    [EnumMember(Value = "circle-of-dots")]
    CircleOfDots,

    [EnumMember(Value = "coffee")]
    Coffee,

    [EnumMember(Value = "color-wheel")]
    ColorWheel,

    [EnumMember(Value = "curtained-window")]
    CurtainedWindow,

    Default,

    [EnumMember(Value = "document")]
    Document,

    [EnumMember(Value = "doughnut")]
    Doughnut,

    [EnumMember(Value = "fence")]
    Fence,

    [EnumMember(Value = "galaxy")]
    Galaxy,

    [EnumMember(Value = "gears")]
    Gears,

    [EnumMember(Value = "globe")]
    Globe,

    [EnumMember(Value = "green-backpack")]
    GreenBackpack,

    [EnumMember(Value = "green-gem")]
    GreenGem,

    [EnumMember(Value = "handshake")]
    Handshake,

    [EnumMember(Value = "heart-with-monitor")]
    HeartWithMonitor,

    [EnumMember(Value = "house")]
    House,

    [EnumMember(Value = "id-card")]
    IdCard,

    [EnumMember(Value = "jet")]
    Jet,

    [EnumMember(Value = "large-ship")]
    LargeShip,

    [EnumMember(Value = "luggage")]
    Luggage,

    [EnumMember(Value = "plant")]
    Plant,

    [EnumMember(Value = "porthole")]
    Porthole,

    [EnumMember(Value = "puzzle")]
    Puzzle,

    [EnumMember(Value = "rainbow")]
    Rainbow,

    [EnumMember(Value = "record")]
    Record,

    [EnumMember(Value = "round-door")]
    RoundDoor,

    [EnumMember(Value = "sandals")]
    Sandals,

    [EnumMember(Value = "scales")]
    Scales,

    [EnumMember(Value = "screwdriver")]
    Screwdriver,

    [EnumMember(Value = "shop")]
    Shop,

    [EnumMember(Value = "tall-window")]
    TallWindow,

    [EnumMember(Value = "treasure-chest")]
    TreasureChest,

    [EnumMember(Value = "vault-door")]
    VaultDoor,

    [EnumMember(Value = "vehicle")]
    Vehicle,

    [EnumMember(Value = "wallet")]
    Wallet,

    [EnumMember(Value = "wrench")]
    Wrench
}