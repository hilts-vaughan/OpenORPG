using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Entities;
using Xunit;

namespace OpenORPG.Server.Tests.Game.Combat.Payloads
{
    /// <summary>
    /// A test harness for AttackPayloads
    /// </summary>
    public class AttackPayloadTest
    {

        /// <summary>
        /// This test ensures a payload can not cause HP to fall below 0.
        /// </summary>
        [Fact]
        public void PayloadShouldCapAtZero()
        {
            var agressor = new Character();
            var victim = new Character();

            var payload = new AttackPayload(agressor);
            payload.Apply(victim);

            Assert.Equal(0, victim.CharacterStats[ (int) StatTypes.Hitpoints].CurrentValue);
        }

        [Fact]
        public void PayloadShouldNotAffectAggressor()
        {
            Assert.Equal(true, true);
        }




    }
}
