using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Content;
using Xunit;

namespace OpenORPG.Server.Tests.Content
{
    /// <summary>
    /// A test harness class for testing out the <see cref="AssetCache"/> and ensuring it has proper behavior.
    /// </summary>
    public class AssetCacheTests
    {

        /// <summary>
        /// Confirms that the asset cache returns a value that is null when an asset cannot be found.
        /// </summary>
        [Fact]
        public void TestMissingAssetReturnsNull()
        {
            var assetCache = new AssetCache();
            var asset = assetCache.GetAsset("anasset");

            Assert.Equal(null, asset);
        }

        
        /// <summary>
        /// Confirms that an asset thrown into the asset cache can be extracted as a whole new object.
        /// </summary>
        public void TestReturnsClonedAssetWhenFetched()
        {
            var assetCache = new AssetCache();
            var asset = assetCache.GetAsset("anasset");
        }


    }
}
