namespace OnePassword.Common;

internal interface ITracked
{
    bool Changed { get; }
    void AcceptChanges();
}