using Grocery.Core.Helpers;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Grocery.Core.Data.Repositories;

namespace TestCore
{
    public class TestRegistration
    {
        private ClientService _clientService;
        private ClientRepository _clientRepository;

        [SetUp]
        public void Setup()
        {
            _clientRepository = new ClientRepository();
            _clientService = new ClientService(_clientRepository);
        }

        // Happy path tests
        [Test]
        public void Register_WithValidData_CreatesNewClient()
        {
            // Arrange
            string name = "Test User";
            string email = "test@example.com";
            string password = "Password123!";

            // Act
            var result = _clientService.Register(name, email, password);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.EmailAddress, Is.EqualTo(email));
            Assert.That(PasswordHelper.VerifyPassword(password, result.Password), Is.True);
        }

        [TestCase("John Doe", "john@test.com", "SecurePass1!")]
        [TestCase("Jane Smith", "jane@example.org", "MyPassword2@")]
        public void Register_WithValidVariousInputs_CreatesClients(string name, string email, string password)
        {
            // Act
            var result = _clientService.Register(name, email, password);

            // Assert
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.EmailAddress, Is.EqualTo(email));
        }

        // Unhappy path tests
        [Test]
        public void Register_WithExistingEmail_ThrowsInvalidOperationException()
        {
            // Arrange
            string existingEmail = "user1@mail.com"; // Bestaat al in repository

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _clientService.Register("New User", existingEmail, "Password123!"));

            Assert.That(exception.Message, Is.EqualTo("Dit email adres is al geregistreerd"));
        }

        [TestCase("user2@mail.com")]
        [TestCase("user3@mail.com")]
        public void Register_WithExistingEmails_ThrowsException(string existingEmail)
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _clientService.Register("Another User", existingEmail, "Password123!"));
        }

        [Test]
        public void Register_WithCaseInsensitiveExistingEmail_ThrowsException()
        {
            // Arrange
            string existingEmailDifferentCase = "USER1@MAIL.COM";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _clientService.Register("New User", existingEmailDifferentCase, "Password123!"));
        }
    }
}