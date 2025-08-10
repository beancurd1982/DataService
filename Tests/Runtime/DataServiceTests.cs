using NUnit.Framework;
using Yang.Data;

namespace Yang.Data.Tests
{
    public class DataServiceTests
    {
        [Test]
        public void CanInstantiateDataService()
        {
            var service = new DataService();
            Assert.IsNotNull(service);
        }
    }
}
