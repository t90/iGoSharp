using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using iGoSharp;
using Interfaces;
using NUnit.Framework;
using Rhino.Mocks.Constraints;
using iGoSharp;

namespace Tests
{
    public class Type1
    {
        public void MyMethod(){}
        public void MyMethod<T>(){}
        public void MyMethod<T1,T2>() where T1 : class { }
    }

    public class Type2
    {
        public void MyMethod(){}
    }

    public interface Interface1
    {
        void MyMethod<T>();
    }

    public interface Interface2
    {
        void Z();
    }

    public interface Inteface3
    {
        void Z();
        void MyMethod();
    }

    [TestFixture]
    public class WorldTest
    {
        [Test]
        public void MethodToTypesReturnCorrectNumberOfInstances()
        {
            var methodNameToTypes = World.GetMethodNameToTypes(new []{typeof(Type1),typeof(Type2)});
            Assert.AreEqual(2, methodNameToTypes["System.Void MyMethod()"].Count);
        }

        [Test]
        public void TestSimpleMethodNameSignatureCreation()
        {
            Assert.AreEqual(
                "System.Void MyMethod()",
                World.GetMethodSignature(typeof (Type2).GetMethods().Where(m => m.Name == "MyMethod").First())
                );
        }

        [Test]
        public void TestProperDuckTyping()
        {
            var world = new World
                (
                    classes: new []{typeof(Type1),typeof(Type2)},
                    interfaces: new []{ typeof(Interface1), typeof(Interface2), typeof(Inteface3) }
                );
            var duckTypeInterfaces = world.DuckTypeInterfaces();
            Assert.AreEqual(typeof(Interface1),duckTypeInterfaces.First().Item2.First());
        }
    }
}
