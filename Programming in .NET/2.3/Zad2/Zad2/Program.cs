using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Zad2
{
    class BinaryTreeNode<T> where T : IComparable<T>
    {
        BinaryTreeNode<T> left, right;
        T value;

        public BinaryTreeNode(T val)
        {
            value = val;
            left = null;
            right = null;
        }

        public BinaryTreeNode()
        {
            left = null;
            right = null;
        }

        public void add(T val)
        {
            if (val.CompareTo(value) >= 0)
            {
                if (right == null)
                {
                    right = new BinaryTreeNode<T>(val);
                }
                else
                {
                    right.add(val);
                }
            }
            else
            {
                if (left == null)
                {
                    left = new BinaryTreeNode<T>(val);
                }
                else
                {
                    left.add(val);
                }
            }
        }

        public void showAll()
        {
            if (left != null) left.showAll();
            Console.WriteLine(value);
            if (right != null) right.showAll();
        }

        //Standard DFS 
        public IEnumerable DfsEnumerator
        {
            get { return new DFSEnum(this); }
        }


        class DFSEnum : IEnumerator, IEnumerable
        {
            BinaryTreeNode<T> current;
            Stack<BinaryTreeNode<T>> s;

            public DFSEnum(BinaryTreeNode<T> tree)
            {
                current = new BinaryTreeNode<T>();
                current.right = tree;
                s = new Stack<BinaryTreeNode<T>>();
            }
            public object Current
            {
                get
                {
                    current = s.Pop();
                    return current.value;
                }
            }

            public IEnumerator GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                if (current.right != null)
                {
                    s.Push(current.right);
                }

                if (current.left != null)
                {
                    s.Push(current.left);
                }
                return s.Any();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        //Yield DFS 
        public IEnumerable DfsYieldEnumerator
        {
            get
            {
                //BinaryTreeNode<T> cur = this;
                yield return this.value;
                if(this.left != null)
                {
                    foreach(T val in this.left.DfsYieldEnumerator)
                    {
                        yield return val;
                    }
                }

                if (this.right != null)
                {
                    foreach (T val in this.right.DfsYieldEnumerator)
                    {
                        yield return val;
                    }

                }
            }
        }


        //Standard DFS 
        public IEnumerable BfsEnumerator
        {
            get { return new BFSEnum(this); }
        }


        class BFSEnum : IEnumerator, IEnumerable
        {
            BinaryTreeNode<T> current;
            Queue<BinaryTreeNode<T>> q;

            public BFSEnum(BinaryTreeNode<T> tree)
            {
                current = new BinaryTreeNode<T>();
                current.right = tree;
                q = new Queue<BinaryTreeNode<T>>();
            }
            public object Current
            {
                get
                {
                    current = q.Dequeue();
                    return current.value;
                }
            }

            public IEnumerator GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                if (current.left != null)
                {
                    q.Enqueue(current.left);
                }

                if (current.right != null)
                {
                    q.Enqueue(current.right);
                }
                return q.Any();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        //Yield BFS 
        public IEnumerable BfsYieldEnumerator
        {
            get
            {
                Queue<BinaryTreeNode<T>> q = new Queue<BinaryTreeNode<T>>();
                q.Enqueue(this);

                while(q.Any())
                {
                    BinaryTreeNode<T> current =  q.Dequeue();
                    if (current.left != null)
                    {
                        q.Enqueue(current.left);
                    }

                    if (current.right != null)
                    {
                        q.Enqueue(current.right);
                    }

                    yield return current.value;
                }
            }
        }


    }


    class Program
    {
        static void Main(string[] args)
        {
            BinaryTreeNode<int> tree = new BinaryTreeNode<int>(4);
            tree.add(2);
            tree.add(6);
            tree.add(18);
            tree.add(1);
            //tree.showAll();

            Console.WriteLine("DFS bez yield:");
            foreach (int i in tree.DfsEnumerator)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("DFS z yield:");
            foreach (int i in tree.DfsYieldEnumerator)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("BFS bez yield:");
            foreach (int i in tree.BfsEnumerator)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("BFS z yield:");
            foreach (int i in tree.BfsYieldEnumerator)
            {
                Console.WriteLine(i);
            }


            Console.ReadLine();
        }
    }
}
