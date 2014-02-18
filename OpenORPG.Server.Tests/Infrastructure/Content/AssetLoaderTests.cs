using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Content;
using Server.Infrastructure.Content.AssetLoaders;
using Xunit;

namespace OpenORPG.Server.Tests.Content
{
    /// <summary>
    /// Confirms that all asset loaders are working as they should be and return their correct model data.
    /// </summary>
    public class AssetLoaderTests
    {
        public class TestData
        {
            public string Test { get; set; }
        }

        private dynamic _testHarness;

        public AssetLoaderTests()
        {
       
            _testHarness = new ExpandoObject();
            _testHarness.test = "string";
        }

        /// <summary>
        /// Confirms that the JSON loader is capable of loading JSON correctly
        /// </summary>
        [Fact]       
        public void JsonAssetLoaderLoadsCorrect()
        {
            var jsonAssetLoader = new JsonAssetLoader();
            var assetReader = new AssetReader(new FileStream("Data\\Test.json", FileMode.Open), typeof (ExpandoObject));

            dynamic asset = jsonAssetLoader.LoadAsset(assetReader, typeof (ExpandoObject));

            Assert.Equal("string", asset.test);
        }

        /// <summary>
        /// Confirms that the XML loader is capable of loading the XML correctly
        /// </summary>
        [Fact]
        public void XmlAssetLoaderLoadsCorrect()
        {
            var xmlAssetLoader = new XmlAssetLoader();
            var assetReader = new AssetReader(new FileStream("Data\\Test.xml", FileMode.Open), typeof(TestData));

            var asset = (TestData) xmlAssetLoader.LoadAsset(assetReader, typeof(TestData));

            Assert.Equal("string", asset.Test);
        }


    }
}
