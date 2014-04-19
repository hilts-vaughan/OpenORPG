using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.AI
{
    /// <summary>
    /// A basic, just for fun AI.
    /// </summary>
    public class BasicAi : AiBase
    {
        public BasicAi(Character character) : base(character)
        {
        }

        protected override void PerformUpdate(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
