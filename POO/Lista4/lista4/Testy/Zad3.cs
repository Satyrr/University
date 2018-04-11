using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad3;

namespace Zad1Tests
{
    [TestFixture]
    class TestsZad3
    {
        [Test]
        public void TestObjectPool()
        {
            Plane p1 = Airport.AcquirePlane();
            Plane p2 = Airport.AcquirePlane();
            Plane p3 = Airport.AcquirePlane();

            Assert.Throws<Exception>(() => Airport.AcquirePlane());

            Assert.DoesNotThrow(() => Airport.ReleasePlane(p1));


            Plane otherPlane = new Plane("aaa");
            Assert.Throws<ArgumentException>(() => Airport.ReleasePlane(otherPlane));


        }
    }
}
