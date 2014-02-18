using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Synchronization;
using Xunit;

namespace OpenORPG.Server.Tests.Infrastructure.Synchronization
{
    /// <summary>
    /// Provides tests for the <see cref="SyncMonitor"/> class to test the implementation.
    /// </summary>
    public class SyncMonitorTests
    {
              
        /// <summary>
        /// Confirms that a flush returns a null set when there's nothing to sync.
        /// </summary>
        [Fact]
        public void FlushShouldReturnNullWhenEmpty()
        {
            var monitor = new SyncMonitor();
            var results = monitor.GetAndFlushValues();

            Assert.Equal(null, results);
        }

        /// <summary>
        /// Confirms that a flush returns a proper set of properties when called
        /// </summary>
        [Fact]
        public void FlushShouldReturnResultsWhenNotEmpty()
        {
            var monitor = new SyncMonitor(); 
            monitor.WriteValue("TestValue", 0);

            var results = monitor.GetAndFlushValues();

            Assert.NotEqual(null, results);
        }

        /// <summary>
        /// Confirms that the list of properties is cleared when a flush is performed
        /// </summary>
        [Fact]
        public void FlushShouldClearSyncListWhenCalled()
        {
            var monitor = new SyncMonitor();
            monitor.WriteValue("TestValue", 0);
            monitor.GetAndFlushValues();

            // Check that the internal list state is empty after this
            Assert.Equal(0, monitor.SyncValues.Count);
        }


        /// <summary>
        /// Verifies that an entry is created when it does not exist.
        /// </summary>
        [Fact]
        public void WriteValueAddsEntryWhenDoesNotExist()
        {
            var monitor = new SyncMonitor();
            monitor.WriteValue("test", 0);

            Assert.Equal(0, monitor.SyncValues["test"]);
        }

        /// <summary>
        /// Confirms that an entry is updated if written when it already exists
        /// and that no duplicate entries are generated.
        /// </summary>
        [Fact]       
        public void WriteValueModifiesEntryWhenExists()
        {
            var monitor = new SyncMonitor();
            monitor.WriteValue("test", 0);
            monitor.WriteValue("test", 1);

            Assert.Equal(1, monitor.SyncValues["test"]);
            Assert.Equal(1, monitor.SyncValues.Count);
        }



    }
}
