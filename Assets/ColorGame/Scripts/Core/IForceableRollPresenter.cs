using ColorGame.Core;

namespace ColorGame.Core
{
    /// <summary>
    /// A roll presenter that can be asked (DEV TEST only) to end with given colors.
    /// In release builds SetForcedResult does nothing.
    /// </summary>
    public interface IForceableRollPresenter
    {
        void SetForcedResult(GameColor[] colors);
    }
}