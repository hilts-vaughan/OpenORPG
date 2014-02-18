using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.World;

namespace Server.Infrastructure.Synchronization
{
    /// <summary>
    /// Marks a property on an <see cref="Entity"/> as being in sync with all clients.
    /// A property will not be sent the client to begin  or ever unless it's marked with this. 
    /// </summary>
    public class SyncAttribute : Attribute
    {
        /// <summary>
        /// An enumeration representing the scope in which a property is synced. 
        /// </summary>
        public enum SyncScope
        {
            Self,
            Everyone,
            EveryoneButMe
        }

        /// <summary>
        /// The scope of this property
        /// </summary>
        public SyncScope Scope { get; set; }

        public SyncAttribute()
        {
            Scope = SyncScope.Everyone;
        }

        public SyncAttribute(SyncScope scope)
        {
            Scope = scope;
        }
    }
}
