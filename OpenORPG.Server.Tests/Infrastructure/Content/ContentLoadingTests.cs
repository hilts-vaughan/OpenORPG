using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Content;
using Xunit;

namespace OpenORPG.Server.Tests.Content
{
    /// <summary>
    /// Contains tests that test the content manager and loading of content.
    /// Usually, this involves testing the <see cref="ContentManager"/> class.
    /// </summary>
    public class ContentLoadingTests
    {
        /// <summary>
        /// Confirms that loading a piece of unsupported content throws an exception
        /// </summary>
        [Fact]
        public void ContentLoadShouldThrowExceptionForBadExtension()
        {
            ContentManager.Create("Data");
            var ex = Record.Exception(() => { ContentManager.Current.Load<dynamic>("SomeAsset.extnotsupported"); });

            Assert.IsType(typeof (NotSupportedException), ex);
        }

        [Fact]
        public void ContentLoadShouldThrowExceptionForFileNotFound()
        {
            ContentManager.Create("Data");
            var ex = Record.Exception(() => { ContentManager.Current.Load<dynamic>("SomeAsset.json"); });

            Assert.IsType(typeof(FileNotFoundException), ex);

        }



    }
}
