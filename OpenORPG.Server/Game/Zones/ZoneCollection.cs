using System.Collections;
using System.Collections.Generic;

namespace Server.Game.Zones
{
    /// <summary>
    ///     A collection of zones that can be examined and manipulated behind the hood.
    /// </summary>
    public class ZoneCollection : IEnumerable<Zone>
    {
        private readonly List<Zone> _zones = new List<Zone>();
        private List<Zone> _zonesToAdd = new List<Zone>();
        private List<Zone> _zonesToRemove = new List<Zone>();

        public IEnumerator<Zone> GetEnumerator()
        {
            return _zones.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _zones.GetEnumerator();
        }

        public void Add(Zone zone)
        {
            _zones.Add(zone);
        }

        public void Remove(Zone zone)
        {
            _zones.Remove(zone);
        }

        /// <summary>
        ///     Commits the changes zone to the zones. This is only required after adding and removing zones.
        ///     It will be called automatically by the manager, there is no need to call this more than once.
        ///     In fact, it's harmful to do so.
        /// </summary>
        public void Commit()
        {
        }
    }
}