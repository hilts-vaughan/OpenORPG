using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Infrastructure.Dialog.Conditions
{
    /// <summary>
    /// A public contract specifying in which cases a certain <see ref="DialogLink"/> may be available.
    /// </summary>
    public interface IDialogCondition
    {

        /// <summary>
        /// Performs a verification on the activating player to ensure they are qualified to interact with this
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        bool Verify(Player player);

    }
}
