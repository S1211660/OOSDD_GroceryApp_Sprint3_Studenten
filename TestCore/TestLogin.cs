using Grocery.Core.Services;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Models;

namespace TestCore
{
    public class TestLogin
    {
        private AuthService _authService;
        private ClientService _clientService;
        private ClientRepository _clientRepository;

        [SetUp]
        public void Setup()
        {
            _clientRepository = new ClientRepository();
            _clientService = new ClientService(_clientRepository);
            _authService = new AuthService(_clientService);
        }

        // Happy path tests
        [Test]
        public void Login_WithValidCredentials_ReturnsClient()
        {
            // Arrange
            string email = "user3@mail.com";
            string password = "user3";

            // Act
            Client? result = _authService.Login(email, password);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmailAddress, Is.EqualTo(email));
            Assert.That(result.Name, Is.EqualTo("A.J. Kwak"));
        }

        [TestCase("user1@mail.com", "user1", "M.J. Curie")]
        [TestCase("user2@mail.com", "user2", "H.H. Hermans")]
        [TestCase("user3@mail.com", "user3", "A.J. Kwak")]
        public void Login_WithValidVariousCredentials_ReturnsCorrectClient(string email, string password, string expectedName)
        {
            // Act
            Client? result = _authService.Login(email, password);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
            Assert.That(result.EmailAddress, Is.EqualTo(email));
        }

        // Unhappy path tests
        [Test]
        public void Login_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            string invalidEmail = "nietbestaand@mail.com";
            string password = "user3";

            // Act
            Client? result = _authService.Login(invalidEmail, password);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            string email = "user3@mail.com";
            string wrongPassword = "verkeerd_wachtwoord";

            // Act
            Client? result = _authService.Login(email, wrongPassword);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase("", "user3")]
        [TestCase("user3@mail.com", "")]
        [TestCase("", "")]
        public void Login_WithEmptyCredentials_ReturnsNull(string email, string password)
        {
            // Act
            Client? result = _authService.Login(email, password);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_WithCaseSensitiveEmail_ReturnsNull()
        {
            // Arrange
            string emailWrongCase = "USER3@MAIL.COM";
            string password = "user3";

            // Act
            Client? result = _authService.Login(emailWrongCase, password);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}