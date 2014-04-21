using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Entities
{
    /// <summary>
    /// Represents a state that a character can be in. These states help keep track of what
    /// is currently going on in the world
    /// </summary>
    public enum CharacterState
    {
        /// <summary>
        /// This state indicates that a <see cref="Character"/> is currently idle in the world.
        /// 
        /// It possible to change to most states from this one.
        /// </summary>
        Idle = 0,
        
        /// <summary>
        /// This state indicates that a <see cref="Character"/> is currently moving in the world
        /// </summary>
        Moving = 1,

        /// <summary>
        /// This state indicates that a <see cref="Character"/> is currently executing a skill in the world.
        /// </summary>
        UsingSkill = 2,
    }
}
