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

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> inData)
        {
            var hashSet = new HashSet<T>(inData);
            return hashSet;
        }
    }

    public class World
    {
        private readonly Type[] _interfaces;
        private readonly Type[] _classes;

        public World(Type[] interfaces, Type[] classes)
        {
            _interfaces = interfaces;
            _classes = classes;


        }

        

        public IEnumerable<Tuple<Type,Type[]>> DuckTypeInterfaces()
        {

            var methodsInInterfaces = GetMethodNameToTypes(_interfaces);
            var methodsInClasses = GetMethodNameToTypes(_classes);
            var methodClassInterface =
                (
            from i in methodsInInterfaces
            join c in methodsInClasses on i.Key equals c.Key into groupJoin
            from cj in groupJoin.DefaultIfEmpty()
            select new
            {
                Method = i.Key,
                Interface = i.Value,
                Class = (cj.Equals(default(KeyValuePair<string, List<Type>>)) ? null : cj.Value)
            }
                ).ToList();


            var interfacesWithoutImplementation = methodClassInterface
                .Where(mci => mci.Class == null)
                .SelectMany(mci => mci.Interface)
                .Distinct().ToHashSet();

            var methodClassInterfaceClean = methodClassInterface
                .Where(mci => mci.Class != null)
                .Select(mci => new
                {
                    mci.Method, 
                    mci.Class,
                    Interface = mci.Interface.Where(i => !interfacesWithoutImplementation.Contains(i)).ToList()
                }).ToList();

            // needs something like
            // method1 class1 interface1
            // method2 class1 interface1
            
            var flatenned =
            methodClassInterfaceClean
                .SelectMany(m => m.Class
                    .Select(i => new
                    {
                        m.Method,
                        Class = i,
                        Interfaces = m.Interface
                    }))
                .SelectMany(m => m.Interfaces
                    .Select(i => new
                    {
                        m.Method,
                        m.Class,
                        Interface = i,
                    })).OrderBy(i => i.Interface.FullName).ToList();
                    



            var classToMethods = methodClassInterfaceClean
                .SelectMany(mci => mci.Class
                    .Select(c => new
                    {
                        Class = c, 
                        Method = mci.Method
                    }))
                .ToListDictionary(cm => cm.Class, cm => cm.Method);



            classToMethods.ToString();

            return null;
        }
    
        public static Dictionary<string, List<Type>> GetMethodNameToTypes(Type[] inTypes)
        {
            return 
            inTypes
                .Select(t => new {Type = t, Methods = t.GetMethods()})
                .SelectMany(i => i.Methods
                    .Select(m => new
                    {
                        MethodSignature = GetMethodSignature(m), 
                        Type = i.Type
                    })
                ).ToListDictionary(i => i.MethodSignature, i => i.Type);
        }

        public static string GetMethodSignature(MethodInfo method)
        {
            return string.Format("{0} {1}{2}({3})",
                method.ReturnType.FullName,
                method.Name,
                !method.IsGenericMethodDefinition ? "" : "`" + method.GetGenericArguments().Length,
                string.Join(",", method.GetParameters().Select(p => p.ParameterType.FullName).ToArray()));
        }
    }

}
