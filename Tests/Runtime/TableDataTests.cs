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
            config.NameChanged += (sender, e) =>
            {
                oldName = e.OldName;
                newName = e.NewName;
            };

            config.Modifier.Name = "Alice";
            Assert.AreEqual(null, oldName);
            Assert.AreEqual("Alice", newName);

            config.Modifier.Name = "Bob";
            Assert.AreEqual("Alice", oldName);
            Assert.AreEqual("Bob", newName);
        }

        [Test]
        public void AgeChange_RaisesEvent_WithCorrectValues()
        {
            var config = new TableConfig();
            int oldAge = -1, newAge = -1;
            config.AgeChanged += (sender, e) =>
            {
                oldAge = e.OldAge;
                newAge = e.NewAge;
            };

            config.Modifier.Age = 10;
            Assert.AreEqual(0, oldAge);
            Assert.AreEqual(10, newAge);

            config.Modifier.Age = 20;
            Assert.AreEqual(10, oldAge);
            Assert.AreEqual(20, newAge);
        }

        [Test]
        public void NameChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var config = new TableConfig();
            int eventCount = 0;
            config.Modifier.Name = "Alice";
            config.NameChanged += (sender, e) => eventCount++;
            config.Modifier.Name = "Alice";
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void AgeChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var config = new TableConfig();
            int eventCount = 0;
            config.Modifier.Age = 42;
            config.AgeChanged += (sender, e) => eventCount++;
            config.Modifier.Age = 42;
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void SetNameAndAge_RaisesBothEvents()
        {
            var config = new TableConfig();
            bool nameChanged = false, ageChanged = false;
            config.NameChanged += (sender, e) => nameChanged = true;
            config.AgeChanged += (sender, e) => ageChanged = true;

            config.Modifier.SetNameAndAge("Charlie", 99);
            Assert.IsTrue(nameChanged);
            Assert.IsTrue(ageChanged);
            Assert.AreEqual("Charlie", config.Accessor.Name);
            Assert.AreEqual(99, config.Accessor.Age);
        }
    }

    public class TableStatusTests
    {
        [Test]
        public void GameNumberChange_RaisesEvent_WithCorrectValues()
        {
            var status = new TableStatus();
            uint oldValue = 0, newValue = 0;
            status.GameNumberChanged += (sender, e) =>
            {
                oldValue = e.OldGameNumber;
                newValue = e.NewGameNumber;
            };

            status.Modifier.GameNumber = 42;
            Assert.AreEqual(0, oldValue);
            Assert.AreEqual(42, newValue);

            status.Modifier.GameNumber = 100;
            Assert.AreEqual(42, oldValue);
            Assert.AreEqual(100, newValue);
        }

        [Test]
        public void GameStateChange_RaisesEvent_WithCorrectValues()
        {
            var status = new TableStatus();
            TableStatus.GameState oldState = TableStatus.GameState.NewGame, newState = TableStatus.GameState.NewGame;
            status.GameStateChanged += (sender, e) =>
            {
                oldState = e.OldGameState;
                newState = e.NewGameState;
            };

            status.Modifier.GameState = TableStatus.GameState.ResultConfirmed;
            Assert.AreEqual(TableStatus.GameState.NewGame, oldState);
            Assert.AreEqual(TableStatus.GameState.ResultConfirmed, newState);

            status.Modifier.GameState = TableStatus.GameState.NewGame;
            Assert.AreEqual(TableStatus.GameState.ResultConfirmed, oldState);
            Assert.AreEqual(TableStatus.GameState.NewGame, newState);
        }

        [Test]
        public void GameNumberChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var status = new TableStatus();
            int eventCount = 0;
            status.Modifier.GameNumber = 7;
            status.GameNumberChanged += (sender, e) => eventCount++;
            status.Modifier.GameNumber = 7;
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void GameStateChange_DoesNotRaiseEvent_IfValueIsSame()
        {
            var status = new TableStatus();
            int eventCount = 0;
            status.Modifier.GameState = TableStatus.GameState.NewGame;
            status.GameStateChanged += (sender, e) => eventCount++;
            status.Modifier.GameState = TableStatus.GameState.NewGame;
            Assert.AreEqual(0, eventCount);
        }

        [Test]
        public void Accessor_ReflectsLatestValues()
        {
            var status = new TableStatus();
            status.Modifier.GameNumber = 123;
            status.Modifier.GameState = TableStatus.GameState.ResultConfirmed;

            Assert.AreEqual(123, status.Accessor.GameNumber);
            Assert.AreEqual(TableStatus.GameState.ResultConfirmed, status.Accessor.GameState);
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
            tableData.Config.NameChanged += (sender, e) =>
            {
                oldName = e.OldName;
                newName = e.NewName;
            };

            tableData.Config.Modifier.Name = "TestName";
            Assert.AreEqual(null, oldName);
            Assert.AreEqual("TestName", newName);
        }

        [Test]
        public void TableData_Config_AgeChange_RaisesEvent()
        {
            var tableData = new TableData();
            int oldAge = -1, newAge = -1;
            tableData.Config.AgeChanged += (sender, e) =>
            {
                oldAge = e.OldAge;
                newAge = e.NewAge;
            };

            tableData.Config.Modifier.Age = 42;
            Assert.AreEqual(0, oldAge);
            Assert.AreEqual(42, newAge);
        }

        [Test]
        public void TableData_Status_GameNumberChange_RaisesEvent()
        {
            var tableData = new TableData();
            uint oldNumber = 0, newNumber = 0;
            tableData.Status.GameNumberChanged += (sender, e) =>
            {
                oldNumber = e.OldGameNumber;
                newNumber = e.NewGameNumber;
            };

            tableData.Status.Modifier.GameNumber = 99;
            Assert.AreEqual(0, oldNumber);
            Assert.AreEqual(99, newNumber);
        }

        [Test]
        public void TableData_Status_GameStateChange_RaisesEvent()
        {
            var tableData = new TableData();
            TableStatus.GameState oldState = TableStatus.GameState.NewGame, newState = TableStatus.GameState.NewGame;
            tableData.Status.GameStateChanged += (sender, e) =>
            {
                oldState = e.OldGameState;
                newState = e.NewGameState;
            };

            tableData.Status.Modifier.GameState = TableStatus.GameState.ResultConfirmed;
            Assert.AreEqual(TableStatus.GameState.NewGame, oldState);
            Assert.AreEqual(TableStatus.GameState.ResultConfirmed, newState);
        }

        [Test]
        public void TableData_Config_Accessor_ReflectsLatestValues()
        {
            var tableData = new TableData();
            tableData.Config.Modifier.Name = "AccessorTest";
            tableData.Config.Modifier.Age = 77;

            Assert.AreEqual("AccessorTest", tableData.Config.Accessor.Name);
            Assert.AreEqual(77, tableData.Config.Accessor.Age);
        }

        [Test]
        public void TableData_Status_Accessor_ReflectsLatestValues()
        {
            var tableData = new TableData();
            tableData.Status.Modifier.GameNumber = 1234;
            tableData.Status.Modifier.GameState = TableStatus.GameState.ResultConfirmed;

            Assert.AreEqual(1234, tableData.Status.Accessor.GameNumber);
            Assert.AreEqual(TableStatus.GameState.ResultConfirmed, tableData.Status.Accessor.GameState);
        }
    }
}
