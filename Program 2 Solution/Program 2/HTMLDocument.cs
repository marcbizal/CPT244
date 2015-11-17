// =================================
// AUTHOR       : MARCUS BIZAL
// CREATE DATE  : 11/11/2015
// PURPOSE      : HTMLParse is a
// very lightweight library designed
// to parse HTML files into a format
// more that is developer-friendly.
// HTMLParse.Document is the main
// class of HTMLParse and holds
// information about the HTML
// document itself.
// =================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParse
{
    public class Document
    {
        private string HTML;
        public Node Root { get; internal set; }
        private int Position;
        public int MaxDepth { get; internal set; }
        public int NodeCount { get; internal set; }

        public Document(string html_)
        {
            HTML = String.Copy(html_);
            Root = new Node("DocumentRoot", null);
            Root.setInnerHTML(0, HTML.Length, HTML);
            Position = 0;
            MaxDepth = 0;
            NodeCount = 0;
        }

        private bool EOF
        {
            get { return Position >= HTML.Length; }
        }

        private bool SkipToNextTag()
        {
            while (!EOF && Peek() != '<') Move();
            if (EOF)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string ParseTagName()
        {
            int start = Position;
            while (!EOF && !Char.IsWhiteSpace(Peek()) && Peek() != '>') Move();
            return HTML.Substring(start, Position - start);
        }

        private string ParseAttributeName()
        {
            int start = Position;
            while (!EOF && !Char.IsWhiteSpace(Peek()) && Peek() != '>' && Peek() != '=') Move();
            return HTML.Substring(start, Position - start);
        }

        private string ParseAttributeValue()
        {
            int start, end;
            char c = Peek();
            if (c == '"' || c == '\'')
            {
                Move();

                start = Position;
                Position = HTML.IndexOfAny(new char[] { c, '\r', '\n' }, start);
                end = Position;

                if (Peek() == c) Move();
            }
            else
            {
                start = Position;
                while (!EOF && !Char.IsWhiteSpace(c) && c != '>')
                {
                    Move();
                    c = Peek();
                }
                end = Position;
            }
            return HTML.Substring(start, end - start);
        }

        private void SkipWhitespace()
        {
            while (!EOF && Char.IsWhiteSpace(Peek())) Move();
        }

        public bool Parse()
        {
            int depth = 0;
            Node currentTag = Root;
            Stack<string> hierarchy = new Stack<string>();

            while (SkipToNextTag())
            {
                Move();

                char c = Peek();
                if (c == '/')
                {
                    // Found the closing tag, close the node.

                    currentTag.InnerHTMLEnd = Position - 1;
                    Move();
                    string closeName = ParseTagName();

                    // NOTE:
                    // Despite the point of this program being the use of a Stack,
                    // the Stack is actually unneeded. The way the DOM tree is parsed,
                    // children are effectively "pushed" and "popped" by changing the 
                    // node currentTag is refrencing. 
                    // As a result, the line below could be written:
                    // (closeName == currentTag.Name)
                    //
                    // This completely eliminates the need for a stack.

                    if (closeName == hierarchy.Peek())
                    {
                        Move();

                        currentTag.CloseTagEnd = Position;
                        currentTag = currentTag.Parent;
                        depth--;

                        hierarchy.Pop();
                    }
                    else
                    {
                        Console.WriteLine("Error: tried to close {0} before closing {1}!", closeName, currentTag.Name);
                        return false;
                    }
                }
                else
                {
                    NodeCount++;
                    depth++;
                    if (depth > MaxDepth) MaxDepth = depth;

                    currentTag = new Node(ParseTagName(), currentTag);
                    if (currentTag.OpenTagStart == -1) currentTag.OpenTagStart = Position - currentTag.Name.Length - 1;

                    hierarchy.Push(currentTag.Name);

                    SkipWhitespace();
                    while (Peek() != '>')
                    {
                        if (Peek() == '/')
                        {
                            // Handle trailing slashes on tags like <br/> and close the node.

                            Move();

                            currentTag.TrailingSlash = true;
                            currentTag.CloseTagEnd = Position;
                            currentTag = currentTag.Parent;
                            depth--;

                            hierarchy.Pop();

                            SkipWhitespace();
                        }
                        else
                        {
                            // Parse Attributes

                            string attrName = ParseAttributeName();
                            string attrValue = "";

                            SkipWhitespace();
                            if (Peek() == '=')
                            {
                                Move();
                                SkipWhitespace();
                                attrValue = ParseAttributeValue();
                                SkipWhitespace();
                            }

                            currentTag.addAttribute(attrName, attrValue);
                        }
                    }

                    Move();
                    currentTag.InnerHTMLStart = Position;
                }
            }
            return true;
        }

        private char Peek(int ahead)
        {
            int target = Position + ahead;
            if (target < HTML.Length)
            {
                return HTML[Position];
            }
            else
            {
                return (char)0;
            }
        }

        private char Peek()
        {
            return Peek(0);
        }

        private void Move()
        {
            if (!EOF) Position++;
        }
    }
}
