using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.ContentProcessor.Persistence
{
    /// <summary>
    /// A persistance interface used to persist content to the disk.
    /// </summary>
    public interface IContentPersister
    {

        /// <summary>
        /// Persists the given content
        /// </summary>
        /// <param name="content"></param>
        void Persist(object content, string fileName);

    }
}
