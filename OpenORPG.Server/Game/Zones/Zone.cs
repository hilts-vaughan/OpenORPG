﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server.Game.AI;
using Server.Game.Chat;
using Server.Game.Combat;
using Server.Game.Database.Models;
using Server.Game.Dialog;
using Server.Game.Entities;
using Server.Game.Movement;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
using Server.Game.Network.Packets.Server;
using Server.Game.Quests;
using Server.Game.Zones.Spawns;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Math;
using Server.Infrastructure.Network.Packets;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;
using Server.Utils;
using Server.Utils.Math;
using TiledSharp;

namespace Server.Game.Zones
{

    public class ZoneLoginItem
    {
        public Player Player { get; set; }

        public bool Ready { get; set; }

        public ZoneLoginItem(Player player)
        {
            Player = player;
            Ready = false;
        }
    }

    public class GameClientCollection : IEnumerable<GameClient>
    {
        private readonly List<GameClient> _internalList;
        private readonly Zone _zone;

        private const int ViewWidth = 1920;
        private const int ViewHeight = 1080;

        public GameClientCollection(Zone zone)
        {
            _zone = zone;

            _internalList = new List<GameClient>();
        }

        IEnumerator<GameClient> IEnumerable<GameClient>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }


        internal void Add(GameClient client)
        {
            client.Zone = _zone;
            _internalList.Add(client);
        }

        internal void Remove(GameClient client)
        {
            _internalList.Remove(client);
            client.Zone = null;
        }

        public bool IsEmpty
        {
            get { return _internalList.Count == 0; }
        }
    }

    /// <summary>
    ///     A zone represents a single given subspace in the world that can be contained.
    ///     This is similar to a 'map' or 'area' in traditional MMO worlds.
    ///     The world within the zone contains the actual physical mainfestation and collection of objects.
    /// </summary>
    public class Zone
    {

        private Rectangle topZoneArea, bottomZoneArea, leftZoneArea, rightZoneArea;

       

        public Zone(long id)
        {
            GameClients = new GameClientCollection(this);
            ChatChannel = ChatManager.Current.CreateChannel(ChannelType.Zone);
            Id = id;


            string zonePath = Path.Combine(PathHelper.AssetBasePath, PathHelper.MapPath,
                id + PathHelper.MapExtension);
            try
            {
                TileMap = new TmxMap(zonePath);
            }
            catch (IOException exception)
            {
                // Mark the world as offline
                Available = false;

                Logger.Instance.Error(
                    "The world in zone #{0} could not be started. The tilemap could not be found. \n" +
                    "Zone path: \"{1}\"\n" + exception, id, zonePath);
            }

            AddGameSystems();

            // Create our empty slots
            ZoneExitPoints = new long[4] { -1, -1, -1, -1 };

            // We can extract and setup our exit points now
            SetupExitPoints();

            // Setup our sensors
            SetupExitRectangles();

            SpawnNpcs();
        }

        private void SetupExitRectangles()
        {
            // Create a top sensor
            topZoneArea = new Rectangle(0, 0, TileMap.Width * TileMap.TileWidth, TileMap.TileHeight + 16);

            // Bottom sensor
            bottomZoneArea = new Rectangle(0, (TileMap.Height * TileMap.TileHeight) - 48, TileMap.Width * TileMap.TileWidth,
                TileMap.TileHeight + 16);

            // Left
            leftZoneArea = new Rectangle(0, 0, TileMap.TileWidth + 16, TileMap.Height * TileMap.TileHeight);

            // Right
            rightZoneArea = new Rectangle(TileMap.Width * (TileMap.TileWidth - 1), 0, TileMap.TileWidth + 16,
                TileMap.Height * TileMap.TileHeight);

        }

        private void SetupExitPoints()
        {
            string up;
            string right;
            string down;
            string left;

            var suc = TileMap.Properties.TryGetValue("UpExitPoint", out up);
            if (suc)
                ZoneExitPoints[(int)Direction.North] = Convert.ToInt64(up);

            suc = TileMap.Properties.TryGetValue("RightExitPoint", out right);
            if (suc)
                ZoneExitPoints[(int)Direction.East] = Convert.ToInt64(right);

            suc = TileMap.Properties.TryGetValue("DownExitPoint", out down);
            if (suc)
                ZoneExitPoints[(int)Direction.South] = Convert.ToInt64(down);

            suc = TileMap.Properties.TryGetValue("LeftExitPoint", out left);
            if (suc)
                ZoneExitPoints[(int)Direction.West] = Convert.ToInt64(left);

        }

        public bool CanLeave(Direction direction, Player player)
        {

            var playerRect = new Rectangle(player.X + player.Body.OffsetX, player.Y + player.Body.OffsetY,
                player.Body.Width, player.Body.Height);

            switch (direction)
            {
                case Direction.North:
                    return topZoneArea.Intersects(playerRect);
                case Direction.East:
                    return rightZoneArea.Intersects(playerRect);
                case Direction.South:
                    return bottomZoneArea.Intersects(playerRect);
                case Direction.West:
                    return leftZoneArea.Intersects(playerRect);
            }

            return false;
        }

        private void AddGameSystems()
        {
            // Add all our various require systems to deal with logic

            GameSystems.Add(new SpawnGameSystem(this));
            GameSystems.Add(new CombatSystem(this));
            GameSystems.Add(new AiSystem(this));
            GameSystems.Add(new ZoneEntityMonitorSystem(this));
            GameSystems.Add(new LevelService(this));
            GameSystems.Add(new ChatService(this));
            GameSystems.Add(new QuestService(this));
            GameSystems.Add(new DialogService(this));
        }


        private void SpawnNpcs()
        {
            string NpcSpawnSetName = "NpcSpawns";

            // If we have this layer, parse it
            if (TileMap.ObjectGroups.Contains(NpcSpawnSetName))
            {
                var groups = TileMap.ObjectGroups[NpcSpawnSetName];

                foreach (var npcSpawn in groups.Objects)
                {
                    if (npcSpawn.Type == "NpcSpawn")
                    {
                        var npcId = Convert.ToInt64(npcSpawn.Properties["NpcId"]);

                        var x = npcSpawn.X;
                        var y = npcSpawn.Y;
                        var width = npcSpawn.Width;
                        var height = npcSpawn.Height;

                        var spawnArea = new Rectangle(x, y, width, height);

                        var npc = GameObjectFactory.CreateNpc(npcId);
                        npc.Position = new Vector2(x, y);

                        AddEntity(npc);

                    }
                }
            }
        }


        /// <summary>
        /// A collection of entities that this game world is responsible for handling.
        /// </summary>
        protected
        List<Entity> Entities = new List<Entity>();

        private readonly List<Entity> _toAdd = new List<Entity>();
        private readonly List<Entity> _toRemove = new List<Entity>();

        protected List<GameSystem> GameSystems = new List<GameSystem>();

        /// <summary>
        /// An array of zone exit point IDs that users might choose to leave by
        /// </summary>
        public long[] ZoneExitPoints { get; set; }

        /// <summary>
        /// Indicates whether or not the world is currently available. If something fatal happens which requires
        /// recovery, this flag will be set.
        /// 
        /// Once this flag is set, only a manual reset of the world can fix it.
        /// </summary>
        public bool Available { get; private set; }

        /// <summary>
        /// This is the internal representation of the world.
        /// </summary>
        public TmxMap TileMap { get; private set; }

        public IEnumerable<Character> ZoneCharacters
        {
            get
            {
                var entities = Entities.Where(x => x is Character);

                return entities.Select(entity => entity as Character).ToList();
            }
        }

        public IEnumerable<Npc> Npcs
        {
            get
            {
                var entities = Entities.Where(x => x is Npc);

                return entities.Select(entity => entity as Npc).ToList();
            }
        }



        /// <summary>
        /// Retrieves a particular game system from the world for usage.
        /// </summary>
        /// <typeparam name="T">The type of game system to retrieve from the collection</typeparam>
        /// <returns></returns>
        public T GetGameSystem<T>() where T : GameSystem
        {
            return (T)GameSystems.First(sys => sys.GetType() == typeof(T));
        }

        List<Player> _pendingLogin = new List<Player>();

        public void QueueLogin(Player player)
        {
            _pendingLogin.Add(player);

            // Assign the world so we know where they belong
            player.Zone = this;
        }

        public void TryAndAddPlayer(Player player)
        {
            //TODO: We should probably clean this up if they're idle too long or disconnect

            var found = _pendingLogin.FirstOrDefault(x => x.Id == player.Id);

            if (found != null)
                AddEntity(player);
            else
                Logger.Instance.Warn("Attempted to login a player that wasn't waiting in queue.");
        }

        public void AddEntity(Entity entity)
        {
            entity.Zone = this;
            _toAdd.Add(entity);
            NotifySystemsAdd(entity);
        }

        private void NotifySystemsAdd(Entity entity)
        {
            foreach (GameSystem system in GameSystems)
                system.OnEntityAdded(entity);


        }

        private void NotifySystemsRemove(Entity entity)
        {
            foreach (GameSystem system in GameSystems)
                system.OnEntityRemoved(entity);

 
        }

        /// <summary>
        /// This is invoked when a new player is added so the according steps can be taken.
        /// </summary>
        /// <param name="player"></param>
        private void ProcessNewPlayer(Player player)
        {
            
            // TODO: Hack... doesn't make much sense but is required to send the intital state
            player.CharacterState = CharacterState.Moving;
            player.CharacterState = CharacterState.Idle;

            // Before the player can be notified, let's change if their position is legal
            if (!IsPositionLegal(player.Position))
            {
                // If it's illegal, we need to move them as soon as we can
                player.Position = GetLegalPosition();
            }

            // Notify the player about this change
            var packet = new ServerZoneChangedPacket(Id, player.Id, Entities);
            player.Client.Send(packet);

            OnClientEnter(player.Client, player);
        }

        private bool IsPositionLegal(Vector2 position)
        {
            if(position.X < 0 || position.Y < 0)
                return false;

            if (position.X > TileMap.Width*TileMap.TileWidth)
                return false;

            if (position.Y > TileMap.Height*TileMap.TileHeight)
                return false;

            return true;

        }

        private Vector2 GetLegalPosition()
        {
            // Find a safe spot...
            for (int x = 0; x < TileMap.Width; x++)
            {
                for (int y = 0; y < TileMap.Height; y++)
                {
                    if (!TileMap.BlockMap[x, y])
                    {
                        return new Vector2(x * TileMap.TileWidth, y * TileMap.TileHeight);
                    }                    
                }
            }

            // Set to map middle.. what else can you do? Flag a warning
            Logger.Instance.Error("Attempted to find a legal position but could not find one. Used map middle instead. #{0}", Id);
            return new Vector2(TileMap.Width/2 * TileMap.TileWidth, TileMap.Height/2 * TileMap.TileHeight);

        }

        public void RemoveEntity(Entity entity)
        {
            entity.Zone = null;
            _toRemove.Add(entity);
            NotifySystemsRemove(entity);
        }

        private double _timer = 0f;

        public void Update(double deltaTime)
        {




            // Remove and add elements that need to be

            foreach (var entity in _toRemove)
            {
                ProcessRemovedEntity(entity);
                Entities.Remove(entity);

            }



            foreach (var entity in _toAdd)
            {
                ProcessAddedEntity(entity);
                Entities.Add(entity);

                if (entity is Player)
                    ProcessNewPlayer(entity as Player);

            }

            _toRemove.Clear();
            _toAdd.Clear();

            SyncEntityProperties();



            // Update each game system
            GameSystems.ForEach(x => x.Update(deltaTime));
        }

        private void ProcessAddedEntity(Entity entity)
        {
            var packet = new ServerMobCreatePacket(entity);

            if (entity is Player)
                SendToEveryoneBut(packet, entity as Player);
            else
                SendToEveryone(packet);
        }



        private void ProcessRemovedEntity(Entity entity)
        {
            if (entity is Player)
                OnClientLeave((entity as Player).Client);

            var packet = new ServerMobDestroyPacket(entity.Id);
            SendToEveryone(packet);
        }

        private void SyncEntityProperties()
        {
            // Check for syncing of packets
            foreach (var entity in Entities)
            {
                var properties = entity.GetSyncProperties();

                if (properties != null)
                {
                    var packet = new ServerEntityPropertyChange(properties, entity.Id);
                    SendToEveryone(packet);
                }
            }
        }

        /// <summary>
        /// Sends a packet to the entire zone.
        /// </summary>
        /// <param name="packet">The packet to send to the zone</param>
        public void SendToEveryone(IPacket packet)
        {
            foreach (var client in GameClients)
            {
                client.Send(packet);
            }
        }

        private void SendToEveryoneBut(IPacket packet, Player player)
        {
            foreach (var client in GameClients)
            {
                if (client.HeroEntity.Id != player.Id)
                    client.Send(packet);
            }
        }

        /// <summary>
        /// Sends a packet to all clients in range of the source. 
        /// This is useful for information that only needs to be broadcast to some.
        /// </summary>
        /// <param name="packet">The packet to send to the clients</param>
        /// <param name="source">The source entity we check all other clients against</param>
        public void SendToEntitiesInRange(IPacket packet, Entity source)
        {
            foreach (var client in GameClients.Where(client => client.HeroEntity.IsInView(source)))
            {
                client.Send(packet);
            }
        }



        /// <summary>
        /// Sends a packet to all clients in range of the source, excluding the source.
        /// </summary>
        /// <param name="packet">The packet to be sent to the clients.</param>
        /// <param name="source">The source entity that is checked against for ranges.</param>
        public void SendToEntitiesInRangeExcludingSource(IPacket packet, Entity source)
        {
            foreach (var client in GameClients.Where(client => client.HeroEntity.IsInView(source) && (client.HeroEntity.Id != source.Id)))
            {
                client.Send(packet);
            }

        }

        /// <summary>
        ///     The name of the actual zone
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     the unique ID that belongs to this particular zone
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     A read-only copy of the world, it cannot be over-written.
        /// </summary>
        public GameWorld World { get; private set; }

        /// <summary>
        ///     A collection of <see cref="GameClient" />s that are a part of this zone.
        /// </summary>
        public GameClientCollection GameClients { get; private set; }


        /// <summary>
        /// A reference to the chat channel this zone maintains, used for communication purposes
        /// </summary>
        public ChatChannel ChatChannel { get; set; }

        /// <summary>
        /// Returns true when the zone has no players, otherwise false.
        /// This is typically used to protect systems from performing costly updates when nobody is actually occupying a zone.
        /// </summary>
        public bool IsEmpty
        {
            get { return GameClients.IsEmpty; }
        }


        protected void OnClientLeave(GameClient client)
        {
            // Get the player
            var player = client.HeroEntity;



            GameClients.Remove(client);
            ChatChannel.Leave(client);

        }



        protected void OnClientEnter(GameClient client, Player heroEntity)
        {
            GameClients.Add(client);
            ChatChannel.Join(client);


            string name = heroEntity.Name;

            // Send inventory and stuff
            var outboundPacket = new ServerSendHeroStoragePacket(heroEntity.Backpack, StorageType.Inventory);
            client.Send(outboundPacket);

            // Send skills list
            // Notify the player that they learned this skill, send it over
            var packet = new ServerSkillChangePacket(heroEntity.Skills);
            heroEntity.Client.Send(packet);

            var questUpdate = new ServerSendQuestListPacket(heroEntity.QuestLog);
            heroEntity.Client.Send(questUpdate);


            // Send equipment
            foreach (var equipment in heroEntity.Equipment)
            {
                if (equipment == null)
                    continue;

                var request = new ServerEquipmentUpdatePacket(equipment, equipment.Slot);
                heroEntity.Client.Send(request);
            }



            Logger.Instance.Info("{0} has entered the zone {1} [#{2}]", name, Name, Id);

        }



    }

}