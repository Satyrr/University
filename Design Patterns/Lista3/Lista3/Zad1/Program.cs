using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    interface IRepository<T>
    {
        T Get(int id);
        List<T> GetAll();
        void Add(T t);
    }

    class School
    {
        IRepository<Student> _students;
        IRepository<Instructor> _instructors;
        IRepository<Course> _courses;

        public School(IRepository<Student> s, IRepository<Instructor> i, IRepository<Course> c)
        {
            _students = s;
            _instructors = i;
            _courses = c;
        }

        public List<Student> GetStudents()
        {
            return _students.GetAll();
        }

        public List<Instructor> GetInstructors()
        {
            return _instructors.GetAll();
        }

        public List<Course> GetCourses()
        {
            return _courses.GetAll();
        }

        public Student GetStudent(int id)
        {
            return _students.Get(id);
        }

        public Instructor GetInstructor(int id)
        {
            return _instructors.Get(id);
        }

        public Course GetCourse(int id)
        {
            return _courses.Get(id);
        }
    }

    class Person
    {
        protected int _id;
        protected string _name, _surname;

        public int ID { get { return _id; } }

        public virtual string GetInfo()
        {
            return string.Format("{0} {1}, id:{2}  ", _name, _surname, _id);
        }
    }

    class Student : Person
    {
        int _year;
        List<Course> _courses;

        public Student(int i, string n, string s, int y)
        {
            _id = i;
            _name = n; _surname = s;
            _year = y;
            _courses = new List<Course>();
        }

        public override string GetInfo()
        {
            return string.Format("{0} {1}, {2} year, id:{3}  ", _name, _surname, _year, _id);
        }

        public List<Course> GetCourses()
        {
            return _courses;
        }

        public void AddCourse(Course c)
        {
            _courses.Add(c);
        }
    }

    class Instructor : Person
    {
        List<Course> _courses;

        public Instructor(int i, string n, string s)
        {
            _id = i;
            _name = n; _surname = s;
            _courses = new List<Course>();
        }

        public List<Course> GetCourses()
        {
            return _courses;
        }

        public void AddCourse(Course c)
        {
            _courses.Add(c);
        }
    }

    class Course
    {
        int _id;
        public int ID { get { return _id; } }
        string _name;
        Instructor _instructor;

        public Course(int id, string n, Instructor i)
        {
            _id = id;
            _name = n;
            _instructor = i;
        }

        public string GetInfo()
        {
            return string.Format("{0}, Instructor: {1}", _name, _instructor.GetInfo());
        }
    }

    class StudentRepo : IRepository<Student>
    {
        List<Student> list = new List<Student>();
        public void Add(Student t)
        {
            list.Add(t);
        }

        public Student Get(int id)
        {
            return list.Single(s => s.ID == id);
        }

        public List<Student> GetAll()
        {
            return list;
        }
    }

    class InstructorRepo : IRepository<Instructor>
    {
        List<Instructor> list = new List<Instructor>();
        public void Add(Instructor t)
        {
            list.Add(t);
        }

        public Instructor Get(int id)
        {
            return list.Single(s => s.ID == id);
        }

        public List<Instructor> GetAll()
        {
            return list;
        }
    }

    class CourseRepo : IRepository<Course>
    {
        List<Course> list = new List<Course>();
        public void Add(Course t)
        {
            list.Add(t);
        }

        public Course Get(int id)
        {
            return list.Single(s => s.ID == id);
        }

        public List<Course> GetAll()
        {
            return list;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Instructor i1 = new Instructor(1, "Instruktor1", "Nazwisko1");
            Instructor i2 = new Instructor(2, "Instruktor2", "Nazwisko2");

            Course c1 = new Course(1, "Kurs1", i1);
            Course c2 = new Course(2, "Kurs2", i2);

            i1.AddCourse(c1);
            i2.AddCourse(c2);

            Student s1 = new Student(1, "Student1", "Student1", 1);
            Student s2 = new Student(2, "Student2", "Student2", 2);

            CourseRepo cr = new CourseRepo();
            cr.Add(c1); cr.Add(c2);
            StudentRepo sr = new StudentRepo();
            sr.Add(s1); sr.Add(s2);
            InstructorRepo ir = new InstructorRepo();
            ir.Add(i1); ir.Add(i2);

            School school = new School(sr, ir, cr);

            foreach(var course in school.GetCourses())
            {
                Console.WriteLine(course.GetInfo());
            }
            Console.Read();

        }
    }
}
