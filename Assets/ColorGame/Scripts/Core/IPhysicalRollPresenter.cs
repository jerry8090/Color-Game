using System;
using System.Collections;

namespace ColorGame.Core
{
    /// <summary>
    /// A roll presenter whose result comes from REAL physics.
    /// ColorGameManager does not tell it what to show. It starts the roll, waits, and then
    /// receives the colors the cubes really ended up showing. That is the official result.
    /// (The older IRollPresenter, where the result is decided first, still works for ColorMachine.)
    /// </summary>
    public interface IPhysicalRollPresenter
    {
        /// <summary>True while the cubes are rolling or returning to the shelf.</summary>
        bool IsBusy { get; }

        /// <param name="onResult">Called once, after every cube has genuinely settled, with the top-face
        /// colors in slot order (exactly 3). NOT called if the cubes could not settle cleanly.</param>
        IEnumerator PlayPhysicalRoll(Action<GameColor[]> onResult);
    }
}
