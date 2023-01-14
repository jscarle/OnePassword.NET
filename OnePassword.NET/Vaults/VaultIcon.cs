using OnePassword.Common;

namespace OnePassword.Vaults;

/// <summary>
/// Represents the icon of a 1Password vault.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<VaultIcon>))]
public enum VaultIcon
{
    /// <summary>
    /// Airplane
    /// </summary>
    [EnumMember(Value = "airplane")]
    Airplane,

    /// <summary>
    /// Application
    /// </summary>
    [EnumMember(Value = "application")]
    Application,

    /// <summary>
    /// Art Supplies
    /// </summary>
    [EnumMember(Value = "art-supplies")]
    ArtSupplies,

    /// <summary>
    /// Bankers Box
    /// </summary>
    [EnumMember(Value = "bankers-box")]
    BankersBox,

    /// <summary>
    /// Brown Briefcase
    /// </summary>
    [EnumMember(Value = "brown-briefcase")]
    BrownBriefcase,

    /// <summary>
    /// Brown Gate
    /// </summary>
    [EnumMember(Value = "brown-gate")]
    BrownGate,

    /// <summary>
    /// Buildings
    /// </summary>
    [EnumMember(Value = "buildings")]
    Buildings,

    /// <summary>
    /// Cabin
    /// </summary>
    [EnumMember(Value = "cabin")]
    Cabin,

    /// <summary>
    /// Castle
    /// </summary>
    [EnumMember(Value = "castle")]
    Castle,

    /// <summary>
    /// Circle of Dots
    /// </summary>
    [EnumMember(Value = "circle-of-dots")]
    CircleOfDots,

    /// <summary>
    /// Coffee
    /// </summary>
    [EnumMember(Value = "coffee")]
    Coffee,

    /// <summary>
    /// Color Wheel
    /// </summary>
    [EnumMember(Value = "color-wheel")]
    ColorWheel,

    /// <summary>
    /// Curtained Window
    /// </summary>
    [EnumMember(Value = "curtained-window")]
    CurtainedWindow,

    /// <summary>
    /// The default vault icon.
    /// </summary>
    Default,

    /// <summary>
    /// Document
    /// </summary>
    [EnumMember(Value = "document")]
    Document,

    /// <summary>
    /// Doughnut
    /// </summary>
    [EnumMember(Value = "doughnut")]
    Doughnut,

    /// <summary>
    /// Fence
    /// </summary>
    [EnumMember(Value = "fence")]
    Fence,

    /// <summary>
    /// Galaxy
    /// </summary>
    [EnumMember(Value = "galaxy")]
    Galaxy,

    /// <summary>
    /// Gears
    /// </summary>
    [EnumMember(Value = "gears")]
    Gears,

    /// <summary>
    /// Globe
    /// </summary>
    [EnumMember(Value = "globe")]
    Globe,

    /// <summary>
    /// Green Backpack
    /// </summary>
    [EnumMember(Value = "green-backpack")]
    GreenBackpack,

    /// <summary>
    /// Green Gem
    /// </summary>
    [EnumMember(Value = "green-gem")]
    GreenGem,

    /// <summary>
    /// Handshake
    /// </summary>
    [EnumMember(Value = "handshake")]
    Handshake,

    /// <summary>
    /// Heart with Monitor
    /// </summary>
    [EnumMember(Value = "heart-with-monitor")]
    HeartWithMonitor,

    /// <summary>
    /// House
    /// </summary>
    [EnumMember(Value = "house")]
    House,

    /// <summary>
    /// ID Card
    /// </summary>
    [EnumMember(Value = "id-card")]
    IdCard,

    /// <summary>
    /// Jet
    /// </summary>
    [EnumMember(Value = "jet")]
    Jet,

    /// <summary>
    /// Large Ship
    /// </summary>
    [EnumMember(Value = "large-ship")]
    LargeShip,

    /// <summary>
    /// Luggage
    /// </summary>
    [EnumMember(Value = "luggage")]
    Luggage,

    /// <summary>
    /// Plant
    /// </summary>
    [EnumMember(Value = "plant")]
    Plant,

    /// <summary>
    /// Porthole
    /// </summary>
    [EnumMember(Value = "porthole")]
    Porthole,

    /// <summary>
    /// Puzzle
    /// </summary>
    [EnumMember(Value = "puzzle")]
    Puzzle,

    /// <summary>
    /// Rainbow
    /// </summary>
    [EnumMember(Value = "rainbow")]
    Rainbow,

    /// <summary>
    /// Record
    /// </summary>
    [EnumMember(Value = "record")]
    Record,

    /// <summary>
    /// Round Door
    /// </summary>
    [EnumMember(Value = "round-door")]
    RoundDoor,

    /// <summary>
    /// Sandals
    /// </summary>
    [EnumMember(Value = "sandals")]
    Sandals,

    /// <summary>
    /// Scales
    /// </summary>
    [EnumMember(Value = "scales")]
    Scales,

    /// <summary>
    /// Screwdriver
    /// </summary>
    [EnumMember(Value = "screwdriver")]
    Screwdriver,

    /// <summary>
    /// Shop
    /// </summary>
    [EnumMember(Value = "shop")]
    Shop,

    /// <summary>
    /// Tall Window
    /// </summary>
    [EnumMember(Value = "tall-window")]
    TallWindow,

    /// <summary>
    /// Treasure Chest
    /// </summary>
    [EnumMember(Value = "treasure-chest")]
    TreasureChest,

    /// <summary>
    /// Vault Door
    /// </summary>
    [EnumMember(Value = "vault-door")]
    VaultDoor,

    /// <summary>
    /// Vehicle
    /// </summary>
    [EnumMember(Value = "vehicle")]
    Vehicle,

    /// <summary>
    /// The vault icon is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// Wallet
    /// </summary>
    [EnumMember(Value = "wallet")]
    Wallet,

    /// <summary>
    /// Wrench
    /// </summary>
    [EnumMember(Value = "wrench")]
    Wrench
}