using Grocery.Core.Interfaces.Services;
using System.Text.Json;
using Grocery.Core.Models;

namespace TestCore
{
    public class MockFileSaverService : IFileSaverService
    {
        public bool ShouldThrowException { get; set; }
        public string LastSavedFileName { get; private set; }
        public string LastSavedContent { get; private set; }

        public async Task SaveFileAsync(string fileName, string content, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (ShouldThrowException)
                throw new IOException("Mock exception for testing");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Invalid filename", nameof(fileName));

            LastSavedFileName = fileName;
            LastSavedContent = content;

            await Task.Delay(10, cancellationToken);
        }
    }

    public class TestFileSaver
    {
        private MockFileSaverService _fileSaverService;

        [SetUp]
        public void Setup()
        {
            _fileSaverService = new MockFileSaverService();
        }

        // Happy path tests
        [Test]
        public async Task SaveFileAsync_WithValidContent_SavesCorrectly()
        {
            // Arrange
            string fileName = "TestBoodschappenlijst.json";
            var testData = new List<GroceryListItem>
            {
                new GroceryListItem(1, 1, 1, 3)
            };
            string content = JsonSerializer.Serialize(testData);
            var cancellationToken = CancellationToken.None;

            // Act
            await _fileSaverService.SaveFileAsync(fileName, content, cancellationToken);

            // Assert
            Assert.That(_fileSaverService.LastSavedFileName, Is.EqualTo(fileName));
            Assert.That(_fileSaverService.LastSavedContent, Is.EqualTo(content));
        }

        [Test]
        public async Task SaveFileAsync_WithEmptyContent_HandlesGracefully()
        {
            // Arrange
            string fileName = "EmptyFile.json";
            string emptyContent = "";
            var cancellationToken = CancellationToken.None;

            // Act
            await _fileSaverService.SaveFileAsync(fileName, emptyContent, cancellationToken);

            // Assert
            Assert.That(_fileSaverService.LastSavedFileName, Is.EqualTo(fileName));
            Assert.That(_fileSaverService.LastSavedContent, Is.EqualTo(emptyContent));
        }

        [TestCase("ValidFileName.json")]
        [TestCase("AnotherFile.txt")]
        public async Task SaveFileAsync_WithValidFileNames_SavesCorrectly(string fileName)
        {
            // Arrange
            string content = "test content";
            var cancellationToken = CancellationToken.None;

            // Act
            await _fileSaverService.SaveFileAsync(fileName, content, cancellationToken);

            // Assert
            Assert.That(_fileSaverService.LastSavedFileName, Is.EqualTo(fileName));
        }

        // Unhappy path tests
        [Test]
        public async Task SaveFileAsync_WithCancellation_ThrowsOperationCanceledException()
        {
            // Arrange
            string fileName = "CancelledFile.json";
            string content = "test content";
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            var exception = Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _fileSaverService.SaveFileAsync(fileName, content, cancellationTokenSource.Token)
            );

            Assert.That(exception, Is.Not.Null);
        }

        [Test]
        public async Task SaveFileAsync_WithMockException_ThrowsIOException()
        {
            // Arrange
            _fileSaverService.ShouldThrowException = true;
            string fileName = "TestFile.json";
            string content = "test content";
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<IOException>(async () =>
                await _fileSaverService.SaveFileAsync(fileName, content, cancellationToken)
            );

            Assert.That(exception, Is.Not.Null);
        }

        [TestCase("")]
        [TestCase("   ")]
        public async Task SaveFileAsync_WithInvalidFileName_ThrowsArgumentException(string invalidFileName)
        {
            // Arrange
            string content = "test content";
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _fileSaverService.SaveFileAsync(invalidFileName, content, cancellationToken)
            );

            Assert.That(exception, Is.Not.Null);
        }

        [Test]
        public async Task SaveFileAsync_WithNullFileName_ThrowsArgumentException()
        {
            // Arrange
            string content = "test content";
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _fileSaverService.SaveFileAsync(null, content, cancellationToken)
            );

            Assert.That(exception, Is.Not.Null);
        }
    }
}