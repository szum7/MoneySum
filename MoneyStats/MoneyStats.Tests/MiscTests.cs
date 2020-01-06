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

        [TestMethod]
        public void Test_TagRepository_GetAllTagDetailedSummary()
        {
            // Arrange
            var repo = new TagRepository();

            // Act
            var details = repo.GetAllTagDetailedSummary(new System.DateTime(1999, 1, 1), System.DateTime.Now);

            // Assert
            Assert.IsNotNull(details); // Not really a test, only for debug
        }
    }
}
