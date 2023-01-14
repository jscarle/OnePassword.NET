namespace OnePassword.Common;

/// <summary>
/// Defines properties and methods that a class implements in order to support tracking changes to its instances. 
/// </summary>
internal interface ITracked
{
    /// <summary>
    /// Returns <see langword="true"/> when the object has changed, <see langword="false"/> otherwise.
    /// </summary>
    bool Changed { get; }
    
    /// <summary>
    /// Accepts changes to the object.
    /// </summary>
    void AcceptChanges();
}