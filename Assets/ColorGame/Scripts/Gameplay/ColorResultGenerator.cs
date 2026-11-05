using ColorGame.Core;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// Local prototype result generator. Completely independent from the UI:
    /// it only returns colors and never touches buttons, animation, or the wallet.
    /// Uses UnityEngine.Random, which is fine for a virtual-credit prototype
    /// but is NOT secure enough for real-money gaming.
    /// </summary>
    public class ColorResultGenerator : IResultService
    {
        private readonly GameColor[] allColors = (GameColor[])System.Enum.GetValues(typeof(GameColor));

        public GameColor[] GenerateResult()
        {
            GameColor[] results = new GameColor[ColorGameConfig.ResultSlotCount];

            for (int i = 0; i < results.Length; i++)
            {
                // Range(int, int): the second number is exclusive, so every color has equal odds.
                int index = UnityEngine.Random.Range(0, allColors.Length);
                results[i] = allColors[index];
            }

            return results;
        }
    }
}