using System;
using System.Collections;

namespace ColorGame.Core
{
    /// <summary>
    /// Anything that can PRESENT a roll to the player (for example the physics dice machine).
    /// ColorGameManager decides the official result. The presenter only performs it visually
    /// and reports back what it ended up showing.
    /// </summary>
    public interface IRollPresenter
    {
        /// <param name="officialResult">The colors decided by the game logic (exactly 3).</param>
        /// <param name="onConfirmed">Called once, after the presentation finished, with the colors the
        /// presenter actually shows. Not called if the presentation failed.</param>
        IEnumerator PlayRoll(GameColor[] officialResult, Action<GameColor[]> onConfirmed);
    }
}