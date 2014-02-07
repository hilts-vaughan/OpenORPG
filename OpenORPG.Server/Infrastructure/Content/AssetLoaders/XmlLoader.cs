using System;
using System.Xml.Serialization;

namespace Server.Infrastructure.Content.AssetLoaders
{
    /// <summary>
    ///     An asset loader designed for loading strongly typed XML from an <see cref="AssetReader" />
    /// </summary>
    [AssetLoader(".xml")]
    public class XmlAssetLoader : AssetLoader<object>
    {
        /// <summary>
        ///     Handles the loading of the asset from the disk and deserialization
        /// </summary>
        /// <returns></returns>
        public override object LoadAsset(AssetReader input, Type type)
        {
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(input.BaseStream);
        }
    }
}