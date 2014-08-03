using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.ContentProcessor.Persistence;

namespace OpenORPG.ContentProcessor.Extractors
{
    /// <summary>
    /// A content extractor is responsible for taking a piece of content and extracting the required data, and persisting it out for client side use.
    /// This helps reduce data leak and bandwidth usage over the wire. It allows for graphical data to stay local and keep info transmitted (like secret stats)
    /// to a minimum.
    /// </summary>
    public interface IContentExtractor
    {

        void ProcessContent(IContentPersister persister);
    }



}
