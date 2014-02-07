using System;

namespace Server.Infrastructure.Content
{
    /// <summary>
    ///     Defines a strong interface for an asset laoder
    /// </summary>
    public interface IAssetLoader
    {
        object LoadAsset(AssetReader input, Type t);
    }

    public abstract class AssetLoader<T> : IAssetLoader
    {
        object IAssetLoader.LoadAsset(AssetReader input, Type T)
        {
            return LoadAsset(input, T);
        }

        public abstract T LoadAsset(AssetReader input, Type T);
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class AssetLoaderAttribute : Attribute
    {
        public readonly string[] Extensions;

        public AssetLoaderAttribute(params string[] extensions)
        {
            Extensions = extensions;
        }
    }
}