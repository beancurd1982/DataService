using NUnit.Framework;

namespace Yang.Data.Tests
{
    public class TableConfigTests
    {
        [Test]
        public void NameChange_RaisesEvent_WithCorrectValues()
        {
            var config = new TableConfig();
            string oldName = null, newName = null;
            config.AsRead().NameChanged += (sender, e) =>
            {
                oldName = e.OldName;
                newName = e.NewName;
            };

            config.AsWrite().SetName("Alice");
            Assert.AreEqual(null, oldName);
            Assert.AreEqual("Alice", newName);

            config.AsWrite().SetName("Bob");
            Assert.AreEqual("Alice", oldName);
            Assert.AreEqual("Bob", newName);
        }

        [Test]
        public void AgeChange_RaisesEvent_WithCorrectValues()
        {
            var config = new TableConfig();
            int oldAge = -1, newAge = -1;
            config.AsRead().AgeChanged += (sender, e) =>
            {
                oldAge = e.OldAge;
                newAge = e.NewAge;
            };

            config.AsWrite().SetAge(10);
            Assert.AreEqual(0, oldAge);
            Assert.AreEqual(10, newAge);

            config.AsWrite().SetAge(20);
            Assert.AreEqual(10, oldAge);
            Assert.AreEqual(20, newAge);
        }

        [Test]
        public void NameChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var config = new TableConfig();
            int eventCount = 0;
            config.AsWrite().SetName("Alice");
            config.AsRead().NameChanged += (sender, e) => eventCount++;
            config.AsWrite().SetName("Alice");
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void AgeChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var config = new TableConfig();
            int eventCount = 0;
            config.AsWrite().SetAge(42);
            config.AsRead().AgeChanged += (sender, e) => eventCount++;
            config.AsWrite().SetAge(42);
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void SetNameAndAge_RaisesBothEvents()
        {
            var config = new TableConfig();
            bool nameChanged = false, ageChanged = false;
            config.AsRead().NameChanged += (sender, e) => nameChanged = true;
            config.AsRead().AgeChanged += (sender, e) => ageChanged = true;

            config.AsWrite().SetNameAndAge("Charlie", 99);
            Assert.IsTrue(nameChanged);
            Assert.IsTrue(ageChanged);
            Assert.AreEqual("Charlie", config.AsRead().Name);
            Assert.AreEqual(99, config.AsRead().Age);
        }
    }

    public class TableStatusTests
    {
        [Test]
        public void GameNumberChange_RaisesEvent_WithCorrectValues()
        {
            var status = new TableStatus();
            uint oldValue = 0, newValue = 0;
            status.AsRead().GameNumberChanged += (sender, e) =>
            {
                oldValue = e.OldGameNumber;
                newValue = e.NewGameNumber;
            };

            status.AsWrite().SetGameNumber(42);
            Assert.AreEqual(0, oldValue);
            Assert.AreEqual(42, newValue);

            status.AsWrite().SetGameNumber(100);
            Assert.AreEqual(42, oldValue);
            Assert.AreEqual(100, newValue);
        }

        [Test]
        public void GameStateChange_RaisesEvent_WithCorrectValues()
        {
            var status = new TableStatus();
            GameState oldState = GameState.NewGame, newState = GameState.NewGame;
            status.AsRead().GameStateChanged += (sender, e) =>
            {
                oldState = e.OldGameState;
                newState = e.NewGameState;
            };

            status.AsWrite().SetGameState(GameState.ResultConfirmed);
            Assert.AreEqual(GameState.NewGame, oldState);
            Assert.AreEqual(GameState.ResultConfirmed, newState);

            status.AsWrite().SetGameState(GameState.NewGame);
            Assert.AreEqual(GameState.ResultConfirmed, oldState);
            Assert.AreEqual(GameState.NewGame, newState);
        }

        [Test]
        public void GameNumberChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var status = new TableStatus();
            int eventCount = 0;
            status.AsWrite().SetGameNumber(7);
            status.AsRead().GameNumberChanged += (sender, e) => eventCount++;
            status.AsWrite().SetGameNumber(7);
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void GameStateChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var status = new TableStatus();
            int eventCount = 0;
            status.AsWrite().SetGameState(GameState.NewGame);
            status.AsRead().GameStateChanged += (sender, e) => eventCount++;
            status.AsWrite().SetGameState(GameState.NewGame);
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void Accessor_ReflectsLatestValues()
        {
            var status = new TableStatus();
            status.AsWrite().SetGameNumber(123);
            status.AsWrite().SetGameState(GameState.ResultConfirmed);

            Assert.AreEqual(123, status.AsRead().GameNumber);
            Assert.AreEqual(GameState.ResultConfirmed, status.AsRead().CurrentGameState);
        }
    }

    public class TableDataTests
    {
        [Test]
        public void TableData_Config_And_Status_AreInitialized()
        {
            var tableData = new TableData();
            Assert.IsNotNull(tableData.Config);
            Assert.IsNotNull(tableData.Status);
        }

        [Test]
        public void TableData_Config_NameChange_RaisesEvent()
        {
            var tableData = new TableData();
            string oldName = null, newName = null;
            tableData.Config.AsRead().NameChanged += (sender, e) =>
            {
                oldName = e.OldName;
                newName = e.NewName;
            };

            tableData.Config.AsWrite().SetName("TestName");
            Assert.AreEqual(null, oldName);
            Assert.AreEqual("TestName", newName);
        }

        [Test]
        public void TableData_Config_AgeChange_RaisesEvent()
        {
            var tableData = new TableData();
            int oldAge = -1, newAge = -1;
            tableData.Config.AsRead().AgeChanged += (sender, e) =>
            {
                oldAge = e.OldAge;
                newAge = e.NewAge;
            };

            tableData.Config.AsWrite().SetAge(42);
            Assert.AreEqual(0, oldAge);
            Assert.AreEqual(42, newAge);
        }

        [Test]
        public void TableData_Status_GameNumberChange_RaisesEvent()
        {
            var tableData = new TableData();
            uint oldNumber = 0, newNumber = 0;
            tableData.Status.AsRead().GameNumberChanged += (sender, e) =>
            {
                oldNumber = e.OldGameNumber;
                newNumber = e.NewGameNumber;
            };

            tableData.Status.AsWrite().SetGameNumber(99);
            Assert.AreEqual(0, oldNumber);
            Assert.AreEqual(99, newNumber);
        }

        [Test]
        public void TableData_Status_GameStateChange_RaisesEvent()
        {
            var tableData = new TableData();
            GameState oldState = GameState.NewGame, newState = GameState.NewGame;
            tableData.Status.AsRead().GameStateChanged += (sender, e) =>
            {
                oldState = e.OldGameState;
                newState = e.NewGameState;
            };

            tableData.Status.AsWrite().SetGameState(GameState.ResultConfirmed);
            Assert.AreEqual(GameState.NewGame, oldState);
            Assert.AreEqual(GameState.ResultConfirmed, newState);
        }

        [Test]
        public void TableData_Config_Accessor_ReflectsLatestValues()
        {
            var tableData = new TableData();
            tableData.Config.AsWrite().SetName("AccessorTest");
            tableData.Config.AsWrite().SetAge(77);

            Assert.AreEqual("AccessorTest", tableData.Config.AsRead().Name);
            Assert.AreEqual(77, tableData.Config.AsRead().Age);
        }

        [Test]
        public void TableData_Status_Accessor_ReflectsLatestValues()
        {
            var tableData = new TableData();
            tableData.Status.AsWrite().SetGameNumber(1234);
            tableData.Status.AsWrite().SetGameState(GameState.ResultConfirmed);

            Assert.AreEqual(1234, tableData.Status.AsRead().GameNumber);
            Assert.AreEqual(GameState.ResultConfirmed, tableData.Status.AsRead().CurrentGameState);
        }
    }
}
