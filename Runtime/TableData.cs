using System;

namespace Yang.Data
{
    public class TableData
    {
        public TableConfig Config { get; } = new();
        public TableStatus Status { get; } = new();
    }

    public class TableConfig
    {
        private string _name;
        private int _age;

        public event EventHandler<NameChangedEvent> NameChanged;
        public event EventHandler<AgeChangedEvent> AgeChanged;

        public TableConfigAccessor Accessor { get; private set; }
        public TableConfigModifier Modifier { get; private set; }

        public TableConfig()
        {
            Accessor = new TableConfigAccessor(this);
            Modifier = new TableConfigModifier(this);
        }

        public void Dispose()
        {
            NameChanged = null;
            AgeChanged = null;
        }

        public class TableConfigAccessor
        {
            private readonly TableConfig _tableConfig;

            public TableConfigAccessor(TableConfig tableConfig)
            {
                _tableConfig = tableConfig ??
                             throw new ArgumentNullException(nameof(tableConfig), "TableConfig cannot be null.");
            }

            public string Name => _tableConfig._name;
            public int Age => _tableConfig._age;
        }

        public class TableConfigModifier
        {
            private readonly TableConfig _tableConfig;

            public TableConfigModifier(TableConfig tableConfig)
            {
                _tableConfig = tableConfig ??
                             throw new ArgumentNullException(nameof(tableConfig), "TableConfig cannot be null.");
            }

            public string Name
            {
                set
                {
                    if (_tableConfig._name == value) return;
                    var oldName = _tableConfig._name;
                    _tableConfig._name = value;
                    _tableConfig.NameChanged?.Invoke(_tableConfig, new NameChangedEvent(oldName, value));
                }
            }

            public int Age
            {
                set
                {
                    if(_tableConfig._age == value) return;
                    var oldAge = _tableConfig._age;
                    _tableConfig._age = value;
                    _tableConfig.AgeChanged?.Invoke(_tableConfig, new AgeChangedEvent(oldAge, value));
                }
            }

            public void SetNameAndAge(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }

        public readonly struct NameChangedEvent
        {
            public readonly string OldName;
            public readonly string NewName;

            public NameChangedEvent(string oldName, string newName)
            {
                OldName = oldName;
                NewName = newName;
            }
        }

        public readonly struct AgeChangedEvent
        {
            public readonly int OldAge;
            public readonly int NewAge;
            public AgeChangedEvent(int oldAge, int newAge)
            {
                OldAge = oldAge;
                NewAge = newAge;
            }
        }
    }

    public class TableStatus
    {
        public enum GameState
        {
            NewGame,
            ResultConfirmed,
        }

        private uint _gameNumber;
        private GameState _gameState;

        public event EventHandler<GameNumberChangedEvent> GameNumberChanged;
        public event EventHandler<GameStateChangedEvent> GameStateChanged;

        public TableStatusAccessor Accessor { get; private set; }
        public TableStatusModifier Modifier { get; private set; }

        public TableStatus()
        {
            Accessor = new TableStatusAccessor(this);
            Modifier = new TableStatusModifier(this);
        }

        public void Dispose()
        {
            GameNumberChanged = null;
            GameStateChanged = null;
        }

        public class TableStatusAccessor
        {
            private readonly TableStatus _tableStatus;

            public TableStatusAccessor(TableStatus status)
            {
                _tableStatus = status ??
                             throw new ArgumentNullException(nameof(status), "TableConfig cannot be null.");
            }

            public uint GameNumber => _tableStatus._gameNumber;
            public GameState GameState => _tableStatus._gameState;
        }

        public class TableStatusModifier
        {
            private readonly TableStatus _tableStatus;

            public TableStatusModifier(TableStatus status)
            {
                _tableStatus = status ??
                             throw new ArgumentNullException(nameof(status), "TableConfig cannot be null.");
            }

            public uint GameNumber
            {
                set
                {
                    if (_tableStatus._gameNumber == value) return;
                    var oldValue = _tableStatus._gameNumber;
                    _tableStatus._gameNumber = value;
                    _tableStatus.GameNumberChanged?.Invoke(_tableStatus, new GameNumberChangedEvent(oldValue, value));
                }
            }

            public GameState GameState
            {
                set
                {
                    if (_tableStatus._gameState == value) return;
                    var oldValue = _tableStatus._gameState;
                    _tableStatus._gameState = value;
                    _tableStatus.GameStateChanged?.Invoke(_tableStatus, new GameStateChangedEvent(oldValue, value));
                }
            }
        }

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
}