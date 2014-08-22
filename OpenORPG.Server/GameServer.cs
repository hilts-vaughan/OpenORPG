using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Server.Game;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Network;
using Server.Game.Network.Handlers;
using Server.Game.Network.Packets;
using Server.Game.Zones;
using Server.Infrastructure.Content;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Logging.Loggers;
using Server.Infrastructure.Network;
using Server.Infrastructure.Network.Handlers;
using Server.Infrastructure.Network.Packets;
using Server.Infrastructure.Quests;
using Server.Utils;

namespace Server
{
    /// <summary>
    ///     Contains entry point information and a beginning of the server application
    /// </summary>
    public class GameServer
    {
        public static GameServer Current;

        private readonly Dictionary<OpCodes, IPacketHandler> _packetHandlers = new Dictionary<OpCodes, IPacketHandler>();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public GameServer()
        {
            SetupPacketHandlers();
            var x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

        }

        /// <summary>
        ///     Sets up packet handlers for the server to begin recieving.
        /// </summary>
        private void SetupPacketHandlers()
        {
            ReflectionHelper.GetMethodsWithAttritube<PacketHandlerAttribute>((method, attribute) =>
                {
                    OpCodes opCode = attribute.OpCode;

                    Type packetType = method.GetParameters()[1].ParameterType;
                    Delegate del =
                        Delegate.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(GameClient), packetType),
                                                method);
                    var handler =
                        (IPacketHandler)
                        Activator.CreateInstance(typeof(PacketHandler<>).MakeGenericType(packetType), del);

                    _packetHandlers.Add(opCode, handler);
                });
        }

        public void Update()
        {
            TimeSpan delta = _stopwatch.Elapsed;
            ZoneManager.Instance.Update(delta);
            _stopwatch.Restart();


            GameClient client;
            while (_logoutQueue.TryDequeue(out client))
            {
                LoginHandler.Logout(client);
            }

            PacketTask task;
            while (_packetTasks.TryDequeue(out task))
            {
                IPacketHandler handler = _packetHandlers[task.Packet.OpCode];
                handler.Invoke(task.Client, task.Packet);
            }
         

        }

        public void Run()
        {
            SetupLoggers();

            ResetOnlineFlags();
            SetupManagers();


            StartZones();

            // Start our stopwatch
            _stopwatch.Start();

            Logger.Instance.Info("Game server has started succesfully.");

            while (true)
            {
                Update();

                //TODO: Remove when having a CPU core sucked up is acceptable
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Begins adding zones to the world. A zone is created per file that can be found. 
        /// </summary>
        private void StartZones()
        {

            var mapPath = Path.Combine(PathHelper.AssetBasePath, PathHelper.MapPath);
            int count = 0;
            try
            {
                var mapFiles = Directory.GetFileSystemEntries(mapPath).ToList();

                foreach (var mapFile in mapFiles)
                {
                    var filename = Path.GetFileNameWithoutExtension(mapFile);
                    long id;
                    if (long.TryParse(filename, out id) && Path.GetExtension(mapFile) == ".tmx")
                    {
                        ZoneManager.Instance.AddZone(new Zone(id));
                        count++;
                    }
                }
            }
            catch (DirectoryNotFoundException exception)
            {
                Logger.Instance.Warn("The server could not locate the map path. Zones are shutdown. \n" + exception);
            }

            Logger.Instance.Info("{0} zones were created succesfully.", count);

        }

        private static void SetupLoggers()
        {
            // Add a console logger to aid with debugging
            Logger.Instance.AddLogger(new ConsoleLogger());
        }

        private void SetupManagers()
        {
            ChatManager.Create();
            ZoneManager.Create();
            ContentManager.Create(PathHelper.AssetBasePath);
            NetworkManager.Create(4488);
            NetworkManager.Current.Initialize();
            NetworkManager.Current.Connected += OnConnection;
            NetworkManager.Current.PacketRecieved += OnPacketRecieved;
            NetworkManager.Current.Disconnected += OnDisconnected;
        }

        private static void ResetOnlineFlags()
        {
            using (var context = new GameDatabaseContext())
            {

                List<UserAccount> accounts = context.Accounts.Where(x => x.IsOnline).ToList();
                accounts.ForEach(x => x.IsOnline = false);

                context.SaveChanges();
            }

            using (var context = new GameDatabaseContext())
            {

            }



        }

        private ConcurrentQueue<GameClient> _logoutQueue = new ConcurrentQueue<GameClient>();

        private void OnDisconnected(Connection obj)
        {
            _logoutQueue.Enqueue(obj.Client);
        }

        private void OnPacketRecieved(Connection connection, IPacket packet)
        {
            // The game couuld be in absolutely any state, we'll store these for later
            _packetTasks.Enqueue(new PacketTask(connection.Client, packet));
            Logger.Instance.Trace(connection + " sent " + packet.GetType().Name);
        }

        private readonly ConcurrentQueue<PacketTask> _packetTasks = new ConcurrentQueue<PacketTask>();


        private void OnConnection(Connection connection)
        {
            var client = new GameClient(connection);
            connection.Client = client;
        }


    }
}