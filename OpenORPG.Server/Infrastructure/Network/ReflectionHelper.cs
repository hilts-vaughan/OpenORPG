using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Server.Infrastructure.Network
{


    internal class ReflectionHelper
    {
        public static void GetTypesWithAttribute<T>(Action<Type, T> action) where T : Attribute
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                var attribute = (T) type.GetCustomAttributes(typeof (T), false).SingleOrDefault();
                if (attribute != null)
                {
                    action(type, attribute);
                }
            }
        }

        public static void GetMethodsWithAttritube<T>(Action<MethodInfo, T> action) where T : Attribute
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    var attribute = (T) method.GetCustomAttributes(typeof (T), false).SingleOrDefault();
                    if (attribute != null)
                    {
                        action(method, attribute);
                    }
                }
            }
        }
    }
}