using System.ComponentModel;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Movement
{
    /// <summary>
    /// A movement system is responsible for handling all movement of entities throughout the game.
    /// 
    /// This includes:
    /// 
    /// * Requests to be moved in the game world
    /// * Teleportation around the world
    /// * Knockbacks that may occur or need to be observed
    /// </summary>
    public class MovementSystem : GameSystem 
    {


        public MovementSystem(GameWorld world) : base(world)
        {
        }

        public override void Update(float frameTime)
        {
            
        }

        public override void OnEntityAdded(Entity entity)
        {

        }


        public override void OnEntityRemoved(Entity entity)
        {
  
        }

    }
}
