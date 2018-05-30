using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lista4;
using Zad2;

namespace Zad1Tests
{
    [TestFixture]
    class Zad2
    {
        [Test]
        public void TestShapeFactory()
        {
            ShapeFactory sf = new ShapeFactory();
            IShape square1 = sf.CreateShape("Square", 4);
            Assert.AreEqual(square1.GetArea(), 16);

            Assert.Throws(typeof(ArgumentException), () =>
            {
                IShape square2 = sf.CreateShape("Square");
            });

            Assert.Throws(typeof(ArgumentException), () =>
            {
                IShape square2 = sf.CreateShape("Rectangle");
            });

            Assert.Throws<InvalidCastException>(() =>
            {
                IShape square2 = sf.CreateShape("Square", "5");
            });

            sf.RegisterWorker(new RectangleShapeFactoryWorker());

            IShape rectangle1 = sf.CreateShape("Rectangle", 4, 5);
            Assert.AreEqual(rectangle1.GetArea(), 20);


        }
    }
}
