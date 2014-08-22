using System.IO;

namespace Server.Utils
{
    /// <summary>
    ///     Maintains paths for certain assets for making seeking to certain directories easier and less hardcoded.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        ///     This is the main asset path; it isn't advised to reference this directly as it will create nasty hard coded links
        ///     //TODO: We should set the server to copy its own files at some point but for now using the clients will suffice
        /// </summary>
        public static readonly string AssetBasePath = "..\\..\\..\\..\\OpenORPG.TypeScriptClient\\assets\\";


        // These paths are not releative to the AssetBasePath because the content loader already assumes the base asset path as the working directory
        static PathHelper()
        {
            AssetBasePath = Path.Combine("..", "..", "..", "..", "OpenORPG.TypeScriptClient", "assets");
        }

        public static string MapExtension
        {
            get { return ".TMX"; }
        }


        /// <summary>
        ///     A path to the maps folder
        /// </summary>
        public static string MapPath
        {
            get { return "Maps"; }
        }

        public static string SpritesPath
        {
            get { return "sprites"; }
        }
    }
}