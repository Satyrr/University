using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    public class Airport
    {
        static readonly int MaxAcquired = 3; 
        static List<Plane> _availablePlanes;
        static List<Plane> _inUsePlanes;

        static Airport()
        {
            _availablePlanes = new List<Plane>();//{
            /*    new Plane("Samolot1"),
                new Plane("Samolot2"),
                new Plane("Samolot3"),
                new Plane("Samolot4"),
                new Plane("Samolot5")
            };*/
            _inUsePlanes = new List<Plane>();
        }

        public static Plane AcquirePlane()
        {
            if(_inUsePlanes.Count == MaxAcquired)
            {
                throw new Exception("No more planes available");
            }
            if(_availablePlanes.Count == 0)
            {
                _availablePlanes.Add(new Plane("Samolot" + (_inUsePlanes.Count + 1)));
            }

            Plane p = _availablePlanes.ElementAt(0);
            _availablePlanes.Remove(p);
            _inUsePlanes.Add(p);

            return p;
        }

        public static void ReleasePlane(Plane plane)
        {
            if (!_inUsePlanes.Remove(plane))
            {
                throw new ArgumentException();
            }

            _availablePlanes.Add(plane);
        }

    }

    public class Plane
    {
        public string Name { get; }
        public Plane(string _Name)
        {
            Name = _Name;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
