using Grocery.Core.Helpers;

namespace TestCore
{
    public class TestHelpers
    {
        [SetUp]
        public void Setup()
        {
        }

        // Happy flow
        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";
            bool result = PasswordHelper.VerifyPassword(password, passwordHash);
            Assert.IsTrue(result);
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue_WithValidCredentials(string password, string passwordHash)
        {
            bool result = PasswordHelper.VerifyPassword(password, passwordHash);
            Assert.IsTrue(result);
        }

        // Unhappy flow
        [Test]
        public void TestPasswordHelperReturnsFalse_WithWrongPassword()
        {
            string wrongPassword = "verkeerd_wachtwoord";
            string correctHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";
            bool result = PasswordHelper.VerifyPassword(wrongPassword, correctHash);
            Assert.IsFalse(result);
        }

        [Test]
        public void TestPasswordHelperReturnsFalse_WithInvalidHashFormat()
        {
            string password = "user1";
            string invalidHash = "enkeldeelzonderpunt";
            bool result = PasswordHelper.VerifyPassword(password, invalidHash);
            Assert.IsFalse(result);
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA")]
        public void TestPasswordHelperThrowsFormatException_WithInvalidBase64(string password, string passwordHash)
        {
            Assert.Throws<System.FormatException>(() =>
                PasswordHelper.VerifyPassword(password, passwordHash)
            );
        }
    }
}