using ekread;
using NUnit.Framework;

namespace ekreadTests {
    public class Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestGood() {
            Assert.IsTrue(IpValidator.IsValidIp("192.168.1.1"));
        }

        [Test]
        public void TestEmpty() {
            Assert.IsFalse(IpValidator.IsValidIp(""));
            Assert.IsFalse(IpValidator.IsValidIp("192.168.1"));
            Assert.IsFalse(IpValidator.IsValidIp("192.168.1.5.6"));
        }

        [Test]
        public void TestCipher() {
            Assert.IsTrue(IpValidator.IsValidIp("192.168.1.1/"));
            Assert.IsTrue(IpValidator.IsValidIp("192.168.1.1"));
        }
    }
}