using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game;
using Server.Game.Entities;
using Server.Infrastructure.World;
using Server.Utils.Math;
using Xunit;

namespace OpenORPG.Server.Tests.Infrastructure.World
{
    /// <summary>
    /// A test harness for testing various <see cref="Entity"/>
    /// </summary>
    public class EntityTests
    {

        /// <summary>
        /// Confirms that the entity IsInViewMethod works correctly for entities being contained
        /// </summary>
        [Fact]
        public void TestIsInViewDetectsInBoundsCorrectly()
        {
            var entity = new DummyEntity("");
            entity.Position = new Vector2(0, 0);

            var entity2 = new DummyEntity("");
            entity2.Position = new Vector2(1920, 1079);

            Assert.Equal(true, entity.IsInView(entity2));
        }

        /// <summary>
        /// Confirms that the entity IsInViewMethod works correctly for entities not being contained
        /// </summary>
        [Fact]
        public void TestIsInViewDetectsOutOfBoundsCorrectly()
        {
            var entity = new DummyEntity("");
            entity.Position = new Vector2(0, 0);

            var entity2 = new DummyEntity("");
            entity.Position = new Vector2(1931, 1085);

            Assert.Equal(false, entity.IsInView(entity2));
        }



    }
}
