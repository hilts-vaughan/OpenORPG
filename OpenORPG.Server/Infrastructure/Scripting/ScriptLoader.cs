using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private Assembly _scriptAssembly;
        private const string SCRIPT_DLL = "OpenORPG.Server.Scripts.dll";

        protected ScriptLoader()
        {
            // Load our assembly
            _scriptAssembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, SCRIPT_DLL));
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
            // Search the assembly for all subtypes of 
            var subtypes = _scriptAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(SkillScript)));
            Type type = null;
            
            foreach (var cType in subtypes)
            {
                var attribute = cType.GetCustomAttribute<GameScriptAttribute>();

                if (attribute != null && attribute.ScriptName == scriptKey)
                    type = cType;
            }

            if (type != null)
            {
                var script = (SkillScript) Activator.CreateInstance(type);
                script.Init(skill);

                _skillScriptCache.Add(scriptKey, script);
            }
            else
            {
                _skillScriptCache.Add(scriptKey, new SkillScript());
            }


        }

        public static Type GetTypeWithAttributeValue<TAttribute>(Assembly aAssembly, Predicate<TAttribute> pred)
        {
            foreach (Type type in aAssembly.GetTypes())
            {
                foreach (TAttribute oTemp in type.GetCustomAttributes(typeof(TAttribute), true))
                {
                    if (pred(oTemp))
                    {
                        return type;
                    }
                }
            }
            return typeof(string); //otherwise return a string type
        }



    }
}
