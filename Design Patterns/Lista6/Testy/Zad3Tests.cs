using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad3;

namespace Testy
{
    [TestFixture]
    class Zad3Tests
    {
        [Test]
        public void VisitorWithoutKnowledgeTest()
        {
            AbstractTree root =
                new TreeNode(
                    4,
                    new TreeNode(
                        2,
                        new TreeLeaf(5),
                        new TreeLeaf(4)
                        ),
                    new TreeNode(
                        2,
                        new TreeLeaf(64),
                        new TreeNode(
                            2,
                            new TreeLeaf(5),
                            null
                            )
                        )
                    );

            DepthCounterVisitor v = new DepthCounterVisitor();
            root.Accept(v);
            Assert.AreEqual(3, v.Depth);
        }

        [Test]
        public void VisitorWithKnowledgeTest()
        {
            AbstractTree root =
                new TreeNode(
                    4,
                    new TreeNode(
                        2,
                        new TreeLeaf(5),
                        new TreeLeaf(4)
                        ),
                    new TreeNode(
                        2,
                        new TreeLeaf(64),
                        new TreeNode(
                            2,
                            new TreeLeaf(5),
                            null
                            )
                        )
                    );

            DepthTreeVisitor v = new DepthTreeVisitor();
            v.Visit(root);
            Assert.AreEqual(3, v.Depth);
        }
    }
}
