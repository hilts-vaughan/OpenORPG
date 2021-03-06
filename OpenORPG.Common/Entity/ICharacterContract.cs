﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Common.Entity
{
    public interface ICharacterContract
    {
        int Level { get; set; }

        /// <summary>
        /// Verifies that the character contract
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        bool IsOnQuestStep(int questId, int step);
    }
}
