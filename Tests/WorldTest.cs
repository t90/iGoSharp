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

    [TestFixture]
    public class WorldTest
    {
        [Test]
        public void GetMethodNameToTypes()
        {
            var methodNameToTypes = World.GetMethodNameToTypes(new []{typeof(Type1),typeof(Type2)});
            Assert.AreEqual(2, methodNameToTypes["System.Void MyMethod()"].Count);
        }
    }
}
