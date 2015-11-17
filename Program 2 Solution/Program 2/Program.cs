using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTMLParse;

namespace Program_2
{
    class Program
    {
        const string HTML_FILENAME = "C:\\Demo.HTML";
        const string SEARCH_ATTR_NAME = "class";
        const string SEARCH_ATTR_VALUE = "navL2";

        static void Main(string[] args)
        {
            Document doc = null;
            using (StreamReader sr = new StreamReader(HTML_FILENAME))
            {
                doc = new Document(sr.ReadToEnd());
            }

            if (doc.Parse())
            {
                Console.WriteLine("HTML structure in {0} reaches a maximum depth of {1} nodes, and has a total of {2} tags.\n", HTML_FILENAME, doc.MaxDepth, doc.NodeCount);

                List<Node> searchResults = doc.Root.FindWithMatchingAttribute(SEARCH_ATTR_NAME, SEARCH_ATTR_VALUE);
                Console.WriteLine("Found {0} tags with attribute \"{1}\", that maches \"{2}\"!", searchResults.Count, SEARCH_ATTR_NAME, SEARCH_ATTR_VALUE);

                doc.Root.PrintBasicModel();
            }
            Console.Read();
        }
    }
}
