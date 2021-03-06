using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using ekread;

namespace ekreadTests {
    public class EkreadTest {
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
    public class CompressorInfo2 {
        public int Cnumber { get; set; }
        public string Cip { get; set; }
    }

    public class JsonTest {
        [Test]
        public void TestJsonCompressorInfo() {
            string jsonText = @"[{cnumber:1,cip:""192.168.1.1""}, {cnumber:2,cip:""192.168.1.2""}]";
            var compressors = JsonConvert.DeserializeObject<List<CompressorInfo2>>(jsonText);
            Assert.AreEqual(2, compressors.Count);
            var compressor1 = compressors[0];
            var compressor2 = compressors[1];
            Assert.AreEqual(1, compressor1.Cnumber);
            Assert.AreEqual(2, compressor2.Cnumber);
            Assert.AreEqual("192.168.1.1", compressor1.Cip);
            Assert.AreEqual("192.168.1.2", compressor2.Cip);
        }
    }

    public class ShortTest {
        [Test]
        public void TestFloat() {
            // 39322 16781 <-> 17.7
            ushort us1 = 39322;
            ushort us2 = 16781;
            float expected = 17.7f;
            float actual = Ushort2float(us1, us2);
            Assert.AreEqual(expected, actual, 0.0001);
        }
        [Test]
        public void TestShort() {
            // 39322 16781 <-> 17.7
            ushort us1 = 39322;
            ushort us2 = 16781;
            float ff = Ushort2float(us1, us2);
            var tt = Float2ushort(ff);
            Assert.AreEqual(39322, tt.sh1);
            Assert.AreEqual(16781, tt.sh2);
        }

        float Ushort2float(ushort sh1, ushort sh2) {
            return BitConverter.ToSingle(BitConverter.GetBytes(((uint)sh2 << 16) + sh1), 0);
        }
        (ushort sh1, ushort sh2) Float2ushort(float ff) {
            byte[] bb = BitConverter.GetBytes(ff);
            return (BitConverter.ToUInt16(bb, 0), BitConverter.ToUInt16(bb, 2));
        }
    }
}