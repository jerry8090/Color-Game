namespace ColorGame.Core
{
    /// <summary>
    /// The six colors of the game, matching the six cube faces.
    /// Saved data stores the NUMBER of each value, so the order must never change.
    /// (Magenta used to be Purple, White used to be Orange.)
    /// </summary>
    public enum GameColor
    {
        Red,      // 0
        Green,    // 1
        Blue,     // 2
        Yellow,   // 3
        Pink,     // 4
        White     // 5
    }
}