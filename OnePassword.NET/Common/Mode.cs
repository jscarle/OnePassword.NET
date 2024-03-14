namespace OnePassword.Common;

/// <summary>Represents the 1Password operating mode.</summary>
public enum Mode
{
    /// <summary>Operate in interactive mode.</summary>
    Interactive = 0,

    /// <summary>Operate in app-integrated mode.</summary>
    AppIntegrated = 1,

    /// <summary>Operate in service account mode.</summary>
    ServiceAccount = 2
}
