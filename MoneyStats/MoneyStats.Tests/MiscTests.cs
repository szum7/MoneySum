using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyStats.BL;

namespace MoneyStats.Tests.ExcelReaderTesters
{
    [TestClass]
    public class MiscTests
    {
        [TestMethod]
        public void Test_TransactionTagConnRepository_GetWithEntities()
        {
            // Arrange
            var repo = new TransactionTagConnRepository();

            // Act
            var entities = repo.GetWithEntities();

            // Assert
            if (entities.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsNotNull(entities[0].Tag);
                Assert.IsNotNull(entities[0].Transaction);
            }
        }
    }
}
