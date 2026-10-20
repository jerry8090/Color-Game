namespace ScatterGame.Core
{
    /// <summary>
    /// The configuration the game uses. Same symbols, ids and weights as DefaultSlotConfig, with a tuned paytable.
    /// Measured on a prototype simulator: about 96% RTP at 100 credits bet, 40% lower variance than the default.
    /// These numbers are invented for this prototype. They are not the original game's numbers.
    /// DefaultSlotConfig stays untouched because the unit tests rely on its exact values.
    /// </summary>
    public static class TunedSlotConfig
    {
        // 3, 4 and 5 reels, thousandths of the total bet PER WAY.
        private static readonly int[][] Pays =
        {
            new[] { 1, 4, 10 },   // Ten
            new[] { 2, 5, 13 },   // Jack
            new[] { 2, 7, 18 },   // Queen
            new[] { 3, 10, 26 },  // King
            new[] { 4, 16, 39 }   // Ace
        };

        public static SlotConfig Create()
        {
            SlotConfig config = DefaultSlotConfig.Create();
            for (int id = DefaultSlotConfig.Ten; id <= DefaultSlotConfig.Ace; id++)
            {
                config.GetSymbol(id).PayMilli = (int[])Pays[id].Clone();
            }
            return config;
        }
    }
}