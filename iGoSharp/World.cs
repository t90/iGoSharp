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
                List<Value> valueList;
                if (!dictionary.TryGetValue(keyFunc(element), out valueList))
                {
                    valueList = new List<Value>();
                    dictionary.Add(keyFunc(element), valueList);
                }
                valueList.Add(valueFunc(element));
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
        private Dictionary<MethodInfo, List<InterfaceInfo>> _methods;
        private Dictionary<PropertyInfo, List<InterfaceInfo>> _properties;

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

            _methods = _interfaces
                .Values.SelectMany(i => i
                    .Methods.Select(m => new
                    {
                        Interface = i, 
                        Method = m
                    }))
                .ToListDictionary(i => i.Method, i => i.Interface);

            _properties = _interfaces
                .Values.SelectMany(i => i.Properties
                    .Select(p => new
                    {
                        Interface = i,
                        Property = p
                    }))
                .ToListDictionary(i => i.Property, i => i.Interface);


        }

        public static Dictionary<string, List<Type>> GetMethodNameToTypes(Type[] inTypes)
        {
            return 
            inTypes
                .Select(t => new {Type = t, Methods = t.GetMethods()})
                .SelectMany(i => i.Methods
                    .Select(m => new
                    {
                        MethodSignature = string.Format("{0} {1}{2}({3})",
                            m.ReturnType.FullName,
                            m.Name,
                            !m.IsGenericMethodDefinition ? "" : "`" + m.GetGenericArguments().Length,
                            string.Join(",", m.GetParameters().Select(p => p.ParameterType.FullName).ToArray())), 
                        Type = i.Type
                    })
                ).ToListDictionary(i => i.MethodSignature, i => i.Type);
        }

        public IEnumerable<Tuple<ClassInfo, IEnumerable<InterfaceInfo>>> DuckType()
        {
            return _classes.Select(classInfo => new Tuple<ClassInfo, IEnumerable<InterfaceInfo>>(classInfo, GetInterfacesForClass(classInfo, _methods,_properties)));
        }

        private IEnumerable<InterfaceInfo> GetInterfacesForClass(ClassInfo classInfo, Dictionary<MethodInfo, List<InterfaceInfo>> interfaces, Dictionary<PropertyInfo, List<InterfaceInfo>> properties)
        {
            throw new NotImplementedException();
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
