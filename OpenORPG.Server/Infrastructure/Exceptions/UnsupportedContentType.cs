using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Infrastructure.Content;

namespace Server.Infrastructure.Exceptions
{
    /// <summary>
    /// An exception thrown when a a particular piece of content is attempted to be loaded from <see cref="ContentManager"/> but
    /// is not supported by the platform. 
    /// </summary>
    public class UnsupportedContentType : Exception
    {
        public string UnsupportedPath { get; set; }

        public UnsupportedContentType(string message, string path)
            : base(message)
        {
            UnsupportedPath = path;
        }

    }
}
