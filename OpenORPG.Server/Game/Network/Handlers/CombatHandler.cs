using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
using Server.Infrastructure.Network.Handlers;
using ServiceStack;

namespace Server.Game.Network.Handlers
{
    public class CombatHandler
    {

        [PacketHandler(OpCodes.CMSG_USE_SKILL)]
        public static void OnHeroSkillUse(GameClient client, ClientUseSkillPacket packet)
        {
            var hero = client.HeroEntity;
            hero.UseSkill(packet.SkillId, packet.TargetId);
        }

        [PacketHandler(OpCodes.CMSG_ENTITY_TARGET)]
        public static void OnHeroTarget(GameClient client, ClientTargetEntityPacket packet)
        {
            var hero = client.HeroEntity;
            var zone = hero.Zone;

            // Attempt to find target in zone
            var target = zone.ZoneCharacters.FirstOrDefault(x => x.Id == packet.EntityId);


            if (target != null)
                hero.SelectTarget(target);
        }


    }
}
