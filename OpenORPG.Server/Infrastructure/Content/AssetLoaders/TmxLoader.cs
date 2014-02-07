using Server.Infrastructure.Content.Tilemaps;

namespace Server.Infrastructure.Content.AssetLoaders
{
    /// <summary>
    ///     An asset loader designed for loading strongly typed TMX maps.
    ///     This is used when loading tilemaps into the server for AI processing and the like.
    /// </summary>
    [AssetLoader(".tmx")]
    public class TmxLoader : IAssetLoader
    {
        public object LoadAsset(AssetReader input)
        {
            return new TmxMap(input.BaseStream);
        }
    }
}