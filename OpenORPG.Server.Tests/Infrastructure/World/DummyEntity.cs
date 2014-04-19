using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace OpenORPG.Server.Tests.Infrastructure.World
{
    public class DummyEntity : Entity 
    {
        public DummyEntity(string sprite) : base(sprite)
        {
        }
    }
}
