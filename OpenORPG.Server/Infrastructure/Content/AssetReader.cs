using System;
using System.IO;

namespace Server.Infrastructure.Content
{
    public class AssetReader : StreamReader
    {
        public Type TargetType;

        public AssetReader(Stream stream, Type targetType) : base(stream)
        {
            TargetType = targetType;
        }
    }
}