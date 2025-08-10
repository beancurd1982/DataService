using NUnit.Framework;
using System;
using System.Collections.Generic;
using Yang.Data;

namespace Yang.Data.Tests
{
    public class DataServiceTests
    {
        private class TestTableData : ITableData
        {
            public uint TableId { get; set; }
            public string Value { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            // Clean up before each test
            DataService.UnregisterTableData(1);
            DataService.UnregisterTableData(2);
        }

        [Test]
        public void RegisterTableData_Succeeds_WhenUniqueId()
        {
            var data = new TestTableData { TableId = 1, Value = "A" };
            DataService.RegisterTableData(data);

            var retrieved = DataService.GetTableData<TestTableData>(1);
            Assert.AreSame(data, retrieved);
        }

        [Test]
        public void RegisterTableData_Throws_WhenDuplicateId()
        {
            var data1 = new TestTableData { TableId = 1, Value = "A" };
            var data2 = new TestTableData { TableId = 1, Value = "B" };
            DataService.RegisterTableData(data1);

            Assert.Throws<InvalidOperationException>(() => DataService.RegisterTableData(data2));
        }

        [Test]
        public void GetTableData_Throws_WhenNotRegistered()
        {
            Assert.Throws<KeyNotFoundException>(() => DataService.GetTableData<TestTableData>(999));
        }

        [Test]
        public void UnregisterTableData_RemovesInstance()
        {
            var data = new TestTableData { TableId = 2, Value = "B" };
            DataService.RegisterTableData(data);

            DataService.UnregisterTableData(2);

            Assert.Throws<KeyNotFoundException>(() => DataService.GetTableData<TestTableData>(2));
        }
    }
}