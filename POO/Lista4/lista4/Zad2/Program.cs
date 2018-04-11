using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    public interface IShape
    {
        float GetArea();
    }

    public interface IShapeFactoryWorker
    {
        IShape CreateShape(string Shape, params object[] parameters);
    }

    public class Square : IShape
    {
        private int _side;
        public Square(int a)
        {
            _side = a;
        }

        public float GetArea()
        {
            return _side * _side;
        }
    }

    public class Rectangle : IShape
    {
        private int _a, _b;
        public Rectangle(int a, int b)
        {
            _a = a; _b = b;
        }

        public float GetArea()
        {
            return _a * _b;
        }
    }

    public class DefaultShapeFactoryWorker : IShapeFactoryWorker
    {
        public IShape CreateShape(string Shape, params object[] parameters)
        {
            if(Shape == "Square" && parameters.Count() == 1)
            {
                return new Square((int)(parameters[0]));
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }

    public class RectangleShapeFactoryWorker : IShapeFactoryWorker
    {
        public IShape CreateShape(string Shape, params object[] parameters)
        {
            if (Shape == "Square" && parameters.Count() == 1)
            {
                return new Square((int)(parameters[0]));
            }
            else if(Shape == "Rectangle" && parameters.Count() == 2)
            {
                return new Rectangle((int)(parameters[0]), (int)(parameters[1]));
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }

    public class ShapeFactory
    {
        IShapeFactoryWorker _shapeFactoryWorker;
        public ShapeFactory()
        {
            _shapeFactoryWorker = new DefaultShapeFactoryWorker();
        }

        public void RegisterWorker(IShapeFactoryWorker worker)
        {
            _shapeFactoryWorker = worker;
        }

        public IShape CreateShape(string ShapeName, params object[] parameters)
        {
            return _shapeFactoryWorker.CreateShape(ShapeName, parameters);
        }
    }

    public class Program
    {
        static void A(object a)
        {
            Console.WriteLine((int)a);
        }

        static void Main(string[] args)
        {
            A("abc");
        }
    }
}
