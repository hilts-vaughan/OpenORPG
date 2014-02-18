using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Cryptography;
using Xunit;

namespace OpenORPG.Server.Tests.Infrastructure.Cryptography
{
    /// <summary>
    /// Provides a collection of tests for hashes to ensure they're working correctly.
    /// </summary>
    public class HashTests
    {

        /// <summary>
        /// This test ensures the MD5 function is working correctly
        /// </summary>
        [Fact]
        public void Md5ShouldReturnLowercaseMd5()
        {
            var testCase = HashHelper.GetMd5("thisissomething");
            const string expectedTestCase = "b95b8b34ac2872a1192f803aa9dd92a8";

            var testCase2 = HashHelper.GetMd5("thisisareallylongstring!@#44withalotofelements");
            const string expectedTestCase2 = "e1060c0600512023069ec54f8de79708";

            var testCase3 = HashHelper.GetMd5("");
            const string expectedTestCase3 = "d41d8cd98f00b204e9800998ecf8427e";

            Assert.Equal(expectedTestCase, testCase);
            Assert.Equal(expectedTestCase2, testCase2);
            Assert.Equal(expectedTestCase3, testCase3);
        }

        /// <summary>
        /// This test ensures the Sha512 function is working correctly.
        /// </summary>
        [Fact]
        public void Sha512ShouldReturnLowercaseMd5()
        {
            var testCase = HashHelper.GetSha512("thisissomething");
            var testCase2 = HashHelper.GetSha512("thisisareallylongstring!@#44withalotofelements");
            var testCase3 = HashHelper.GetSha512("");

            var expected =
                "1A2860F4E7F6F74827185CFBF3B798D69119C4F19FFB63925DCAB92A0E38FA0C2F9FE8944B784625DFE3D718A2DFC6F057226F92F735F89D9B6EE91837AAD456"
                    .ToLower();
            var expected2 =
                "D9984731A2C39BCDFA995DF6AF41040C3BAE584639EA361BF6B2F9736B617D8833532585871076BF6F200C294939788AF9AF8B98C7FD3D32F74E2F078039C92C"
                    .ToLower();
            var expected3 =
                "cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e"
                    .ToLower();

            Assert.Equal(expected, testCase);
            Assert.Equal(expected2, testCase2);
            Assert.Equal(expected3, testCase3);
        }


    }
}
