namespace OnePassword.Items;

public struct PasswordRecipe
{
    public bool Letters { get; private set; }
    public bool Digits { get; private set; }
    public bool Symbols { get; private set; }
    public int Length { get; private set; }

    public PasswordRecipe(bool letters, bool digits, bool symbols, int length)
    {
        if (length < 0 || length > 64)
            throw new ArgumentOutOfRangeException($"{nameof(length)} must be between 1 and 64.");
        if (!letters && !digits && !symbols)
            throw new ArgumentException($"{nameof(letters)}, {nameof(digits)}, and {nameof(symbols)} are all false.");

        Letters = letters;
        Digits = digits;
        Symbols = symbols;
        Length = length;
    }
}