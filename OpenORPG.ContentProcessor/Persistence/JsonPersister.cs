using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace OpenORPG.ContentProcessor.Persistence
{
    public class JsonPersister : IContentPersister
    {
        private string _path;

        public JsonPersister(string filePath)
        {
            _path = filePath;
        }

        public void Persist(object content, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path + path) );

            using (var fs = new FileStream(_path + path, FileMode.Create))
            {
                JsonSerializer.SerializeToStream(content, fs);
            }
        }

     
    }
}
