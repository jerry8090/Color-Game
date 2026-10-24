namespace ScatterGame.Core
{
    public enum SymbolCategory
    {
        Regular,
        Wild,
        Scatter
    }

    /// <summary>
    /// One symbol. Plain data, so it can be built from ScriptableObjects later without changing the engine.
    /// A golden symbol has GoldenOfSymbolId set to its regular symbol. It pays and matches as that symbol.
    /// </summary>
    public sealed class SymbolDefinition
    {
        public int Id;
        public string Name;
        public SymbolCategory Category;

        /// <summary>Generation weight per reel (length = SlotConfig.Reels). 0 means this symbol never lands on that reel.</summary>
        public int[] ReelWeights = new int[SlotConfig.Reels];

        /// <summary>Pay in thousandths of the total bet, PER WAY, for 3, 4 and 5 reels. Regular, non-golden symbols only.</summary>
        public int[] PayMilli = new int[SlotConfig.PayTiers];

        /// <summary>If 0 or higher, this is the golden variant of that regular symbol id.</summary>
        public int GoldenOfSymbolId = -1;

        /// <summary>Regular symbols only: may a Wild stand in for this symbol?</summary>
        public bool WildCanSubstitute = true;

        /// <summary>Optional keys the UI layer can map to sprites and sounds.</summary>
        public string SpriteKey;
        public string SoundKey;

        public bool IsGolden => GoldenOfSymbolId >= 0;
        public bool IsWild => Category == SymbolCategory.Wild;
        public bool IsScatter => Category == SymbolCategory.Scatter;
    }
}