using Grocery.Core.Models;

namespace TestCore
{
    public class TestSearchProducts
    {
        private List<Product> testProducts;

        [SetUp]
        public void Setup()
        {
            testProducts = new List<Product>
            {
                new Product(1, "Melk", 300),
                new Product(2, "Kaas", 100),
                new Product(3, "Brood", 400),
                new Product(4, "Cornflakes", 0)
            };
        }

        // Happy path tests
        [Test]
        public void SearchProducts_WithValidSearchTerm_FiltersCorrectly()
        {
            // Arrange
            string searchTerm = "Melk";

            // Act
            var filteredProducts = FilterProducts(testProducts, searchTerm);

            // Assert
            Assert.That(filteredProducts.Count, Is.EqualTo(1));
            Assert.That(filteredProducts[0].Name, Is.EqualTo("Melk"));
        }

        [TestCase("melk", "Melk")]
        [TestCase("MELK", "Melk")]
        [TestCase("kaas", "Kaas")]
        public void SearchProducts_WithDifferentCasing_FiltersCorrectly(string searchTerm, string expectedProduct)
        {
            // Act
            var filteredProducts = FilterProducts(testProducts, searchTerm);

            // Assert
            Assert.That(filteredProducts.Count, Is.EqualTo(1));
            Assert.That(filteredProducts[0].Name, Is.EqualTo(expectedProduct));
        }

        [Test]
        public void SearchProducts_WithEmptyString_ShowsAllProductsWithStock()
        {
            // Act
            var filteredProducts = FilterProducts(testProducts, "");

            // Assert
            Assert.That(filteredProducts.Count, Is.EqualTo(3));
            Assert.That(filteredProducts.Any(p => p.Name == "Cornflakes"), Is.False);
        }

        // Unhappy path tests
        [Test]
        public void SearchProducts_WithNonExistentProduct_ShowsNoResults()
        {
            // Arrange
            string nonExistentProduct = "xyz123";

            // Act
            var filteredProducts = FilterProducts(testProducts, nonExistentProduct);

            // Assert
            Assert.That(filteredProducts.Count, Is.EqualTo(0));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void SearchProducts_WithEmptyOrWhitespace_ShowsProductsWithStock(string searchTerm)
        {
            // Act
            var filteredProducts = FilterProducts(testProducts, searchTerm);

            // Assert
            Assert.That(filteredProducts.Count, Is.EqualTo(3));
            Assert.That(filteredProducts.All(p => p.Stock > 0), Is.True);
        }

        [Test]
        public void FilterProducts_ExcludesProductsWithZeroStock()
        {
            // Act
            var filteredProducts = FilterProducts(testProducts, "");

            // Assert
            Assert.That(filteredProducts.Any(p => p.Stock == 0), Is.False);
        }

        // Helper methode die de filtering van ViewModel simuleert
        private List<Product> FilterProducts(List<Product> products, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return products.Where(p => p.Stock > 0).ToList();
            }

            return products.Where(p =>
                p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) &&
                p.Stock > 0
            ).ToList();
        }
    }
}