using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Network.Packets;
using Server.Infrastructure.Network.Handlers;
using ServiceStack;

namespace Server.Game.Network.Handlers
{
    public class CombatHandler
    {

        [PacketHandler(OpCodes.CMSG_USE_SKILL)]
        public static void OnHeroSkillUse(GameClient client, ClientUseSkillPacket packet)
        {
            var zone = client.HeroEntity.Zone;
            var combatSystem = zone.GetGameSystem<CombatSystem>();

            // Pipe the request to the right system on the given zone
            combatSystem.ProcessCombatRequest(client.HeroEntity, packet);

        }


    }
}
