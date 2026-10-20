namespace ScatterGame.Core
{
    /// <summary>
    /// Starting prototype configuration. The weights and pays are NOT tuned and are not the original game's numbers.
    /// Run RtpSimulator and adjust them.
    /// Ids: 0 Ten, 1 Jack, 2 Queen, 3 King, 4 Ace, 5 Wild, 6 Scatter, 7-11 Golden Ten..Ace.
    /// </summary>
    public static class DefaultSlotConfig
    {
        public const int Ten = 0, Jack = 1, Queen = 2, King = 3, Ace = 4, Wild = 5, Scatter = 6;
        public const int GoldenOffset = 7; // golden id = GoldenOffset + base id

        public static SlotConfig Create()
        {
            SlotConfig config = new SlotConfig();

            AddRegular(config, Ten, "Ten", 20, new[] { 10, 30, 80 });
            AddRegular(config, Jack, "Jack", 18, new[] { 12, 40, 100 });
            AddRegular(config, Queen, "Queen", 16, new[] { 16, 50, 140 });
            AddRegular(config, King, "King", 14, new[] { 20, 80, 200 });
            AddRegular(config, Ace, "Ace", 12, new[] { 30, 120, 300 });

            config.Symbols.Add(new SymbolDefinition
            {
                Id = Wild, Name = "Wild", Category = SymbolCategory.Wild,
                ReelWeights = new[] { 0, 2, 2, 2, 0 }
            });

            config.Symbols.Add(new SymbolDefinition
            {
                Id = Scatter, Name = "Scatter", Category = SymbolCategory.Scatter,
                ReelWeights = new[] { 2, 2, 2, 2, 2 }
            });

            for (int baseId = Ten; baseId <= Ace; baseId++)
            {
                config.Symbols.Add(new SymbolDefinition
                {
                    Id = GoldenOffset + baseId,
                    Name = "Golden " + config.GetSymbol(baseId).Name,
                    Category = SymbolCategory.Regular,
                    GoldenOfSymbolId = baseId,
                    ReelWeights = new[] { 0, 1, 1, 1, 0 }
                });
            }

            return config;
        }

        private static void AddRegular(SlotConfig config, int id, string name, int weight, int[] payMilli)
        {
            config.Symbols.Add(new SymbolDefinition
            {
                Id = id,
                Name = name,
                Category = SymbolCategory.Regular,
                ReelWeights = new[] { weight, weight, weight, weight, weight },
                PayMilli = payMilli
            });
        }
    }
}