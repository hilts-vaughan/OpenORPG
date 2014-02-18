using System;
using System.Collections.Generic;
using System.IO;
using Server.Infrastructure.Network;

namespace Server.Infrastructure.Content
{
    /// <summary>
    ///     A content manager is responsible for loading data and creating strongly typed defintions from them that are usable in the code.
    ///     This keeps loading code in a neat and accessible place.
    /// </summary>
    public class ContentManager
    {
        // A list of internal asset loaders used to load files
        private readonly Dictionary<string, IAssetLoader> _assetLoaders = new Dictionary<string, IAssetLoader>();

        public string Root;

        private ContentManager(string root)
        {
            Root = root;

            ReflectionHelper.GetTypesWithAttribute<AssetLoaderAttribute>((type, attribute) =>
                {
                    foreach (string extension in attribute.Extensions)
                    {
                        _assetLoaders.Add(extension, (IAssetLoader)Activator.CreateInstance(type));
                    }
                });
        }

        /// <summary>
        ///     The current instance of a singleton
        /// </summary>
        public static ContentManager Current { get; set; }

        /// <summary>
        ///     Creates a copy of this manager for use
        /// </summary>
        public static void Create(string root)
        {
            Current = new ContentManager(root);
        }

        /// <summary>
        ///     Loads a particular piece of content and returns the object modelling it
        /// </summary>
        /// <typeparam name="T">The type of object expected to be returned</typeparam>
        /// <returns></returns>
        public T Load<T>(string assetName)
        {
            // TODO: Implement the AssetCache class with proper cloning

            string fileExtension = Path.GetExtension(assetName);

            IAssetLoader assetLoader;
            _assetLoaders.TryGetValue(fileExtension, out assetLoader);



            if (assetLoader == null)
                throw new NotSupportedException(
                    "The given extension type at the file path was not supported by the content manager. Consider implementing an AssetLoader for it.");

            Stream stream = File.OpenRead(Path.Combine(Root, assetName));

            var input = new AssetReader(stream, typeof(T));

            return (T)_assetLoaders[fileExtension].LoadAsset(input, typeof(T));
        }
    }
}