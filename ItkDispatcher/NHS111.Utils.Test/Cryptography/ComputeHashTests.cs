using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Utils.Cryptography;
using NUnit.Framework;

namespace NHS111.Utils.Test.Cryptography
{
    [TestFixture()]
    public class ComputeHashTests
    {
        private const string HashEmptyString = "D41D8CD98F00B204E9800998ECF842";
        private const string HashObjectString = "C078C1597EEA8327C935796628AB4B";

        private HashTestParent _hashTestParent;

        [SetUp]
        public void SetUp()
        {
            _hashTestParent = new HashTestParent()
            {
                someProperty = "test",
                someChild = new HashTestChild() { childProperty = "test Child" }
            };
        }

        [Test]
        public void Compuate_with_null_throws_error_test()
        {
            var compute = new ComputeHash();
            Assert.That(() => compute.Compute(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Compuate_with_empty_string_returns_hash_test()
        {
            var compute = new ComputeHash();
            var result = compute.Compute(string.Empty);
            Assert.AreEqual(HashEmptyString, result);
        }

        [Test]
        public void Compuate_with_json_string_returns_hash_test()
        {
            var compute = new ComputeHash();
            var result = compute.Compute(JsonConvert.SerializeObject(_hashTestParent));
            Assert.AreEqual(HashObjectString, result);
        }

        [Test]
        public void Compuate_identical_hash_returns_true_test()
        {
            var compute = new ComputeHash();

            var obj = new HashTestParent()
            {
                someProperty = "test",
                someChild = new HashTestChild() { childProperty = "test Child" }
            };
            var latestHash = compute.Compute(JsonConvert.SerializeObject(obj));
            var result = compute.Compare(HashObjectString, latestHash);

            Assert.IsTrue(result);
        }

        [Test]
        public void Compuate_different_hash_returns_false_test()
        {
            var compute = new ComputeHash();

            var obj = new HashTestParent()
            {
                someProperty = "test one",
                someChild = new HashTestChild() { childProperty = "test Child" }
            };
            var latestHash = compute.Compute(JsonConvert.SerializeObject(obj));
            var result = compute.Compare(HashObjectString, latestHash);

            Assert.IsFalse(result);
        }
    }

    internal sealed class HashTestParent
    {
        public string someProperty { get; set; }

        public HashTestChild someChild { get; set; }
    }

    internal sealed class HashTestChild
    {
        public string childProperty { get; set; }
    }
}
