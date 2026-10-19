namespace ColorGame.Core
{
    /// <summary>
    /// Anything that can produce the three result colors for a round.
    /// The prototype uses ColorResultGenerator (local random).
    /// In a real-money version, results must come from an authorized backend,
    /// never from the Unity client.
    /// </summary>
    public interface IResultService
    {
        /// <summary>Returns exactly ColorGameConfig.ResultSlotCount colors.</summary>
        GameColor[] GenerateResult();
    }
}