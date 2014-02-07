using System;

namespace Server.Infrastructure.Content.AssetLoaders
{
    /// <summary>
    ///     An asset loader designed for loading strongly typed JSON
    /// </summary>
    [AssetLoader(".json")]
    public class JsonAssetLoader : AssetLoader<object>
    {
        private readonly JsonAssetSerializer _serializer = new JsonAssetSerializer();

        /// <summary>
        ///     Handles the loading of the asset from the disk and deserialization
        /// </summary>
        /// <returns></returns>
        public override object LoadAsset(AssetReader input, Type T)
        {
            return _serializer.Deserialize(input.ReadToEnd(), input.TargetType);
        }
    }
}