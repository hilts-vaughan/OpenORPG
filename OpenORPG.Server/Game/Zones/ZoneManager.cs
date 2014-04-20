using System;
using System.Collections.Generic;
using System.Linq;
using Server.Game.Entities;
using Server.Utils.Math;

namespace Server.Game.Zones
{
    /// <summary>
    ///     A manager dedicated to handling operations performed on all <see cref="Zones" />s.
    ///     A zone manager is exclusively used for managing these.
    /// </summary>
    public class ZoneManager
    {
        // A list of zones this manager is currently responsbile for
        private readonly List<Zone> _zones = new List<Zone>();
        private readonly List<Zone> _zonesToAdd = new List<Zone>();
        private readonly List<Zone> _zonesToRemove = new List<Zone>();

        /// <summary>
        ///     The current instance of the zone manager, readonly.
        /// </summary>
        public static ZoneManager Instance { get; private set; }

        public int ZoneCount
        {
            get { return _zones.Count; }
        }

        public static void Create()
        {
            if (Instance != null)
                throw new Exception("Creating the ZoneManager twice is not allowed. Only initialize it once.");

            Instance = new ZoneManager();
        }

        /// <summary>
        ///     Adds a zone to the <see cref="ZoneManager" /> and begins updating it next frame.
        ///     Additions are queued and only added after the current tick.
        /// </summary>
        /// <param name="zone"></param>
        public void AddZone(Zone zone)
        {
            _zonesToAdd.Add(zone);
        }

        /// <summary>
        ///     Removes a zone from the <see cref="ZoneManager" /> and removes it the next frame.
        ///     Removals are queued and only removed after the current tick.
        ///     For safety reasons, an exception will be thrown if any <see cref="NetStateComponent" /> are
        ///     left inside the zone upon removal.
        /// </summary>
        /// <param name="zone"></param>
        public void RemoveZone(Zone zone)
        {
            //TODO: Check for outstanding NetStates
            _zonesToRemove.Add(zone);
        }

        /// <summary>
        /// Switches a player from their current zone to one specified.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="zone">The zone to switch the player into</param>
        /// <param name="position"></param>
        public void SwitchToZoneAndPosition(Player player, Zone zone, Vector2 position)
        {
            // Remove our player from here
            player.Zone.RemoveEntity(player);

            // Add them to the new zone and move them
            zone.AddEntity(player);
            player.Position = position;
        }

        /// <summary>
        /// Returns the zone with the given ID.
        /// If it cannot be found, null is returned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Zone FindZone(long id)
        {
            return _zones.FirstOrDefault(z => z.Id == id);
        }

        public void Update(TimeSpan deltaTime)
        {
            ProcessQueue();

            foreach (Zone item in _zones)
                item.Update(deltaTime);
        }

        private void ProcessQueue()
        {
            foreach (var toAdd in _zonesToAdd)
                _zones.Add(toAdd);

            foreach (var toRemove in _zonesToRemove)
                _zones.Remove(toRemove);

            _zonesToAdd.Clear();
            _zonesToRemove.Clear();
        }
    }
}