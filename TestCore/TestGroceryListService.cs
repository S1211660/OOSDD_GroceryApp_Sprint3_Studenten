using Grocery.Core.Services;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Models;

namespace TestCore
{
    public class TestGroceryListService
    {
        private GroceryListService _groceryListService;
        private GroceryListRepository _groceryListRepository;

        [SetUp]
        public void Setup()
        {
            _groceryListRepository = new GroceryListRepository();
            _groceryListService = new GroceryListService(_groceryListRepository);
        }

        // Happy path tests - UC4 kleur wijzigen succesvol
        [Test]
        public void Update_WithValidColorChange_UpdatesList()
        {
            // Arrange
            var existingList = _groceryListService.Get(1);
            Assert.That(existingList, Is.Not.Null);

            string newColor = "#FF0000";
            existingList.Color = newColor;

            // Act
            var result = _groceryListService.Update(existingList);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Color, Is.EqualTo(newColor));
        }

        [TestCase("#FF0000", "Rood")]
        [TestCase("#00FF00", "Groen")]
        [TestCase("#0000FF", "Blauw")]
        public void Update_WithValidHexCodes_UpdatesCorrectly(string hexColor, string description)
        {
            // Arrange
            var existingList = _groceryListService.Get(1);
            existingList.Color = hexColor;

            // Act
            var result = _groceryListService.Update(existingList);

            // Assert
            Assert.That(result.Color, Is.EqualTo(hexColor), $"Failed for {description}");
        }

        [Test]
        public void Get_WithValidId_ReturnsGroceryList()
        {
            // Act
            var result = _groceryListService.Get(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Boodschappen familieweekend"));
        }

        [Test]
        public void GetAll_ReturnsAllGroceryLists()
        {
            // Act
            var result = _groceryListService.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.Any(g => g.Name.Contains("familieweekend")), Is.True);
        }

        // Unhappy path tests
        [Test]
        public void Get_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = _groceryListService.Get(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Update_WithNonExistentList_ReturnsNull()
        {
            // Arrange
            var nonExistentList = new GroceryList(999, "Non-existent", DateOnly.MinValue, "#000000", 1);

            // Act
            var result = _groceryListService.Update(nonExistentList);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}