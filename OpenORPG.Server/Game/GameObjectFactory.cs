using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Game.Network.Handlers;
using Server.Game.Network.Packets;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace Server.Game
{
    /// <summary>
    ///     This factory provides methods for quickly creating objects that are frequently used throughout the server
    ///     This allows quick construction of game objects that are used frequently.
    /// </summary>
    public static class GameObjectFactory
    {


        public static Player CreateHero(UserHero userHero, GameClient gameClient)
        {
            var entity = new Player(gameClient);
            entity.Position = new Vector2(userHero.PositionX, userHero.PositionY);
            entity.Name = userHero.Name;

            return entity;
        }

        public static Npc CreateMonster(ulong id)
        {
            //TODO: Please create me
            return null;
        }

    }
}