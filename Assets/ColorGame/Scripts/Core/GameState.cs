namespace ColorGame.Core
{
    /// <summary>
    /// Where the game currently is. Players can only change selections or start a round in Idle.
    /// </summary>
    public enum GameState
    {
        Idle,              // Ready: player can pick a color and a bet
        SelectingColor,    // Reserved for future use
        SelectingBet,      // Reserved for future use
        Rolling,           // Dice animation is playing
        ShowingResult,     // Results are visible, matches are highlighted
        ProcessingPayout   // Winnings are being added and the round saved
    }
}