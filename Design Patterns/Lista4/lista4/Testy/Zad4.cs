using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad4;

namespace Zad1Tests
{
    [TestFixture]
    class TestTagBuilder
    {
        [Test]
        public void TestIdentTags()
        {
            TagBuilder tag = new TagBuilder();
            tag.IsIndented = true;
            tag.Indentation = 4;
            var script =
            tag.StartTag("parent")
             .AddAttribute("parentproperty1", "true")
             .AddAttribute("parentproperty2", "5")
                 .StartTag("child1")
                 .AddAttribute("childproperty1", "c")
                 .AddContent("childbody")
                 .EndTag()
                 .StartTag("child2")
                 .AddAttribute("childproperty2", "c")
                 .AddContent("childbody")
                 .EndTag()
             .EndTag()
             .StartTag("script")
             .AddContent("$.scriptbody();")
             .EndTag()
             .ToString();

            TagBuilder tag2 = new TagBuilder();
            tag2.IsIndented = true;
            tag2.Indentation = 4;

            var script2 = tag2.StartTag("parent").AddContent("aaa").EndTag().ToString();

            Assert.AreEqual(script.Replace("\r", ""), 
@"<parent parentproperty1='true' parentproperty2='5'>
    <child1 childproperty1='c'>
        childbody
    </child1>
    <child2 childproperty2='c'>
        childbody
    </child2>
</parent>
<script>
    $.scriptbody();
</script>".Replace("\r", ""));

            Assert.AreEqual(script2.Replace("\r", ""), 
@"<parent>
    aaa
</parent>".Replace("\r", ""));
        }
    }
}
