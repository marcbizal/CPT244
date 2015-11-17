// =================================
// AUTHOR       : MARCUS BIZAL
// CREATE DATE  : 11/11/2015
// PURPOSE      : HTMLParse.Node
// represents a single tag in HTML,
// and stores data about the inner
// and outer HTML, attributes,
// parent and children, etc..
// It also provides some various
// utility methods for searching and
// manipulating the DOM.
// =================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParse
{
    public class Node
    {
        public string Name { get; internal set; }

        private Dictionary<string, string> Attributes;
        private List<Node> Children;
        public Node Parent { get; internal set; }

        public bool TrailingSlash { get; internal set; }

        public string HTMLDoc { get; internal set; }

        // We don't need OpenTagEnd or CloseTagStart since we have innerHTML
        public int OpenTagStart { get; internal set; }
        public int InnerHTMLStart { get; internal set; }
        public int InnerHTMLEnd { get; internal set; }
        public int CloseTagEnd { get; internal set; }

        public int Depth { get { return getDepth(0); } }
        public bool IsRoot { get { return Parent == null; } }
        public string InnerHTML
        {
            get
            {
                if (!TrailingSlash && HTMLDoc != null && InnerHTMLStart != -1 && InnerHTMLEnd != -1)
                {
                    return HTMLDoc.Substring(InnerHTMLStart, InnerHTMLEnd - InnerHTMLStart);
                }
                else
                {
                    // Not enough information to identify InnerHTML...
                    return "";
                }
            }
        }

        public string OuterHTML
        {
            get
            {
                if (OpenTagStart != -1 && CloseTagEnd != -1)
                {
                    return HTMLDoc.Substring(OpenTagStart, CloseTagEnd - OpenTagStart);
                }
                else
                {
                    // Not enough information to identify OuterHTML...
                    return "";
                }
            }
        }

        public Node(string name_, Node parent_ = null)
        {
            Name = name_;
            Parent = parent_;
            Attributes = new Dictionary<string, string>();
            Children = new List<Node>();

            TrailingSlash = false;

            HTMLDoc = Parent != null ? Parent.HTMLDoc : null;
            OpenTagStart = -1;
            InnerHTMLStart = -1;
            InnerHTMLEnd = -1;
            CloseTagEnd = -1;

            if (Parent != null)
            {
                Parent.addChild(this);
            }
        }

        public void addAttribute(string Name, string value)
        {
            Attributes.Add(Name, value);
        }

        public string getAttribute(string id)
        {
            if (Attributes.ContainsKey(id))
            {
                return Attributes[id];
            }
            else
            {
                return null;
            }
        }

        public void addChild(Node tag)
        {
            Children.Add(tag);
        }

        public void setInnerHTML(int innerHTMLStart_, int innerHTMLEnd_, string HTMLDoc_ = null)
        {
            InnerHTMLStart = innerHTMLStart_;
            InnerHTMLEnd = innerHTMLEnd_;
            if (!String.IsNullOrEmpty(HTMLDoc_)) HTMLDoc = HTMLDoc_;
        }

        public List<Node> FindWithMatchingAttribute(string attributeName, string attributeValue)
        {
            List<Node> queryResults = new List<Node>();
            foreach (Node child in Children)
            {
                queryResults.AddRange(child.FindWithMatchingAttribute(attributeName, attributeValue));
            }
            if (getAttribute(attributeName) == attributeValue) queryResults.Add(this);
            return queryResults;
        }

        protected internal int getDepth(int currentDepth)
        {
            if (IsRoot)
            {
                return currentDepth;
            }
            else
            {
                currentDepth++;
                return Parent.getDepth(currentDepth);
            }
        }

        public void PrintBasicModel()
        {
            Console.Write("{0}|", Depth.ToString().PadLeft(3));
            for (int i = 0; i < Depth; i++) Console.Write(" ");
            Console.Write("{0}\n", Name);

            foreach (Node child in Children) child.PrintBasicModel();
        }
    }
}
