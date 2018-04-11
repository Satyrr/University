using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Zad4
{
    public class TagBuilder
    {
        public TagBuilder() { }

        public TagBuilder(string TagName, TagBuilder Parent)
        {
            this.tagName = TagName;
            this.parent = Parent;
            this.IsIndented = Parent.IsIndented;
            this.Indentation = Parent.Indentation;
            this.IndentDepth = Parent.IndentDepth + 1;
        }

        public bool IsIndented { get; set; }
        public int Indentation { get; set; }
        public int IndentDepth { get; set; }

        private string tagName;
        private TagBuilder parent;
        private StringBuilder body = new StringBuilder();
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();

        public TagBuilder AddContent(string Content)
        {
            body.Append("\n\r");
            body.Append(Content);
            return this;
        }

        public TagBuilder AddContentFormat(string Format, params object[] args)
        {
            body.Append("\n\r");
            body.AppendFormat(Format, args);
            return this;
        }

        public TagBuilder StartTag(string TagName)
        {
            TagBuilder tag = new TagBuilder(TagName, this);

            return tag;
        }

        public TagBuilder EndTag()
        {
            parent.AddContent(this.ToString());
            return parent;
        }

        public TagBuilder AddAttribute(string Name, string Value)
        {
            _attributes.Add(Name, Value);
            return this;
        }

        public override string ToString()
        {
            StringBuilder tag = new StringBuilder();

            // preamble
            if (!string.IsNullOrEmpty(this.tagName))
                tag.AppendFormat( "<{0}", tagName);

            if (_attributes.Count > 0)
            {
                tag.Append(" ");
                tag.Append(
                    string.Join(" ",
                        _attributes
                            .Select(
                                kvp => string.Format("{0}='{1}'", kvp.Key, kvp.Value))
                            .ToArray()));
            }

            // body/ending
            if (body.Length > 0)
            {
                string bodyString;
                if (!string.IsNullOrEmpty(this.tagName) || this._attributes.Count > 0)
                {
                    tag.Append(">");
                    bodyString = (body.ToString()).Replace("\n\r", "\n\r" + new string(' ', Indentation));
                }
                else
                {
                    bodyString = body.ToString();
                }
                tag.Append(bodyString);
                if (!string.IsNullOrEmpty(this.tagName))
                    tag.AppendFormat("\n\r</{0}>", this.tagName);
            }
            else
                if (!string.IsNullOrEmpty(this.tagName))
                tag.Append("/>");


            if (!string.IsNullOrEmpty(tagName))
                return tag.ToString();
            else
                return tag.ToString().TrimStart('\n');
        }
    }

    class Program
    {
        static void Main(string[] args)
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

            Console.Write(script);
            Debug.Write(script2);
            Console.Read();
        }
    }
}
