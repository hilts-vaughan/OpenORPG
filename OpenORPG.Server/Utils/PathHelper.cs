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
        public const string AssetBasePath = "..\\..\\..\\..\\DevContent\\";


        // These paths are not releative to the AssetBasePath because the content loader already assumes the base asset path as the working directory


        public static string MapExtension
        {
            get { return ".TMX";  }
        }

        /// <summary>
        ///     A path to the attributes folder
        /// </summary>
        public static string AttributesPath
        {
            get { return "Attributes\\"; }
        }

        /// <summary>
        ///     A path to the maps folder
        /// </summary>
        public static string MapPath
        {
            get { return "Maps\\"; }
        }
    }
}