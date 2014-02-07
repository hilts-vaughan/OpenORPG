using System.Collections.Generic;

namespace Server.Infrastructure.Content
{
    /// <summary>
    ///     Contains storage for various pieces of content.
    ///     This is a basic implementation of a cache, it will retain everything in memory until the program is terminated.
    ///     Most assets are classes and refernce types, so it
    /// </summary>
    public class AssetCache
    {
        private readonly Dictionary<string, object> _cachedAssets = new Dictionary<string, object>();

        /// <summary>
        ///     Adds an asset to the cache, preparing it for usage.
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="assetObject"></param>
        public void AddAsset(string assetName, object assetObject)
        {
            _cachedAssets.Add(assetName, assetObject);
        }

        /// <summary>
        ///     Retrieves an asset from the cache if one exists, otherwise null.
        /// </summary>
        /// <param name="assetName">The asset to get</param>
        /// <returns>The asset if it exists, null otherwise.</returns>
        public object GetAsset(string assetName)
        {
            if (_cachedAssets.ContainsKey(assetName))
                return _cachedAssets[assetName];
            return null;
        }
    }
}