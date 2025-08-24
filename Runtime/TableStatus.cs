using System;

namespace Yang.Data
{
    public enum GameState
    {
        NewGame,
        ResultConfirmed,
    }

    /// <summary>
    /// Internal write interface for TableStatus (only friend assemblies can see/resolve it).
    /// </summary>
    internal interface ITableStatusWrite
    {
        void SetGameNumber(uint value);
        void SetGameState(GameState value);
        void Set(uint gameNumber, GameState state);
    }

    /// <summary>
    /// Public read-only interface for TableStatus (safe for UI/consumers).
    /// </summary>
    public interface ITableStatusRead
    {
        uint GameNumber { get; }
        GameState CurrentGameState { get; }

        event EventHandler<GameNumberChangedEvent> GameNumberChanged;
        event EventHandler<GameStateChangedEvent> GameStateChanged;

        public readonly struct GameNumberChangedEvent
        {
            public readonly uint OldGameNumber;
            public readonly uint NewGameNumber;

            public GameNumberChangedEvent(uint oldGameNumber, uint newGameNumber)
            {
                OldGameNumber = oldGameNumber;
                NewGameNumber = newGameNumber;
            }
        }

        public readonly struct GameStateChangedEvent
        {
            public readonly GameState OldGameState;
            public readonly GameState NewGameState;

            public GameStateChangedEvent(GameState oldGameState, GameState newGameState)
            {
                OldGameState = oldGameState;
                NewGameState = newGameState;
            }
        }
    }

    public sealed class TableStatus : ITableStatusRead, ITableStatusWrite
    {
        private uint _gameNumber;
        private GameState _currentGameState;

        private event EventHandler<ITableStatusRead.GameNumberChangedEvent> _gameNumberChanged;
        private event EventHandler<ITableStatusRead.GameStateChangedEvent> _gameStateChanged;

        /// <summary>
        /// Public read-only view for any consumer.
        /// </summary>
        public ITableStatusRead AsRead() => this;

        /// <summary>
        /// Internal write view (requires InternalsVisibleTo to the writer assembly).
        /// </summary>
        internal ITableStatusWrite AsWrite() => this;

        #region ITableStatusRead (explicit implementation; hidden from typical call sites)
        event EventHandler<ITableStatusRead.GameNumberChangedEvent> ITableStatusRead.GameNumberChanged
        {
            add => _gameNumberChanged += value;
            remove => _gameNumberChanged -= value;
        }
        event EventHandler<ITableStatusRead.GameStateChangedEvent> ITableStatusRead.GameStateChanged
        {
            add => _gameStateChanged += value;
            remove => _gameStateChanged -= value;
        }

        uint ITableStatusRead.GameNumber => _gameNumber;
        GameState ITableStatusRead.CurrentGameState => _currentGameState;
        #endregion

        #region ITableStatusWrite (explicit implementation; hidden from typical call sites)
        void ITableStatusWrite.SetGameNumber(uint value)
        {
            if (_gameNumber == value) return;
            var old = _gameNumber;
            _gameNumber = value;
            _gameNumberChanged?.Invoke(this, new ITableStatusRead.GameNumberChangedEvent(old, value));
        }

        void ITableStatusWrite.SetGameState(GameState value)
        {
            if (_currentGameState == value) return;
            var old = _currentGameState;
            _currentGameState = value;
            _gameStateChanged?.Invoke(this, new ITableStatusRead.GameStateChangedEvent(old, value));
        }

        void ITableStatusWrite.Set(uint gameNumber, GameState state)
        {
            // Use individual setters to preserve dedup/event ordering semantics.
            ((ITableStatusWrite)this).SetGameNumber(gameNumber);
            ((ITableStatusWrite)this).SetGameState(state);
        }
        #endregion
    }
}