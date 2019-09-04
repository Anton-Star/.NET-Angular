using FactWeb.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class EncryptionTests
    {
        private const string Key = "my enc key";
        private const string Value = "Password";
        private const string EncValue = "SdYNDmrbywOgz3SdE6rt5A==";

        [TestMethod]
        public void TestEncryption()
        {
            var encValue = EncryptionHelper.Encrypt(Value, Key, true);

            Assert.AreEqual(EncValue, encValue);
        }

        [TestMethod]
        public void TestDecryption()
        {
            var value = EncryptionHelper.Decrypt(EncValue, Key, true);

            Assert.AreEqual(Value, value);
        }
    }
}
