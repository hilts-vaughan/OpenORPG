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

        public static IEnumerable<Type> GetAllTypesThatImplement<T>()
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            return types;
        }

    }
}
