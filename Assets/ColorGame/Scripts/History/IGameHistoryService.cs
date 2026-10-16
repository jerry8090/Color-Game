using System.Collections.Generic;
using ColorGame.Gameplay;

namespace ColorGame.History
{
    /// <summary>
    /// Everything the game needs from a round-history store.
    /// The prototype uses LocalGameHistoryService. In a real-money version, round IDs, round records
    /// and statistics must be created and stored by an authorized backend, never trusted from the client.
    /// (A server version would be asynchronous. This prototype interface is synchronous for simplicity.)
    /// </summary>
    public interface IGameHistoryService
    {
        /// <summary>Oldest first.</summary>
        IReadOnlyList<GameRound> Rounds { get; }
        long LastRoundNumber { get; }
        GameStatistics Statistics { get; }

        /// <summary>Hands out the next unique ID, for example ROUND-000001.</summary>
        string ReserveNextRoundId();

        /// <summary>Stores a finished round, updates statistics, trims to the limit.</summary>
        void AddRound(GameRound round);

        /// <summary>Removes rounds and statistics. The round counter keeps going so IDs are never reused.</summary>
        void ClearHistory();

        /// <summary>Removes everything including the round counter (full game reset).</summary>
        void ResetAll();

        /// <summary>Restores saved data. If statistics are missing, they are rebuilt from the rounds.</summary>
        void LoadState(long savedLastRoundNumber, IEnumerable<GameRound> savedRounds, GameStatistics savedStatistics);
    }
}