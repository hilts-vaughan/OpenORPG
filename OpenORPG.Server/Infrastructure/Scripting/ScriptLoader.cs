using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Infrastructure.Scripting.Combat;

namespace Server.Infrastructure.Scripting
{

    /// <summary>
    /// A generic script loader which is capable of fetching and retrieving scripts from a cache.
    /// </summary>
    public class ScriptLoader
    {
        // A cache of skill scripts
        private Dictionary<string, SkillScript> _skillScriptCache = new Dictionary<string, SkillScript>();

        private static ScriptLoader _instance;

        protected ScriptLoader()
        {

        }

        public static ScriptLoader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScriptLoader();
                return _instance;
            }
        }


        public SkillScript GetSkillScript(Skill skill)
        {
            // If we can fetch from the cache directly, do so fast
            var scriptKey = skill.SkillTemplate.Script;

            // If the script key is null, force a blank key to be used
            if (scriptKey == null)
                scriptKey = "";

            if (!_skillScriptCache.ContainsKey(scriptKey))
                PopulateSkillCache(skill, scriptKey);
            return _skillScriptCache[scriptKey];
        }



        private void PopulateSkillCache(Skill skill, string scriptKey)
        {
            _skillScriptCache.Add(scriptKey, new SkillScript(skill));
        }

    }
}
