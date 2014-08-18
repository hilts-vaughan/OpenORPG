using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Scripting
{

    /// <summary>
    /// A utility attribute that makes it easier for the script loader to locate certain scripts when using reflection.
    /// Allows the specification of friendly names to ease development.
    /// </summary>
    public class GameScriptAttribute : Attribute
    {
        public string ScriptName { get; private set; }

        public GameScriptAttribute(string scriptName)
        {
            ScriptName = scriptName;
        }
    }
}
