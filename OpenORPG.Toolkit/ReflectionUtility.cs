using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Toolkit
{
    public static class ReflectionUtility
    {

        public static IEnumerable<Type> GetAllTypesWithSubclass<T>()
        {
            var subclasses =
               from assembly in AppDomain.CurrentDomain.GetAssemblies()
               from type in assembly.GetTypes()
               where type.IsSubclassOf(typeof(T))
               select type;

            return subclasses;
        }

    }
}
