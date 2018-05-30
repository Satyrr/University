using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
   
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    #region Visitor without knowledge
    public abstract class Visitor
    {
        public abstract void VisitNode(TreeNode t);
        public abstract void VisitLeaf(TreeLeaf t);
    }

    public class DepthCounterVisitor : Visitor
    {
        public int Depth
        {
            get
            {
                return depths.Values.Max();
            }
        }

        private Dictionary<AbstractTree, int> depths = new Dictionary<AbstractTree, int>();

        public override void VisitLeaf(TreeLeaf t)
        {

        }

        public override void VisitNode(TreeNode t)
        {
            int depth = 0;
            if (depths.ContainsKey(t))
                depth = depths[t];

            if(t.Left != null)
                depths.Add(t.Left, depth + 1);

            if(t.Right != null)
                depths.Add(t.Right, depth + 1);
        }
    }

    #endregion


    #region Visitor with knowledge

    public abstract class TreeVisitor
    {
        public void Visit(AbstractTree tree)
        {
            if (tree is TreeNode)
                this.VisitNode((TreeNode)tree);
            if (tree is TreeLeaf)
                this.VisitLeaf((TreeLeaf)tree);
        }

        public virtual void VisitNode(TreeNode node)
        {
            // tu wiedza o odwiedzaniu struktury
            if (node != null)
            {
                this.Visit(node.Left);
                this.Visit(node.Right);
            }
        }

        public virtual void VisitLeaf(TreeLeaf leaf)
        {

        }
    }

    public class DepthTreeVisitor : TreeVisitor
    {
        public int Depth
        {
            get
            {
                return depths.Values.Max();
            }
        }

        private Dictionary<AbstractTree, int> depths = new Dictionary<AbstractTree, int>();

        public override void VisitNode(TreeNode node)
        {
            int depth = 0;
            if (depths.ContainsKey(node))
                depth = depths[node];

            if (node.Left != null)
                depths.Add(node.Left, depth + 1);

            if (node.Right != null)
                depths.Add(node.Right, depth + 1);

            base.VisitNode(node);
        }
    }


    #endregion

    public abstract class AbstractTree
    {
        public abstract void Accept(Visitor v);
    }

    public class TreeNode : AbstractTree
    {
        public AbstractTree Left { get; set; }
        public AbstractTree Right { get; set; }
        public int Value { get; set; }

        public TreeNode(int value, AbstractTree left, AbstractTree right)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        public override void Accept(Visitor v)
        {
            v.VisitNode(this);
            if(Left != null)
                Left.Accept(v);
            if(Right != null)
                Right.Accept(v);
        }
    }

    public class TreeLeaf : AbstractTree
    {
        int _value;

        public TreeLeaf(int value)
        {
            _value = value;
        }

        public override void Accept(Visitor v)
        {
            v.VisitLeaf(this);
        }
    }
}
