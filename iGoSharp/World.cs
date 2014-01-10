using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iGoSharp
{
    internal static class Extension
    {
        public static Dictionary<Key, List<Value>> ToListDictionary<InValue, Key, Value>(this IEnumerable<InValue> elements, Func<InValue,Key> keyFunc, Func<InValue, Value> valueFunc)
        {
            var dictionary = new Dictionary<Key, List<Value>>();

            foreach (var element in elements)
            {
                (dictionary[keyFunc(element)] = dictionary[keyFunc(element)] ?? new List<Value>()).Add(valueFunc(element));
            }

            return dictionary;
        }
    }

    public class World
    {
        private readonly Type _interfaceFromInterfaceLibrary;
        private readonly Type _classFromClassLibrary;
        private Dictionary<Type, InterfaceInfo> _interfaces;
        private IEnumerable<ClassInfo> _classes;

        public World(Type interfaceFromInterfaceLibrary, Type classFromClassLibrary)
        {
            _interfaceFromInterfaceLibrary = interfaceFromInterfaceLibrary;
            _classFromClassLibrary = classFromClassLibrary;


            _interfaces = _interfaceFromInterfaceLibrary
                .Assembly.GetTypes().Where(t => t.IsInterface)
                .Select(i => new InterfaceInfo
                {
                    Interface = i, 
                    Methods = i.GetMethods(), 
                    Properties = i.GetProperties()
                })
                .ToDictionary(i => i.Interface);

            _classes = _classFromClassLibrary
                .Assembly.GetTypes().Where(t => t.IsClass)
                .Select(c => new ClassInfo
                {
                    Class = c, 
                    Methods = c.GetMethods(), 
                    Properties = c.GetProperties()
                }).ToArray();

            var methods = _interfaces
                .Values.SelectMany(i => i
                    .Methods.Select(m => new
                    {
                        Interface = i, 
                        Method = m
                    }))
                .ToListDictionary(i => i.Method, i => i);

            var properties = _interfaces
                .Values.SelectMany(i => i.Properties
                    .Select(p => new
                    {
                        Interface = i,
                        Property = p
                    }))
                .ToListDictionary(i => i.Property, i => i);

//            _classes
//                .Select(c => c.Methods
//                    .Select(m => new{ Method = m, Class = c }) )
//                .SelectMany(i => i)
//                .Select(i => i.Class.Properties
//                    .Select(p => new { Property = p, Class = i.Class, Method = i.Method }))
//                .SelectMany(i => i).Where(flatClassInfo => )


        }



    }

    public class ClassInfo
    {
        public Type Class { get; set; }
        public MethodInfo[] Methods { get; set; }
        public PropertyInfo[] Properties { get; set; }
    }

    public class InterfaceInfo
    {
        public Type Interface { get; set; }
        public MethodInfo[] Methods { get; set; }
        public PropertyInfo[] Properties { get; set; }
    }
}
